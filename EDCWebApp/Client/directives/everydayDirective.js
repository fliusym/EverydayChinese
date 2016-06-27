/**
*@name edcEveryday
*@description
* show every day topics
*@param {string@} title  every day topic in english
*@param {string@} chinese every day topic in chinese
*/
angular.module('learnChineseApp.directive')
.directive('edcEveryday', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/everyday.html',
        scope: {},
        bindToController: {
            title: '@',
            chinese: '@'
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);