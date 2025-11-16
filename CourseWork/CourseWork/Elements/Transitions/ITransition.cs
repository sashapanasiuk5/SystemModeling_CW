namespace CourseWork;

public interface ITransition
{
    int Id { get; }
    int Priority { get; }
    bool CanTrigger { get; }
    double NearestTMoment { get; }

    void EntryTokens(double tCurrent);
    void ExitTokens(double t);
}