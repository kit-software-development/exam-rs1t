// const serverUrl = "https://crypto-wallet.azurewebsites.net/api/";
const serverUrl = "https://localhost:5001/api";

angular.module('index', [])
    .controller('MenuController', function ($scope) {
        $scope.selectedMenuItem = "";

        $scope.setSelectedMenuItem = function (menuItem) {
            $scope.selectedMenuItem = menuItem;
            localStorage.setItem('menuItem', menuItem);
        };

        $scope.getSelectedMenuItem = function () {
            $scope.selectedMenuItem = localStorage.getItem('menuItem');
            if (!$scope.selectedMenuItem)
                $scope.setSelectedMenuItem('auth');
        }
    })
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
        $scope.transactionStatus = "";
        $scope.fee = 0.0001;

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
            ;
        };

        $scope.send = function () {
            $scope.transactionStatus = "Loading...";
            $http.post(`${serverUrl}/transactions`, JSON.stringify(
                {
                    fromAddress: $scope.selectedAddress,
                    toAddress: $scope.toAddress,
                    amount: $scope.amount,
                    fee: $scope.fee,
                }), {withCredentials: true})
                .then(response => {
                    alert(response.data);
                    $scope.transactionStatus = "";
                })
                .catch(reason => {
                    alert(reason.status + " " + reason.statusText);
                    $scope.transactionStatus = "";
                });
        }
    })

    .controller('HistoryController', function ($scope, $http) {
        $scope.historyStatus = "";
        $scope.history = {};

        $scope.getTransactionHistory = function () {
            $scope.historyStatus = "Loading...";
            $http.get(`${serverUrl}/transactions`)
                .then(response => {
                    $scope.history = response.data.transactionHistory;
                    if (Object.entries($scope.history).length === 0 && $scope.history.constructor === Object)
                        $scope.historyStatus = "No history yet!";
                    else
                        $scope.historyStatus = "";
                })
                .catch(response => {
                    $scope.historyStatus = "An error occured. Please try again";
                });
        };
    })

    .controller('AuthController', function ($scope, $http) {
        $scope.isUserAuthorized = false;
        $scope.pageType = "login";

        $scope.email = "";
        $scope.password = "";
        $scope.emailRegister = "";
        $scope.passwordRegister = "";
        $scope.confirmPassword = "";

        $scope.userEmail = "";

        $scope.updateIsUserAuthorized = function () {
            $scope.isUserAuthorized = document.cookie.indexOf('.AspNetCore.Cookies=') !== -1;
            $scope.userEmail = localStorage.getItem('email');
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
                    alert("Login is successful!");
                    localStorage.setItem('email', $scope.email);
                    location.reload();
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
                    localStorage.removeItem('email');
                    location.reload();
                })
        };
    });

