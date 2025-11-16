using MathNet.Numerics.Distributions;

namespace CourseWork.Distributions;

public class DistributionAdapter(IContinuousDistribution distribution) : IGenerator
{
    public double Generate()
    {
        return distribution.Sample();
    }
}