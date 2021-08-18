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
    /// Interaction logic for RelationControlUI.xaml
    /// </summary>
    public partial class RelationControlUI : Window
    {
        public RelationControlUI()
        {
            InitializeComponent();
            relationPreviewTextBlock.Text = isSavedTextBlock.Text = relationNameTextBox.Text = "";
            entity1ComboBox.SelectedIndex = -1;
            entity2ComboBox.SelectedIndex = -1;
        }

        private void previewRelationButton_Click(object sender, RoutedEventArgs e)
        {
            if (relationNameTextBox.Text == "" || entity1ComboBox.Text == "")
            {
                relationPreviewTextBlock.Text = "Please create your relation";
            }
            else
            {
                relationPreviewTextBlock.Text = entity1ComboBox.Text + " " + relationNameTextBox.Text;
                if ((bool)selfTargetedRelationCheckbox.IsChecked)
                {
                    relationPreviewTextBlock.Text += " SELF";
                }
                else
                {
                    relationPreviewTextBlock.Text += " " + entity2ComboBox.Text;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (relationNameTextBox.Text == "" || entity1ComboBox.Text == "")
            {
                relationPreviewTextBlock.Text = "Please create your relation";
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}