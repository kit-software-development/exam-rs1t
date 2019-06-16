// const serverUrl = "https://crypto-wallet.azurewebsites.net/api/";
const serverUrl = "https://localhost:5001/api";

angular.module('index', [])
    .controller('WalletsController', function ($scope, $http) {
        $scope.walletsStatus = "";
        $scope.wallets = {};

        $scope.getWalletBalances = function () {
            $scope.walletsStatus = "Loading...";
            $http.get(`${serverUrl}/wallets/balances`)
                .then(response => {
                    $scope.wallets = response.data.balances;
                    if (Object.entries($scope.wallets).length === 0 && $scope.wallets.constructor === Object)
                        $scope.walletsStatus = "No wallets found 😔";
                    else
                        $scope.walletsStatus = "";
                })
                .catch(response => {
                    $scope.walletsStatus = "An error occured. Please try again";
                });
        };
        $scope.createWallet = function () {
            $http.post(`${serverUrl}/wallets`)
                .then(response => {
                    $scope.getWalletBalances();
                })
        };
    })

    .controller('SendController', function ($scope, $http) {
        $scope.wallets = {};
        $scope.walletsStatus = "";

        $scope.getWalletBalances = function () {
            $scope.walletsStatus = "Loading...";
            $http.get(`${serverUrl}/wallets/balances`)
                .then(response => {
                    $scope.wallets = response.data.balances;
                    if (Object.entries($scope.wallets).length === 0 && $scope.wallets.constructor === Object)
                        $scope.walletsStatus = "No wallets found 😔";
                    else
                        $scope.walletsStatus = "";
                })
                .catch(response => {
                    $scope.walletsStatus = "An error occured. Please try again";
                });;
        };

        $scope.send = function () {
            console.log($scope.selectedAddress);
            console.log($scope.toAddress);
            console.log($scope.amount);
            console.log($scope.fee);
        }
    })

    .controller('HistoryController', function ($scope, $http) {

    })

    .controller('AuthController', function ($scope, $http) {
        $scope.isUserAuthorized = false;
        $scope.pageType = "login";

        $scope.email = "alice@mailinator.com";
        $scope.password = "Qweqwe_1";
        $scope.emailRegister = "";
        $scope.passwordRegister = "";
        $scope.confirmPassword = "";

        $scope.updateIsUserAuthorized = function () {
            $scope.isUserAuthorized = document.cookie.indexOf('.AspNetCore.Cookies=') !== -1;
        };


        $scope.togglePageType = function () {
            if ($scope.pageType === "login")
                $scope.pageType = "register";
            else
                $scope.pageType = "login";
        };

        $scope.signIn = function () {
            $http.post(`${serverUrl}/auth/sign-in`, JSON.stringify(
                {
                    email: $scope.email,
                    password: $scope.password
                }), {withCredentials: true})
                .then(response => {
                    $scope.updateIsUserAuthorized();
                    alert("Login is successful!")
                })
                .catch(reason => alert(reason.status + " " + reason.statusText));
        };

        $scope.signUp = function () {
            console.log($scope.emailRegister);
            console.log($scope.passwordRegister);
            console.log($scope.confirmPassword);
            if ($scope.passwordRegister !== $scope.confirmPassword) {
                alert("Passwords are not equal");
                return;
            }
            $http.post(`${serverUrl}/auth/sign-up`, JSON.stringify(
                {
                    email: $scope.emailRegister,
                    password: $scope.passwordRegister
                }), {withCredentials: true})
                .then(response => {
                    $scope.updateIsUserAuthorized();
                    alert("Registration is successful!")
                })
                .catch(reason => alert(reason.status + " " + reason.statusText));
        };

        $scope.signOut = function () {
            $http.post(`${serverUrl}/auth/sign-out`)
                .then(response => {
                    $scope.updateIsUserAuthorized();
                })
        };
    });

