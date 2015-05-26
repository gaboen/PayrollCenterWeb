
/// <reference path="/scripts/jquery-1.4.1-vsdoc.js" />
/// <reference path="/Scripts/Common/Server.js" />

$(function () {

    infuser.defaults.templateUrl = "templates";

    function PayDataComparisonViewModel() {

        var self = this;
        hostName = PRCSettings.HostName;
        self.division = ko.observable();
        self.ProcessText = ko.observable();
        self.DivisionName = 'Akron';
        self.updateEnabled = ko.observable(true);
        self.ProcessText(self.division.ProcessText);
        self.BatchNumber = '0';


        var SubmitDivisionRequestResult = {
            ResultCode: '',
            RequestID: 0
        };

        var queries = {};

        $.each(document.location.search.substr(1).split('&'), function (c, q) {
            try {
                var i = q.split('=');
                queries[i[0].toString()] = i[1].toString();
                self.DivisionCode = queries.Division;
                self.BatchNumber = queries.BatchNumber;

            } catch (e) {

            }


        });

        var payData = {
            ProVisitGrossPay: '',
            ProExpenseGrossPay: '',
            ProVisitTotalBilling: '',
            ProExpenseTotalBilling: '',

            ExtractVisitGrossPay: '',
            ExtractExpenseGrossPay: '',
            ExtractVisitTotalBilling: '',
            ExtractExpenseTotalBilling: ''

        };

        var stagingTotals = {
            GrossPay: '',
            TravelPay: '',
            ContractorServices: '',
            TotalGrossPay: '',
            TotalBilling: '',
            ProcuraPayTotal: '',
            ProcuraBillTotal: ''
        };

        self.PayData = ko.observable(payData);
        self.StagingTotals = ko.observable(stagingTotals);

        self.PayDataTotals = ko.computed(function () {

            var tpd = (self.PayData().ExtractVisitGrossPay + self.PayData().ExtractExpenseGrossPay) -
                                        (self.PayData().ProVisitGrossPay + self.PayData().ProExpenseGrossPay);
            var tbd = (self.PayData().ExtractVisitTotalBilling + self.PayData().ExtractExpenseTotalBilling) -
                                            (self.PayData().ProVisitTotalBilling + self.PayData().ProExpenseTotalBilling);

//            var proTotalPay = parseFloat(self.PayData().ProVisitGrossPay + self.PayData().ProExpenseGrossPay);

//            var proTotalBilling = 0 + (self.PayData().ProVisitTotalBilling + self.PayData().ProExpenseTotalBilling);

//            var extractTotalPay = 0 + (self.PayData().ExtractVisitGrossPay + self.PayData().ExtractExpenseGrossPay);

//            var extractTotalBilling = 0 + (self.PayData().ExtractVisitTotalBilling + self.PayData().ExtractExpenseTotalBilling);

            //accounting.formatMoney(self.PayDataTotals().ProTotalBilling, { format: "%v" })
            var retVal = {

              ProVisitGrossPay: accounting.formatMoney(self.PayData().ProVisitGrossPay, { format: "%v" }),
              ProExpenseGrossPay: accounting.formatMoney(self.PayData().ProExpenseGrossPay, { format: "%v" }),
              ProVisitTotalBilling: accounting.formatMoney(self.PayData().ProVisitTotalBilling, { format: "%v" }),
              ProExpenseTotalBilling: accounting.formatMoney(self.PayData().ProExpenseTotalBilling, { format: "%v" }),
              ExtractVisitGrossPay: accounting.formatMoney(self.PayData().ExtractVisitGrossPay, { format: "%v" }),
              ExtractExpenseGrossPay: accounting.formatMoney(self.PayData().ExtractExpenseGrossPay, { format: "%v" }),
              ExtractVisitTotalBilling: accounting.formatMoney(self.PayData().ExtractVisitTotalBilling, { format: "%v" }),
              ExtractExpenseTotalBilling: accounting.formatMoney(self.PayData().ExtractExpenseTotalBilling, { format: "%v" }),

                ProTotalPay: accounting.formatMoney(self.PayData().ProVisitGrossPay + self.PayData().ProExpenseGrossPay, { format: "%v" }),
                ProTotalBilling: accounting.formatMoney(self.PayData().ProVisitTotalBilling + self.PayData().ProExpenseTotalBilling, { format: "%v" }),
                ExtractTotalPay: accounting.formatMoney(self.PayData().ExtractVisitGrossPay + self.PayData().ExtractExpenseGrossPay, { format: "%v" }),
                ExtractTotalBilling: accounting.formatMoney(self.PayData().ExtractVisitTotalBilling + self.PayData().ExtractExpenseTotalBilling, { format: "%v" }),
                TotalPayDifference: tpd.toFixed(2),
                TotalBillingDifference: tbd.toFixed(2)
            }

            return retVal;
        });


        self.StagingPaydataTotals = ko.computed(function () {

            var tpd = (self.StagingTotals().TotalGrossPay - self.StagingTotals().ProcuraPayTotal);
            var retVal = {
              
              GrossPay: accounting.formatMoney(self.StagingTotals().GrossPay, { format: "%v" }),
              TravelPay: accounting.formatMoney(self.StagingTotals().TravelPay, { format: "%v" }),
              ContractorServices: accounting.formatMoney(self.StagingTotals().ContractorServices, { format: "%v" }),
              TotalGrossPay: accounting.formatMoney(self.StagingTotals().TotalGrossPay, { format: "%v" }),
              TotalBilling: accounting.formatMoney(self.StagingTotals().TotalBilling, { format: "%v" }),
              ProcuraPayTotal: accounting.formatMoney(self.StagingTotals().ProcuraPayTotal, { format: "%v" }),
              ProcuraBillTotal: accounting.formatMoney(self.StagingTotals().ProcuraBillTotal, { format: "%v" }),
              TotalPayDifference: accounting.formatMoney(tpd.toFixed(2), { format: "%v" })
            }

            return retVal;
        });

        FormatMoneyBoxes = function () {

          //accounting.formatMoney(self.StagingTotals().GrossPay, { format: "%v" })

        };


        CompareAB = function () {
            $('#StagingFiguresTable').hide();
            $('#LoaderText').text('Loading Extract Comparison Data...');

            var request = {
                division: self.DivisionCode,
                batchNumber: self.BatchNumber // '213'
            };

            function MyCallback(result) {
                try {
                    self.PayData(result.GetPayDataSumsResult);
                    $('#LoaderDiv').fadeOut('slow', null);
                } catch (e) {
                    alert(e.Message);
                }
            }
            PostHelper("POST", request, "GetPayDataSums", MyCallback);
        };

        CompareAC = function () {
            $('#ExtractFiguresTable').hide();
            $('#LoaderText').text('Loading Staging Comparison Data...');

            var request = {
                division: self.DivisionCode,
                batchNumber: self.BatchNumber //  '213'
            };

            function MyCallback(result) {
                try {
                    self.StagingTotals(result.GetStagingComparisonResult);
                    $('#LoaderDiv').fadeOut('slow', null);
                } catch (e) {
                    alert(e.Message);
                }
            }
            PostHelper("POST", request, "GetStagingComparison", MyCallback);


        };


        if (queries.Comp == 'extract') {
            CompareAB();
        }
        else if (queries.Comp == 'staging') {
            CompareAC();
            //            alert('Coming soon.');
        }

    }; // End of PayDataComparisonViewModel


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

    ko.applyBindings(new PayDataComparisonViewModel());

});
