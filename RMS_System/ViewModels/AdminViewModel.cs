using RMS_System.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RMS_System.ViewModels
{
    public class AdminViewModel
    {
        public List<Order> Orders { get; set; }

		public List<DateTime?> RevenueOrders { get; set; }

		public Double TotalRevenueInfoBox { get; set; }
        public Double CashRevenueInfoBox { get; set; }
        public Double CardRevenueInfoBox { get; set; }
        public int NoOfSessions { get; set; }
        public (string,string) ChartData { get; set; }
        public DateTime date { get; set; }

		public List<DishWiseData> DishWiseData { get; set; }
        public List<OrderWiseData> OrderWiseData { get; set; }


    }

	public class DishWiseData
	{
		public string ItemName { get; set; }
		public int OrderCount { get; set; }
		public double Revenue { get; set; }
	}


	public class OrderWiseData
	{
		public int OrderCount { get; set; }
		public double CashRevenue { get; set; }
		public double CardRevenue { get; set; }
		public double TotalRevenue { get; set; }
        public DateTime OrderDate { get; set; }
    }



	[DataContract]
	public class DataPoint
	{
		public DataPoint(DateTime x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "x")]
		public Nullable<DateTime> X = null;

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "y")]
		public Nullable<double> Y = null;
	}


	[DataContract]
	public class DataPoint2
	{
		public DataPoint2(string label, int y)
		{
			this.Label = label;
			this.Y = y;
		}

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "label")]
		public string Label = null;

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "y")]
		public Nullable<int> Y = null;
	}


	[DataContract]
	public class DataPoint3
	{
		public DataPoint3(string label, int y)
		{
			this.Label = label;
			this.Y = y;
		}

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "label")]
		public string Label = null;

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "y")]
		public Nullable<int> Y = null;
	}





}