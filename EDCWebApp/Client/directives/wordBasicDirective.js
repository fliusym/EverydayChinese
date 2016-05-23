angular.module('learnChineseApp.directive')
.directive('edcWordBasic', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/wordBasic.html',
        scope: {},

        bindToController: {
            pinyin: '@',
            english: '@',
            audiosrc: '@'
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);