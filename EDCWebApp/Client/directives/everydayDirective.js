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