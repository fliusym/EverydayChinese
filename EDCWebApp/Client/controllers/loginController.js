angular.module('learnChineseApp.controller')
.controller('RegisterController', ['$location', 'authenticationFactory', 'errorFactory', function ($location, authenticationFactory,errorFactory) {
    'use strict';
    var vm = this;
    vm.onRegister = function () {
        var valid = vm.registerForm.$valid;
        if (valid) {
            var data = {
                email: vm.emailaddress,
                password: vm.password,
                confirmPassword: vm.confirmpassword
            };
            authenticationFactory.register(data);
            vm.error = errorFactory.getErrorMsg();
        }

    };

    vm.onCancel = function () {
        $location.path('/default');
    };
    

}]).controller('LoginController', ['$scope', '$location', 'authenticationFactory', function ($scope, $location, authenticationFactory) {
    'use strict';

    var vm = this;

    vm.hasErrorFlag = false;

    vm.onLogin = function () {
        var valid = vm.loginForm.$valid;
        if (valid) {
            var data = {
                email: vm.emailaddress,
                password: vm.password
            };
            authenticationFactory.login(data);
        }

    };
    vm.onCancelLogin = function () {
        $location.path('/default');
    };
    $scope.$on('loginFail', function (event, args) {
        vm.emailaddress = '';
        vm.password = '';
        vm.hasErrorFlag = true;
        vm.loginfailmsg = args.data.error_description;
    });

    $scope.$on('loginSuccess', function (event, data) {
        vm.hasErrorFlag = false;
        //$location.path('/default');
    });

}]).controller('ConfirmEmailController', ['$routeParams', function ($routeParams) {
    'use strict';
    var vm = this;
    this.emailAddress = $routeParams.userEmail;
}]).controller('ResetPasswordController', ['authenticationFactory', '$routeParams', function (authenticationFactory, $routeParams) {
    'use strict';
    var vm = this;
    vm.useremail = $routeParams.userEmail;
    vm.onSubmit = function () {
        authenticationFactory.resetPassword($routeParams.userEmail, $routeParams.code, vm.password, vm.confirmpassword);
    }
}]).controller('ForgotPasswordController', ['authenticationFactory', '$location', function (authenticationFactory, $location) {
    'use strict';
    var vm = this;
    vm.onForgot = function () {
        var email = vm.emailaddress;
        authenticationFactory.forgotPassword(email);
    }
    vm.onForgotCancel = function () {
        $location.path('/');
    }
}]);