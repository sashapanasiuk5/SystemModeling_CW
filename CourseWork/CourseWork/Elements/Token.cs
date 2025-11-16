namespace CourseWork;

public struct Token
{
    public TokenType Type { get; set; }
    
    public Token()
    {
        Type = TokenType.None;
    }
}