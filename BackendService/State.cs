
namespace SystemState
{
    public enum State
    {
        Startup = 0,
        Idle = 1,
        GrabBottle = 2,
        PourBottle = 3,
        ReturnBottle = 4,
        RemoveBottle = 5,
        GetNewBottle = 6,
        Paused = 7,
        PauseGetNewBottle = 8,
        PauseGrabBottle = 9,
        PauseRemoveBottle = 10,
        PauseReturnBottle = 11,
    }
}