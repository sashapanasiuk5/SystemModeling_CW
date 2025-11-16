namespace CourseWork;

public class TypeDeterminingPlace: Place
{
    private Place _freeRunways;
    
    private Place _queueRefueled;
    
    public TypeDeterminingPlace(string name, Place freeRunways, Place queueRefueled) : base(name)
    {
        _freeRunways = freeRunways;
        _queueRefueled = queueRefueled;
    }

    public TypeDeterminingPlace(string name, Place freeRunways, Place queueRefueled, int initialTokenCount) : base(name, initialTokenCount)
    {
        _freeRunways = freeRunways;
        _queueRefueled = queueRefueled;
    }

    public override void ReceiveToken(Token token)
    {
        token.Type = GetTokenType();
        base.ReceiveToken(token);
    }

    public override void ReceiveTokens(List<Token> tokens)
    {
        foreach (var token in tokens)
        {
            ReceiveToken(token);
        }
    }

    private TokenType GetTokenType()
    {
        if (_freeRunways.TokenCount > 0 && TokenCount == 0 && _queueRefueled.TokenCount == 0)
        {
            return TokenType.LandingWithoutWaiting;
        }
        return TokenType.LandingWithWaiting;
    }
}