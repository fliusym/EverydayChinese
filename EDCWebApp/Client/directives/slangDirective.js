angular.module('learnChineseApp.directive')
.directive('edcSlang', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/slang.html',
        scope: {},
        bindToController: {
            slang: '='
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);