using System.Windows;

namespace DiplomskoDelo
{
    /// <summary>
    /// Interaction logic for textInputWindow.xaml
    /// </summary>
    public partial class textInputWindow : Window
    {
        public textInputWindow(string input)
        {
            InitializeComponent();
            ResponseTextBox.Text = input;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ResponseTextBox.Focus();
        }

        public string EditedText
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ResponseTextBox.Text == "")
            {
                MessageBoxResult result = MessageBox.Show("Please enter some text");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}