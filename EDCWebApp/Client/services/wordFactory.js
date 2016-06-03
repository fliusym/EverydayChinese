angular.module('learnChineseApp.service')
.factory('wordFactory', ['$resource', function ($resource) {
    'use strict';
    var curWordId;
    return {
        getWord: function (date) {
            var resource = $resource('/api/Words', null, null);
            return resource.get({ date: date });
        },
        setCurrentWordIdToAdd: function (id) {
            curWordId = id;
        },
        getCurrentWordIdToAdd: function () {
            return curWordId;
        }
    };
}]);