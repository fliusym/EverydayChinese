angular.module('learnChineseApp.controller').controller('DefaultController',
    ['wordFactory', '$location', 'errorFactory','timeFactory','scenarioFactory',
        function (wordFactory, $location, errorFactory, timeFactory, scenarioFactory) {
            'use strict';
            var vm = this;
            var getCurrentDayWord = function (date) {
                
                wordFactory.getWord(date).$promise.then(function (word) {
                    vm.error = null;
                    vm.info = {};
                    vm.info['svgname'] = word.Character;
                    vm.info['id'] = word.Id;
                    vm.wordEnglish = word.BasicMeanings;
                    vm.pinyin = word.Pinyin;
                    vm.audioSrc =  word.Audio;
                    if (word.Phrases) {
                        vm.phraselist = word.Phrases.map(function (val) {
                            return {
                                chinese: val.Chinese,
                                english: val.English,
                                audioid: val.Pinyin,
                                examples: val.Examples ? val.Examples.map(function(e){
                                    return {
                                        chinese: e.Chinese,
                                        english: e.English
                                    }
                                }) : []
                            };
                        });
                    }
                    if (word.Slangs) {
                        vm.slang = {
                            titleChinese: '俚语',
                            titleEnglish: 'Slang Words',
                            slangs:  word.Slangs.map(function (value) {
                                return {
                                    english: value.SlangEnglish,
                                    chinese: value.SlangChinese,
                                    exampleEnglish: value.SlangExampleEnglish,
                                    exampleChinese: value.SlangExampleChinese
                                };
                            })
                        };
                    }

                }, function (error) {
                    errorFactory.setErrorFromException(error);
                    vm.error = errorFactory.getErrorMsg();
                    vm.error.persistent = true;
                });
            };
            vm.currentDate = new Date();
            
            getCurrentDayWord(vm.currentDate);

            vm.quotetitle = {
                chinese: '俚语',
                english: 'Slang Words'
            };
            var getCurrentDayScenario = function (date) {
                scenarioFactory.getScenario(vm.currentDate).$promise.then(function (data) {
                    vm.scenarioError = null;
                    vm.scenario = {
                        id: data.Id,
                        scenariotitle: {
                            chinese: '生活场景介绍',
                            english: 'Everyday life scenario'
                        },
                        scenarioquotes: [
                            {
                                chinese: data.ThemeChinese,
                                hasfooter: true,
                                who: data.ThemeEnglish
                            }
                        ],
                        imagelist: data.Images.map(function (val) {
                            return {
                                src: val.Image,
                                words: val.Words.map(function (word) {
                                    return{
                                        chinese: word.Word,
                                        pinyin: word.Pinyin,
                                        audiosrc: word.Audio
                                    }
                                })
                            }
                        })
                    };
                }, function (error) {
                    errorFactory.setErrorFromException(error);
                    vm.scenarioError = errorFactory.getErrorMsg();
                    vm.scenarioError.persistent = true;
                });
            }

            getCurrentDayScenario(vm.currentDate);
            vm.datechange = function (date) {
                vm.currentDate = date;
                getCurrentDayWord(date);
                getCurrentDayScenario(date);
            }
            vm.addScenario = function (id) {
                timeFactory.setCurrentDate(vm.currentDate);
                scenarioFactory.setCurrentScenarioIdToAdd(id);
                $location.path('/addScenario');
            }

        }]);