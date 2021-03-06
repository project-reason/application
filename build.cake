#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin nuget:?package=SharpZipLib
#addin nuget:?package=Cake.Compression
#addin nuget:?package=Cake.Npm
#addin nuget:?package=Cake.Kudu.Client&version=0.5.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Information($"Running target {target} in configuration {configuration}");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////
// Define directories.
var distDirectory = Directory("./dist");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() => {
    CleanDirectory(distDirectory);
});


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() => {
     DotNetCoreRestore("./src/");
});

Task("Restore-Npm-Packages")
    .Does(() => {     
        
        var _NpmInstallSettings = new NpmInstallSettings() {
            LogLevel = NpmLogLevel.Info,
            WorkingDirectory = "./src/Main.Site/",
            Production = true
        };

        NpmInstall(_NpmInstallSettings);
});


Task("Running-Grunt-Task-copy")
    .Does(() => {        
    var _NpmRunScriptSettings = new NpmRunScriptSettings(){
        LogLevel = NpmLogLevel.Verbose,
        ScriptName = "grunt:copy",
        WorkingDirectory = "./src/Main.Site/"
    };
    NpmRunScript(_NpmRunScriptSettings);      
});

Task("Running-Grunt-Task-sass")
    .Does(() => {        
    var _NpmRunScriptSettings = new NpmRunScriptSettings(){
        LogLevel = NpmLogLevel.Info,
        ScriptName = "grunt:sass",
        WorkingDirectory = "./src/Main.Site/"
    };
    NpmRunScript(_NpmRunScriptSettings);      
});

Task("Running-Grunt-Task-cssmin")
    .Does(() => {        
    var _NpmRunScriptSettings = new NpmRunScriptSettings(){
        LogLevel = NpmLogLevel.Info,
        ScriptName = "grunt:cssmin",
        WorkingDirectory = "./src/Main.Site/"
    };
    NpmRunScript(_NpmRunScriptSettings);      
});

Task("Running-Grunt-Tasks")
    .IsDependentOn("Restore-Npm-Packages")
    .IsDependentOn("Running-Grunt-Task-copy")
    .IsDependentOn("Running-Grunt-Task-sass")
    .IsDependentOn("Running-Grunt-Task-cssmin");

 Task("Execute-Dotnet-Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Restore-Npm-Packages")
    .IsDependentOn("Running-Grunt-Tasks")
    .Does(() =>
    {
        DotNetCoreBuild("./src/Main.sln",
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
    });

Task("Execute-Xunit-Test")
    .IsDependentOn("Execute-Dotnet-Build")
    .Does(() =>
    {
        var projects = GetFiles("./src/tests/**/*.csproj");
        foreach(var project in projects)
        {
            Information("Testing project " + project);
            DotNetCoreTest(
                project.ToString(),
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true,
                    ArgumentCustomization = args => args.Append("--no-restore"),
                });
        }
    });

Task("Execute-Publish-Web")
    .IsDependentOn("Execute-Xunit-Test")
    .Does(() =>
    {
        DotNetCorePublish(
            "./src/Main.Site/Main.Site.csproj",
            new DotNetCorePublishSettings()
            {
                Configuration = configuration,
                OutputDirectory = distDirectory,
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
    });

Task("DeployToAzure")
    .Description("Deploy to Azure ")
    .Does(() =>
    {
        // https://hackernoon.com/run-from-zip-with-cake-kudu-client-5c063cd72b37
        string baseUri  = EnvironmentVariable("KUDU_CLIENT_BASEURI"),
               userName = EnvironmentVariable("KUDU_CLIENT_USERNAME"),
               password = EnvironmentVariable("KUDU_CLIENT_PASSWORD");
        Information($"Kudu deploy to {baseUri} {userName} {password}");
        IKuduClient kuduClient = KuduClient(
            baseUri,
            userName,
            password);
        var skipPostDeploymentValidation = true; // .NET core apps don't report their version number
        FilePath deployFilePath = kuduClient.ZipRunFromDirectory(distDirectory, skipPostDeploymentValidation);
        Information("Deployed to {0}", deployFilePath);
    });


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////
Task("Default")
    .IsDependentOn("Execute-Publish-Web");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////
RunTarget(target);
