
using CourseWork;

using MathNet.Numerics.Statistics;

double RunModel(int n)
{
	var values = new List<double>();
	for (int i = 0; i < 20; i++)
	{
		var (model, paybackFunc) = ModelFactory.GetNet(n);

		model.Run(1440);
		var paybackTime = paybackFunc(365);

		if(paybackTime != Double.PositiveInfinity)
			values.Add(paybackTime);
	}
	var mean  = values.Mean();

	Console.WriteLine($"N={n}: Mean payback time = {mean}");
	return mean;
}

int FibonacciSearch(int left, int right, Func<int, double> func)
{
	List<int> fib = new List<int> { 1, 1 };
	while (fib[fib.Count - 1] < (right - left + 1))
		fib.Add(fib[fib.Count - 1] + fib[fib.Count - 2]);

	int k = fib.Count - 1;

	while (left < right)
	{
		if (k < 2)
			break;

		int x1 = left + (fib[k - 2] * (right - left)) / fib[k];
		int x2 = left + (fib[k - 1] * (right - left)) / fib[k];

		if (x1 == x2)
			x2++;

		if (x2 > right) x2 = right;
		if (x1 < left) x1 = left;

		double f1 = func(x1);
		double f2 = func(x2);

		if (f1 > f2)
		{
			left = x1 + 1;
		}
		else
		{
			right = x2 - 1;
		}

		k--;
	}

	return left;
}

var bestN = FibonacciSearch(3, 12, RunModel);
Console.WriteLine("Best number of runways: " + bestN);

