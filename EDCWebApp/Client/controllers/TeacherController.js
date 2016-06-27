'use strict';
/**
*@name TeacherController
*@description
* when teacher log in, it will show all learn requests
*/
angular.module('learnChineseApp.controller')
.controller('TeacherController', ['$routeParams', 'loginUserFactory', '$location',
    '$filter', 'errorFactory', 'timeFactory',
    function ($routeParams, loginUserFactory, $location, $filter, errorFactory, timeFactory) {
        
        var vm = this;
        var teacherName = $routeParams.user;
        vm.authorized = false;
        vm.teacherLearnRequests = [];

        loginUserFactory.getTeacherResource(teacherName).$promise.then(function (result) {
            if (result) {
                vm.authorized = true;
                var data = result.LearnRequests;
                vm.teacherLearnRequests = data.map(function (val) {
                    return {
                        id: val.Id,
                        date: val.Date,
                        startTime: val.StartTime,
                        endTime: val.EndTime,
                        students: val.StudentNames,
                        startToEndTime: val.StartTime + ' - ' + val.EndTime
                    };
                });
            }
        }, function (error) {
            vm.authorized = false;
            vm.error = {
                persistent: true,
                message: "There is something wrong while retrieving learn requests.",
                type: 'error'
            };
        });
        
        vm.deleteLR = function (index) {
            var learnRequest = vm.teacherLearnRequests[index];
            if (learnRequest) {
                var id = learnRequest.id;
                loginUserFactory.deleteLearnRequest(id)
                    .$promise.then(function () {
                        vm.teacherLearnRequests.splice(index, 1);
                    }, function (error) {
                        vm.teacherLearnRequests[index].error = {};
                        vm.teacherLearnRequests[index].error['message'] = "There is something wrong.";
                    });
            }

        }
    }]);