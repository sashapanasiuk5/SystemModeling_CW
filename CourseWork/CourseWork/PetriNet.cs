namespace CourseWork;

public class PetriNet
{
    private readonly List<ITransition> _transitions;

    private readonly List<Place> _places;

    public PetriNet(List<ITransition> transitions, List<Place> places)
    {
        _transitions = transitions;
        _places = places;
    }

    public void Run(double tModel)
    {
        double t = 0.0;
        while (t < tModel)
        {
            var tNext = _transitions.Min(x => x.NearestTMoment);
            if (tNext != Double.MaxValue)
            {
                var transition = _transitions.First(x => x.NearestTMoment == tNext);
                transition.ExitTokens(tNext);
                
                Console.WriteLine("------------- T Next - "+ tNext + "---------------");
                foreach (var place in _places)
                {
                    place.CollectStatistic(tNext - t);
                    Console.WriteLine(place.Name + ": " + place.TokenCount);
                }
                Console.WriteLine("Transition "+ transition.Id + " was triggered");
                Console.WriteLine("-----------------------------------------------\n");
                
                t = tNext;
            }

            var transitionsForEntrance = _transitions.Where(x => x.CanTrigger).ToArray();
            
            Random.Shared.Shuffle(transitionsForEntrance);
            var prioritizedTransitions = transitionsForEntrance.OrderByDescending(x => x.Priority).ToList();
            while (prioritizedTransitions.Count > 0)
            {
                var transition = prioritizedTransitions[0];
                prioritizedTransitions.RemoveAt(0);
                
                transition.EntryTokens(t);
                prioritizedTransitions = prioritizedTransitions.Where(x => x.CanTrigger).OrderByDescending(x => x.Priority).ToList();
            }
        }
    }

    private void PrintInfo()
    {
        
    }
}