'use strict';
/**
*@name DefaultController
*@description
*default controller used for getting current day's word and scenario
*/
angular.module('learnChineseApp.controller').controller('DefaultController',
    ['wordFactory', '$location', 'errorFactory','timeFactory','scenarioFactory',
        function (wordFactory, $location, errorFactory, timeFactory, scenarioFactory) {
            
            var vm = this;
            //get current day scenario
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
                                    return {
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
                    vm.scenarioError = angular.copy(errorFactory.getErrorMsg());
                    vm.scenarioError.persistent = true;
                });
            }
            //get current day word and scenario. Here to get the word first, then get the scenario
            var getCurrentDay = function (date) {
                
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
                    getCurrentDayScenario(date);
                }, function (error) {
                    errorFactory.setErrorFromException(error);
                    vm.error = angular.copy(errorFactory.getErrorMsg());
                    vm.error.persistent = true;
                    getCurrentDayScenario(date);
                });
            };
            //by default the current day 
            vm.currentDate = new Date();
            
            getCurrentDay(vm.currentDate);

            vm.quotetitle = {
                chinese: '俚语',
                english: 'Slang Words'
            };
  
            //if date changed, then call getCurrentDay
            vm.datechange = function (date) {
                vm.currentDate = date;
                getCurrentDay(date);
            }
            //If add scenario, need to ask user to log in first
            vm.addScenario = function (id) {
                timeFactory.setCurrentDate(vm.currentDate);
                scenarioFactory.setCurrentScenarioIdToAdd(id);
                $location.path('/addScenario');
            }
            //if add word, need to ask user to log in first
            vm.add = function (id) {
                timeFactory.setCurrentDate(vm.currentDate);
                wordFactory.setCurrentWordIdToAdd(id);
                $location.path('/addWord');
            }
        }]);