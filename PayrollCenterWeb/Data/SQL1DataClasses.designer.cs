﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PayrollCenterWeb.Data
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Staging")]
	public partial class SQL1DataClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public SQL1DataClassesDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["StagingConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public SQL1DataClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SQL1DataClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SQL1DataClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SQL1DataClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="prc.ReturnPayDataComparison")]
		public ISingleResult<ReturnPayDataComparisonResult> ReturnPayDataComparison([global::System.Data.Linq.Mapping.ParameterAttribute(Name="BatchNumber", DbType="VarChar(6)")] string batchNumber, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="DivisionName", DbType="VarChar(50)")] string divisionName)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), batchNumber, divisionName);
			return ((ISingleResult<ReturnPayDataComparisonResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="prc.ReturnStagingPayDataTotals")]
		public ISingleResult<ReturnStagingPayDataTotalsResult> ReturnStagingPayDataTotals([global::System.Data.Linq.Mapping.ParameterAttribute(Name="BatchNumber", DbType="VarChar(6)")] string batchNumber, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="DivisionName", DbType="VarChar(50)")] string divisionName)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), batchNumber, divisionName);
			return ((ISingleResult<ReturnStagingPayDataTotalsResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="prc.ReturnPayDataLineByLineDifferences")]
		public ISingleResult<ReturnPayDataLineByLineDifferencesResult> ReturnPayDataLineByLineDifferences([global::System.Data.Linq.Mapping.ParameterAttribute(Name="BatchNumber", DbType="VarChar(6)")] string batchNumber, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="DivisionCode", DbType="VarChar(50)")] string divisionCode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), batchNumber, divisionCode);
			return ((ISingleResult<ReturnPayDataLineByLineDifferencesResult>)(result.ReturnValue));
		}
	}
	
	public partial class ReturnPayDataComparisonResult
	{
		
		private string _ProVisitGrossPay;
		
		private string _ProVisitTotalBilling;
		
		private string _ProExpenseGrossPay;
		
		private string _ProExpenseTotalBilling;
		
		private string _ExtractVisitGrossPay;
		
		private string _ExtractVisitTotalBilling;
		
		private string _ExtractExpenseGrossPay;
		
		private string _ExtractExpenseTotalBilling;
		
		public ReturnPayDataComparisonResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProVisitGrossPay", DbType="VarChar(20)")]
		public string ProVisitGrossPay
		{
			get
			{
				return this._ProVisitGrossPay;
			}
			set
			{
				if ((this._ProVisitGrossPay != value))
				{
					this._ProVisitGrossPay = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProVisitTotalBilling", DbType="VarChar(20)")]
		public string ProVisitTotalBilling
		{
			get
			{
				return this._ProVisitTotalBilling;
			}
			set
			{
				if ((this._ProVisitTotalBilling != value))
				{
					this._ProVisitTotalBilling = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProExpenseGrossPay", DbType="VarChar(20)")]
		public string ProExpenseGrossPay
		{
			get
			{
				return this._ProExpenseGrossPay;
			}
			set
			{
				if ((this._ProExpenseGrossPay != value))
				{
					this._ProExpenseGrossPay = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProExpenseTotalBilling", DbType="VarChar(20)")]
		public string ProExpenseTotalBilling
		{
			get
			{
				return this._ProExpenseTotalBilling;
			}
			set
			{
				if ((this._ProExpenseTotalBilling != value))
				{
					this._ProExpenseTotalBilling = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExtractVisitGrossPay", DbType="VarChar(20)")]
		public string ExtractVisitGrossPay
		{
			get
			{
				return this._ExtractVisitGrossPay;
			}
			set
			{
				if ((this._ExtractVisitGrossPay != value))
				{
					this._ExtractVisitGrossPay = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExtractVisitTotalBilling", DbType="VarChar(20)")]
		public string ExtractVisitTotalBilling
		{
			get
			{
				return this._ExtractVisitTotalBilling;
			}
			set
			{
				if ((this._ExtractVisitTotalBilling != value))
				{
					this._ExtractVisitTotalBilling = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExtractExpenseGrossPay", DbType="VarChar(20)")]
		public string ExtractExpenseGrossPay
		{
			get
			{
				return this._ExtractExpenseGrossPay;
			}
			set
			{
				if ((this._ExtractExpenseGrossPay != value))
				{
					this._ExtractExpenseGrossPay = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ExtractExpenseTotalBilling", DbType="VarChar(20)")]
		public string ExtractExpenseTotalBilling
		{
			get
			{
				return this._ExtractExpenseTotalBilling;
			}
			set
			{
				if ((this._ExtractExpenseTotalBilling != value))
				{
					this._ExtractExpenseTotalBilling = value;
				}
			}
		}
	}
	
	public partial class ReturnStagingPayDataTotalsResult
	{
		
		private string _BatchNumber;
		
		private string _DivisionName;
		
		private System.Nullable<decimal> _GrossPayAmount;
		
		private System.Nullable<decimal> _ContractServicesPayAmount;
		
		private System.Nullable<decimal> _TravelPayAmount;
		
		private System.Nullable<decimal> _TotalGrossPayAmount;
		
		private System.Nullable<decimal> _TotalBillingAmount;
		
		public ReturnStagingPayDataTotalsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BatchNumber", DbType="VarChar(6)")]
		public string BatchNumber
		{
			get
			{
				return this._BatchNumber;
			}
			set
			{
				if ((this._BatchNumber != value))
				{
					this._BatchNumber = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DivisionName", DbType="VarChar(50)")]
		public string DivisionName
		{
			get
			{
				return this._DivisionName;
			}
			set
			{
				if ((this._DivisionName != value))
				{
					this._DivisionName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GrossPayAmount", DbType="Decimal(8,2)")]
		public System.Nullable<decimal> GrossPayAmount
		{
			get
			{
				return this._GrossPayAmount;
			}
			set
			{
				if ((this._GrossPayAmount != value))
				{
					this._GrossPayAmount = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ContractServicesPayAmount", DbType="Decimal(8,2)")]
		public System.Nullable<decimal> ContractServicesPayAmount
		{
			get
			{
				return this._ContractServicesPayAmount;
			}
			set
			{
				if ((this._ContractServicesPayAmount != value))
				{
					this._ContractServicesPayAmount = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TravelPayAmount", DbType="Decimal(8,2)")]
		public System.Nullable<decimal> TravelPayAmount
		{
			get
			{
				return this._TravelPayAmount;
			}
			set
			{
				if ((this._TravelPayAmount != value))
				{
					this._TravelPayAmount = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TotalGrossPayAmount", DbType="Decimal(8,2)")]
		public System.Nullable<decimal> TotalGrossPayAmount
		{
			get
			{
				return this._TotalGrossPayAmount;
			}
			set
			{
				if ((this._TotalGrossPayAmount != value))
				{
					this._TotalGrossPayAmount = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TotalBillingAmount", DbType="Decimal(8,2)")]
		public System.Nullable<decimal> TotalBillingAmount
		{
			get
			{
				return this._TotalBillingAmount;
			}
			set
			{
				if ((this._TotalBillingAmount != value))
				{
					this._TotalBillingAmount = value;
				}
			}
		}
	}
	
	public partial class ReturnPayDataLineByLineDifferencesResult
	{
		
		private string _SocialSecurityNumber;
		
		private string _EmployeeSurname;
		
		private string _EmployeeFirstName;
		
		private decimal _ExtractBilling;

    private decimal _ProcuraBilling;

    private decimal _ExtractPay;

    private decimal _ProcuraPay;

    private decimal _BillingDifference;

    private decimal _PayDifference;
		
		public ReturnPayDataLineByLineDifferencesResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SocialSecurityNumber", DbType="VarChar(3) NOT NULL", CanBeNull=false)]
		public string SocialSecurityNumber
		{
			get
			{
				return this._SocialSecurityNumber;
			}
			set
			{
				if ((this._SocialSecurityNumber != value))
				{
					this._SocialSecurityNumber = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmployeeSurname", DbType="VarChar(3) NOT NULL", CanBeNull=false)]
		public string EmployeeSurname
		{
			get
			{
				return this._EmployeeSurname;
			}
			set
			{
				if ((this._EmployeeSurname != value))
				{
					this._EmployeeSurname = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmployeeFirstName", DbType="VarChar(3) NOT NULL", CanBeNull=false)]
		public string EmployeeFirstName
		{
			get
			{
				return this._EmployeeFirstName;
			}
			set
			{
				if ((this._EmployeeFirstName != value))
				{
					this._EmployeeFirstName = value;
				}
			}
		}

    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ExtractBilling", DbType = "Decimal(8,2) NOT NULL")]
    public decimal ExtractBilling
		{
			get
			{
				return this._ExtractBilling;
			}
			set
			{
				if ((this._ExtractBilling != value))
				{
					this._ExtractBilling = value;
				}
			}
		}

    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ProcuraBilling", DbType = "Decimal(8,2) NOT NULL")]
    public decimal ProcuraBilling
		{
			get
			{
				return this._ProcuraBilling;
			}
			set
			{
				if ((this._ProcuraBilling != value))
				{
					this._ProcuraBilling = value;
				}
			}
		}

    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ExtractPay", DbType = "Decimal(8,2) NOT NULL")]
    public decimal ExtractPay
		{
			get
			{
				return this._ExtractPay;
			}
			set
			{
				if ((this._ExtractPay != value))
				{
					this._ExtractPay = value;
				}
			}
		}

    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ProcuraPay", DbType = "Decimal(8,2) NOT NULL")]
    public decimal ProcuraPay
		{
			get
			{
				return this._ProcuraPay;
			}
			set
			{
				if ((this._ProcuraPay != value))
				{
					this._ProcuraPay = value;
				}
			}
		}

    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BillingDifference", DbType = "Decimal(8,2) NOT NULL")]
    public decimal BillingDifference
		{
			get
			{
				return this._BillingDifference;
			}
			set
			{
				if ((this._BillingDifference != value))
				{
					this._BillingDifference = value;
				}
			}
		}

    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PayDifference", DbType = "Decimal(8,2) NOT NULL")]
    public decimal PayDifference
		{
			get
			{
				return this._PayDifference;
			}
			set
			{
				if ((this._PayDifference != value))
				{
					this._PayDifference = value;
				}
			}
		}
	}
}
#pragma warning restore 1591