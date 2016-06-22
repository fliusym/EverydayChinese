angular.module('learnChineseApp.service')
.factory('scenarioFactory', ['$resource', function ($resource) {
    'use strict';
    var curScenarioId;
    return {
        getScenario: function (date) {
            var resource = $resource('/api/Scenes', null, null);
            return resource.get({ date: date });
        },
        setCurrentScenarioIdToAdd: function (id) {
            curScenarioId = id;
        },
        getCurrentScenarioIdToAdd: function () {
            return curScenarioId;
        }
    };
}]);