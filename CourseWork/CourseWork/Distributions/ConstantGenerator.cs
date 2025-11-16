namespace CourseWork.Distributions;

public class ConstantGenerator(double value) : IGenerator
{
    public double Generate()
    {
        return value;
    }
}