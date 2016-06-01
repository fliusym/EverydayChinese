angular.module('learnChineseApp.service')
.factory('canvasDrawFactory', [function () {
    'use strict';
    return {
        draw: function (ctx, options) {
            if (ctx && options) {
                ctx.save();
                ctx.beginPath();
                ctx.moveTo(options.prevX, options.prevY);
                ctx.lineTo(options.currX, options.currY);
                if (options.eraseFlag) {
                    ctx.globalCompositeOperation = 'destination-out';
                    ctx.lineWidth = 10;
                    ctx.stroke();
                } else {
                    ctx.strokeStyle = 'black';
                    ctx.lineWidth = 1;
                    ctx.stroke();
                }
                ctx.closePath();
                ctx.restore();
            }
        }
    };
}]);