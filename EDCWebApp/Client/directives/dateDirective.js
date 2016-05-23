angular.module('learnChineseApp.directive')
.directive('edcDate', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/date.html',
        scope: {
            datechange: '&'
        },
        controller: ['$scope', '$filter', function ($scope, $filter) {
            $scope.today = new Date();

            this.getCurrentDate = function () {
                var cur = $scope.today;
                return $filter('date')(cur, "MM/dd/yyyy");
            }
        }],
        link: function (scope, element, attributes) {
            scope.datemore = true;
            //when yesterday is called
            scope.onYesterday = function () {
                var date = scope.today;
                date.setDate(date.getDate() - 1);
                scope.today = date;
                var cur = new Date();
                if (Math.abs(date - cur) / (1000 * 60 * 60 * 24) > 3) {
                    scope.dateless = true;
                } else {
                    scope.datemore = false;
                    scope.dateless = false;
                }
                this.datechange()(scope.today);
            };

            scope.onTomorrow = function () {
                var date = scope.today;
                date.setDate(date.getDate() + 1);
                scope.today = date;
                var cur = new Date();
                var curYear = cur.getYear();
                var curDate = cur.getDate();
                var year = date.getYear();
                if (curYear === year && curDate === date.getDate()) {
                    scope.datemore = true;
                } else {
                    /*if (Math.abs(date - cur) / (1000 * 60 * 60 * 24) > 3)*/
                    if (Math.abs(date - cur) / (1000 * 60 * 60 * 24) > 3) {
                        scope.datemore = true;
                    } else {
                        scope.dateless = false;
                        scope.datemore = false;
                    }
                }
                this.datechange()(scope.today);
            }
        }
    };
}]);