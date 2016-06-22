angular.module('learnChineseApp.controller')
.controller('StudentController', ['$routeParams', 'loginUserFactory', '$location',
    '$filter', 'errorFactory', 'timeFactory','$scope',
    function ($routeParams, loginUserFactory, $location, $filter, errorFactory, timeFactory,$scope) {
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
                vm.userWordItems = words.map(function (word) {
                    return {
                        id: word.Id,
                        pinyin: word.Pinyin,
                        chinese: word.Character,
                        audiosrc: word.Audio,
                        basicMeaning: word.BasicMeanings,
                        quotes: {
                            slangs: word.Slangs.map(function (value) {
                                return {
                                    english: value.SlangEnglish,
                                    chinese: value.SlangChinese,
                                    exampleEnglish: value.SlangExampleEnglish,
                                    exampleChinese: value.SlangExampleChinese
                                }

                            })
                        },
                        date: word.Date,
                        phrases: word.Phrases.map(function (p) {
                            return {
                                chinese: p.Chinese,
                                english: p.English,
                                audioid: '/' + p.Pinyin,
                                examples: p.Examples.map(function (e) {
                                    return {
                                        chinese: e.Chinese,
                                        english: e.English
                                    }
                                })
                            }
                        })
                    }
                });



                //learn requests
                var learnRequests = data.LearnRequests;
                vm.userLearnRequests = learnRequests.map(function (request) {
                    return {
                        id: request.Id,
                        date: request.Date,
                        startTime: request.StartTime,
                        endTime: request.EndTime,
                        startToEndTime: request.StartTime + ' - ' + request.EndTime
                    }
                });
           //     loginUserFactory.setCachedStudentLearnRequests(vm.userLearnRequests);

                //scenarios
                var scenarios = data.Scenarios;
                vm.userScenarios = scenarios.map(function (scenario) {
                    return {
                        id: scenario.Id,
                        themeChinese: scenario.ThemeChinese,
                        themeEnglish: scenario.ThemeEnglish,
                        images: scenario.Images.map(function (image) {
                            return {
                                imgSrc: image.Image,
                                words: image.Words.map(function (word) {
                                    return {
                                        chinese: word.Word,
                                        pinyin: word.Pinyin,
                                        audio: word.Audio
                                    }
                                })
                            }
                        })
                    }
                });
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
        vm.editLearnRequest = function (index, newTime) {

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
                loginUserFactory.removeWord(userName, id).$promise.then(function () {
                    vm.userWordItems.splice(index, 1);
                }, function (error) {
                    vm.userWordItems[index].error = {};
                    vm.userWordItems[index].error['message'] = error.data ? error.data.Message : 'There is something wrong with this operation.';
                });
            }
        }

        vm.removeScenario = function (index) {
            var scenario = vm.userScenarios[index];
            if (scenario) {
                var id = scenario.id;
                loginUserFactory.removeScenario(userName, id).$promise.then(function () {
                    vm.userScenarios.splice(index, 1);
                }, function (error) {
                    vm.userScenarios[index].error = {};
                    vm.userScenarios[index].error['message'] = error.data ? error.data.Message : "There is something wrong with this operation.";
                });
            }
        }

        vm.getScenarioDetail = function (index) {
            var scenario = vm.userScenarios[index];
            if (scenario) {
                $location.path('/scenarioDetail').search({ index: index });
            } else {
                vm.userScenarios[index].error = {};
                vm.userScenarios[index].error['message'] = "There is something wrong.";
            }
        }
        $scope.$watch(angular.bind(vm,function(){
            return vm.userScenarios;
        }), function (newVal) {
            loginUserFactory.setCachedScenarios(newVal);
        });
        $scope.$watch(angular.bind(vm, function () {
            return vm.userLearnRequests;
        }), function (newVal) {
            loginUserFactory.setCachedStudentLearnRequests(newVal);
        });
        $scope.$on('logout', function (msg) {
            loginUserFactory.setCachedScenarios(null);
            loginUserFactory.setCachedStudentLearnRequests(null);
        });
    }]);