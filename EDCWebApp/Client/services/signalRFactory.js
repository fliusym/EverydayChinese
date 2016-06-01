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
            learnRequestProxy.on('userConnected', function (user) {
                broadcastFactory.send('userConnected', user);
            });
            learnRequestProxy.on('userReconnected', function (user) {
                broadcastFactory.send('userReconnected', user);
            });
            learnRequestProxy.on('userDisconnected', function (user,stopCalled) {
                broadcastFactory.send('userDisconnected', user,stopCalled);
            });
            connection.start()
            .done(function () {
                connected = true;
                broadcastFactory.send('hubConnectionSuccess');
            }).fail(function () {
                connected = false;
                broadcastFactory.send('hubConnectionError');
            });
        },
        updateBoardPosition: function (board) {
            if (learnRequestProxy && connected) {
                learnRequestProxy.invoke('updatePosition', board);
            }
            
        },
        stopConnection: function () {
            if (connection && learnRequestProxy) {
                connected = false;
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