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
using System.Windows.Shapes;

namespace DiplomskoDelo
{
    /// <summary>
    /// Interaction logic for StoryEventEditorWindow.xaml
    /// </summary>
    public partial class StoryEventEditorWindow : Window
    {
        public StoryEventEditorWindow(string name, string time)
        {
            InitializeComponent();
            eventNameTextBox.Text = name;
            eventTimeTextBox.Text = time;
        }

        public string EventNameText
        {
            get { return eventNameTextBox.Text; }
            set { eventNameTextBox.Text = value; }
        }

        public string EventTimeText
        {
            get { return eventTimeTextBox.Text; }
            set { eventTimeTextBox.Text = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (eventNameTextBox.Text == "" || eventTimeTextBox.Text == "")
            {
                MessageBoxResult result = MessageBox.Show("Please fill in both fields");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}