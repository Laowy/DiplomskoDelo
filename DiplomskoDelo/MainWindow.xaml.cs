using Microsoft.Win32;
using System;
using System.IO;
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
        private StoryViewModel VM = new StoryViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = VM;

            VM.ActiveEvent = VM.StoryTimeline[0]; //this sets the first event in the timeline as active
            storyEventListBox.SelectedIndex = 0;//this too
        }

        public BitmapImage UrlToImageSource(string url)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri("https://fbcdn-profile-a.akamaihd.net/hprofile-ak-prn2/187738_100000230436565_1427264428_q.jpg"); ;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        private void storyEventListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VM.ActiveEvent = VM.StoryTimeline[storyEventListBox.SelectedIndex];

            eventLogListBox.ItemsSource = VM.ActiveEvent.StoryEventRelations;//set event relation log
        }

        private void addRelationButton_Click(object sender, RoutedEventArgs e)
        {
            VM.AddNewRelationToStoryEvent();
            eventLogListBox.Items.Refresh();
        }

        private void charactersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VM.ActiveEntity = VM.CharacterList[charactersListBox.SelectedIndex];
        }

        private void editMapImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
            if (ofd.ShowDialog() == true)
            {
                VM.ActiveEvent.StoryEventMapSource = ofd.FileName;
            }
        }

        private void editCharacterAttributeButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new textInputWindow(VM.ActiveAttribute);
            if (dialog.ShowDialog() == true)
            {
                VM.EditEntityAttribute(dialog.EditedText);
            }

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
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

        private void editEntityButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new textInputWindow(VM.ActiveEntity.EntityName);
            if (dialog.ShowDialog() == true)
            {
                VM.ActiveEntity.EntityName = dialog.EditedText;
            }
            charactersListBox.Items.Refresh();
        }

        private void changeEntityImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
            if (ofd.ShowDialog() == true)
            {
                VM.ActiveEntity.EntityImageSource = ofd.FileName;
            }
        }

        private void addCharacterAttributeButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new textInputWindow("");
            if (dialog.ShowDialog() == true)
            {
                VM.AddNewEntityAttribute(dialog.EditedText);
            }
            characterNotesListBox.Items.Refresh();
        }

        private void addNewStoryEventButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new StoryEventEditorWindow("", "");
            if (dialog.ShowDialog() == true)
            {
                VM.AddStoryEvent(dialog.EventNameText, dialog.EventTimeText);
            }
            storyEventListBox.Items.Refresh();
        }

        private void editStoryEventButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new StoryEventEditorWindow(VM.ActiveEvent.StoryEventName, VM.ActiveEvent.StoryEventTime);
            if (dialog.ShowDialog() == true)
            {
                VM.AddStoryEvent(dialog.EventNameText, dialog.EventTimeText);
            }
            storyEventListBox.Items.Refresh();
        }
    }
}