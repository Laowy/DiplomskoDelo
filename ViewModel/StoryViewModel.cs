using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Input;
using System.ComponentModel;

using System.Windows;

using System.IO;
using System.Windows.Controls;
using System.Text.Json.Serialization;
using Microsoft.Win32;
using System.IO.Compression;

namespace DiplomskoDelo
{
    internal class StoryViewModel : INotifyPropertyChanged
    {
        //LASTNOSTI

        //main storage objects

        public List<StoryEvent> StoryEvents
        {
            get => (List<StoryEvent>)_storyTimeline;
            set { _storyTimeline = value; OnPropertyChanged("StoryEvents"); }
        }

        public List<Entity> Entitys
        {
            get => (List<Entity>)_characterList;
            set { _characterList = value; OnPropertyChanged("Entitys"); }
        }

        //currently active units

        public StoryEvent ActiveEvent
        {
            get => activeEvent;
            set { activeEvent = value; OnPropertyChanged("ActiveEvent"); }
        }

        public Entity ActiveEntity
        {
            get => activeEntity;
            set { activeEntity = value; OnPropertyChanged("ActiveEntity"); }
        }

        public Relation ActiveRelation
        {
            get => activeRelation;
            set { activeRelation = value; OnPropertyChanged("ActiveRelation"); }
        }

        public string ActiveAttribute
        {
            get => activeAttribute;
            set { activeAttribute = value; OnPropertyChanged("ActiveAttribute"); }
        }

        public string ActiveNote
        {
            get => activeNote;
            set { activeNote = value; OnPropertyChanged("ActiveNote"); }
        }

        public MapMarker ActiveMapMarker
        {
            get => activeMapMarker;
            set { activeMapMarker = value; OnPropertyChanged("ActiveMapMarker"); }
        }

        public StoryViewModel()
        {
            _storyTimeline = new List<StoryEvent>();
            _characterList = new List<Entity>();
        }

        [JsonConstructor]
        public StoryViewModel(List<StoryEvent> storyevents, List<Entity> entitys)
        {
            _storyTimeline = storyevents;
            _characterList = entitys;
        }

        public void AddNewMapMarker(Canvas canvas, System.Windows.Point click, string text)
        {
            double wide = canvas.ActualWidth;
            double high = canvas.ActualHeight;

            activeEvent.MapMarkers.Add(new MapMarker((float)(click.X / wide), (float)(click.Y / high), text));
        }

        public void EditMapMarkerNote(string text)
        {
            var temp = ActiveMapMarker;
            activeEvent.MapMarkers[activeEvent.MapMarkers.IndexOf(temp)].MapMarkerNote = activeMapMarker.MapMarkerNote = text;
        }

        public void AddNewEventNote(string text)
        {
            activeEvent.StoryEventNotes.Add(text);
        }

        public void EditEventNote(string text)
        {
            string temp = activeNote;
            activeEvent.StoryEventNotes[activeEvent.StoryEventNotes.IndexOf(temp)] = activeNote = text;
        }

        public void AddStoryEvent(string title, string time)
        {
            if (_storyTimeline.Count == 0)//prvi event dodamo
            {
                _storyTimeline.Add(new StoryEvent(title, time));
            }
            else if (_storyTimeline.IndexOf(activeEvent) + 1 == _storyTimeline.Count)//cist na koncu appendamo
            {
                _storyTimeline.Add(new StoryEvent(title, time));
            }
            else//neki vmes
            {
                _storyTimeline.Insert(_storyTimeline.IndexOf(activeEvent) + 1, new StoryEvent(title, time));
            }
        }

        private void SwapEvents(int firstIndex, int secondIndex)
        {
            StoryEvent tmp = _storyTimeline[firstIndex];
            _storyTimeline[firstIndex] = _storyTimeline[secondIndex];
            _storyTimeline[secondIndex] = tmp;
        }

        public void MoveActiveEventForward()
        {
            if (_storyTimeline.IndexOf(activeEvent) + 1 == _storyTimeline.Count)//cist na koncu
            {
                MessageBox.Show("Event is already at the end");
            }
            else if (_storyTimeline.Count == 1)
            {
                MessageBox.Show("What do you intend to swap exactly?");
            }
            else
            {
                SwapEvents(_storyTimeline.IndexOf(activeEvent), _storyTimeline.IndexOf(activeEvent) + 1);
            }
        }

        public void MoveActiveEventBack()
        {
            if (_storyTimeline.IndexOf(activeEvent) == 0)//cist na zacetku
            {
                MessageBox.Show("Event is already in the beninging");
            }
            else if (_storyTimeline.Count == 1)
            {
                MessageBox.Show("What do you intend to swap exactly?");
            }
            else
            {
                SwapEvents(_storyTimeline.IndexOf(activeEvent), _storyTimeline.IndexOf(activeEvent) - 1);
            }
        }

        public void DeleteActiveEvent()
        {
            if (_storyTimeline.Count == 0)//cist na zacetku
            {
                MessageBox.Show("Nothing to delete");
            }
            else if (_storyTimeline.Count == 1)
            {
                _storyTimeline.Remove(activeEvent);
            }
            else
            {
                if (_storyTimeline.IndexOf(activeEvent) == 0)
                {
                    ActiveEvent = _storyTimeline[_storyTimeline.IndexOf(activeEvent) + 1];//go one fwd
                    _storyTimeline.RemoveAt(_storyTimeline.IndexOf(activeEvent) - 1);//remove the prev one
                }
                else
                {
                    ActiveEvent = _storyTimeline[_storyTimeline.IndexOf(activeEvent) - 1];//go one back
                    _storyTimeline.RemoveAt(_storyTimeline.IndexOf(activeEvent) + 1);//remove the next one
                }
            }
        }

        public void DeleteEventNote()
        {
            ActiveEvent.StoryEventNotes.Remove(activeNote);
        }

        public void DeleteEntityNote()
        {
            ActiveEntity.EntityAttributes.Remove(activeAttribute);
        }

        public void DeleteMapmarker()
        {
            activeEvent.MapMarkers.Remove(activeMapMarker);
        }

        public void DeleteRelation()
        {
            activeEvent.Relations.Remove(activeRelation);
        }

        public void EditStoryEvent(string title, string time)
        {
            var temp = activeEvent;
            activeEvent.StoryEventName = title;
            activeEvent.StoryEventTime = time;
            _storyTimeline[_storyTimeline.IndexOf(temp)] = activeEvent;
        }

        public void AddNewEntityAttribute(string attribute)
        {
            ActiveEntity.EntityAttributes.Add(attribute);
        }

        public void EditEntityAttribute(string input)
        {
            string temp = ActiveAttribute;

            activeEntity.EntityAttributes[ActiveEntity.EntityAttributes.IndexOf(temp)] = ActiveAttribute = input;

            return;
        }

        public void AddNewRelationToStoryEvent()
        {
            RelationControlUI window = new RelationControlUI("", -1, -1, false);
            window.Width = 400;
            window.Height = 300;
            window.DataContext = this;
            window.ShowDialog();//returns when window is closed

            if (window.RelationName == "" || window.Entity1Index == -1)
            {
                MessageBoxResult result = MessageBox.Show("Relation creation failed");
            }
            else
            {
                if (window.Entity2Index == -1)
                {
                    ActiveEvent.Relations.Add
                          (
                     new Relation(
                          window.RelationName,
                          false,
                          false,
                          window.IsRelationSelfTargeted,
                          _characterList[window.Entity1Index],
                          new Entity("SELF")
                          )
                    );
                }
                else
                {
                    ActiveEvent.Relations.Add
                          (
                     new Relation(
                          window.RelationName,
                          false,
                          false,
                          window.IsRelationSelfTargeted,
                          _characterList[window.Entity1Index],
                          _characterList[window.Entity2Index]
                          )
                     );
                }
            }

            return;
        }

        public void EditRelation()
        {
            //iskanje pravega indeksa za entitete v seznamu
            int e1 = _characterList.IndexOf(_characterList.Where(
                    p => p.EntityName == activeRelation.FirstEntity.EntityName).FirstOrDefault()),
                e2 = _characterList.IndexOf(_characterList.Where(
                    p => p.EntityName == activeRelation.SecondEntity.EntityName).FirstOrDefault());

            //odpiranje okna za urejanje relacije
            RelationControlUI window = new RelationControlUI(activeRelation.RelationName, e1, e2, activeRelation.IsSelfTargeted);

            window.Width = 400;
            window.Height = 300;
            window.DataContext = this; //
            window.ShowDialog();//returns when window is closed

            if (window.RelationName == "" || window.Entity1Index == -1)//ce manjkajo podatki
            {
                MessageBoxResult result = MessageBox.Show("Relation modification failed");
            }
            else
            {
                //nastavljanje imena relacije, prve entitete in refleksivnosti
                activeRelation.RelationName = window.RelationName;
                activeRelation.FirstEntity = _characterList[window.Entity1Index];
                activeRelation.IsSelfTargeted = window.IsRelationSelfTargeted;

                if (ActiveRelation.IsSelfTargeted)
                {
                    activeRelation.SecondEntity = new Entity("SELF");//izjema v primeru refleksivnost
                }
                else
                {
                    activeRelation.SecondEntity = _characterList[window.Entity2Index];
                }
            }
            return;
        }

        public void AddNewEntity(string entityName)
        {
            _characterList.Add(new Entity(entityName));

            return;
        }

        public string ReadJson()
        {
            ActiveAttribute = null;
            ActiveEntity = null;
            ActiveEvent = null;
            ActiveMapMarker = null;
            ActiveNote = null;
            ActiveRelation = null;
            string ret = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                ret = File.ReadAllText(ofd.FileName);
            }
            return ret;
        }

        public string ReadCompressedJson()
        {
            ActiveAttribute = null;
            ActiveEntity = null;
            ActiveEvent = null;
            ActiveMapMarker = null;
            ActiveNote = null;
            ActiveRelation = null;
            string ret = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSB files (*.csb)|*.csb";
            if (ofd.ShowDialog() == true)
            {
                using (GZipStream instream = new GZipStream(File.OpenRead(ofd.FileName), CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(instream))
                    {
                        ret = reader.ReadToEnd();

                        reader.Close();
                    }
                }
            }
            return ret;
        }

        public void SaveJsonToFile(string json)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "story.json";
            savefile.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";

            if (savefile.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName))
                    sw.WriteLine(json);
            }
        }

        public void SaveJsonToCompressedFile(string json)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "story.csb";
            savefile.Filter = "Compressed storyboard files (*.csb)|*.csb";
            if (savefile.ShowDialog() == true)
            {
                using (GZipStream outStream = new GZipStream(File.OpenWrite(savefile.FileName), CompressionMode.Compress))
                {
                    using (StreamWriter sw = new StreamWriter(outStream))
                    {
                        sw.Write(json);
                        sw.Close();
                    }
                }
            }
        }

        //POLJA

        private ICommand mUpdater;

        //glavne podatkovne strukture

        private IList<StoryEvent> _storyTimeline;
        private IList<Entity> _characterList;

        //za trenutno izbrane objekte

        private StoryEvent activeEvent;
        private Entity activeEntity;
        private string activeAttribute;
        private string activeNote;
        private Relation activeRelation;
        private MapMarker activeMapMarker;

        [JsonIgnore]
        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private class Updater : ICommand
        {
            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
            }

            #endregion ICommand Members
        }
    }
}