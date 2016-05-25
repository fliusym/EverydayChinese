angular.module('learnChineseApp.controller')
.controller('StudentController', ['$routeParams', 'loginUserFactory', '$location',
    '$filter', 'errorFactory', 'timeFactory',
    function ($routeParams, loginUserFactory, $location, $filter, errorFactory, timeFactory) {
        'use strict';
        var vm = this;
        var userName = $routeParams.user;
        vm.authorized = false;

        vm.userWordItems = [];
        vm.userLearnRequests = [];

        loginUserFactory.getUserResources(userName).$promise.then(function (data) {
            if (data) {
                vm.authorized = true;
                //   vm.isTeacher = data.IsTeacher;
                var words = data.Words;
                for (var i = 0; i < words.length; i++) {
                    var word = words[i];
                    var item = {
                        id: word.Id,
                        pinyin: word.Pinyin,
                        chinese: word.Character,
                        audiosrc: word.Audio,
                        basicMeaning: word.BasicMeanings,
                        date: word.Date
                    };
                    vm.userWordItems.push(item);
                }

                //learn requests
                var learnRequests = data.LearnRequests;
                for (var i = 0; i < learnRequests.length; i++) {
                    var request = learnRequests[i];
                    var requestItem = {
                        id: request.Id,
                        date: request.Date,
                        startTime: request.StartTime,
                        endTime: request.EndTime,
                        //      isTeacher: vm.isTeacher
                    };
                    vm.userLearnRequests.push(requestItem);
                }
                loginUserFactory.setCachedStudentLearnRequests(vm.userLearnRequests);
            }
        }, function (error) {
            vm.authorized = false;
            errorFactory.setErrorFromException(error);
            vm.error = errorFactory.getErrorMsg();
        });

        vm.removeLearnRequest = function (index) {
            if (index > -1 && index < vm.userLearnRequests.length) {
                var learnRq = vm.userLearnRequests[index];
                loginUserFactory.removeLearnRequest(learnRq.id)
                    .$promise.then(function () {
                        vm.userLearnRequests.splice(index, 1);
                    }, function (error) {
                        errorFactory.setErrorFromException(error);
                        vm.userLearnRequests[index].error = errorFactory.getErrorMsg();
                    });
            }
            
            
        }
        vm.editLearnRequest = function (index,newTime) {

            if (index > -1 && index < vm.userLearnRequests.length) {
                var learnRq = vm.userLearnRequests[index];
                if (learnRq) {
                    var startToEndString = learnRq.startTime + ' - ' + learnRq.endTime;
                    if ((learnRq.date === newTime.selectedDate) && (startToEndString === newTime.selectedTime)) {
                        return;
                    }
                    loginUserFactory.editLearnRequest(learnRq.id, newTime.selectedDate, newTime.selectedTime)
                    .$promise.then(function () {
                        vm.userLearnRequests[index].date = newTime.selectedDate;
                        vm.userLearnRequests[index].startTime = timeFactory.getStartTime(newTime.selectedTime);
                        vm.userLearnRequests[index].endTime = timeFactory.getEndTime(newTime.selectedTime);
                    }, function (error) {
                        errorFactory.setErrorFromException(error);
                        vm.userLearnRequests[index].error = errorFactory.getErrorMsg();
                    });
                }
            }
        }

        vm.removeWord = function (index) {
            var word = vm.userWordItems[index];
            if (word) {
                var id = word.id;
                loginUserFactory.removeWord(id).$promise.then(function () {
                    vm.userWordItems.splice(index, 1);
                }, function (error) {
                    vm.userWordItems[index].error = {};
                    vm.userWordItems[index].error['message'] = error.data ? error.data.Message : 'There is something wrong with this operation.';
                });
            }
        }


}]);