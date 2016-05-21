angular.module('learnChineseApp.service')
.factory('wordFactory', ['$resource', function ($resource) {
    'use strict';
    return {
        getWord: function (date) {
            var resource = $resource('/api/Words', null, null);
            return resource.get({ date: date });
        },

    };
}]);