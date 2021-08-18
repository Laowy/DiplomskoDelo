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
    /// Interaction logic for textInputUserControl.xaml
    /// </summary>
    public partial class textInputUserControl : UserControl
    {
        public textInputUserControl()
        {
            InitializeComponent();
        }

        private void saveTextButton_Click(object sender, RoutedEventArgs e)
        {
            saveLabel.Content = "Saved text, you may close the window";
        }
    }
}