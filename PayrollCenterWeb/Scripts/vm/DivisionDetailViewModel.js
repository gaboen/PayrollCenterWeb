
/// <reference path="/scripts/jquery-1.4.1-vsdoc.js" />
/// <reference path="/Scripts/Common/Server.js" />

$(function () {

    infuser.defaults.templateUrl = "templates";

    function DivisionDetailViewModel() {

        var self = this;
        hostName = PRCSettings.HostName;
        self.division = ko.observable();
        self.ProcessText = ko.observable();
        self.DivisionName = ko.observable();
        self.DivisionCode = '1';
        self.pittsburghStatus = ko.observable();
        self.BatchNumber = ko.observable('0');
        self.DivisionNameLabel = ko.computed(function () { return 'Division: ' + self.DivisionName() });
        self.DivisionBatchLabel = ko.computed(function () { return ' Batch ' + self.BatchNumber() });
        self.updateEnabled = ko.observable(true);
        self.ProcessText(self.division.ProcessText);
        self.UnbalancedExtract = ko.observable();
        self.FailedValidation = ko.observable();
        self.DivisionPaused = ko.computed(function () {
            var retVal = {
                value: 'false',
                text: '',
                buttonText: ''
            };

            try {
                retVal.text = self.division().PauseDivision == 'T' ? 'Division Paused' : '';
                retVal.value = self.division().PauseDivision == 'T' ? true : false;
                retVal.buttonText = self.division().PauseDivision == 'T' ? 'Un-Pause Division Processing' : 'Pause Division Processing';
            } catch (e) {

            }
            return retVal;
        });

        self.ExtractPaused = ko.computed(function () {
            var retVal = {
                value: 'false',
                text: ''
            };

            try {
                retVal.text = self.division().PauseExtract == 'T' ? 'Extract Paused' : '';
                retVal.value = self.division().PauseExtract == 'T' ? true : false;
                retVal.buttonText = self.division().PauseExtract == 'T' ? 'Un-Pause Extract' : 'Pause Extract';
            } catch (e) {

            }
            return retVal;
        });

        self.ImportPaused = ko.computed(function () {
            var retVal = {
                value: 'false',
                text: ''
            };

            try {
                retVal.text = self.division().PauseImport == 'T' ? 'Import Paused' : '';
                retVal.value = self.division().PauseImport == 'T' ? true : false;
                retVal.buttonText = self.division().PauseImport == 'T' ? 'Un-Pause Staging Import' : 'Pause Staging Import';
            } catch (e) {

            }
            return retVal;
        });

        GetBatchNumber = function () {
            function MyCallback(result) {
                try {
                    self.BatchNumber(result); // = result;
                } catch (e) {
                    alert(e.Message);
                }
            }
            GetHelper("GetCurrentBatchNumber", MyCallback);
        };




        Initialize = function () {
            var queries = {};
            $.each(document.location.search.substr(1).split('&'), function (c, q) {
                try {
                    var i = q.split('=');
                    queries[i[0].toString()] = i[1].toString();
                    self.DivisionCode = queries.Division;
                } catch (e) {
                    alert(e.Message);
                }
            });
        };

      // *********************************************************************

      // Pittsburg handling....

        var GetPittsburgStatusResult = {
          PITStatus: '',
          PITStatusColor: '',
          JOHStatus: '',
          JOHStatusColor: '',
          ERIStatus: '',
          ERIStatusColor: '',
        };

        self.pittsburghStatus(GetPittsburgStatusResult);

        FormatForPittsburgDivision = function () {

          $.ajax({
            type: "GET",
            url: "https://" + hostName + "/PayrollCenterWeb/Services/PRCService.svc/GetPittsburgStatus",
            contentType: "application/json",
            success: function (result) {
              
              try {
                GetPittsburgStatusResult = result.GetPittsburgStatusResult;
              } catch (e) {
                alert(e.Message);
              }

              if (GetPittsburgStatusResult.PITStatus === 'Open') {
                GetPittsburgStatusResult.PITStatusColor = '#BDB76B';
              }
              else {
                GetPittsburgStatusResult.PITStatusColor = '#FCB03D';
              }

              if (GetPittsburgStatusResult.JOHStatus === 'Open') {
                GetPittsburgStatusResult.JOHStatusColor = '#BDB76B';
              }
              else {
                GetPittsburgStatusResult.JOHStatusColor = '#FCB03D';
              }

              if (GetPittsburgStatusResult.ERIStatus === 'Open') {
                GetPittsburgStatusResult.ERIStatusColor = '#BDB76B';
              }
              else {
                GetPittsburgStatusResult.ERIStatusColor = '#FCB03D';
              }
              self.pittsburghStatus(GetPittsburgStatusResult);
            }
          });

        };

      // *********************************************************************


        Initialize();
        FormatForPittsburgDivision();
        GetBatchNumber();
        UpdateStatus();

        var GetDivisionDetailStatusResult = {
            ActionStatus: '',
            ActionsStatusColor: '',
            DivisionCode: '',
            DivisionName: '',
            DivisionStatus: '',
            DivisionStatusColor: '',
            ErrorStatus: '',
            ErrorStatusColor: '',
            RequestCode: '',
            RequestID: '',
            StatusDetailText: '',
            PauseDivision: '',
            PauseExtract: '',
            PauseImport: ''
        }

        var SubmitDivisionRequestResult = {
            ResultCode: '',
            RequestID: 0
        }


        self.division(GetDivisionDetailStatusResult);

        setInterval(function () {
            if (self.updateEnabled()) {
                UpdateStatus();
            }
        }, 3000);


        function setButtonStatus() {

          self.UnbalancedExtract(false);
          self.FailedValidation(false);

          switch (self.division().ErrorStatus) {
            case "Out of Balance":
              self.UnbalancedExtract(true);
              break;
            case "Validation Error":
              self.FailedValidation(true);
              break;
          }

        };

        function UpdateStatus() {
            var divisionReq = {
                divisionCode: self.DivisionCode
            };

            var jsonReq = JSON.stringify(divisionReq);

            $.ajax({
                type: "POST",
                url: "https://" + hostName + "/PayrollCenterWeb/Services/PRCService.svc/GetDivisionDetailStatus",
                contentType: "application/json",
                data: jsonReq,
                success: function (result) {
                    self.division(result.GetDivisionDetailStatusResult);
                    self.DivisionName(self.division().DivisionName);
                    setButtonStatus();
                    try {
                        GetDivisionDetailStatusResult = result.GetDivisionDetailStatusResult;
                    } catch (e) {
                        alert(e.Message);
                    }

                }
            });
        };


        SubmitJob1 = function () {
            var request = {
                divisionRequest: {
                    DivisionCode: self.DivisionCode,
                    BatchNumber: self.BatchNumber(),
                    RequestType: 3,  // GeneralRequest
                    RequestText: 'This is now a new test of the general request',
                    RequesterID: 'TestJob_TESTER'
                }
            };

            function MyCallback(result) {
                try {
                    SubmitDivisionRequestResult = result.SubmitDivisionRequestResult;
                    alert(SubmitDivisionRequestResult.ResultCode);

                } catch (e) {
                    alert(e.Message);
                }
            }
            PostHelper("POST", request, "SubmitDivisionRequest", MyCallback);
        };

        SubmitDivisionPause = function () {
            var submitValue
            if (self.DivisionPaused().value == false) {
                submitValue = 'T'
            }
            else {
                submitValue = 'F'
            }

            PauseDivisionRequest(0, submitValue);
        };

        PauseExtractRequest = function () {
            var submitValue
            if (self.ExtractPaused().value == false) {
                submitValue = 'T'
            }
            else {
                submitValue = 'F'
            }

            PauseDivisionRequest(1, submitValue);
        };

        PauseStagingImport = function () {
            var submitValue
            if (self.ImportPaused().value == false) {
                submitValue = 'T'
            }
            else {
                submitValue = 'F'
            }

            PauseDivisionRequest(2, submitValue);
        };

        CompareAB = function () {
            $('#ComparisonFrame').attr('src', 'PayDataComparison.htm?Division=' + self.DivisionCode + '&BatchNumber=' + self.BatchNumber() + '&Comp=extract');
            $('#PayDataComparisonDiv').slideDown(500);
        };

        HideComparisonDiv = function () {
            $('#PayDataComparisonDiv').slideUp(500);
        };

        CompareAC = function () {
            $('#ComparisonFrame').attr('src', 'PayDataComparison.htm?Division=' + self.DivisionCode + '&BatchNumber=' + self.BatchNumber() + '&Comp=staging');
            $('#PayDataComparisonDiv').slideDown(500);
        };

        DisplayValidation = function () {
            PRCSettings.CurrentDivisionName = self.DivisionName();
            window.location = "https://" + hostName + "/PayrollCenterWeb/DivisionDetail/PreImportValidation.htm?Division=" + self.DivisionCode;
        }

        DisplayLineByLineComparison = function () {
          PRCSettings.CurrentDivisionName = self.DivisionName();
          window.location = "https://" + hostName + "/PayrollCenterWeb/DivisionDetail/LineByLineComparison.htm?Division=" + self.DivisionCode;
        }

        ReturnToMainView = function () {
            window.location = "https://" + hostName + "/PayrollCenterWeb/MainPage.htm";
        }


        PauseDivisionRequest = function (requestType, value) {
            var request = {
                divisionRequest: {
                    DivisionCode: self.DivisionCode,
                    BatchNumber: self.BatchNumber(),
                    RequestType: requestType,
                    RequestText: value,
                    RequesterID: 'Pause_TESTER'
                }
            };

            function MyCallback(result) {
                try {
                    SubmitDivisionRequestResult = result.SubmitDivisionRequestResult;
                } catch (e) {
                    alert(e.Message);
                }
            }
            PostHelper("POST", request, "SubmitDivisionRequest", MyCallback);
        };

        $('#PayDataComparisonDiv').hide();

    }; // End of DivisionDetailViewModel


    function PostHelper(transactionType, data, apiName, callback) {

        var jsonReq = JSON.stringify(data);

        $.ajax({
            type: transactionType,
            url: "https://" + hostName + "/PayrollCenterWeb/Services/PRCService.svc/" + apiName,
            contentType: "application/json",
            data: jsonReq,
            success: callback
        });

    };

    function GetHelper(apiName, callback) {

        //var jsonReq = JSON.stringify(data);

        $.ajax({
            type: "GET",
            url: "https://" + hostName + "/PayrollCenterWeb/Services/PRCService.svc/" + apiName,
            contentType: "application/json",
            data: null,
            success: callback
        });

    };

    ko.applyBindings(new DivisionDetailViewModel());

});
