angular.module('learnChineseApp.directive')
.directive('edcDismissibleError', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/dismissibleError.html',

        scope: {},

        bindToController: {
            error: '='
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);