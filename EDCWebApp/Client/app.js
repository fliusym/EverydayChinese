angular.module('learnChineseApp', [
    'ngRoute',
    'ui.bootstrap',
    'learnChineseApp.controller',
    'learnChineseApp.directive',
    'learnChineseApp.filter',
    'learnChineseApp.service',
    'ngSanitize',
    'ngMessages'
])
.value('tokenKey', 'accessToken')
.value('userInfo', 'userInfo')
.config(function ($routeProvider) {
    var viewBase = '/Client/views/';
    $routeProvider.when('/register', {
        templateUrl: viewBase + 'register.html',
        controller: 'RegisterController',
        controllerAs: 'registerCtrl'
    }).when('/login', {
        templateUrl: viewBase + 'login.html',
        controller: 'LoginController',
        controllerAs: 'loginCtrl'
    }).when('/confirmEmail', {
        templateUrl: viewBase + 'confirmEmail.html',
        controller: 'ConfirmEmailController',
        controllerAs: 'confirmEmailCtrl'
    }).when('/logout', {
        resolve: {
            logout: ['authenticationFactory', function (authenticationFactory) {
                authenticationFactory.logout();
            }]
        }
    }).when('/forgot_password', {
        templateUrl: viewBase + 'forgotpwd.html',
        controller: 'ForgotPasswordController',
        controllerAs: 'forgotPasswordCtrl'
    }).when('/forgot_password_confirmed', {
        templateUrl: viewBase + 'forgotpwdconfirmed.html',

    }).when('/resetpassword', {
        templateUrl: viewBase + 'resetpwd.html',
        controller: 'ResetPasswordController',
        controllerAs: 'resetPasswordCtrl'
    }).when('/user', {
        templateUrl: viewBase + 'student.html',
        controller: 'StudentController',
        controllerAs: 'loginUserCtrl'
    }).when('/teacher', {
        templateUrl: viewBase + 'teacher.html',
        controller: 'TeacherController',
        controllerAs: 'loginTeacherCtrl'
    }).when('/teacherLearnRequest', {
        templateUrl: viewBase + 'teacherLearnRequest.html',
        controller: 'TeacherLearnRequestController',
        controllerAs: 'teacherLRCtrl'
    }).when('/studentLearnRequest', {
        templateUrl: viewBase + 'studentLearnRequest.html',
        controller: 'StudentLearnRequestController',
        controllerAs: 'studentLRCtrl'
    }).when('/addLearnRequest', {
        templateUrl: viewBase + 'addLearnRequest.html',
        controller: 'AddLearnRequestController',
        controllerAs: 'addLRCtrl'
    }).when('/addWord', {
        resolve: {
            addWord: ['loginUserFactory', 'timeFactory','wordFactory', function (loginUserFactory,
                timeFactory,  wordFactory) {
                //   var date = timeFactory.getCurrentDate();
                //var user = authenticationFactory.getLoginInfo().user;
                var id = wordFactory.getCurrentWordIdToAdd();
                loginUserFactory.addWord(id);
            }]
        }
    }).when('/addNewWord', {
        templateUrl: viewBase + 'addNewWord.html',
        controller: 'AddNewWordController',
        controllerAs: 'addNWCtrl'
    }).when('/default', {
        templateUrl: viewBase + 'default.html',
        controller: 'DefaultController',
        controllerAs: 'defaultCtrl'
    }).otherwise({
        redirectTo: '/default'
    });

})
.run(['authenticationFactory', '$rootScope', '$location', function (authenticationFactory, $rootScope, $location) {
    $rootScope.$on('$routeChangeStart', function (event, next, current) {
        var logged = authenticationFactory.getLoginInfo();
        if (!logged || !logged.flag) {
            if (next.templateUrl) {
                if (next.templateUrl === '/Client/views/addLearnRequest.html') {
                    event.preventDefault();
                    var nextUrl = next.$$route.originalPath;
                    authenticationFactory.setRedirectUrlAfterLogin('/addLearnRequest');
                    $location.path('/login');
                }
            } else if (next.$$route) {
                if (next.$$route.originalPath === '/addWord') {
                    event.preventDefault();
                    authenticationFactory.setRedirectUrlAfterLogin('/addWord');
                    $location.path('/login');
                }

            }
        }
    });

}]);
