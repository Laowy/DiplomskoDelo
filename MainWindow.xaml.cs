using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace DiplomskoDelo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StoryViewModel VM;
        private Point lastMouseClick;
        private bool areWeAddingMapMarkers = false;

        public MainWindow()
        {
            InitializeComponent();
            //VM = new StoryViewModel();
            this.DataContext = VM;
            /*
            VM.ActiveEvent = VM.StoryTimeline[0]; //this sets the first event in the timeline as active
            storyEventListBox.SelectedIndex = 0;//this too
            */
        }

        public void UpdateAllMapmarkers()
        {
            mapCanvas.Children.Clear();
            if (VM != null)
            {
                if (VM.ActiveEvent != null)
                {
                    if (VM.ActiveEvent.MapMarkers.Count != 0)
                    {
                        foreach (var marker in VM.ActiveEvent.MapMarkers)
                        {
                            Ellipse ellipse = new Ellipse();
                            ellipse.Fill = Brushes.Black;
                            ellipse.Width = 10;
                            ellipse.Height = 10;
                            ellipse.StrokeThickness = 2;
                            ellipse.MouseLeftButtonDown += OnEllipseMouseLeftButtonDown;

                            mapCanvas.Children.Add(ellipse);

                            Canvas.SetLeft(ellipse, (int)(mapCanvas.ActualWidth * marker.MapMarkerRatioX));
                            Canvas.SetTop(ellipse, (int)(mapCanvas.ActualHeight * marker.MapMarkerRatioY));
                        }
                    }
                }
            }
        }

        public BitmapImage UrlToImageSource(string url)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(url);
            bitmapImage.EndInit();

            return bitmapImage;
        }

        private void storyEventListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (storyEventListBox.SelectedIndex != -1)
            {
                mapCanvas.Children.Clear();

                VM.ActiveEvent = VM.StoryEvents[storyEventListBox.SelectedIndex];
                UpdateAllMapmarkers();
            }
        }

        private void addRelationButton_Click(object sender, RoutedEventArgs e)
        {
            VM.AddNewRelationToStoryEvent();
            eventLogListBox.Items.Refresh();
        }

        private void charactersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VM.ActiveEntity = VM.Entitys[charactersListBox.SelectedIndex];
        }

        private void editMapImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
            if (ofd.ShowDialog() == true)
            {
                string relative = System.IO.Path.GetRelativePath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), ofd.FileName);
                VM.ActiveEvent.StoryEventMapSource = relative;

                //VM.ActiveEvent.StoryEventMapSource = ofd.FileName;
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
            if (extraNotesListBox.SelectedIndex != -1)
            {
                VM.ActiveNote = VM.ActiveEvent.StoryEventNotes[extraNotesListBox.SelectedIndex];
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
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
                VM.EditStoryEvent(dialog.EventNameText, dialog.EventTimeText);
            }
            storyEventListBox.Items.Refresh();
        }

        private void addMapMarkerButton_Click(object sender, RoutedEventArgs e)
        {
            areWeAddingMapMarkers = true;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateAllMapmarkers();
        }

        private void mapImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastMouseClick = e.GetPosition(mapCanvas);

            if (areWeAddingMapMarkers)
            {
                var dialog = new textInputWindow("");
                dialog.Title = "Marker title";
                dialog.Height = 300;
                dialog.Width = 300;

                if (dialog.ShowDialog() == true)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Fill = Brushes.Black;
                    ellipse.Width = 10;
                    ellipse.Height = 10;
                    ellipse.StrokeThickness = 2;
                    ellipse.MouseLeftButtonDown += OnEllipseMouseLeftButtonDown;

                    mapCanvas.Children.Add(ellipse);

                    Canvas.SetLeft(ellipse, lastMouseClick.X - (ellipse.Width / 2));
                    Canvas.SetTop(ellipse, lastMouseClick.Y - (ellipse.Height / 2));

                    VM.AddNewMapMarker(mapCanvas, lastMouseClick, dialog.EditedText);
                }

                areWeAddingMapMarkers = false;
            }
        }

        private void OnEllipseMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse marker = sender as Ellipse;
            var i = mapCanvas.Children.IndexOf(marker);
            VM.ActiveMapMarker = VM.ActiveEvent.MapMarkers[i];
        }

        private void addEventNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new textInputWindow("");
            if (dialog.ShowDialog() == true)
            {
                VM.AddNewEventNote(dialog.EditedText);
            }
            extraNotesListBox.Items.Refresh();
        }

        private void editEventNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new textInputWindow(VM.ActiveNote);
            if (dialog.ShowDialog() == true)
            {
                VM.EditEventNote(dialog.EditedText);
            }
            extraNotesListBox.Items.Refresh();
        }

        private void editMapMarkerButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new textInputWindow(VM.ActiveMapMarker.MapMarkerNote);
            if (dialog.ShowDialog() == true)
            {
                VM.EditMapMarkerNote(dialog.EditedText);
            }
        }

        private void moveEventForwardButton_Click(object sender, RoutedEventArgs e)
        {
            VM.MoveActiveEventForward();
            storyEventListBox.Items.Refresh();
        }

        private void moveEventBackButton_Click(object sender, RoutedEventArgs e)
        {
            VM.MoveActiveEventBack();
            storyEventListBox.Items.Refresh();
        }

        private void deleteEventButton_Click(object sender, RoutedEventArgs e)
        {
            VM.DeleteActiveEvent();
            storyEventListBox.Items.Refresh();
        }

        private void editRelationButton_Click(object sender, RoutedEventArgs e)
        {
            VM.EditRelation();
            eventLogListBox.Items.Refresh();
        }

        private void eventLogListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (eventLogListBox.SelectedIndex != -1)
            {
                VM.ActiveRelation = VM.ActiveEvent.Relations[eventLogListBox.SelectedIndex];
            }
        }

        private void deleteEventNoteButton_Click(object sender, RoutedEventArgs e)
        {
            VM.DeleteEventNote();
            extraNotesListBox.Items.Refresh();
        }

        private void deleteCharacterAttributeButton_Click(object sender, RoutedEventArgs e)
        {
            VM.DeleteEntityNote();
            characterNotesListBox.Items.Refresh();
        }

        private void deleteMapMarkerButton_Click(object sender, RoutedEventArgs e)
        {
            VM.DeleteMapmarker();
            UpdateAllMapmarkers();
        }

        private void deleteRelationButton_Click(object sender, RoutedEventArgs e)
        {
            VM.DeleteRelation();
            eventLogListBox.Items.Refresh();
        }

        private void saveFileButton_Click(object sender, RoutedEventArgs e)
        {
            VM.ActiveAttribute = null;
            VM.ActiveEntity = null;
            VM.ActiveEvent = null;
            VM.ActiveMapMarker = null;
            VM.ActiveNote = null;
            VM.ActiveRelation = null;
            //JSONconsoleBox.Text = JsonSerializer.Serialize<StoryViewModel>(VM);

            SaveFileDialog savefile = new SaveFileDialog();
            // set a default file name
            savefile.FileName = "unknown.txt";
            // set filters - this can be done in properties as well
            savefile.Filter = "Text files (*.txt)|*.txt|JSON files (*.json)|*.json";

            if (savefile.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName))
                    sw.WriteLine(JSONconsoleBox.Text);
            }
        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt|JSON files (*.json)|*.json";
            if (ofd.ShowDialog() == true)
            {
                //VM = new StoryViewModel();
                VM = JsonSerializer.Deserialize<StoryViewModel>(File.ReadAllText(ofd.FileName));
                /*
                VM = new StoryViewModel
                {
                    CharacterList = value.CharacterList,
                    StoryTimeline = value.StoryTimeline
                };
                */
                this.DataContext = VM;
                VM.ActiveEvent = VM.StoryEvents[0]; //this sets the first event in the timeline as active
                storyEventListBox.SelectedIndex = 0;//this too
                VM.ActiveAttribute = null;
                VM.ActiveEntity = null;
                VM.ActiveEvent = null;
                VM.ActiveMapMarker = null;
                VM.ActiveNote = null;
                VM.ActiveRelation = null;

                storyEventListBox.Items.Refresh();
                eventLogListBox.Items.Refresh();
                characterNotesListBox.Items.Refresh();
                charactersListBox.Items.Refresh();
                extraNotesListBox.Items.Refresh();
            }
        }
    }
}