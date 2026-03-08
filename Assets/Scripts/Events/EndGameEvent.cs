namespace Events
{
    public class EndGameEvent
    {
        public bool Result;
        
        public EndGameEvent(bool result)
        {
            Result = result;
        }
    }
}