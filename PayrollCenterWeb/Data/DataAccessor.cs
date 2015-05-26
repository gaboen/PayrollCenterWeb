using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Utils
{
  public static class SQLDataAccessor
  {

    public static List<String> GetDBList(string connectionString, string sqlQueryString)
    {
      List<string> returnList = new List<string>();
      SqlConnection sqlconnection = new SqlConnection(connectionString);
      SqlCommand sqlCmd = new SqlCommand(sqlQueryString, sqlconnection);
      sqlCmd.CommandTimeout = 120;

      try
      {
        sqlconnection.Open();
        SqlDataReader dr = sqlCmd.ExecuteReader();

        while (dr.Read())
        {
          returnList.Add(dr[0].ToString());
        }

        sqlconnection.Close();
      }
      catch (Exception)
      {
        throw;
      }

      return returnList;
    }


    public static List<String> GetDBList(string connectionString, SqlCommand sqlCmd)
    {
      List<string> returnList = new List<string>();
      SqlConnection sqlconnection = new SqlConnection(connectionString);

      sqlCmd.Connection = sqlconnection;
      sqlCmd.CommandTimeout = 120;

      try
      {
        sqlconnection.Open();
        SqlDataReader dr = sqlCmd.ExecuteReader();

        while (dr.Read())
        {
          returnList.Add(dr[0].ToString());
        }

        sqlconnection.Close();
      }
      catch (Exception)
      {
        throw;
      }

      return returnList;
    }


    public static string GetDBValue(string connectionString, string sqlQueryString)
    {
      string retVal = "";
      SqlConnection sqlconnection = new SqlConnection(connectionString);
      SqlCommand sqlCmd = new SqlCommand(sqlQueryString, sqlconnection);
      sqlCmd.CommandTimeout = 120;
      try
      {
        sqlconnection.Open();
        SqlDataReader dr = sqlCmd.ExecuteReader();

        if (dr.Read())
        {
          retVal = dr[0].ToString();
        }

        sqlconnection.Close();
      }
      catch (Exception)
      {
        throw;
      }

      return retVal;

    }

    public static DataTable GetDataTable(string connectionString, string sqlString)
    {
      SqlConnection sqlconnection = new SqlConnection(connectionString);
      SqlCommand sqlCmd = new SqlCommand(sqlString);
      sqlCmd.CommandTimeout = 120;
      DataTable dt = new DataTable();

      try
      {
        sqlconnection.Open();
        sqlCmd.Connection = sqlconnection;
        SqlDataReader dr = sqlCmd.ExecuteReader();
        dt.Load((IDataReader)dr);
        sqlconnection.Close();
      }
      catch (Exception)
      {
        throw;
      }

      return dt;

    }

    public static DataTable GetDataTable(string connectionString, SqlCommand sqlCmd)
    {
      SqlConnection sqlconnection = new SqlConnection(connectionString);
      DataTable dt = new DataTable();
      sqlCmd.CommandTimeout = 120;
      sqlCmd.Connection = sqlconnection;

      try
      {
        sqlconnection.Open();
        SqlDataReader dr = sqlCmd.ExecuteReader();
        if (dr.Read())
        {
          dt.Load((IDataReader)dr);
        }
        sqlconnection.Close();
      }
      catch (Exception)
      {
        throw;
      }

      return dt;

    }

    public static DataSet GetDataSet(string connectionString, string sqlString, string[] tableNames)
    {
      SqlConnection sqlconnection = new SqlConnection(connectionString);
      SqlCommand sqlCmd = new SqlCommand(sqlString);
      sqlCmd.CommandTimeout = 120;
      DataSet ds = new DataSet();

      try
      {
        sqlconnection.Open();
        sqlCmd.Connection = sqlconnection;

        SqlDataReader dr = sqlCmd.ExecuteReader();

        ds.Load((IDataReader)dr, LoadOption.Upsert, tableNames);
        sqlconnection.Close();
      }
      catch (Exception)
      {
        throw;
      }

      return ds;

    }

    public static void RunDBCmd(string connectionString, ref SqlCommand sqlCmd)
    {
      string retVal = "";
      SqlConnection sqlconnection = new SqlConnection(connectionString);
      sqlCmd.Connection = sqlconnection;
      sqlCmd.CommandTimeout = 120;

      try
      {
        sqlconnection.Open();
        SqlDataReader dr = sqlCmd.ExecuteReader();

        if (dr.Read())
        {
          retVal = dr[0].ToString();
        }

        sqlconnection.Close();
      }
      catch (Exception e)
      {
        string err = e.Message;
        throw;
      }
    }


    public static void ExecNonQuery(string connectionString, string sqlString)
    {
      SqlConnection sqlconnection = new SqlConnection(connectionString);
      SqlCommand sqlCmd = new SqlCommand(sqlString, sqlconnection);

      try
      {
        sqlconnection.Open();
        sqlCmd.Connection = sqlconnection;
        sqlCmd.CommandTimeout = 300;
        sqlCmd.ExecuteNonQuery();
      }
      catch (Exception)
      {

        throw;
      }
    }

    public static string ExecNonQueryStoredProcedure(string connectionString, string spName, params string[] spParams)
    {
      string retVal = "";
      SqlConnection sqlconn = new SqlConnection(connectionString);

      SqlCommand sp = new SqlCommand(spName, sqlconn);
      sp.CommandText = spName;
      sp.CommandType = CommandType.StoredProcedure;
      sp.CommandTimeout = 120;

      foreach (string parameter in spParams)
      {
        string[] paramParts = parameter.Split('=');
        sp.Parameters.AddWithValue(paramParts[0].Trim(), paramParts[1].Trim());
      }
      // run Command
      RunDBCmd(connectionString, ref sp);

      return retVal;
    }



  }
}
