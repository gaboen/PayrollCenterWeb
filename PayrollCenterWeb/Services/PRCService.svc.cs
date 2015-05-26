using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Web.Configuration;
using PayrollCenterWeb.Data;
using System.Collections;

namespace PayrollCenterWeb
{
  [ServiceContract(Namespace = "")]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class PRCService
  {
    // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
    // To create an operation that returns XML,
    //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
    //     and include the following line in the operation body:
    //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";

    //System.Configuration.ConfigurationManager.ConnectionStrings["PYRCNTConnectionString"].ConnectionString;


    //******************************************************************************************************************
    // Set up connection strings according to TestMode

    public static bool TestMode = bool.Parse(WebConfigurationManager.AppSettings["TestMode"]);

    public static string StagingConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["StagingConnectionString"].ConnectionString;

    public static string PYRCNTConnectionString = TestMode == true ? System.Configuration.ConfigurationManager.ConnectionStrings["PYRCNT_TESTConnectionString"].ConnectionString
                                                  : System.Configuration.ConfigurationManager.ConnectionStrings["PYRCNTConnectionString"].ConnectionString;
    
    //******************************************************************************************************************
    
    PRCDataClassesDataContext dc = new PRCDataClassesDataContext(PYRCNTConnectionString);
    SQL1DataClassesDataContext sql1 = new SQL1DataClassesDataContext(StagingConnectionString);
    
    [OperationContract]
    [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
    public List<DivisionStatusView> GetDivisionStatusAll()
    {
      List<DivisionStatusView> response = new List<DivisionStatusView>();

      
      var divisionStatus = from division in dc.DivisionStatusViews
                           select division;
      return divisionStatus.ToList();
    }

    [OperationContract]
    [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
    public string GetCurrentBatchNumber()
    {
      return PRCDataAccessor.GetCurrentBatchNumber();
    }

    [OperationContract]
    [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
    public string GetApplicationRunStatus()
    {
      return PRCDataAccessor.GetApplicationRunStatus();
    }

    [OperationContract]
    [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
    public string CheckGroupMembership()
    {
      return PRCDataAccessor.CheckGroupMembership();
    }


    [OperationContract]
    [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
    public PayDataSums GetPayDataSums(string batchNumber, string division)
    {
      PayDataSums pds = new PayDataSums();

      string divisionName = PRCDataAccessor.GetDivision(null, division);

      System.Data.Linq.ISingleResult<ReturnPayDataComparisonResult> result = sql1.ReturnPayDataComparison(batchNumber, divisionName);

      var vals = result.FirstOrDefault();

      Decimal.TryParse(vals.ProVisitGrossPay, out pds.ProVisitGrossPay);
      Decimal.TryParse(vals.ProVisitTotalBilling, out pds.ProVisitTotalBilling);
      Decimal.TryParse(vals.ProExpenseGrossPay, out pds.ProExpenseGrossPay);
      Decimal.TryParse(vals.ProExpenseTotalBilling, out pds.ProExpenseTotalBilling);
      Decimal.TryParse(vals.ExtractVisitGrossPay, out pds.ExtractVisitGrossPay);
      Decimal.TryParse(vals.ExtractVisitTotalBilling, out pds.ExtractVisitTotalBilling);
      Decimal.TryParse(vals.ExtractExpenseGrossPay, out pds.ExtractExpenseGrossPay);
      Decimal.TryParse(vals.ExtractExpenseTotalBilling, out pds.ExtractExpenseTotalBilling);

      // Round figures
      pds.ProVisitGrossPay = Decimal.Round(pds.ProVisitGrossPay, 2);
      pds.ProVisitTotalBilling = Decimal.Round(pds.ProVisitTotalBilling, 2);
      pds.ProExpenseGrossPay = Decimal.Round(pds.ProExpenseGrossPay, 2);
      pds.ProExpenseTotalBilling = Decimal.Round(pds.ProExpenseTotalBilling, 2);
      pds.ExtractVisitGrossPay = Decimal.Round(pds.ExtractVisitGrossPay, 2);
      pds.ExtractVisitTotalBilling = Decimal.Round(pds.ExtractVisitTotalBilling, 2);
      pds.ExtractExpenseGrossPay = Decimal.Round(pds.ExtractExpenseGrossPay, 2);
      pds.ExtractExpenseTotalBilling = Decimal.Round(pds.ExtractExpenseTotalBilling, 2);


      return pds;

    }

    [OperationContract]
    [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
    public StagingTotals GetStagingComparison(string batchNumber, string division)
    {

      sql1.CommandTimeout = 60;
      // First get the paydata figures...
      PayDataSums pds = new PayDataSums();
      pds = GetPayDataSums(batchNumber, division);

      StagingTotals st = new StagingTotals();


      string divisionName = PRCDataAccessor.GetDivision(null, division);

      System.Data.Linq.ISingleResult<ReturnStagingPayDataTotalsResult> result = sql1.ReturnStagingPayDataTotals(batchNumber, divisionName);

      var vals = result.FirstOrDefault();

      st.GrossPay = (decimal)vals.GrossPayAmount;
      st.ContractorServices = (decimal)vals.ContractServicesPayAmount;
      st.TravelPay = (decimal)vals.TravelPayAmount;
      st.TotalGrossPay = (decimal)vals.TotalGrossPayAmount;
      st.TotalBilling = (decimal)vals.TotalBillingAmount;
      st.ProcuraPayTotal = pds.ProVisitGrossPay + pds.ProExpenseGrossPay;
      st.ProcuraBillTotal = pds.ProVisitTotalBilling + pds.ProExpenseTotalBilling;

      return st;

    }

    [OperationContract]
    [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
    public DivisionStatusView GetDivisionDetailStatus(string divisionCode)
    {
      DivisionStatusView response = new DivisionStatusView();

      //PRCDataClassesDataContext dc = new PRCDataClassesDataContext();
      var divisionStatus = from division in dc.DivisionStatusViews
                           where division.DivisionCode == divisionCode
                           select division;
      //return divisionStatus.FirstOrDefault();
      response = divisionStatus.FirstOrDefault();
      return response;

    }

    [OperationContract]
    [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
    public GetPittsburgStatusResult GetPittsburgStatus()
    {
      GetPittsburgStatusResult result = dc.GetPittsburgStatus().FirstOrDefault();
      return result;
    }

    [OperationContract]
    [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
    DivisionRequestResult SubmitDivisionRequest(DivisionRequest divisionRequest)
    {
      DivisionRequest dr = divisionRequest;
      DivisionRequestResult response = new DivisionRequestResult{ ResultCode = "UNKNOWN"};

      try
      {
        switch (dr.RequestType)
        {
          case RequestTypeEnum.PauseDivisionProcessing:
            dc.PauseDivision(dr.DivisionCode, "Division", char.Parse(dr.RequestText), dr.RequesterID);
            response.ResultCode = "SUCCEEDED";
            break;
          case RequestTypeEnum.PauseExtract:
            dc.PauseDivision(dr.DivisionCode, "Extract", char.Parse(dr.RequestText), dr.RequesterID);
            response.ResultCode = "SUCCEEDED";
            break;
          case RequestTypeEnum.PauseStagingImport:
            dc.PauseDivision(dr.DivisionCode, "StagingImport", char.Parse(dr.RequestText), dr.RequesterID);
            response.ResultCode = "SUCCEEDED";
            break;
          case RequestTypeEnum.InitializeExtract:
            if (!(PRCDataAccessor.GetDivisionRequests(dr).Rows.Count > 0))
            {
              dc.SubmitDivisionRequest(dr.DivisionCode, "InitializeExtract", "", dr.RequesterID);
              response.ResultCode = "SUCCEEDED";
            }
            else
            {
              response.ResultCode = "PENDINGREQUEST";
            }
            break;
          case RequestTypeEnum.RecheckValidation:
            if (!(PRCDataAccessor.GetDivisionRequests(dr).Rows.Count > 0))
            {
              dc.SubmitDivisionRequest(dr.DivisionCode, "RecheckValidation", "", dr.RequesterID);
              response.ResultCode = "SUCCEEDED";
            }
            else
            {
              response.ResultCode = "PENDINGREQUEST";
            }
            break;
          case RequestTypeEnum.RestartImport:
            if (!(PRCDataAccessor.GetDivisionRequests(dr).Rows.Count > 0))
            {
              dc.SubmitDivisionRequest(dr.DivisionCode, "RestartImport", "", dr.RequesterID);
              response.ResultCode = "SUCCEEDED";
            }
            else
            {
              response.ResultCode = "PENDINGREQUEST";
            }
            break;
          case RequestTypeEnum.OverrideExtractBalance:

            response.RequestID = dc.SubmitDivisionRequest(dr.DivisionCode, dr.RequestType.ToString(), dr.RequestText, dr.RequesterID);
            //dc.SubmitOverrideExtractBalanceRequest(dr.DivisionCode, dr.RequesterID);
            response.ResultCode = "SUCCEEDED";
            break;
          case RequestTypeEnum.RollbackDivision:
            if (!(PRCDataAccessor.GetDivisionRequests(dr).Rows.Count > 0))
            {
              response.RequestID = dc.SubmitDivisionRequest(dr.DivisionCode, dr.RequestType.ToString(), dr.RequestText, dr.RequesterID);
              response.ResultCode = "SUCCEEDED";
            }
            else
            {
              response.ResultCode = "PENDINGREQUEST";
            }
            break;
          case RequestTypeEnum.GeneralRequest:
            response.RequestID = dc.SubmitDivisionRequest(dr.DivisionCode, dr.RequestType.ToString(), dr.RequestText, dr.RequesterID);
            response.ResultCode = "SUCCEEDED";
            break;
          default:
            break;
        }

      }
      catch (Exception)
      {
        response.ResultCode = "FAILED";
      }
      return response;
    }


    [OperationContract]
    [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
    public ValidationData PreImportValidation(DivisionRequest divisionRequest)
    {

      DataSet results = PRCDataAccessor.GetPreImportValidationResults(divisionRequest.DivisionCode);
      ValidationData vd = new ValidationData();

      foreach (DataRow row in results.Tables[0].Rows) // HIRE DATES
      {
        vd.HireDateValidation.Add(new HireDateValidation
        {
          Area = row["AREA"].ToString(),
          Emp_Id = row["EMP_ID"].ToString(),
          LastName = row["LASTNAME"].ToString(),
          FirstName = row["FIRSTNAME"].ToString(),
          Dept = row["DEPT"].ToString(),
          IntakeUser = row["INTAKEUSER"].ToString(),
          IntakeDate = row["INTAKEDATE"].ToString(),
          ChgUser = row["CHGUSER"].ToString(),
          ChgDate = row["CHGDATE"].ToString(),
          HireDate = row["HIREDATE"].ToString()
        });

      }

      foreach (DataRow row in results.Tables[1].Rows) // SSNs
      {
        vd.SSNValidation.Add(new SSNValidation
        {
          Area = row["AREA"].ToString(),
          Emp_Id = row["EMP_ID"].ToString(),
          LastName = row["LASTNAME"].ToString(),
          FirstName = row["FIRSTNAME"].ToString(),
          Dept = row["DEPT"].ToString(),
          IntakeUser = row["INTAKEUSER"].ToString(),
          IntakeDate = row["INTAKEDATE"].ToString(),
          ChgUser = row["CHGUSER"].ToString(),
          ChgDate = row["CHGDATE"].ToString(),
          SSN = row["SSN"].ToString()
        });

      }

      foreach (DataRow row in results.Tables[2].Rows) // RefNumbers
      {
        vd.RefNumberValidation.Add(new RefNumberValidation
        {
          Area = row["AREA"].ToString(),
          Emp_Id = row["EMP_ID"].ToString(),
          LastName = row["LASTNAME"].ToString(),
          FirstName = row["FIRSTNAME"].ToString(),
          Dept = row["DEPT"].ToString(),
          IntakeUser = row["INTAKEUSER"].ToString(),
          IntakeDate = row["INTAKEDATE"].ToString(),
          ChgUser = row["CHGUSER"].ToString(),
          ChgDate = row["CHGDATE"].ToString(),
          SSN = row["SSN"].ToString(),
          FirstDuplicateReferenceNumber = row["FirstDuplicateReferenceNumber"].ToString(),
          SecondDuplicateReferenceNumber = row["SecondDuplicateReferenceNumber"].ToString()
        });

      }

      foreach (DataRow row in results.Tables[3].Rows) // ContractorTINs
      {
        vd.ContractorTINValidation.Add(new ContractorTINValidation
        {
          GPVendorNumber = row["GPVendorNumber"].ToString(),
          LastName = row["LastName"].ToString(),
          FirstName = row["FirstName"].ToString(),
          OfficeCode = row["OfficeCode"].ToString(),
          LastModifiedUserName = row["LastModifiedUserName"].ToString(),
          LastModifiedDateTime = row["LastModifiedDateTime"].ToString()
        });

      }

      return vd;
    }


    [OperationContract]
    [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
    public List<ReturnPayDataLineByLineDifferencesResult> LineByLineComparison(DivisionRequest divisionRequest)
    {
      sql1.CommandTimeout = 180;
      string batchNumber = PRCDataAccessor.GetCurrentBatchNumber();
      System.Data.Linq.ISingleResult<ReturnPayDataLineByLineDifferencesResult> result =
        sql1.ReturnPayDataLineByLineDifferences(batchNumber, divisionRequest.DivisionCode);

      return result.ToList<ReturnPayDataLineByLineDifferencesResult>();
    }



    [DataContract]
    public class DivisionRequest2
    {
      [DataMember]
      public string DivisionCode { get; set; }
    }


    [DataContract]
    public class NullTestType
    {
      [DataMember]
      public string NullTestString { get; set; }
      [DataMember]
      public int NullTestInt { get; set; }
    }

  }
}
