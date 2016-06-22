angular.module('learnChineseApp.directive')
.directive('edcScenarioItem', [function () {
    'use strict';
    return {
        require: '^edcItemContainer',
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/userScenarioItem.html',
        scope: {},

        bindToController: {
            item: '=',
            index: '=',
            remove: '&',
            getDetail: '&'
        },
        controller: function () {
            var ctrl = this;
            ctrl.removeItem = function ($event) {
                $event.preventDefault();
                this.remove()(this.index);
            }
            ctrl.getItemDetail = function () {
                this.getDetail()(this.index);
            }
        },
        controllerAs: 'ctrl'
    };
}]);