
/// <reference path="/scripts/jquery-1.4.1-vsdoc.js" />

$(function () {

    infuser.defaults.templateUrl = "templates";

    function MainPageViewModel() {

        var self = this;
        hostName = PRCSettings.HostName;
        self.divisions = ko.observableArray();
        self.updateEnabled = ko.observable(true);
        self.BatchNumber = ko.observable('0');
        self.RunStatus = ko.observable('');
        self.GroupType = ko.observable('');
        self.DivisionBatchLabel = ko.computed(function () { return ' Batch ' + self.BatchNumber() });

        GetBatchNumber();
        UpdateDivisions();
        GetApplicationStatus();
        CheckGroupMembership();


        setInterval(function () {
            if (self.updateEnabled()) {
              UpdateDivisions();
              GetApplicationStatus();
            }
        }, 3000);

        function GetBatchNumber () {
          function MyCallback(result) {
            try {
              self.BatchNumber(result); // = result;
            } catch (e) {
              alert(e.Message);
            }
          }
          GetHelper("GetCurrentBatchNumber", MyCallback);
        };

        function UpdateDivisions() {
            $.ajax({
                type: "GET",
                url: "https://" + hostName + "/PayrollCenterWeb/Services/PRCService.svc/GetDivisionStatusAll",
                contentType: "application/json",
                data: '',
                success: function (result) {
                    self.divisions(result);
                }
          });
        };

        function GetApplicationStatus() {
          function MyCallback(result) {
            try {
              self.RunStatus(result); // = result;
            } catch (e) {
              alert(e.Message);
            }
          }
          GetHelper("GetApplicationRunStatus", MyCallback);
        };

        function CheckGroupMembership() {
          function MyCallback(result) {
            try {
              self.GroupType(result); // = result;
            } catch (e) {
              alert(e.Message);
            }
          }
          GetHelper("CheckGroupMembership", MyCallback);
        };


        $.fn.setUpContextMenu = function () {
            $(this).dialog({
                autoOpen: false,
                modal: true,
                resizable: false,
                width: 'auto',
                height: 'auto',
                minHeight: 'auto',
                minWidth: 'auto'
            });

            return $(this);
        };

        $.fn.openContextMenu = function (jsEvent, menuType) {
            var menu = $(this);
            menu.css('padding', 0);
            $('.divisionMenuItem').hide();

            switch (menuType) {
                case 'Closed':
                    $('#divisionDetail').show();
                    $('#initExtract').show();
                    break;
                case 'Import':
                    $('#divisionDetail').show();
                    $('#restartImport').show();
                    break;
                case 'Validation':
                    $('#divisionDetail').show();
                    $('#recheckValidation').show();
                    break;
                case 'OverrideExtractUnbalance':
                    $('#divisionDetail').show();
                    $('#overrideUnbalancedExtract').show();
                    if (self.GroupType() === 'Admins') {
                      $('#rollBackDivision').show();
                    }
                    break;
                case 'RollBackDivision':
                    $('#divisionDetail').show();
                    if (self.GroupType() === 'Admins') {
                      $('#rollBackDivision').show();
                    }
                    break;
                case 'Normal':
                    $('#divisionDetail').show();
                default:
                    $('#divisionDetail').show();
                    break;
            }


            menu.dialog('option', 'position', [jsEvent.clientX, jsEvent.clientY]);
            menu.unbind('dialogopen');
            menu.bind('dialogopen', function (event, ui) {
                $('.ui-dialog-titlebar').hide();
                $('.ui-widget-overlay').unbind('click');
                $('.ui-widget-overlay').css('opacity', 0);
                $('.ui-widget-overlay').click(function () {
                    menu.dialog('close');
                });
                $('.divisionMenuItem').unbind('click');
                $('.divisionMenuItem').click(function () {
                    menu.dialog('close');
                    //alert(' menu item clicked: ' + this.id + ' ' + jsEvent.currentTarget.parentElement.children[1].innerHTML);
                    callMenuOption(this.id, jsEvent.currentTarget.parentElement.children[1].innerHTML, $.trim(jsEvent.currentTarget.parentElement.children[2].innerText));
                });
            });
            menu.dialog('open');
            return menu;
        };

        $('#context-menu a').css('display', 'block').button();
        $('#context-menu').setUpContextMenu();


        callMenuOption = function (action, divisionCode, actionInfo) {
            switch (action) {
                case "divisionDetail":
                    window.location = "https://" + hostName + "/PayrollCenterWeb/DivisionDetail/DivisionDetail.htm?Division=" + divisionCode;
                    break;

                case "initExtract":
                    if (confirm("About to initialize Extract Process. Are you sure?")) {
                        initializeExtract(divisionCode);
                    };
                    break;

                case "recheckValidation":
                    if (confirm("Do you want to re-check validation?")) {
                        submitRecheckValidationRequest(divisionCode);
                    };
                    break;
                case "overrideUnbalancedExtract":
                    if (confirm("Are you sure you want to override Extract unbalance and continue processing?")) {
                      submitOverrideExtractBalanceRequest(divisionCode);
                    };
                    break;

                case "restartImport":
                    if (confirm("About to restart Import Process. Are you sure?")) {
                        submitRestartImportRequest(divisionCode);
                    };
                    break;

                case "rollBackDivision":
                    var rollBackType;
                    if (actionInfo === 'Extract Started' || actionInfo === 'Archiving PayData') {
                      rollBackType = 'EXTRACT';
                    }
                    else rollBackType = 'STAGING';

                    if (confirm("Are you sure you want to perform an entire " + rollBackType + " Rollback of Division " + divisionCode + "'s payroll?")) {
                      if (confirm("CAUTION: ABOUT TO PERFORM A " + rollBackType + " ROLLBACK OF PAYROLL FOR DIVISION " + divisionCode + ".  ARE YOU SURE?")) {
                        alert('Performing a ' + rollBackType + ' rollback of Division ' + divisionCode + '.');
                        submitRollbackRequest(divisionCode, rollBackType);
                        };
                      };
                    break;
            };
        };


        addContextMenus = function (elements) {
            var divisionName = $.trim($(elements[1].children[0]).text());
            var divisionCode = $.trim($(elements[1].children[1]).text());
            var divisionStatus = $.trim($(elements[1].children[2]).text());
            var conditionStatus = $.trim($(elements[1].children[4]).text());  

            //Add left-click menus
            $(elements[1].children[0]).bind('click', function (e) {

                var menuType;

                if (divisionStatus === "Timekeeping Closed") {

                    if (conditionStatus === 'Validation Error') {
                        menuType = 'Validation';
                    }
                    else {
                        menuType = 'Closed';
                    }
                }
                else if (divisionStatus === "Importing to Staging" && conditionStatus === "Error") {
                    menuType = 'Import';
                }
                else if (divisionStatus === "Paydata Archived" && conditionStatus === "Out of Balance") {
                  menuType = 'OverrideExtractUnbalance';
                }
                else if (divisionStatus === "Extract Started" && conditionStatus === "Extract Error") {
                  menuType = 'RollBackDivision';
                }
                else if (divisionStatus === "Staging Complete" && conditionStatus === "Out of Balance") {
                  menuType = 'RollBackDivision';
                }
                else if (divisionStatus !== "Importing to Staging" && conditionStatus === "Error") {
                  menuType = 'RollBackDivision';
                }
                else {
                    menuType = 'Normal';
                }

                $('#context-menu').openContextMenu(e, menuType);
                return false;
            });
        };


        initializeExtract = function (divisionCode) {
            var request = {
                divisionRequest: {
                    DivisionCode: divisionCode,
                    BatchNumber: '',
                    RequestType: 3,
                    RequestText: 'Initialize Extract',
                    RequesterID: 'PayrollCenter'
                }
            };

            function MyCallback(result) {
                try {
                    SubmitDivisionRequestResult = result.SubmitDivisionRequestResult;
                    if (SubmitDivisionRequestResult.ResultCode === "SUCCEEDED") {
                        alert("Extract request submitted successfully.");
                    }
                    else if (SubmitDivisionRequestResult.ResultCode === "PENDINGREQUEST") {
                        alert("Pending Extract request already submitted.");
                    };
                } catch (e) {
                    alert(e.Message);
                }
            }
            PostHelper("POST", request, "SubmitDivisionRequest", MyCallback);
        };

        submitRollbackRequest = function (divisionCode, rollbackType) {
          var request = {
            divisionRequest: {
              DivisionCode: divisionCode,
              BatchNumber: '',
              RequestType: 7,
              RequestText: rollbackType,
              RequesterID: 'PayrollCenter'
            }
          };

          function MyCallback(result) {
            try {
              SubmitDivisionRequestResult = result.SubmitDivisionRequestResult;
              if (SubmitDivisionRequestResult.ResultCode === "SUCCEEDED") {
                alert("Rollback request submitted successfully.");
              }
              else if (SubmitDivisionRequestResult.ResultCode === "PENDINGREQUEST") {
                alert("Pending Rollback request already submitted.");
              };
            } catch (e) {
              alert(e.Message);
            }
          }
          PostHelper("POST", request, "SubmitDivisionRequest", MyCallback);
        };


        submitRestartImportRequest = function (divisionCode) {
            var request = {
                divisionRequest: {
                    DivisionCode: divisionCode,
                    BatchNumber: '',
                    RequestType: 4,
                    RequestText: 'Restart Import',
                    RequesterID: 'PayrollCenter'
                }
            };

            function MyCallback(result) {
                try {
                    SubmitDivisionRequestResult = result.SubmitDivisionRequestResult;
                    if (SubmitDivisionRequestResult.ResultCode === "SUCCEEDED") {
                        alert("Restart request submitted successfully.");
                    }
                    else if (SubmitDivisionRequestResult.ResultCode === "PENDINGREQUEST") {
                        alert("Pending Restart request already submitted.");
                    };
                } catch (e) {
                    alert(e.Message);
                }
            }
            PostHelper("POST", request, "SubmitDivisionRequest", MyCallback);
        };


        submitRecheckValidationRequest = function (divisionCode) {
            var request = {
                divisionRequest: {
                    DivisionCode: divisionCode,
                    BatchNumber: '',
                    RequestType: 5,
                    RequestText: 'Recheck Validation',
                    RequesterID: 'PayrollCenter'
                }
            };

            function MyCallback(result) {
                try {
                    SubmitDivisionRequestResult = result.SubmitDivisionRequestResult;
                    if (SubmitDivisionRequestResult.ResultCode === "SUCCEEDED") {
                        alert("Validation request submitted successfully.");
                    }
                    else if (SubmitDivisionRequestResult.ResultCode === "PENDINGREQUEST") {
                        alert("Pending validation request already submitted.");
                    };
                } catch (e) {
                    alert(e.Message);
                }
            }
            PostHelper("POST", request, "SubmitDivisionRequest", MyCallback);
        };

        submitOverrideExtractBalanceRequest = function (divisionCode) {
          var request = {
            divisionRequest: {
              DivisionCode: divisionCode,
              BatchNumber: '',
              RequestType: 6,
              RequestText: 'Override Unbalanced Extract',
              RequesterID: 'PayrollCenter'
            }
          };

          function MyCallback(result) {
            try {
              SubmitDivisionRequestResult = result.SubmitDivisionRequestResult;
              if (SubmitDivisionRequestResult.ResultCode === "SUCCEEDED") {
                alert("Override request submitted successfully.");
              }
            } catch (e) {
              alert(e.Message);
            }
          }
          PostHelper("POST", request, "SubmitDivisionRequest", MyCallback);
        };

    }; // End of MainPageViewModel


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


    ko.applyBindings(new MainPageViewModel());

});
