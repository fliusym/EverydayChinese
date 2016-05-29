angular.module('learnChineseApp.directive')
.directive('edcLearnRequest', [function () {
    'use strict';
    var checkIfNeedToJoin = function (item) {
        var date = new Date();
        var requestTime = item.date + ' ' + item.startTime;
        var requestDate = new Date(requestTime);
        if (Math.abs((date - requestDate) / 60000) < 2160) {
            return true;
        } else {
            return false;
        }
    }
    return {
        require: '^edcItemContainer',
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/userLearnRequestItem.html',
        scope: {},

        bindToController: {
            item: '=',
            isTeacher: '=',
            index: '=',
            remove: '&',
            edit: '&'
        },
        controller: ['loginUserFactory', 'learnRequestFactory',
            'authenticationFactory', '$uibModal',
            function (loginUserFactory, learnRequestFactory, authenticationFactory, $uibModal) {
                var ctrl = this;
                if (checkIfNeedToJoin(this.item)) {
                    this.item.showLearnRequestLink = true;
                } else {
                    this.item.showLearnRequestLink = false;
                }

                ctrl.removeItem = function () {
                    this.remove()(this.index);
                }


                ctrl.editItem = function () {
                    var availableDates = [];
                    var availableTimes = {};
                    var learnRequests = [];
                    learnRequests = loginUserFactory.getCachedStudentLearnRequests();
                    if (!learnRequests) {
                        var userinfo = authenticationFactory.getLoginInfo();
                        var user = userinfo.user;
                        loginUserFactory.getUserResources(user).$promise.then(function (data) {
                            
                            for (var i = 0; i < data.LearnRequests.length; i++) {
                                var request = data.LearnRequests[i];
                                var requestItem = {
                                    date: request.Date,
                                    startTime: request.StartTime,
                                    endTime: request.EndTime,
                                    //      isTeacher: vm.isTeacher
                                };
                               
                                learnRequests.push(requestItem);
                            }
                        }, function (error) {
                            learnRequests = null;
                        });
                    }
                    if (learnRequests) {
                        var availables = learnRequestFactory.getAvailableTime();
                        learnRequestFactory.filterUserLearnRequests(availables, learnRequests);

                        for (var i = 0; i < availables.length; i++) {
                            var date = availables[i].date;
                            availableDates.push(date);
                            availableTimes[date] = [];
                            if (availables[i].morning && availables[i].morning.length > 0) {
                                availableTimes[date].push.apply(availableTimes[date], availables[i].morning);
                            }
                            if (availables[i].afternoon && availables[i].afternoon.length > 0) {
                                availableTimes[date].push.apply(availableTimes[date], availables[i].afternoon);
                            }
                            if (availables[i].evening && availables[i].evening.length > 0) {
                                availableTimes[date].push.apply(availableTimes[date], availables[i].evening);
                            }
                        }
                        if (availableDates.length > 0) {
                            var modalInstance = $uibModal.open({
                                templateUrl: '/Client/views/edcModal.html',
                                controller: 'ModalInstanceCtrl',
                                resolve: {
                                    dates: function () {
                                        return availableDates;
                                    },
                                    times: function () {
                                        return availableTimes;
                                    }
                                }
                            });
                            modalInstance.result.then(function (result) {
                                ctrl.edit()(ctrl.index, result);
                            }, function () {

                            });
                        }
                    }
                };

            }],
        controllerAs: 'ctrl'
    };
}]);

angular.module('learnChineseApp.directive')
.controller('ModalInstanceCtrl', function ($scope, $uibModalInstance, dates, times) {
    'use strict';
    $scope.availableDates = dates;
    $scope.selectedDate = $scope.availableDates[0];
    $scope.availableTimes = times[$scope.selectedDate];
    $scope.selectedTime = $scope.availableTimes[0];

    $scope.onDateChange = function () {
        $scope.availableTimes = times[$scope.selectedDate];
        if ($scope.availableTimes && $scope.availableTimes.length > 0) {
            $scope.selectedTime = $scope.availableTimes[0];
        }
    }
    $scope.ok = function () {
        $uibModalInstance.close({ selectedDate: $scope.selectedDate, selectedTime: $scope.selectedTime });
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
});
