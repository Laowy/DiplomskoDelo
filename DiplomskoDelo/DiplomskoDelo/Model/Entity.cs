using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;

namespace DiplomskoDelo
{
    internal class Entity : INotifyPropertyChanged
    {
        private string entityName;
        private List<string> attributes;
        private string displayArtSource;

        public string EntityImageSource
        {
            get
            {
                return displayArtSource;
            }
            set
            {
                displayArtSource = value;
                OnPropertyChanged("EntityImageSource");
            }
        }

        public string EntityName
        {
            get
            {
                return entityName;
            }
            set
            {
                entityName = value;
                OnPropertyChanged("EntityName");
            }
        }

        public Entity(string name)
        {
            entityName = name;
            attributes = new List<string>();
        }

        public Entity(string name, string imgURL)
        {
            entityName = name;
            attributes = new List<string>();
            displayArtSource = imgURL;
        }

        public List<string> EntityAttributes
        {
            get
            {
                return attributes;
            }
            set
            {
                attributes = value;
                OnPropertyChanged("EntityAttributes");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}