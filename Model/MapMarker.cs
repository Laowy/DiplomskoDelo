using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomskoDelo
{
    internal class MapMarker : INotifyPropertyChanged
    {
        private float markerCoordX; //vsebuje na koliko % sirine je klik
        private float markerCoordY; //na koliko % visine je bil klik
        private string markerNote; //seznam imen na zemljevidu

        public MapMarker(float x, float y, string content)
        {
            markerCoordX = x;
            markerCoordY = y;
            markerNote = content;
        }

        public float MapMarkerRatioX
        {
            get
            {
                return markerCoordX;
            }
            set
            {
                markerCoordX = value;
                OnPropertyChanged("MapMarkerRatioX");
            }
        }

        public float MapMarkerRatioY
        {
            get
            {
                return markerCoordY;
            }
            set
            {
                markerCoordY = value;
                OnPropertyChanged("MapMarkerRatioY");
            }
        }

        public string MapMarkerNote
        {
            get
            {
                return markerNote;
            }
            set
            {
                markerNote = value;
                OnPropertyChanged("MapMarkerNote");
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