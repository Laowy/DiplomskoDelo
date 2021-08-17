using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiplomskoDelo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private storyViewModel VM = new storyViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = VM;
            storyEventListBox.ItemsSource = VM.StoryTimeline;//set timeline
                                                             // charactersListBox.ItemsSource = VM.CharacterList;//set characters

            VM.ActiveEvent = VM.StoryTimeline[0]; //this sets the first event in the timeline as active
            storyEventListBox.SelectedIndex = 0;//this too

            // eventLogListBox.ItemsSource = VM.ActiveEvent.StoryEventRelations;//set event relation log
            extraNotesListBox.ItemsSource = VM.ActiveEvent.StoryEventNotes;//set notes for current event
        }

        private void storyEventListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VM.ActiveEvent = VM.StoryTimeline[storyEventListBox.SelectedIndex];

            eventLogListBox.ItemsSource = VM.ActiveEvent.StoryEventRelations;//set event relation log

            extraNotesListBox.ItemsSource = VM.ActiveEvent.StoryEventNotes;//set notes for current event
        }

        private void addRelationButton_Click(object sender, RoutedEventArgs e)
        {
            VM.AddNewRelationToStoryEvent();
            eventLogListBox.Items.Refresh();
        }

        private void charactersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VM.ActiveEntity = VM.CharacterList[charactersListBox.SelectedIndex];
            // characterNotesListBox.ItemsSource = VM.ActiveEntity.EntityAttributes;
        }

        private void editMapImageButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void editCharacterAttributeButton_Click(object sender, RoutedEventArgs e)
        {
            VM.EditEntityAttribute();
            characterNotesListBox.Items.Refresh();
        }

        private void characterNotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (characterNotesListBox.SelectedIndex != -1)
            {
                VM.ActiveAttribute = VM.ActiveEntity.EntityAttributes[characterNotesListBox.SelectedIndex];
            }
        }

        private void extraNotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            extraNotesListBox.ItemsSource = VM.ActiveEvent.StoryEventNotes;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MapTab.IsSelected)
            {
                charactersListBox.ItemsSource = VM.CharacterList;
                eventLogListBox.ItemsSource = VM.ActiveEvent.StoryEventRelations;
                extraNotesListBox.ItemsSource = VM.ActiveEvent.StoryEventNotes;
            }
            else if (CharactersTab.IsSelected)
            {
                charactersListBox.ItemsSource = VM.CharacterList;
                eventLogListBox.ItemsSource = VM.ActiveEvent.StoryEventRelations;
                extraNotesListBox.ItemsSource = VM.ActiveEvent.StoryEventNotes;
            }
            else if (EventLogTab.IsSelected)
            {
                charactersListBox.ItemsSource = VM.CharacterList;
                eventLogListBox.ItemsSource = VM.ActiveEvent.StoryEventRelations;
                extraNotesListBox.ItemsSource = VM.ActiveEvent.StoryEventNotes;
            }
            else if (NotesTab.IsSelected)
            {
                charactersListBox.ItemsSource = VM.CharacterList;
                eventLogListBox.ItemsSource = VM.ActiveEvent.StoryEventRelations;
                extraNotesListBox.ItemsSource = VM.ActiveEvent.StoryEventNotes;
            }
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            eventLogListBox.ItemsSource = VM.ActiveEvent.StoryEventRelations;
        }

        private void addNewEntityButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new textInputWindow("");
            if (dialog.ShowDialog() == true)
            {
                VM.AddNewEntity(dialog.EditedText);
            }
            charactersListBox.Items.Refresh();
        }
    }
}