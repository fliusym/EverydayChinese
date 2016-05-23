angular.module('learnChineseApp.directive')
.directive('edcItemContainer', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/userItemContainer.html',
        transclude: true,
        scope: {
            title: '@'
        },
        controller: function () { },
        controllerAs: 'ctrl',
        bindToController: true
    };
}]);