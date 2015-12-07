﻿/// <binding AfterBuild='uglify:beauty_target' />
// This file in the main entry point for defining grunt tasks and using grunt plugins.
// Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409

module.exports = function (grunt) {
    grunt.initConfig({
        bower: {
            install: {
                options: {
                    targetDir: "wwwroot/lib",
                    layout: "byComponent",
                    cleanTargetDir: false
                }
            }
        },
        uglify: {
            ugli_target: {
                files: {
                    "wwwroot/scripts/chat.js": ["Scripts/chat.js"]
                }
            },
            beauty_target: {
                options: {
                    beautify: {
                        beautify: true
                    },
                    mangle: false,
                    sourceMap: true
                },
                files: {
                    "wwwroot/scripts/chat.js": ["Scripts/chat.js"]
        }
    }
        }
    });

    // This command registers the default task which will install bower packages into wwwroot/lib
    grunt.registerTask("default", ["bower:install"]);

    // The following line loads the grunt plugins.
    // This line needs to be at the end of this this file.
    grunt.loadNpmTasks("grunt-contrib-uglify");
    grunt.loadNpmTasks("grunt-bower-task");
};