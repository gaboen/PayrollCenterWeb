using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayrollCenterWeb.Data
{
  public class PRCData
  {

  }

  public enum RequestTypeEnum { PauseDivisionProcessing, PauseExtract, PauseStagingImport, InitializeExtract, RestartImport, RecheckValidation, OverrideExtractBalance, RollbackDivision, GeneralRequest }

  public class DivisionRequest
  {
    public string BatchNumber { get; set; }
    public string DivisionCode { get; set; }
    public RequestTypeEnum RequestType { get; set; }
    public string RequestText { get; set; }
    public string RequesterID { get; set; }

  }

  public class DivisionRequestResult
  {
    public string ResultCode { get; set; }
    public int RequestID { get; set; }
  }


  public class PayDataSums
  {
    public decimal ProVisitGrossPay;
    public decimal ProVisitTotalBilling;
    public decimal ProExpenseGrossPay;
    public decimal ProExpenseTotalBilling;

    public decimal ExtractVisitGrossPay;
    public decimal ExtractVisitTotalBilling;
    public decimal ExtractExpenseGrossPay;
    public decimal ExtractExpenseTotalBilling;
  }

  public class StagingTotals
  {
    public decimal GrossPay { get; set; }
    public decimal TravelPay { get; set; }
    public decimal ContractorServices { get; set; }
    public decimal TotalGrossPay { get; set; }
    public decimal TotalBilling { get; set; }
    public decimal ProcuraPayTotal { get; set; }
    public decimal ProcuraBillTotal { get; set; }
  }

  public enum StagingImportStep { EmployeeImport, PayDataImport, EmployeeIntegrations, ContractorImport, APImport }


  public class TestData
  {
    public string TestField1 { get; set; }
    public string TestField2 { get; set; }
  }

  public class HireDateValidation
  {
    public string Area { get; set; }
    public string Emp_Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Dept { get; set; }
    public string IntakeUser { get; set; }
    public string IntakeDate { get; set; }
    public string ChgUser { get; set; }
    public string ChgDate { get; set; }
    public string HireDate { get; set; }

  }

  public class SSNValidation
  {
    public string Area { get; set; }
    public string Emp_Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Dept { get; set; }
    public string IntakeUser { get; set; }
    public string IntakeDate { get; set; }
    public string ChgUser { get; set; }
    public string ChgDate { get; set; }
    public string SSN { get; set; }

  }

  public class RefNumberValidation
  {
    public string Area { get; set; }
    public string Emp_Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Dept { get; set; }
    public string IntakeUser { get; set; }
    public string IntakeDate { get; set; }
    public string ChgUser { get; set; }
    public string ChgDate { get; set; }
    public string SSN { get; set; }
    public string FirstDuplicateReferenceNumber { get; set; }
    public string SecondDuplicateReferenceNumber { get; set; }

  }

  public class ContractorTINValidation
  {
    public string GPVendorNumber { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string OfficeCode { get; set; }
    public string LastModifiedUserName { get; set; }
    public string LastModifiedDateTime { get; set; }
  }



  public class ValidationData
  {
    public ValidationData()
    {
      HireDateValidation = new List<HireDateValidation>();
      SSNValidation = new List<SSNValidation>();
      RefNumberValidation = new List<RefNumberValidation>();
      ContractorTINValidation = new List<ContractorTINValidation>();
      TestData = new List<TestData>();
    }

    public List<HireDateValidation> HireDateValidation { get; set; }
    public List<SSNValidation> SSNValidation { get; set; }
    public List<RefNumberValidation> RefNumberValidation { get; set; }
    public List<ContractorTINValidation> ContractorTINValidation { get; set; }
    public List<TestData> TestData { get; set; }

  }

  
}