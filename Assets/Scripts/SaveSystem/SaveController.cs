using System.Collections.Generic;
using Event = EventServiceSystem.Event;

namespace SaveSystem
{
    public static class SaveController 
    {
        private static readonly SaveFile File = SaveFile.CreateJson("state");
        private static SavedData _state = new();

        private static void SaveCriticalChanged()
        {
            File.Save(_state);
        }

        public static List<Event> SaveRemainingEvents
        {
            get => _state.RemainingEvents;
            set
            {
                _state.RemainingEvents = value;
                SaveCriticalChanged();
            }
        }
        
        public static bool TryLoad()
        {
            if (File.TryReadJson<SavedData>(out var state))
            {
                _state = state;
                return true;
            }

            _state = default;
            return false;
        }
        
    }
}