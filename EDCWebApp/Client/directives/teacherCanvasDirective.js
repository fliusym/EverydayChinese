angular.module('learnChineseApp.directive')
.directive('edcTeacherCanvas', ['canvasDrawFactory', 'signalRFactory', '$rootScope', function (canvasDrawFactory, signalRFactory,$rootScope) {
    'use strict';
    var drawBoard = function (board,ctx) {
        if (board) {
            canvasDrawFactory.draw(ctx, {
                prevX: board.previousPosition.xValue,
                prevY: board.previousPosition.yValue,
                currX: board.currentPosition.xValue,
                currY: board.currentPosition.yValue,
                eraseFlag: board.eraseFlag
            });
        }
    };
    return {
        restrict: 'A',
        scope:{
            candraw: '=',
            erase: '=',
            isStudent: '=',
            enddraw: '&'
        },
        link: function (scope, elem, attrs) {
            $rootScope.$on('drawBoard', function (msg, board) {
                var ctx = elem[0].getContext('2d');
                drawBoard(board, ctx);
            });
            $rootScope.$on('drawBoardFromStudent', function (msg, board) {
                var ctx = elem[0].getContext('2d');
                drawBoard(board, ctx);
            });
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
                if (drawing && (scope.candraw || scope.erase)) {
                  
                    if (event.offsetX !== undefined) {
                        currentX = event.offsetX;
                        currentY = event.offsetY;
                    } else {
                        currentX = event.layerX - event.currentTarget.offsetLeft;
                        currentY = event.layerY - event.currentTarget.offsetTop;
                    }
                    canvasDrawFactory.draw(ctx, { prevX: lastX, prevY: lastY, currX: currentX, currY: currentY,eraseFlag: scope.erase });
                    signalRFactory.updateBoardPosition({
                        'previousPosition': {
                            'xValue': lastX,
                            'yValue': lastY
                        },
                        'currentPosition': {
                            'xValue': currentX,
                            'yValue': currentY
                        },
                        'eraseFlag': scope.erase
                    },scope.isStudent);
                    lastX = currentX;
                    lastY = currentY;
                   
                }
            });
            elem.bind('mouseup', function (event) {
                drawing = false;
                //scope.candraw = false;
                //scope.enddraw()(scope.candraw);
            })
        }
    };
}]);