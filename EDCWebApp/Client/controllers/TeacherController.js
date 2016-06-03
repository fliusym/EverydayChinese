angular.module('learnChineseApp.controller')
.controller('TeacherController', ['$routeParams', 'loginUserFactory', '$location',
    '$filter', 'errorFactory', 'timeFactory',
    function ($routeParams, loginUserFactory, $location, $filter, errorFactory, timeFactory) {
        'use strict';
        var vm = this;
        var teacherName = $routeParams.user;
        vm.authorized = false;
        vm.teacherLearnRequests = [];

        loginUserFactory.getTeacherResource(teacherName).$promise.then(function (result) {
            if (result) {
                vm.authorized = true;
                var data = result.LearnRequests;
                var learnRequests = [];
                for (var i = 0; i < data.length; i++) {
                    var cur = data[i];
                    if (cur) {
                        var item = {
                            id: cur.Id,
                            date: cur.Date,
                            startTime: cur.StartTime,
                            endTime: cur.EndTime,
                            students: cur.StudentNames
                        };
                        item.startToEndTime = item.startTime + ' - ' + item.endTime;
                        learnRequests.push(item);
                    }
                }
                vm.teacherLearnRequests = learnRequests;
            }
        }, function (error) {
            vm.authorized = false;
            vm.error = {
                persistent: true,
                message: "There is something wrong while retrieving learn requests.",
                type: 'error'
            };
        });
        
    }]);