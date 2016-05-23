angular.module('learnChineseApp.directive')
.directive('edcPhrase', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/phrase.html',
        scope: {},

        bindToController: {
            phraseList: '=',
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);