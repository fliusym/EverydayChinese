angular.module('learnChineseApp.service')
.factory('canvasDrawFactory', [function () {
    'use strict';
    return {
        draw: function (ctx, options) {
            if (ctx && options) {
                ctx.save();
                
                
                ctx.lineJoin = 'round';
                ctx.lineCap = 'round';
                ctx.beginPath();
                ctx.moveTo(options.prevX + 0.5 , options.prevY + 0.5);
                //var midx = (options.prevX + options.currX) / 2;
                //var midy = (options.prevY + options.currY) / 2;
                //ctx.quadraticCurveTo(options.currX + 0.5, options.currY + 0.5, midx, midy);
                ctx.lineTo(options.currX + 0.5 , options.currY + 0.5);
                if (options.eraseFlag) {
                    ctx.globalCompositeOperation = 'destination-out';
                    ctx.lineWidth = 10;
                    ctx.stroke();
                } else {
                    ctx.strokeStyle = 'black';
                    ctx.lineWidth = 3;
                    ctx.stroke();
                }
                ctx.closePath();
                ctx.restore();
            }
        },
        drawnew: function (parent, ctx,width,height) {
            //create a temp canvas
            var tmp_canvas = document.createElement('canvas');
            var tmp_ctx = tmp_canvas.getContext('2d');
            tmp_canvas.id = 'tmp_canvas';
            tmp_canvas.width = width;
            tmp_canvas.height = height;

            parent.appendChild(tmp_canvas);
            var mouse = { x: 0, y: 0 };
            var last_mouse = { x: 0, y: 0 };

            // Pencil Points
            var ppts = [];

            /* Mouse Capturing Work */
            tmp_canvas.addEventListener('mousemove', function (e) {
                mouse.x = typeof e.offsetX !== 'undefined' ? e.offsetX : e.layerX;
                mouse.y = typeof e.offsetY !== 'undefined' ? e.offsetY : e.layerY;
            }, false);

            /* Drawing on Paint App */
            tmp_ctx.lineWidth = 5;
            tmp_ctx.lineJoin = 'round';
            tmp_ctx.lineCap = 'round';
            tmp_ctx.strokeStyle = 'blue';
            tmp_ctx.fillStyle = 'blue';

            tmp_canvas.addEventListener('mousedown', function (e) {
                tmp_canvas.addEventListener('mousemove', onPaint, false);

                mouse.x = typeof e.offsetX !== 'undefined' ? e.offsetX : e.layerX;
                mouse.y = typeof e.offsetY !== 'undefined' ? e.offsetY : e.layerY;

                ppts.push({ x: mouse.x, y: mouse.y });

                onPaint();
            }, false);

            tmp_canvas.addEventListener('mouseup', function () {
                tmp_canvas.removeEventListener('mousemove', onPaint, false);

                // Writing down to real canvas now
                ctx.drawImage(tmp_canvas, 0, 0);
                // Clearing tmp canvas
                tmp_ctx.clearRect(0, 0, tmp_canvas.width, tmp_canvas.height);

                // Emptying up Pencil Points
                ppts = [];
            }, false);

            var onPaint = function () {

                // Saving all the points in an array
                ppts.push({ x: mouse.x, y: mouse.y });

                if (ppts.length < 3) {
                    var b = ppts[0];
                    tmp_ctx.beginPath();
                    //ctx.moveTo(b.x, b.y);
                    //ctx.lineTo(b.x+50, b.y+50);
                    tmp_ctx.arc(b.x, b.y, tmp_ctx.lineWidth / 2, 0, Math.PI * 2, !0);
                    tmp_ctx.fill();
                    tmp_ctx.closePath();

                    return;
                }

                // Tmp canvas is always cleared up before drawing.
                tmp_ctx.clearRect(0, 0, tmp_canvas.width, tmp_canvas.height);

                tmp_ctx.beginPath();
                tmp_ctx.moveTo(ppts[0].x, ppts[0].y);

                for (var i = 1; i < ppts.length - 2; i++) {
                    var c = (ppts[i].x + ppts[i + 1].x) / 2;
                    var d = (ppts[i].y + ppts[i + 1].y) / 2;

                    tmp_ctx.quadraticCurveTo(ppts[i].x, ppts[i].y, c, d);
                }

                // For the last 2 points
                tmp_ctx.quadraticCurveTo(
                    ppts[i].x,
                    ppts[i].y,
                    ppts[i + 1].x,
                    ppts[i + 1].y
                );
                tmp_ctx.stroke();

            };

        }
    };
}]);