using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DiplomskoDelo
{
    internal class Relation : INotifyPropertyChanged
    {
        //Polja
        private string relationName;//ime relacije v pretekliku

        private Entity firstEntity;//prva entiteta v relaciji (vir)

        private Entity secondEntity;//druga entiteta v relaciji (tarča)
                                    //nastavi se na posebno vrednost, v primeru refleksivne relacije

        private bool isSelfTargeted;//ali je relacija refleksivna (vir vpliva nase, ne na entiteto 2)

        private bool isFirstMany;// za opis števnosti entitete 1
        private bool isSecondMany;// za opis števnosti entitete 2

        //konstruktor
        public Relation(string name, bool firstMany, bool secondMany, bool isSelf, Entity first, Entity second)
        {
            if (isSelf)
            {
                secondEntity = new Entity("SELF");
            }
            else
            {
                secondEntity = second;
            }
            isSelfTargeted = isSelf;
            relationName = name;
            isFirstMany = firstMany;
            isSecondMany = secondMany;
            firstEntity = first;
        }

        public bool IsFirstMany
        {
            get
            {
                return isFirstMany;
            }
            set
            {
                isFirstMany = value;
                OnPropertyChanged("IsFirstMany");
            }
        }

        public bool IsSecondMany
        {
            get
            {
                return isSecondMany;
            }
            set
            {
                isSecondMany = value;
                OnPropertyChanged("IsSecondMany");
            }
        }

        public bool IsSelfTargeted
        {
            get
            {
                return isSelfTargeted;
            }
            set
            {
                isSelfTargeted = value;
                OnPropertyChanged("IsSelfTargeted");
            }
        }

        public string RelationName
        {
            get
            {
                return relationName;
            }
            set
            {
                relationName = value;
                OnPropertyChanged("RelationName");
            }
        }

        public Entity FirstEntity
        {
            get
            {
                return firstEntity;
            }
            set
            {
                firstEntity = value;
                OnPropertyChanged("FirstEntity");
            }
        }

        public Entity SecondEntity
        {
            get
            {
                return secondEntity;
            }
            set
            {
                secondEntity = value;
                OnPropertyChanged("SecondEntity");
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