using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace DiplomskoDelo
{
    internal class storyViewModel : INotifyPropertyChanged
    {
        //main storage objects

        public List<StoryEvent> StoryTimeline { get => (List<StoryEvent>)_storyTimeline; set { _storyTimeline = value; OnPropertyChanged("StoryTimeline"); } }
        public List<Entity> CharacterList { get => (List<Entity>)_characterList; set { _characterList = value; OnPropertyChanged("CharacterList"); } }

        //currently active units
        public StoryEvent ActiveEvent { get => activeEvent; set { activeEvent = value; OnPropertyChanged("ActiveEvent"); } }

        public string ActiveAttribute { get => activeAttribute; set { activeAttribute = value; OnPropertyChanged("ActiveAttribute"); } }

        public Entity ActiveEntity { get => activeEntity; set { activeEntity = value; OnPropertyChanged("ActiveEntity"); } }

        //relation properties
        public string NewRelationName { get => newRelationName; set { newRelationName = value; OnPropertyChanged("NewRelationName"); } }

        public Entity ChosenEntity1 { get => chosenEntity1; set { chosenEntity1 = value; OnPropertyChanged("ChosenEntity1"); } }
        public Entity ChosenEntity2 { get => chosenEntity2; set { chosenEntity2 = value; OnPropertyChanged("ChosenEntity2"); } }
        public bool IsEntity1Group { get => isEntity1Group; set { isEntity1Group = value; OnPropertyChanged("IsEntity1Group"); } }
        public bool IsEntity2Group { get => isEntity2Group; set { isEntity2Group = value; OnPropertyChanged("IsEntity2Group"); } }
        public bool IsRelationSelfTargeted { get => isRelationSelfTargeted; set { isRelationSelfTargeted = value; OnPropertyChanged("IsRelationSelfTargeted"); } }

        public storyViewModel()
        {
            _storyTimeline = new List<StoryEvent>();
            _characterList = new List<Entity>();

            newRelationName = "";
            chosenEntity1 = chosenEntity2 = null;

            _characterList.Add(new Entity("tim"));
            _characterList[0].EntityAttributes.Add("jezn");
            _characterList[0].EntityImageSource = @"https://picsum.photos/id/237/300/300";
            _characterList.Add(new Entity("tom"));
            _characterList[1].EntityAttributes.Add("kmalu hin");
            _characterList[1].EntityImageSource = @"https://picsum.photos/id/238/300/300";
            _characterList.Add(new Entity("tam"));
            _characterList[2].EntityAttributes.Add("horny");
            _characterList[2].EntityImageSource = @"https://picsum.photos/id/239/300/300";

            _storyTimeline.Add(new StoryEvent("Prvi event", "prej", false, false));
            _storyTimeline[0].StoryEventRelations.Add(new Relation("prefuko", false, false, false, new Entity(_characterList[0].EntityName), new Entity(_characterList[1].EntityName)));
            _storyTimeline[0].StoryEventRelations.Add(new Relation("razjezil", false, false, true, new Entity(_characterList[0].EntityName), new Entity(_characterList[1].EntityName)));
            _storyTimeline[0].StoryEventNotes.Add("test 1");

            _storyTimeline.Add(new StoryEvent("Drugi event", "zaj", true, false));
            _storyTimeline[1].StoryEventRelations.Add(new Relation("izgubil", false, false, false, new Entity(_characterList[0].EntityName), new Entity(_characterList[1].EntityName)));
            _storyTimeline[1].StoryEventRelations.Add(new Relation("ubil", false, false, true, new Entity(_characterList[0].EntityName), new Entity(_characterList[1].EntityName)));
            _storyTimeline[1].StoryEventNotes.Add("test 2");
            _storyTimeline[1].StoryEventNotes.Add("test 3");

            _storyTimeline.Add(new StoryEvent("Tretji event", "pol", true, true));
            _storyTimeline[2].StoryEventRelations.Add(new Relation("pofuko", false, false, false, new Entity(_characterList[2].EntityName), new Entity(_characterList[0].EntityName)));
            _storyTimeline[2].StoryEventRelations.Add(new Relation("razjezil", false, false, true, new Entity(_characterList[0].EntityName), new Entity(_characterList[1].EntityName)));
            _storyTimeline[2].StoryEventNotes.Add("test 4");
            _storyTimeline[2].StoryEventNotes.Add("test 5");
            _storyTimeline[2].StoryEventNotes.Add("test 6");
        }

        public void AddStoryEvent(string title, string time, bool affectother, bool proactive)
        {
            if (_storyTimeline.Count == 0)
            {
                _storyTimeline.Add(new StoryEvent(title, time, affectother, proactive));
            }
            //if()
            else
            {
                _storyTimeline.Insert(((MainWindow)System.Windows.Application.Current.MainWindow).storyEventListBox.SelectedIndex + 1, new StoryEvent(title, time, affectother, proactive));
            }
        }

        public void EditEntityAttribute()
        {
            textInputUserControl txt = new textInputUserControl();
            string temp = ActiveAttribute;

            Window window = new Window
            {
                Title = "Note Editor",
                Content = txt
            };
            window.DataContext = this;
            window.ShowDialog();

            activeEntity.EntityAttributes[ActiveEntity.EntityAttributes.IndexOf(temp)] = ActiveAttribute;

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

        private ICommand mUpdater;
        private string newRelationName;
        private Entity chosenEntity1;
        private Entity chosenEntity2;
        private bool isEntity1Group;
        private bool isEntity2Group;
        private bool isRelationSelfTargeted;
        private IList<Entity> _characterList;
        private IList<StoryEvent> _storyTimeline;
        private StoryEvent activeEvent;
        private Entity activeEntity;
        private string activeAttribute;

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