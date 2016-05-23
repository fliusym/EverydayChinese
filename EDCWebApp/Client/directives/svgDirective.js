angular.module('learnChineseApp.directive')
.directive('edcSvg', ['$compile', function ($compile) {
    'use strict';
    return {
        restrict: 'E',
        template: '<div data-toggle="tooltip" data-placement="right" title="Click to see how it is written"><ng-include src="getTemplateUrl()"/></div>',

        scope: {
            svgname: '=',

        },
        controller: function ($scope) {
            $scope.getTemplateUrl = function () {
                return '/Content/SVG/' + $scope.svgname + '.svg';
            }
        },
        link: function (scope, element, attr) {
            element.on('mousedown', function (event) {
                event.preventDefault();
                var svg = angular.element(element[0].querySelector('svg'));
                svg = $(svg);
                //    var gs = $(this).find('g');
                var gs = svg.find('g');
                var paths = [];
                for (var j = 0; j < gs.length; j++) {
                    var cur = gs[j];
                    if (cur && cur.id === 'layerCharacter') {
                        paths = $(cur).find('path');
                    }
                }
                for (var i = 0; i < paths.length; i++) {
                    //var path = $document.getElementById(paths[i].id);
                    var path = paths[i];
                    var len = path.getTotalLength();
                    //clean previous transition
                    path.style.transition = path.style.WebkitTransition = 'none';
                    //setup the starting position
                    path.style.strokeDasharray = len + ' ' + len;
                    path.style.strokeDashoffset = len;
                    // Trigger a layout so styles are calculated & the browser
                    // picks up the starting position before animating
                    path.getBoundingClientRect();
                    var nexttime = i * 1.0 + 's';
                    path.style.transition = path.style.WebkitTransition =
                                        'stroke-dashoffset 0.5s ' + nexttime + '  ease-in-out';
                    //go animate
                    path.style.strokeDashoffset = len / 2;
                    path.style.strokeDashoffset = len / 2;
                    path.style.strokeWidth = '3px';
                    path.style.stroke = 'rgba(0,0,0,1)';
                }
            });
        }
    };
}]);