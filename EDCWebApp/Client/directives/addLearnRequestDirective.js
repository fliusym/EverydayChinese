angular.module('learnChineseApp.directive')
.directive('edcAddLearnRequest', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/addLearnRequestDirective.html',

        scope: {},

        bindToController: {
            addedLearnRequest: '=',
            morningChoose: '=',
            afternoonChoose: '=',
            eveningChoose: '='
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);