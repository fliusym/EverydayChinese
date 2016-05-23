angular.module('learnChineseApp.directive')
.directive('edcCharacter', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/character.html',
        scope: {

        },
        bindToController: {
            svgname: '@',
            add: '&'
        },
        controller: [function () {
            var ctrl = this;
            ctrl.canadd = false;
            ctrl.onAdd = function () {
                ctrl.canadd = false;
                this.add();
            }
        }],
        controllerAs: 'ctrl',
    }
}]);