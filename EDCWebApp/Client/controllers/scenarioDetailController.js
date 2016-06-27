'use strict';
/**
*@name ScenarioDetailController
*@description
* show the detail of selected scenario
*/
angular.module('learnChineseApp.controller').controller('ScenarioDetailController',
    ['$routeParams','loginUserFactory',
        function ($routeParams, loginUserFactory) {
            
            var vm = this;
            var index = $routeParams.index;
            var scenarios = loginUserFactory.getCachedScenarios();
            if (scenarios) {
                var data = scenarios[index];
                vm.scenario = {
                    id: data.Id,
                    notShowAdd: true,
                    scenarioquotes: [
                        {
                            chinese: data.themeChinese,
                            hasfooter: true,
                            who: data.themeEnglish
                        }
                    ],
                    imagelist: data.images.map(function (val) {
                        return {
                            src: val.imgSrc,
                            words: val.words.map(function (word) {
                                return {
                                    chinese: word.chinese,
                                    pinyin: word.pinyin,
                                    audiosrc: word.audio
                                }
                            })
                        }
                    })
                };
            }
        }]);