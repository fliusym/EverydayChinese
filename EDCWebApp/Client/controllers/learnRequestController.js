angular.module('learnChineseApp.controller').controller('StudentLearnRequestController', [
    'signalRFactory', '$scope', 'broadcastFactory', '$location', '$rootScope',
    function (signalRFactory, $scope, broadcastFactory, $location, $rootScope) {
        'use strict';
        var vm = this;
        signalRFactory.init();
        vm.isTeacherLoggedOn = false;

        $scope.$on('hubConnectionSuccess', function (msg) {
            if (!vm.isTeacherLoggedOn) {
                var teacher = signalRFactory.getIsTeacherLoggedOn();
                if (teacher) {
                    teacher.done(function (user) {
                        if (user) {
                            vm.isTeacherLoggedOn = true;
                            vm.teacher = user;
                            $scope.$apply();
                        }

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
        $scope.$on('userDisconnected', function (msg, data) {
            if (data) {
                if (data.user.isTeacher) {

                    if (data.stopCalled) {
                        vm.isTeacherLoggedOn = false;
                        vm.isLessonEnded = true;
                        vm.infomsg = "Sorry, but the current session is interrupted. Please wait for a few seconds to press F5 to refresh."
                    } else {
                        vm.teacher = '';
                    }
                    $scope.$apply();
                } else {

                }
            }
        });

        $scope.$on('teacherStoppedExplicitly', function (msg) {
            vm.infomsg = 'The session is ended. Thanks for your participation.';
            vm.isTeacherLoggedOn = false;
            vm.isLessonEnded = true;
            $scope.$apply();
        });
    }]).controller('TeacherLearnRequestController', [
        '$scope', 'signalRFactory', '$routeParams', 'loginUserFactory',
        function ($scope, signalRFactory, $routeParams, loginUserFactory) {
            'use strict';
            var vm = this;
            var id = $routeParams.index;
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
            $scope.$on('userDisconnected', function (msg, data) {
                if (!data.user.isTeacher) {
                    getConnectedStudents();
                }
            });

            vm.pencil = function () {
                if (vm.canDraw) {
                    vm.canDraw = false;
                } else {
                    vm.canDraw = true;
                }
                vm.eraseFlag = false;
              //  $scope.$apply();
            };
            vm.erase = function () {
                if (vm.eraseFlag) {
                    vm.eraseFlag = false;
                } else {
                    vm.eraseFlag = true;
                }
                
                vm.canDraw = false;
            };
            //vm.endDraw = function (candraw) {
            //    vm.canDraw = candraw;
            //    $scope.$apply();
            //};
            vm.endSession = function () {
                signalRFactory.stopConnection();
                vm.sessionEnded = true;
                vm.infomsg = "The sesssion is ended.";
                loginUserFactory.deleteLearnRequest(id)
                    .$promise.then(function () {

                    }, function (error) {

                    });
            };
        }]);