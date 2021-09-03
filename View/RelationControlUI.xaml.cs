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
        public RelationControlUI(string relationName, int entity1index, int entity2index, bool isReflexive)
        {
            InitializeComponent();

            relationPreviewTextBlock.Text = isSavedTextBlock.Text = "";
            relationNameTextBox.Text = relationName;
            entity1ComboBox.SelectedIndex = entity1index;
            entity2ComboBox.SelectedIndex = entity2index;
            selfTargetedRelationCheckbox.IsChecked = isReflexive;
        }

        public int Entity1Index
        {
            get { return entity1ComboBox.SelectedIndex; }
            set { entity1ComboBox.SelectedIndex = value; }
        }

        public int Entity2Index
        {
            get { return entity2ComboBox.SelectedIndex; }
            set { entity2ComboBox.SelectedIndex = value; }
        }

        public string RelationName
        {
            get { return relationNameTextBox.Text; }
            set { relationNameTextBox.Text = value; }
        }

        public bool IsRelationSelfTargeted
        {
            get { return (bool)selfTargetedRelationCheckbox.IsChecked; }
            set { selfTargetedRelationCheckbox.IsChecked = value; }
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