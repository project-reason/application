const Assets = require('./assets');

module.exports = function (grunt) {
    const sass = require('node-sass');
    const Fiber = require('fibers');

    require('load-grunt-tasks')(grunt);
    grunt.loadNpmTasks('grunt-contrib-cssmin');

    grunt.initConfig({
        cssmin: {
            target: {
                src: ['./wwwroot/assets/css/style.css'],
                dest: "./wwwroot/assets/css/style.min.css"
            }
        },
        copy: {
            main: {
                files: Assets.map((asset) => {
                    return {
                        src: `./node_modules/${asset}`,
                        dest: `./wwwroot/assets/3rd-party-assets/${asset}`
                    }
                })
            }
        },
        sass: {
            options: {
                implementation: sass,
                fiber: Fiber,
                sourceMap: true
            },
            dist: {
                files: {
                    './wwwroot/assets/css/style.css': './ClientApp/Sass/Style.scss'
                }
            }
        }
    }); 

    grunt.loadNpmTasks('grunt-contrib-copy');
};