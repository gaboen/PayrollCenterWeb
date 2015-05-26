using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Configuration;
using Utils;



namespace PayrollCenterWeb.Data
{
  public class PRCDataAccessor
  {

    public static bool TestMode = bool.Parse(WebConfigurationManager.AppSettings["TestMode"]);
    
    public static string StagingConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["StagingConnectionString"].ConnectionString;

    public static string PYRCNTConnectionString = TestMode == true ? System.Configuration.ConfigurationManager.ConnectionStrings["PYRCNT_TESTConnectionString"].ConnectionString
                                                  : System.Configuration.ConfigurationManager.ConnectionStrings["PYRCNTConnectionString"].ConnectionString;


    public static string GetDivision(string divisionName, string divisionCode)
    {
      
      string retVal = "";

      if (divisionName == null && divisionCode != null)
      {
        retVal = SQLDataAccessor.GetDBValue(StagingConnectionString, "select DivisionName from staging.dbo.Division where DivisionCode = '" + divisionCode + "'");
      }
      else if (divisionCode == null & divisionName != null)
      {
        retVal = SQLDataAccessor.GetDBValue(StagingConnectionString, "select DivisionCode from staging.dbo.Division where DivisionName = '" + divisionName + "'");
      }
      else if (divisionName == null && divisionCode == null)
      {
        throw new Exception("Division Code and Division Name cannot both be null when calling method");
      }
      else if (divisionName != null && divisionCode != null)
      {
        throw new Exception("Division Code and Division Name cannot both be not null when calling method");
      }
      return retVal;
    }


    public static string GetCurrentBatchNumber()
    {
      string retVal = "";
      if (TestMode)
      {
        retVal = SQLDataAccessor.GetDBValue(PYRCNTConnectionString, "SELECT AppValue FROM AppSettings WHERE AppName = 'PYRCNT' AND AppCategory = 'Batch' AND AppKey = 'BatchNumber'");
      }
      else
      {
        retVal = SQLDataAccessor.GetDBValue(StagingConnectionString, "select BatchNumber REFNumber FROM [PayDataBatch] where  DATEDIFF(day, WeekEndingDate, GETDATE()) BETWEEN 1 AND 7");
      }
      return retVal;
    }


    public static string GetApplicationRunStatus()
    {
      string retVal = "";
      retVal = SQLDataAccessor.GetDBValue(PYRCNTConnectionString, "[dbo].[GetApplicationRunStatus]").Trim();
      return retVal;
    }


    public static string CheckGroupMembership()
    {
      string retVal = "";

      System.Security.Principal.IPrincipal user = System.Web.HttpContext.Current.User;

      if (user.IsInRole("TSOT\\PayrollCenterAdmins"))
      {
        retVal = "Admins";
      }
      else retVal = "Users";

      return retVal;
    }


    public static DataTable GetDivisionRequests(DivisionRequest request)
    {
      // TODO: create statement checking to see if there are any outstanding matching requests
      DataTable result = new DataTable();
      string sql = "select * from DivisionDBRequests where DivisionCode = '" + request.DivisionCode +
        "' and RequestCode = '" + request.RequestType + "' and RequestStatus != 'COMP'";
      result = SQLDataAccessor.GetDataTable(PYRCNTConnectionString, sql);
      return result;
    }


    public static DataSet GetPreImportValidationResults(string divisionCode)
    {
      DataSet dsResult = new DataSet();

      if (TestMode)
      {
        dsResult = SQLDataAccessor.GetDataSet(PYRCNTConnectionString, "exec Procura_ReturnPreImportValidation_DEV @DivisionCode = '" + divisionCode + "'", new string[] { "HireDates", "SSNs", "DupRefNums", "ContTIN", "TESTINGTABLE" });
      }
      else
      {
        dsResult = SQLDataAccessor.GetDataSet(StagingConnectionString, "exec prc.ReturnPreImportValidation @DivisionCode = '" + divisionCode + "'", new string[] { "HireDates", "SSNs", "DupRefNums", "ContTIN", "TESTINGTABLE" });
      }

      return dsResult;
    }




  }
}