namespace CourseWork;

public class Place 
{

    private readonly List<Token> _tokens = new();

    private double _meanTokenCount = 0;
    
    public string Name { get; }
    public int TokenCount => _tokens.Count;
    public IReadOnlyCollection<Token> Tokens => _tokens.AsReadOnly();

    public Place(string name)
    {
        Name = name;
    }
    
    public Place(string name, int initialTokenCount)
    {
        Name = name;
        for (int i = 0; i < initialTokenCount; i++)
        {
            _tokens.Add(new Token());
        }
    }

    public virtual void ReceiveTokens(List<Token> tokens)
    {
        _tokens.AddRange(tokens);
    }
    
    public virtual void ReceiveToken(Token token)
    {
        _tokens.Add(token);
    }


    public virtual Token GetToken()
    {
        var token = _tokens.FirstOrDefault();
        _tokens.RemoveAt(0);
        return token;
    }

    public virtual Token GetTokenByType(TokenType tokenType)
    {
        var token = _tokens.FirstOrDefault(t => t.Type == tokenType);
        _tokens.Remove(token);
        return token;
    }
    
    public virtual List<Token> GetTokens(int count)
    {
        var tokens = _tokens.GetRange(0, count);
        _tokens.RemoveRange(0, count);
        return tokens;
    }

    public void CollectStatistic(double deltaT)
    {
        _meanTokenCount += deltaT * TokenCount;
    }

    public double GetMeanTokenCount(double totalTime)
    {
        return _meanTokenCount / totalTime;
	}
}