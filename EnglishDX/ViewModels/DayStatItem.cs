using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace EnglishDX {
        [DebuggerDisplay("Date = {DateStat}")]
    public class DayStatItem :BindableBase{
        public DayStatItem(DateTime _date) {
         //   generalEntity = _ent;
            parentDay =ViewModel.generalEntity.DayStats.Create();
            ViewModel.generalEntity.DayStats.Add(parentDay);
            DateStat = _date;
            LastAnswerTime = DateTime.Now;
        }
        public DayStatItem() {

        }
        public DayStatItem(DayStat _parent) {
            parentDay = _parent;
        }

        public DayStat parentDay;



        public int CompleteToday {
            get { return parentDay.CompleteToday; }
            set {
                parentDay.CompleteToday = value;
            RaisePropertiesChanged("CompleteToday");
            }
        }

        //public int ID { get; set; }
       



        public DateTime DateStat {
            get { return parentDay.DateStat; }
            set { parentDay.DateStat = value; }
        }
        public int AnswerAll {
            get { return parentDay.AnswerAll; }
            set { parentDay.AnswerAll = value;
            RaisePropertyChanged("AnswerAll");
            }
        }


        public int AnswerRight {
            get { return parentDay.AnswerRight; }
            set {
                parentDay.AnswerRight = value;
                AnswerAll++;
                RaisePropertiesChanged("AnswerRight");
            }
        }


        public int AnswerWrong {
            get { return parentDay.AnswerWrong; }
            set {
                parentDay.AnswerWrong = value;
                AnswerAll++;
                RaisePropertiesChanged("AnswerWrong");
            }
        }


        public DateTime LastAnswerTime {
            get { return parentDay.LastAnswerTime; }
            set { parentDay.LastAnswerTime = value;
            RaisePropertiesChanged("LastAnswerTime");
            }
        }
        public int CompleteApproaches {
            get { return parentDay.CompleteApproaches; }
            set {
                parentDay.CompleteApproaches = value;
                LastAnswerTime = DateTime.Now;
                RaisePropertiesChanged("CompleteApproaches");
            }
        }

        public override bool Equals(object obj) {
            if (obj is DayStatItem)
                return (obj as DayStatItem).DateStat == this.DateStat;
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

      

        public void GoToStat(MyWord _word) {

            bool isRight = _word.IsRightAnswer;

            if (isRight)
                AnswerRight++;
            else
                AnswerWrong++;

            if (_word.IsAnswered)
                CompleteToday++;
        }

        public void Save() {

            ViewModel.GlobalSaveChanges();
        }
    }
}
