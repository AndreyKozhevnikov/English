using DevExpress.Mvvm;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Threading;

namespace EnglishDX {
    [DebuggerDisplay("Word-{Word} Comp-{Complexity}, AlA-{AllAnswers}, LstRA-{LastRightAnswers}")]
    public class MyWord : BindableBase, IComparable //класс слово
    {
        public static int RightAnswersToComplete = 5;
        public static int FirstRightAnswersToComplete = 2;

        bool _isRightAnswer;
        bool _isChanged;
        public Datum parentWordEntity;


        #region properties
        public int ID {
            get { return parentWordEntity.id; }

        }
        public string Word {
            get { return parentWordEntity.Word; }
            set {
                parentWordEntity.Word = value;
                RaisePropertyChanged("Word");
                IsChanged = true;
            }
        }
        public string Translate {
            get { return parentWordEntity.Translate; }
            set {
                parentWordEntity.Translate = value;
                RaisePropertyChanged("Translate");
                IsChanged = true;
            }
        }
        public string Example {
            get { return parentWordEntity.Example; }
            set {
                parentWordEntity.Example = value;
                RaisePropertyChanged("Example");
                IsChanged = true;
            }
        }


        public int AllAnswers {
            get { return parentWordEntity.AllAnswers; }
            set { parentWordEntity.AllAnswers = value; }
        }
        public int AllRightAnswers {
            get { return parentWordEntity.AllRightAnswers; }
            set {
                parentWordEntity.AllRightAnswers = value;
                AllAnswers++;
                LastRightAnswers++;
            }
        }
        public int AllWrongAnswers {
            get { return parentWordEntity.AllWrongAnswers; }
            set {
                parentWordEntity.AllWrongAnswers = value;
                AllAnswers++;
                LastRightAnswers = 0;
            }
        }
        public int LastRightAnswers {
            get { return parentWordEntity.LastRightAnswers; }
            set {
                parentWordEntity.LastRightAnswers = value;
                RaisePropertyChanged("LastRightAnswers");
                IsChanged = true;
            }
        }
        public bool IsRightAnswer {
            get { return _isRightAnswer; }
            set {
                _isRightAnswer = value;
                RaisePropertyChanged("IsRightAnswer");
            }
        }
        public bool IsChanged {
            get { return _isChanged; }
            set {
                _isChanged = value;
                RaisePropertyChanged("IsChanged");
            }
        }
        public bool IsEasy {
            get { return parentWordEntity.IsEasy; }
            set {
                parentWordEntity.IsEasy = value;
                //   Hard = 0;
                IsChanged = true;

                RaisePropertyChanged("IsEasy");
            }
        }

        public bool IsAnswered {
            get {
                return parentWordEntity.IsAnswered;
            }
            set {
                parentWordEntity.IsAnswered = value;

            }
        }
        public int? RandomNumber {
            get {
                return parentWordEntity.RandomNumber;
            }
            set {
                parentWordEntity.RandomNumber = value;
            }

        }
        public int Complexity {
            get {
                return parentWordEntity.Complexity;
            }
            set {
                if (parentWordEntity.Complexity != value) {
                    parentWordEntity.Complexity = value;
                    //IsAnswered = false;
                    if (value == 1)
                        IsEasy = true;
                    else
                        IsEasy = false;
                }
            }
        }

        #endregion
        public MyWord() //конструктор
        {

            IsRightAnswer = true;

        }


        public MyWord(NewWordTemplate tmp) {
            parentWordEntity = ViewModel.generalEntity.Data.Create();
            ViewModel.generalEntity.Data.Add(parentWordEntity);
            this.Word = tmp.Word;
            this.Translate = tmp.Translate;
            this.Example = tmp.Example;
            Complexity = 2;
            RandomNumber = 1;
            IsRightAnswer = true;
            IsChanged = false;
            IsEasy = false;
            IsAnswered = false;
        }
        public void Save() //метод сохранения слова
        {
            ViewModel.GlobalSaveChanges();
            IsChanged = false;
        }

        public void GoToStat() {
            if (IsRightAnswer) {
                AllRightAnswers++;
                if (LastRightAnswers > RightAnswersToComplete) {
                    //Complexity = 1;
                    //IsAnswered = false;
                    IsAnswered = true;
                }
                if (LastRightAnswers >= FirstRightAnswersToComplete && LastRightAnswers == AllAnswers) {
                    Complexity = 1;
                    //   MessageBox.Show(this.Word);
                    IsAnswered = true;
                }
            }
            else {
                AllWrongAnswers++;
                Complexity = 2;
                //  IsAnswered = false;
            }

            Logs.Write(this, IsRightAnswer);
            //  InExam = false;

            //Save(); //

        }

        public void Del() //метод удаления
        {
            ViewModel.generalEntity.Data.Remove(parentWordEntity);
        }



        public void OpenInGoogleTranslate() {
            string st = string.Format("http://translate.google.com/#en/ru/{0}", Word);
            Process.Start(st);



            //   Debug.Print("start");
            AutomationElement element = AutomationElement.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
            element.SetFocus();
            if (element != null) {
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => {
                    Thread.Sleep(500);
                    element.SetFocus();
                }), DispatcherPriority.ApplicationIdle);

            }
            //   Process.wa

        }




        public int CompareTo(object obj) //метод сравнения слов
        {
            MyWord tmpWord = (MyWord)obj;
            if (tmpWord.Word == this.Word)
                return 0;
            else
                return 1;
        }

        public override string ToString() {
            return string.Format("{0} - {1} - {2}", Word, LastRightAnswers, AllAnswers);
        }
    }


}
