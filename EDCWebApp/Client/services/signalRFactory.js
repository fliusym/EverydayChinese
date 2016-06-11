angular.module('learnChineseApp.service')
.factory('signalRFactory', ['broadcastFactory', function (broadcastFactory) {
    'use strict';
    var connection, learnRequestProxy;
    var connected = false;
    return {
        init: function () {
            connection = $.hubConnection();
            learnRequestProxy = connection.createHubProxy('learnRequestHub');
            learnRequestProxy.on('draw', function (board) {
                broadcastFactory.send('drawBoard', board);
            });
            learnRequestProxy.on('drawFromStudent', function (board) {
                broadcastFactory.send('drawBoardFromStudent', board);
            });
            learnRequestProxy.on('userConnected', function (user) {
                broadcastFactory.send('userConnected', user);
            });
            learnRequestProxy.on('userReconnected', function (user) {
                broadcastFactory.send('userReconnected', user);
            });
            learnRequestProxy.on('userDisconnected', function (user,stopCalled) {
                broadcastFactory.send('userDisconnected', {user: user, stopCalled: stopCalled});
            });
            learnRequestProxy.on('teacherStoppedExplicitly', function () {
                broadcastFactory.send('teacherStoppedExplicitly');
            });
            learnRequestProxy.on('controlFromTeacher', function () {
                broadcastFactory.send('controlFromTeacher');
            });
            learnRequestProxy.on('cancelFromTeacher', function () {
                broadcastFactory.send('cancelFromTeacher');
            });
            connection.logging = true;
            connection.start()
            .done(function () {
                connected = true;
                broadcastFactory.send('hubConnectionSuccess');
            }).fail(function () {
                connected = false;
                broadcastFactory.send('hubConnectionError');
            });
            connection.error(function (error) {
                console.log('SignalR Error:' + error);
            });
        },
        updateBoardPosition: function (board,fromStudent) {
            if (learnRequestProxy && connected) {
                if (!fromStudent) {
                    learnRequestProxy.invoke('updatePosition', board)
                    .fail(function (error) {
                        console.log('updateBoardPosition error' + error);
                    });
                } else {
                    learnRequestProxy.invoke('updatePositionFromStudent', board)
                    .fail(function (error) {

                    });
                }

            }
            
        },
        giveControlToStudent:function(user){
            if (learnRequestProxy && connected) {
                learnRequestProxy.invoke('giveControlToStudent', user);
            }
        },
        cancelControlFromTeacher:function(user){
            if (learnRequestProxy && connected) {
                learnRequestProxy.invoke('cancelControlFromTeacher',user);
            }
        },
        stopConnection: function () {
            if (connection && learnRequestProxy) {
                connected = false;
                broadcastFactory.send('teacherStopped');
                learnRequestProxy.invoke('teacherStopped');
                connection.stop();
                
            }
        },
        getConnectedStudents: function () {
            if (learnRequestProxy && connected) {
                return learnRequestProxy.invoke('GetConnectedStudents');
            }
        },
        getIsTeacherLoggedOn: function () {
            if (learnRequestProxy && connected) {
                return learnRequestProxy.invoke('isTeacherLoggedIn');
            }
        }

    };
}]);