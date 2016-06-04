angular.module('learnChineseApp.directive')
.directive('edcAddLearnRequest', [function () {
    'use strict';
    var morningOptionClick = function ($event) {
        var temp = 1;

    };
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
        controller: function () {
            var ctrl = this;
            var prevMorningChoose = '', prevAfternoonChoose = '', prevEveningChoose = '';
            ctrl.morningOptionClick = function ($event) {
                if (prevMorningChoose === $event.target.value) {
                    ctrl.morningChoose = false;
                    prevMorningChoose = '';
                } else {
                    prevMorningChoose = ctrl.morningChoose;
                }
            }
            ctrl.afternoonOptionClick = function ($event) {
                if (prevAfternoonChoose === $event.target.value) {
                    ctrl.afternoonChoose = false;
                    prevAfternoonChoose = '';
                } else {
                    prevAfternoonChoose = ctrl.afternoonChoose;
                }
            }
            ctrl.eveningOptionClick = function ($event) {
                if (prevEveningChoose === $event.target.value) {
                    ctrl.eveningChoose = false;
                    prevEveningChoose = '';
                } else {
                    prevEveningChoose = ctrl.eveningChoose;
                }
            }
        },
        controllerAs: 'ctrl'
    };
}]);