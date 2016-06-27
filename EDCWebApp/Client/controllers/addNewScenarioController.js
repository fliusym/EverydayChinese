'use strict';
/**
*@name AddNewScenarioController
*@description:
*AddNewScenarioController is used for adding new scenario from teacher
*/
angular.module('learnChineseApp.controller').controller('AddNewScenarioController', [
    'loginUserFactory', 'errorFactory','$location',
    function (loginUserFactory, errorFactory, $location) {
        
        var vm = this;
        vm.scenario = {};
        vm.onAdd = function () {
            var addObj = {};
            addObj['titleChinese'] = vm.scenario.titleChinese;
            addObj['titleEnglish'] = vm.scenario.titleEnglish;
            addObj['date'] = vm.scenario.scenarioDate;
            addObj['images'] = vm.scenario.images ? vm.scenario.images.map(function (val) {
                return {
                    imageSrc: val.image.src,
                    words: val.image.words.map(function (word) {
                        return {
                            wordChinese: word.word.chinese,
                            wordPinyin: word.word.pinyin,
                            wordAudio: word.word.audio
                        }
                    })
                };
            }) : null;

            loginUserFactory.addNewScenario(addObj).$promise.then(function (data) {
                vm.error = null;
                $location.path('/addNewScenario');
            }, function (error) {
                errorFactory.setErrorFromException(error);
                vm.error = angular.copy(errorFactory.getErrorMsg());
                vm.error.persistent = true;
            });
        }
    }]);