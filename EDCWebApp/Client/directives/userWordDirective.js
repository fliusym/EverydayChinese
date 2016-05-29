angular.module('learnChineseApp.directive')
.directive('edcWordItem', [function () {
    'use strict';
    return {
        require: '^edcItemContainer',
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/userWordItem.html',
        scope: {},

        bindToController: {
            item: '=',
            index: '=',
            remove: '&'
        },
        controller: function () {
            var ctrl = this;
            ctrl.basic = true;
            ctrl.example = false;
            ctrl.quote = false;
            ctrl.removeItem = function () {
                this.remove()(this.index);
            }
            ctrl.exampleClick = function () {
                ctrl.basic = false;
                ctrl.example = true;
                ctrl.quote = false;
            }
            ctrl.basicClick = function () {
                ctrl.basic = true;
                ctrl.example = false;
                ctrl.quote = false;
            }
        },
        controllerAs: 'ctrl'
    };
}]);