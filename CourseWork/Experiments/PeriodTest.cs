using CourseWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiments
{
	internal class PeriodTest
	{

		public void Test()
		{


			var values = new List<double>();
			for (int i = 0; i < 4; i++)
			{
				var (model, paybackFunc) = ModelFactory.GetNet(3);

				model.Run(120);
				var paybackTime = paybackFunc(365 * 12);

				if (paybackTime != Double.PositiveInfinity)
					values.Add(paybackTime);
			}
			double max = values.Max();
			double min = values.Min();

			double diff = max - min;
			Console.WriteLine("2 hours: Max diff: {0:F5}", diff);

			values = new List<double>();
			for (int i = 0; i < 4; i++)
			{
				var (model, paybackFunc) = ModelFactory.GetNet(3);

				model.Run(720);
				var paybackTime = paybackFunc(365 * 2);

				if (paybackTime != Double.PositiveInfinity)
					values.Add(paybackTime);
			}
			max = values.Max();
			min = values.Min();

			diff = max - min;
			Console.WriteLine("12 hours: Max diff: {0:F5}", diff);

			values = new List<double>();
			for (int i = 0; i < 4; i++)
			{
				var (model, paybackFunc) = ModelFactory.GetNet(3);

				model.Run(1440);
				var paybackTime = paybackFunc(365);

				if (paybackTime != Double.PositiveInfinity)
					values.Add(paybackTime);
			}
			max = values.Max();
			min = values.Min();

			diff = max - min;
			Console.WriteLine("1 day: Max diff: {0:F5}", diff);
		}
	}
}
