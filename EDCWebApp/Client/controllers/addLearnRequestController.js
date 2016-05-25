angular.module('learnChineseApp.controller')
.controller('AddLearnRequestController', ['loginUserFactory', 'authenticationFactory',
    'learnRequestFactory', '$location', 'errorFactory',
    function (
    loginUserFactory,
    authenticationFactory,
    learnRequestFactory,
    $location,
    errorFactory) {
        'use strict';
        var vm = this;

        //var userinfo = authenticationFactory.getLoginInfo();
        //if (userinfo) {
        //    var user = userinfo.user;
        //    if (user) {
        //        LoginUserService.getUserResources(user).$promise.then(function (data) {
        //            if (data) {
        //                var learnRequests = data.LearnRequests;
        //                var availables = AddLearnRequestService.getAvailableTime();
        //                AddLearnRequestService.filterUserLearnRequests(availables, learnRequests);
        //                vm.availableTime = availables;
        //            }
        //        });
        //    }
        //}
       // var learnRequests = loginUserFactory.getCachedStudentLearnRequests();
        var availables = learnRequestFactory.getAvailableTime();
    //    learnRequestFactory.filterUserLearnRequests(availables, learnRequests);
        vm.availableTime = availables;
        //  vm.availableTime = AddLearnRequestService.getAvailableTime();
        vm.onSubmit = function () {
            var data = [];
            for (var i = 0; i < vm.availableTime.length; i++) {
                var cur = vm.availableTime[i];
                var date = cur.date;
                var times = [];
                if (cur.morningChoose) {
                    times.push(cur.morningChoose);
                }
                if (cur.afternoonChoose) {
                    times.push(cur.afternoonChoose);
                }
                if (cur.eveningChoose) {
                    times.push(cur.eveningChoose);
                }
                if (times.length > 0) {
                    data.push({
                        'Date': date,
                        'Times': times
                    });
                }

            }
            if (data.length > 0) {
                var requestData = {
                    data: data
                };
                //  var userName = userinfo.user;
                loginUserFactory.addLearnRequest(requestData).$promise.then(function () {
                    $location.path('/user').search({ user: requestData.user });
                }, function (error) {
                    errorFactory.setErrorFromException(error);
                    vm.error = errorFactory.getErrorMsg();
                });
            }
        }
    }]);