angular.module('learnChineseApp.directive')
.directive('edcImageList', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/imagelist.html',
        scope: {},

        bindToController: {
            images: '='
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);