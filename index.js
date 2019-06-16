const serverUrl = "https://crypto-wallet.azurewebsites.net/api/";

angular.module('index', [])
    .controller('WalletsController', function ($scope, $http) {
        $scope.walletsStatus = "";

        $scope.getWalletBalances = function () {
            $http.get(`${serverUrl}/wallets/balances`)
                .then(response => {
                    $scope.walletsStatus = "Loading...";
                    $scope.cryptocurrencySymbols = response.data;
                    $scope.walletsStatus = "";
                })
                .catch(response => {
                    $scope.walletsStatus = "No wallets found 😕😔";
                });
        }
    })

    .controller('SendController', function ($scope, $http) {

    })

    .controller('HistoryController', function ($scope, $http) {

    })

    .controller('AuthController', function ($scope, $http) {

    });

