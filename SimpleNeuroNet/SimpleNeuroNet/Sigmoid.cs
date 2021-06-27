using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleNeuroNet
{
	class Sigmoid
	{
		public static double Output(double x)
		{
			return 1.0 / (1.0 + Math.Exp((float)-x));
		}

		public static double Derivative(double x)
		{
			return x * (1 - x);
		}
	}
}
