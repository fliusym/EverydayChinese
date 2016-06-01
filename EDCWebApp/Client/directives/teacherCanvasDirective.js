angular.module('learnChineseApp.directive')
.directive('edcTeacherCanvas', ['canvasDrawFactory', 'signalRFactory', function (canvasDrawFactory, signalRFactory) {
    'use strict';
    return {
        restrict: 'A',
        scope:{
            candraw: '=',
            erase: '=',
            enddraw: '&'
        },
        link: function (scope, elem, attrs) {
            var ctx = elem[0].getContext('2d');
            var drawing = false;

            var lastX, lastY, currentX, currentY;
            elem.bind('mousedown', function (event) {
                if (event.offsetX !== undefined) {
                    lastX = event.offsetX;
                    lastY = event.offsetY;
                } else {
                    lastX = event.layerX - event.currentTarget.offsetLeft;
                    lastY = event.layerY - event.currentTarget.offsetTop;
                }
                drawing = true;
            });
            elem.bind('mousemove', function (event) {
                if (drawing && scope.candraw) {
                    if (event.offsetX !== undefined) {
                        currentX = event.offsetX;
                        currentY = event.offsetY;
                    } else {
                        currentX = event.layerX - event.currentTarget.offsetLeft;
                        currentY = event.layerY - event.currentTarget.offsetTop;
                    }
                    canvasDrawFactory.draw(ctx, { prevX: lastX, prevY: lastY, currX: currentX, currY: currentY });
                    signalRFactory.updateBoardPosition({
                        'previousPosition': {
                            'xValue': lastX,
                            'yValue': lastY
                        },
                        'currentPosition': {
                            'xValue': currentX,
                            'yValue': currentY
                        },
                        'eraseFlag': false
                    });
                    lastX = currentX;
                    lastY = currentY;
                }
            });
            elem.bind('mouseup', function (event) {
                drawing = false;
                scope.candraw = false;
                scope.enddraw()(scope.candraw);
            })
        }
    };
}]);