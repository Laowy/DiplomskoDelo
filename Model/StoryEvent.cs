using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DiplomskoDelo
{
    internal class StoryEvent : INotifyPropertyChanged
    {
        private string eventName; //ime dogodka
        private string timeAndDate; //čas dogajanja
        private List<Relation> interactions; //seznam relacij oz. interakcij znotraj tega dogodka
        private List<string> notes; //seznam dodatnih zapiskov

        //neobvezne - nullable vrednosti
#nullable enable
        private string? eventMap; // relativna sistemska pot do slike zemljevida
        private List<MapMarker>? storyEventMapMarkers; //seznam oznak na zemljevidu
#nullable disable

        private bool affectOthers; //ali dogodek vpliva na druge
        private bool isProactive; //ali je dogodek proaktiven ali reaktiven

        public StoryEvent(string name, string timeDate)
        {
            eventName = name;
            timeAndDate = timeDate;

            eventMap = "";
            storyEventMapMarkers = new List<MapMarker>();

            interactions = new List<Relation>();
            notes = new List<string>();
        }

        [JsonConstructor]
        public StoryEvent(string storyeventname, string StoryEventTime, bool affectothers, bool StoryEventProactivity, List<Relation> relations, List<string> StoryEventNotes, string StoryEventMapSource, List<MapMarker> mapmarkers)
        {
            eventName = storyeventname;
            timeAndDate = StoryEventTime;
            affectOthers = affectothers;
            isProactive = StoryEventProactivity;

            eventMap = StoryEventMapSource;
            storyEventMapMarkers = mapmarkers;

            interactions = relations;
            notes = StoryEventNotes;
        }

        public string StoryEventName
        {
            get
            {
                return eventName;
            }
            set
            {
                eventName = value;
                OnPropertyChanged("StoryEventName");
            }
        }

        public string StoryEventTime
        {
            get
            {
                return timeAndDate;
            }
            set
            {
                timeAndDate = value;
                OnPropertyChanged("StoryEventTime");
            }
        }

        public bool AffectOthers
        {
            get
            {
                return affectOthers;
            }
            set
            {
                affectOthers = value;
                OnPropertyChanged("AffectOthers");
            }
        }

        public bool StoryEventProactivity
        {
            get
            {
                return isProactive;
            }
            set
            {
                isProactive = value;
                OnPropertyChanged("StoryEventProactivity");
            }
        }

        public string StoryEventMapSource
        {
            get
            {
                return eventMap;
            }
            set
            {
                eventMap = System.IO.Path.GetFullPath(value);
                OnPropertyChanged("StoryEventMapSource");
            }
        }

        public List<Relation> Relations
        {
            get
            {
                return interactions;
            }
            set
            {
                interactions = value;
                OnPropertyChanged("Relations");
            }
        }

        public List<string> StoryEventNotes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
                OnPropertyChanged("StoryEventNotes");
            }
        }

        public List<MapMarker> MapMarkers
        {
            get
            {
                return storyEventMapMarkers;
            }
            set
            {
                storyEventMapMarkers = value; OnPropertyChanged("tMapMarkers");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}