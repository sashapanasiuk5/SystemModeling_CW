using CourseWork.Distributions;
using MathNet.Numerics.Distributions;

namespace CourseWork;

public class Transition: ITransition
{
    private List<Place> _inputPlaces;
    private List<Place> _outputPlaces;
    public IGenerator Generator { get; set; }
    
    

    private List<(double,TokenType)> _tMoments = new();

    private Place? _targetTokenTypePlace;

    public int Id { get; }
    public int Priority { get; }

    public double NearestTMoment
    {
        get
        {
            if(_tMoments.Count == 0)
                return Double.MaxValue;
            return _tMoments.Min(x => x.Item1);
        }
    }

    public bool CanTrigger => _inputPlaces.All(p => p.TokenCount > 0);

    public Transition(int id, List<Place> inputPlaces, List<Place> outputPlaces, int priority = 0, Place? targetTokenTypePlace = null)
    {
        _inputPlaces = inputPlaces;
        _outputPlaces = outputPlaces;
        Priority = priority;
        _targetTokenTypePlace = targetTokenTypePlace;
        Id = id;
    }

    public void EntryTokens(double tCurrent)
    {
        while (CanTrigger)
        {
            var tokenType = TokenType.None;
            foreach (var place in _inputPlaces)
            {
                var token = place.GetToken();
                if(place == _targetTokenTypePlace && tokenType == TokenType.None)
                    tokenType = token.Type;
            }

            var t = tCurrent;
            if (Generator != null)
            {
                t += Generator.Generate();
            }
            _tMoments.Add((t, tokenType));
        }
    }

    public void ExitTokens(double t)
    {
        var (tMoment, tokenType) = _tMoments.Find(x => x.Item1 == t);
        _tMoments.Remove((tMoment, tokenType));
        
        var token = new Token(){Type = tokenType};
        foreach (var place in _outputPlaces)
        {
            place.ReceiveToken(token);
        }
    }
}