
/// <reference path="/scripts/jquery-1.4.1-vsdoc.js" />
/// <reference path="/Scripts/Common/Server.js" />

$(function () {

    infuser.defaults.templateUrl = "templates";

    function PreImportValidationViewModel() {

        var self = this;
        hostName = PRCSettings.HostName;
        self.division = ko.observable();
        self.ProcessText = ko.observable();
        self.DivisionCode = ko.observable();

        self.DivisionName = ko.computed(function () {
            return self.DivisionCode();
        });


        self.DivisionNameLabel = ko.computed(function () { return 'Pre-Validation Results for division ' + self.DivisionName() });
        self.updateEnabled = ko.observable(true);
        self.ProcessText(self.division.ProcessText);

        var SubmitDivisionRequestResult = {
            ResultCode: '',
            RequestID: 0
        };

        self.HireDateValidationData = [];
        self.SSNValidationData = [];
        self.RefNumberValidationData = [];
        self.ContractorTINValidationData = [];

        self.HireDateValidationArray; // = [];
        self.SSNValidationArray = [];
        self.RefNumberValidationArray; // = [];
        self.ContractorTINValidationArray = [];

        self.HireDateRecordsExist = ko.observable(); 
        self.SSNRecordsExist = ko.observable(); 
        self.RefNumberRecordsExist = ko.observable();
        self.ContractorTINRecordsExist = ko.observable();

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

        RunValidation = function () {
            var request = {
                divisionRequest: {
                    DivisionCode: self.DivisionCode(),
                }
            };
            $('#ValidationTablesDiv').hide();

            $('#LoaderText').text('Loading Data...');

            function MyCallback(result) {
                try {
                    self.HireDateValidationData = result.PreImportValidationResult.HireDateValidation;
                    self.SSNValidationData = result.PreImportValidationResult.SSNValidation;
                    self.RefNumberValidationData = result.PreImportValidationResult.RefNumberValidation;
                    self.ContractorTINValidationData = result.PreImportValidationResult.ContractorTINValidation;

                    fillArrays();
                    fillTables();
                    $('#ValidationTablesDiv').show();
                    $('#LoaderDiv').fadeOut('slow', null);

                } catch (e) {
                    alert('Callback error:' + e.Message);
                }
            }
            PostHelper("POST", request, "PreImportValidation", MyCallback);
        };



        Initialize();
        RunValidation();


        fillTables = function () {
            $('#dynamic1').html('<table cellpadding="0" cellspacing="0" border="0" class="display" id="hireDates"></table>');
            $('#dynamic2').html('<table cellpadding="0" cellspacing="0" border="0" class="display" id="SSNs"></table>');
            $('#dynamic3').html('<table cellpadding="0" cellspacing="0" border="0" class="display" id="RefNumbers"></table>');
            $('#dynamic4').html('<table cellpadding="0" cellspacing="0" border="0" class="display" id="ContractorTINs"></table>');
            $('#hireDates').dataTable({
                "aaData": self.HireDateValidationArray,
                "aoColumns": [
						{ "sTitle": "Area" },
						{ "sTitle": "Emp_Id" },
						{ "sTitle": "LastName" },
						{ "sTitle": "FirstName" },
                        { "sTitle": "Dept" },
                        { "sTitle": "IntakeUser" },
                        { "sTitle": "IntakeDate" },
                        { "sTitle": "ChgUser" },
						{ "sTitle": "ChgDate" },
                        { "sTitle": "HireDate" }
                ]
            });
            $('#SSNs').dataTable({
                "aaData": self.SSNValidationArray,
                "aoColumns": [
						{ "sTitle": "Area" },
						{ "sTitle": "Emp_Id" },
						{ "sTitle": "LastName" },
						{ "sTitle": "FirstName" },
                        { "sTitle": "Dept" },
                        { "sTitle": "IntakeUser" },
                        { "sTitle": "IntakeDate" },
                        { "sTitle": "ChgUser" },
						{ "sTitle": "ChgDate" },
                        { "sTitle": "SSN" }
                ]
            });
            $('#RefNumbers').dataTable({
                "aaData": self.RefNumberValidationArray,
                "aoColumns": [
						{ "sTitle": "Area" },
						{ "sTitle": "Emp_Id" },
						{ "sTitle": "LastName" },
						{ "sTitle": "FirstName" },
                        { "sTitle": "Dept" },
                        { "sTitle": "IntakeUser" },
                        { "sTitle": "IntakeDate" },
                        { "sTitle": "ChgUser" },
						{ "sTitle": "ChgDate" },
                        { "sTitle": "SSN" },
                        { "sTitle": "FirstDuplicateReferenceNumber" },
                        { "sTitle": "SecondDuplicateReferenceNumber" }
                ]
            });
            $('#ContractorTINs').dataTable({
                "aaData": self.ContractorTINValidationArray,
                "aoColumns": [
						{ "sTitle": "GPVendorNumber" },
						{ "sTitle": "LastName" },
						{ "sTitle": "FirstName" },
                        { "sTitle": "OfficeCode" },
                        { "sTitle": "LastModifiedUserName" },
                        { "sTitle": "LastModifiedDateTime" }

                ]
            });

        };


        fillArrays = function () {

            self.HireDateValidationArray = [];
            self.SSNValidationArray = [];
            self.RefNumberValidationArray = [];
            self.ContractorTINValidationArray = [];

            self.HireDateValidationData.forEach(function (element, index, array) {
                var e = element;
                var row = [
                    e.Area,
                    e.Emp_Id,
                    e.LastName,
                    e.FirstName,
                    e.Dept,
                    e.IntakeUser,
                    e.IntakeDate,
                    e.ChgUser,
                    e.ChgDate,
                    e.HireDate
                ];
                self.HireDateValidationArray.push(row);
                self.HireDateRecordsExist(true);
            });


            self.SSNValidationData.forEach(function (element, index, array) {
                var e = element;
                var row = [
                    e.Area,
                    e.Emp_Id,
                    e.LastName,
                    e.FirstName,
                    e.Dept,
                    e.IntakeUser,
                    e.IntakeDate,
                    e.ChgUser,
                    e.ChgDate,
                    e.SSN
                ];
                self.SSNValidationArray.push(row);
                self.SSNRecordsExist(true);
            });

            self.RefNumberValidationData.forEach(function (element, index, array) {
                var e = element;
                var row = [
                    e.Area,
                    e.Emp_Id,
                    e.LastName,
                    e.FirstName,
                    e.Dept,
                    e.IntakeUser,
                    e.IntakeDate,
                    e.ChgUser,
                    e.ChgDate,
                    e.SSN,
                    e.FirstDuplicateReferenceNumber,
                    e.SecondDuplicateReferenceNumber
                ];
                self.RefNumberValidationArray.push(row);
                self.RefNumberRecordsExist(true);
            });

            self.ContractorTINValidationData.forEach(function (element, index, array) {
                var e = element;
                var row = [
                    e.GPVendorNumber,
                    e.LastName,
                    e.FirstName,
                    e.OfficeCode,
                    e.LastModifiedUserName,
                    e.LastModifiedDateTime
                ];
                self.ContractorTINValidationArray.push(row);
                self.ContractorTINRecordsExist(true);
            });



        };

        ReturnToDivDetails = function () {
            window.location = "https://" + hostName + "/PayrollCenterWeb/DivisionDetail/DivisionDetail.htm?Division=" + self.DivisionCode();
        };





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

    ko.applyBindings(new PreImportValidationViewModel());

});




