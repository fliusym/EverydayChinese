angular.module('learnChineseApp.filter').filter('contentDirectory', function () {
    'use strict';
    return function (input,partDirectory) {
        //here only get the first 3 characters
        while (partDirectory.charAt(0) === '/') {
            partDirectory = partDirectory.substr(1);
        }
        if (!partDirectory.endsWith('/')) {
            partDirectory += "/";
        }
        return '/Content/' + partDirectory + input;
    };
});