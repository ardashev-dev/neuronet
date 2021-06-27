using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleNeuroNet
{
	public class DataSet
	{
		public double[] Values { get; set; }
		public double[] Targets { get; set; }

		public DataSet(double[] values, double[] targets)
		{
			Values = values;
			Targets = targets;
		}
	}
}
