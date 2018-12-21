namespace CrossCutting.Logging.Data
{
    public static class DBInitializer
    {
        public static void Initialize(Log4netDBContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
