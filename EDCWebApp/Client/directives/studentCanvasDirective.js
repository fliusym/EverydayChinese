angular.module('learnChineseApp.directive')
.directive('edcStudentCanvas', ['canvasDrawFactory', 'signalRFactory', '$rootScope',
    function (canvasDrawFactory, signalRFactory, $rootScope) {
    'use strict';
    return {
        restrict: 'A',
        scope: {
            candraw: '=',
            erase: '=',
        },
        link: function (scope, elem, attrs) {
            $rootScope.$on('drawBoard', function (msg, board) {
                if (board) {
                    var ctx = elem[0].getContext('2d');
                    canvasDrawFactory.draw(ctx, {
                        prevX: board.previousPosition.xValue,
                        prevY: board.previousPosition.yValue,
                        currX: board.currentPosition.xValue,
                        currY: board.currentPosition.yValue,
                        eraseFlag: board.eraseFlag
                    });
                }
            });
        }
    };
}]);