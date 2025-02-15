﻿using System.ComponentModel;
using System.Text.Json.Serialization;

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

        //ne-JSON konstruktor
        public Relation(string name, bool isSelf, Entity first, Entity second)
        {
            if (isSelf)
            {
                secondEntity = new Entity("SELF");//posebna vrednost
            }
            else
            {
                secondEntity = second;
            }
            relationName = name;
            isSelfTargeted = isSelf;
            firstEntity = first;
        }

        [JsonConstructor]
        public Relation(string relationname, bool isfirstMany, bool issecondMany, bool isSelftargeted, Entity firstentity, Entity secondentity)
        {
            if (isSelftargeted)
            {
                secondEntity = new Entity("SELF");
            }
            else
            {
                secondEntity = secondentity;
            }
            isSelfTargeted = isSelftargeted;
            relationName = relationname;
            isFirstMany = isfirstMany;
            isSecondMany = issecondMany;
            firstEntity = firstentity;
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