
/// <reference path="/scripts/jquery-1.4.1-vsdoc.js" />
/// <reference path="/Scripts/Common/Server.js" />

$(function () {

  infuser.defaults.templateUrl = "templates";

  function LineByLineComparisonViewModel() {

    var self = this;
    hostName = PRCSettings.HostName;
    self.division = ko.observable();
    self.ProcessText = ko.observable();
    self.DivisionCode = ko.observable();

    self.DivisionName = ko.computed(function () {
      return self.DivisionCode();
    });


    self.DivisionNameLabel = ko.computed(function () { return 'Line-by-Line Comparison for division ' + self.DivisionName() });
    self.updateEnabled = ko.observable(true);
    self.ProcessText(self.division.ProcessText);

    var SubmitDivisionRequestResult = {
      ResultCode: '',
      RequestID: 0
    };

    self.payData = []; 
    self.payDataArray = [];
    self.payDataRecordsExist = ko.observable();

    Initialize = function () {
      var queries = {};
      $.each(document.location.search.substr(1).split('&'), function (c, q) {
        try {
          var i = q.split('=');
          queries[i[0].toString()] = i[1].toString();
          self.DivisionCode(queries.Division); // = queries.Division;
        } catch (e) {
          alert(e.Message);
        }
      });
    };


    RunComparison = function () {
      var request = {
        divisionRequest: {
          DivisionCode: self.DivisionCode()
          //BatchNumber: "229"
        }
      };

      $('#dynamic1').hide();

      var divwidth = $('#dynamic1').css("width");

      $('#SpreadSheetLayoutDiv').css("width", divwidth);

      $('#LoaderText').text('Loading Data...');

      function MyCallback(result) {
        try {
          self.payData = result.LineByLineComparisonResult;
          fillArray();
          fillTable();
          $('#dynamic1').show();
          $('#LoaderDiv').fadeOut('slow', null);

        } catch (e) {
          alert('Callback error:' + e.Message);
        }
      }
      PostHelper("POST", request, "LineByLineComparison", MyCallback);
    };


    Initialize();
    RunComparison();

    fillTable = function () {
      $('#dynamic1').html('<table cellpadding="0" cellspacing="0" border="0" class="display" id="results"></table>');
      $('#results').dataTable({
        "aaData": self.payDataArray,
        "aoColumns": [
          { "sTitle": "SocialSecurityNumber" },
          { "sTitle": "EmployeeSurname" },
          { "sTitle": "EmployeeFirstName" },
          { "sTitle": "ExtractBilling" },
          { "sTitle": "ProcuraBilling" },
          { "sTitle": "ExtractPay" },
          { "sTitle": "ProcuraPay" },
          { "sTitle": "BillingDifference" },
          { "sTitle": "PayDifference" }
        ]
      });

    };


    fillArray = function () {

      self.payData.forEach(function (element, index, array) {
        var e = element;
        var row = [
            e.SocialSecurityNumber,
            e.EmployeeSurname,
            e.EmployeeFirstName,
            e.ExtractBilling,
            e.ProcuraBilling,
            e.ExtractPay,
            e.ProcuraPay,
            e.BillingDifference,
            e.PayDifference
        ];
        self.payDataArray.push(row);
        self.payDataRecordsExist(true);
      });
    };


    ReturnToDivDetails = function () {
      window.location = "https://" + hostName + "/PayrollCenterWeb/DivisionDetail/DivisionDetail.htm?Division=" + self.DivisionCode();
    };

  }; // End of ViewModel


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

  ko.applyBindings(new LineByLineComparisonViewModel());

});




