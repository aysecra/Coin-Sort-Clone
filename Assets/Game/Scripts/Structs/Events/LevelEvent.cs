using CoinSortClone.Enums;

namespace CoinSortClone.Structs.Event
{
    /// <summary>
    /// Event is triggered when changing level state
    /// </summary>
    public struct LevelEvent
    {
        public LevelState State { get; }
        
        public LevelEvent(LevelState state)
        {
            State = state;
        }
    }
}
