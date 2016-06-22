angular.module('learnChineseApp.directive')
.directive('edcScenario', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/scenario.html',

        scope: {},

        bindToController: {
            scenario: '=',
            add: '&'
        },
        controller: function () {
            var ctrl = this;
            ctrl.onAdd = function () {
                ctrl.canadd = false;
                this.add()(this.scenario.id);
            }
        },
        controllerAs: 'ctrl'
    };
}]);