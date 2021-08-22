using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;

namespace DiplomskoDelo
{
    internal class StoryViewModel : INotifyPropertyChanged
    {
        //main storage objects
        public Dictionary<string, BitmapImage> MapDictionary { get { return imageDict; } set { imageDict = value; OnPropertyChanged("ImageDictionary"); } }

        public List<System.Windows.Point> ScaledMapMarkers { get => scaledMapMarkers; set { scaledMapMarkers = value; OnPropertyChanged("ScaledMapMarkers"); } }
        public List<StoryEvent> StoryTimeline { get => (List<StoryEvent>)_storyTimeline; set { _storyTimeline = value; OnPropertyChanged("StoryTimeline"); } }
        public List<Entity> CharacterList { get => (List<Entity>)_characterList; set { _characterList = value; OnPropertyChanged("CharacterList"); } }

        //currently active units
        public StoryEvent ActiveEvent { get => activeEvent; set { activeEvent = value; OnPropertyChanged("ActiveEvent"); } }

        public Entity ActiveEntity { get => activeEntity; set { activeEntity = value; OnPropertyChanged("ActiveEntity"); } }

        public string ActiveAttribute { get => activeAttribute; set { activeAttribute = value; OnPropertyChanged("ActiveAttribute"); } }
        public string ActiveNote { get => activeNote; set { activeNote = value; OnPropertyChanged("ActiveNote"); } }

        public MapMarker ActiveMapMarker { get => activeMapMarker; set { activeMapMarker = value; OnPropertyChanged("ActiveMapMarker"); } }

        //relation properties
        public string NewRelationName { get => newRelationName; set { newRelationName = value; OnPropertyChanged("NewRelationName"); } }

        public Entity ChosenEntity1 { get => chosenEntity1; set { chosenEntity1 = value; OnPropertyChanged("ChosenEntity1"); } }
        public Entity ChosenEntity2 { get => chosenEntity2; set { chosenEntity2 = value; OnPropertyChanged("ChosenEntity2"); } }
        public bool IsEntity1Group { get => isEntity1Group; set { isEntity1Group = value; OnPropertyChanged("IsEntity1Group"); } }
        public bool IsEntity2Group { get => isEntity2Group; set { isEntity2Group = value; OnPropertyChanged("IsEntity2Group"); } }
        public bool IsRelationSelfTargeted { get => isRelationSelfTargeted; set { isRelationSelfTargeted = value; OnPropertyChanged("IsRelationSelfTargeted"); } }

        public StoryViewModel()
        {
            _storyTimeline = new List<StoryEvent>();
            _characterList = new List<Entity>();

            newRelationName = "";
            chosenEntity1 = chosenEntity2 = null;

            _characterList.Add(new Entity("Oseba 1"));
            _characterList[0].EntityAttributes.Add("Prva lastnost osebe 1");
            _characterList[0].EntityImageSource = @"C:\Users\Jakob\Desktop\FAXE\DiplomskoDelo\DiplomskoDelo\Images\Lixen token.png"; //relative path is ../Images/Lixen token.png
            _characterList.Add(new Entity("Oseba 2"));
            _characterList[1].EntityAttributes.Add("Pomemben podatek o osebi 2");
            _characterList[1].EntityImageSource = @"https://picsum.photos/id/238/300/300";
            _characterList.Add(new Entity("Zgodbi pomemben objekt"));
            _characterList[2].EntityAttributes.Add("Lastnost objekta");
            _characterList[2].EntityImageSource = @"https://picsum.photos/id/239/300/300";

            _storyTimeline.Add(new StoryEvent("Prvi prizor", "torek"));
            _storyTimeline[0].StoryEventRelations.Add(new Relation("poiskal", false, false, false, new Entity(_characterList[0].EntityName), new Entity(_characterList[2].EntityName)));
            _storyTimeline[0].StoryEventRelations.Add(new Relation("razjezil", false, false, true, new Entity(_characterList[0].EntityName), new Entity(_characterList[1].EntityName)));
            _storyTimeline[0].StoryEventNotes.Add("Podrobnost 1");

            _storyTimeline.Add(new StoryEvent("Drugi prizor", "petek"));
            _storyTimeline[1].StoryEventRelations.Add(new Relation("izgubil", false, false, false, new Entity(_characterList[0].EntityName), new Entity(_characterList[2].EntityName)));
            _storyTimeline[1].StoryEventRelations.Add(new Relation("ubil", false, false, false, new Entity(_characterList[0].EntityName), new Entity(_characterList[1].EntityName)));
            _storyTimeline[1].StoryEventNotes.Add("Podrobnost 2");
            _storyTimeline[1].StoryEventNotes.Add("Podrobnost 3");

            _storyTimeline.Add(new StoryEvent("Tretji prizor", "nedelja"));
            _storyTimeline[2].StoryEventRelations.Add(new Relation("žaloval", false, false, true, new Entity(_characterList[0].EntityName), new Entity(_characterList[1].EntityName)));
            _storyTimeline[2].StoryEventRelations.Add(new Relation("oživel", false, false, false, new Entity(_characterList[2].EntityName), new Entity(_characterList[0].EntityName)));
            _storyTimeline[2].StoryEventNotes.Add("Podrobnost 4");
            _storyTimeline[2].StoryEventNotes.Add("Podrobnost 5");
            _storyTimeline[2].StoryEventNotes.Add("Podrobnost 6");
        }

        public void AddNewMapMarker(Canvas canvas, System.Windows.Point click, string text)
        {
            canvas.Children.Clear();
            double wide = canvas.ActualWidth;
            double high = canvas.ActualHeight;

            activeEvent.StoryEventMapMarkers.Add(new MapMarker((float)(wide / click.X), (float)(high / click.Y), text));
        }

        public void UpdateAllMapMarkers(Canvas canvas, System.Windows.Point click)
        {
            canvas.Children.Clear();
            double wide = canvas.ActualWidth;
            double high = canvas.ActualHeight;
            for (int i = 0; i < activeEvent.StoryEventMapMarkers.Count; i++)
            {
            }
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

        public void EditStoryEvent(string title, string time)
        {
            var temp = activeEvent;
            activeEvent.StoryEventName = title;
            activeEvent.StoryEventTime = time;
            _storyTimeline[_storyTimeline.IndexOf(temp)] = activeEvent;
        }

        public void AddNewEntityAttribute(string attribute)
        {
            activeEntity.EntityAttributes.Add(attribute);
        }

        public void EditEntityAttribute(string input)
        {
            string temp = ActiveAttribute;

            activeEntity.EntityAttributes[ActiveEntity.EntityAttributes.IndexOf(temp)] = ActiveAttribute = input;

            return;
        }

        public void AddNewRelationToStoryEvent()
        {
            RelationControlUI window = new RelationControlUI();

            window.DataContext = this;
            window.ShowDialog();//returns when window is closed

            if (newRelationName == "" || chosenEntity1 == null)
            {
                MessageBoxResult result = MessageBox.Show("Relation creation failed");
            }
            else
            {
                ActiveEvent.StoryEventRelations.Add
                      (
                 new Relation(
                      newRelationName,
                      isEntity1Group,
                      isEntity2Group,
                      isRelationSelfTargeted,
                      chosenEntity1,
                      chosenEntity2
                      )
                );
                newRelationName = "";
                chosenEntity1 = chosenEntity2 = null;
            }

            ((MainWindow)Application.Current.MainWindow).eventLogListBox.ItemsSource = activeEvent.StoryEventRelations;

            return;
        }

        public void AddNewEntity(string entityName)
        {
            _characterList.Add(new Entity(entityName));

            return;
        }

        //LASTNOSTI
        private ICommand mUpdater;

        //glavne podatkovne strukture
        private IList<StoryEvent> _storyTimeline;

        private IList<Entity> _characterList;
        private Dictionary<string, BitmapImage> imageDict;

        //za trenutno izbrane objekte
        private StoryEvent activeEvent;

        private Entity activeEntity;
        private string activeAttribute;
        private string activeNote;

        //za relacijski vmesnik
        private string newRelationName;

        private Entity chosenEntity1;
        private Entity chosenEntity2;
        private bool isEntity1Group;
        private bool isEntity2Group;
        private bool isRelationSelfTargeted;
        private MapMarker activeMapMarker;
        private List<System.Windows.Point> scaledMapMarkers;

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