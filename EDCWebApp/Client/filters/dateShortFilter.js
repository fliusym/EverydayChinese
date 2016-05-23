angular.module('learnChineseApp.filter').filter('dateshort', function () {
    'use strict';
    return function (input) {
        //here only get the first 3 characters
        return input.substring(0, 3);
    };
});