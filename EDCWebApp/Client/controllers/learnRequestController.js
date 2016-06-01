angular.module('learnChineseApp.controller').controller('StudentLearnRequestController', [
    'signalRFactory', '$scope',
    function (signalRFactory, $scope) {
        'use strict';
        var vm = this;
        signalRFactory.init();
        vm.isTeacherLoggedOn = false;

        $scope.$on('hubConnectionSuccess', function (msg) {
            if (!vm.isTeacherLoggedOn) {
                var teacher = signalRFactory.getIsTeacherLoggedOn();
                if (teacher) {
                    teacher.done(function (user) {
                        vm.isTeacherLoggedOn = true;
                        vm.teacher = user;
                        $scope.$apply();
                    }).fail(function (err) {

                    });

                }
            }
        });
        $scope.$on('hubConnectionError', function (msg) {
           
        });
        $scope.$on('userConnected', function (msg, user) {
            if (user) {
                if (user.isTeacher) {
                    vm.isTeacherLoggedOn = true;
                    vm.teacher = user.name;
                    $scope.$apply();
                }
            }
        });
        $scope.$on('userReconnected', function (msg, user) {
            if (user) {
                if (user.isTeacher) {
                    vm.isTeacherLoggedOn = true;
                    vm.teacher = user.name;
                    $scope.$apply();
                }
            }
        });
        $scope.$on('userDisconnected', function (msg, user, stopCalled) {
            if (user) {
                if (user.isTeacher) {

                    if (stopCalled) {
                        vm.isTeacherLoggedOn = false;
                        vm.isLessonEnded = true;
                    //    signalRFactory.stopConnection();
                    } else {
                        vm.isTeacherTempLoggedOut = true;
                        vm.teacher = '';
                    }
                    $scope.$apply();
                }
            }
        });
    }]).controller('TeacherLearnRequestController', [
        '$scope', 'signalRFactory',
        function ($scope, signalRFactory) {
            'use strict';
            var vm = this;

            function getConnectedStudents() {
                var p = signalRFactory.getConnectedStudents();
                if (p) {
                    p.done(function (users) {
                        if (users) {
                            var students = [];
                            for (var i = 0; i < users.length; i++) {
                                students.push(users[i]);
                            }
                            vm.students = students;
                            $scope.$apply();
                        }
                    }).fail(function (err) {

                    });
                }
            }
            signalRFactory.init();
            $scope.$on('hubConnectionSuccess', function (msg) {
                getConnectedStudents();
            });
            $scope.$on('hubConnectionError', function (msg) {

            });
            $scope.$on('userConnected', function (msg, user) {
                getConnectedStudents();

            });
            $scope.$on('userReconnected', function (msg, user) {

                getConnectedStudents();

            });
            $scope.$on('userDisconnected', function (msg, user) {
                if (!user.isTeacher) {
                    getConnectedStudents();
                }
            });

            vm.pencil = function () {
                vm.canDraw = true;
                $scope.$apply();
            };
            vm.endDraw = function (candraw) {
                vm.canDraw = candraw;
                $scope.$apply();
            };
            vm.endSession = function () {
                signalRFactory.stopConnection();
            };
        }]);