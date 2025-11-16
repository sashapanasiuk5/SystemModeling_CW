using MathNet.Numerics.Distributions;

namespace CourseWork;

public class OutputTransition: ITransition
{
    public int Priority { get; }

    public int Id { get; }

    public bool CanTrigger
    {
        get
        {
            if (_tokenType != TokenType.None)
            {
                return _inputPlace.Tokens.Any(t => t.Type == _tokenType);
            }

            return _inputPlace.TokenCount > 0;
        }
    }
    public double NearestTMoment
    {
        get
        {
            if(_tMoments.Count == 0)
                return Double.MaxValue;
            return _tMoments.Min();
        }
    }
    
    
    private List<double> _tMoments = new();

    private readonly Place _inputPlace;
    private readonly Place _outputPlace;

    private readonly IContinuousDistribution? _generator;

    private readonly int _outputMultiplicity;
    
    private readonly TokenType _tokenType;

    public OutputTransition(int id, Place inputPlace, Place outputPlace, int upperValue, int lowerValue, TokenType tokenType = TokenType.None, int priority = 0)
    {
        _inputPlace = inputPlace;
        _outputPlace = outputPlace;
        _generator = new ContinuousUniform(lowerValue, upperValue);
        _tokenType = tokenType;
        Priority = priority;
        Id = id;
    }
    
    public OutputTransition(int id, Place inputPlace, Place outputPlace, int outputMultiplicity, TokenType tokenType = TokenType.None, int priority = 0)
    {
        _inputPlace = inputPlace;
        _outputPlace = outputPlace;
        _outputMultiplicity = outputMultiplicity;
        _tokenType = tokenType;
        Priority = priority;
        Id = id;
    }

    public void EntryTokens(double tCurrent)
    {
        while (CanTrigger)
        {
            if (_tokenType != TokenType.None)
            {
                _inputPlace.GetTokenByType(_tokenType);
            }
            else
            {
                _inputPlace.GetToken();
            }
            _tMoments.Add(tCurrent);
        }
    }

    public void ExitTokens(double t)
    {
        _tMoments.Remove(t);

        int tokenCount;
        if (_generator != null)
        {
            tokenCount = (int)_generator.Sample();
        }
        else
        {
            tokenCount = _outputMultiplicity;
        }

        for (int i = 0; i < tokenCount; i++)
        {
            _outputPlace.ReceiveToken(new Token());
        }
    }
}