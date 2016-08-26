using DevExpress.Xpf.Core;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

using System.Runtime.CompilerServices;
using DevExpress.Xpf.Grid;
using System.Windows.Automation;



namespace EnglishDX {
    public partial class ViewModel :BindableBase, ISupportServices {
        public const int COUNTWORKFORONECYCLE = 50;
        public static bool IsTestMode = false;
        public ViewModel() {
            ConnectToDataBase();
            NewDay();
           
        }
        public void NewDay() {
            rnd = new Random();
            NewWordToEnter = new NewWordTemplate();
            Logs.Write("programm started");
            CreateNewDuration(); //говнокод
            UpdateStatistic();
            CreateDayStatItem(); //статистика по текущему дню
            StartWork();
            SelectedTabIndex = 1;
        }

        void SetRandomNumber(Datum dt, Random r) {
            dt.RandomNumber = r.Next();
        }

        private static void ConnectToDataBase() {
            string machineName = System.Environment.MachineName;
            if (IsTestMode) {
                if (machineName == "KOZHEVNIKOV-W10")
                    generalEntity = new EngBaseEntities1("EngBaseEntitiesWorkTest");
                else
                    generalEntity = new EngBaseEntities1("EngBaseEntitiesHomeTest");
                return;
            }
            if (machineName == "KOZHEVNIKOV-W10") {
                generalEntity = new EngBaseEntities1("EngBaseEntitiesWork");
            }
            else {
                generalEntity = new EngBaseEntities1("EngBaseEntitiesHome");
            }
          
        }

        private void StartWork() {
            if (CurrentDayItem == null || CurrentDayItem.DateStat != DateTime.Today)
                CreateDayStatItem();


            GetAllWords();
            GetWordsForWork();
            //  UpdateListLastAnswers();

            CreateNewWord();



        }

        private void ResetAllWords() {
            var v = ListAllWords.Where(w => w.IsAnswered == false).ToList();
            foreach (MyWord wrd in v) {
                wrd.IsAnswered = true;
            }
            GlobalSaveChanges();
        }

        private void UpdateAllData() {
            //ResetAllWords();
            UpdateStatistic();
            StartWork();
        }


        void CreateDayStatItem() {
            var v = DayStatItems.Where(x => x.DateStat == DateTime.Today).FirstOrDefault();
            if (v == null) {
                CurrentDayItem = new DayStatItem(DateTime.Today);
                DayStatItems.Insert(0, CurrentDayItem);
                RaisePropertiesChanged("DayStatItems");
            }
            else {
                CurrentDayItem = v;
            }

        }

        void UpdateStatistic() {
            var v = generalEntity.DayStats.OrderByDescending(x => x.DateStat).Select(x => new DayStatItem() { parentDay = x }).ToList();
            DayStatItems = new ObservableCollection<DayStatItem>(v);


        }

        void GetAllWords() {
            if (ListAllWords != null)
                ListAllWords.Clear();

            var v = ViewModel.generalEntity.Data.Select(x => new MyWord() { parentWordEntity = x }).ToList();
            ListAllWords = new ObservableCollection<MyWord>(v);
            RaisePropertiesChanged("ListAllWords");
        }
        void GetWordsForWork() {
            Logs.Write("create new round");
            GlobalSaveChanges();

            if (ListWordsForWork != null)
                ListWordsForWork.Clear();
            else
                ListWordsForWork = new ObservableCollection<MyWord>();
            List<MyWord> tmpList = null;

            var AllHardWords = ListAllWords.Where(x => x.Complexity == 2 && x.IsAnswered == false).OrderBy(x => x.RandomNumber);
            HardWordsCount = AllHardWords.Count();
            tmpList = AllHardWords.Take(COUNTWORKFORONECYCLE).ToList();
            if (tmpList.Count == 0) {
                MessageBox.Show("no word in hard mode" + ViewModel.IsTestMode.ToString());
            }
            ListWordsForWork = new ObservableCollection<MyWord>(tmpList);


            RaisePropertiesChanged("ListWordsForWork");
            ListWrongAnsweredWords = new ObservableCollection<MyWord>();


            UpdateListLastAnswers();
        }
        void UpdateListLastAnswers() {
            ListLastAnswers = new List<string>(); //
            var dtList = Enumerable.Range(0, 10).OrderByDescending(x => x).Select(x => x.ToString()).ToList();
            var tmpList = ListWordsForWork.GroupBy(x => x.LastRightAnswers).Select(g => new { resultDate = g.Key, resultValue = g.Count() }).OrderByDescending(n => n.resultDate).ToList();
            ListLastAnswers = (from dt in dtList
                               join answerResult in tmpList
                               on dt equals answerResult.resultDate.ToString()
                               into joined
                               from result in joined.DefaultIfEmpty()
                               // select new {c2 = cat.ToString(), c = cc!=null?cc.res.ToString():"0",  }
                               select string.Format("LastAnswers{0}: -- {1}", dt.ToString(), result != null ? result.resultValue.ToString() : "0")
                         ).ToList();
            WordsBeforeEndList = WordsBeforeEndListCound();
            WordsBeforeEnd = WordsBeforeEndList.Count;
            SelectedTabIndex = 1;

            RaisePropertiesChanged("ListLastAnswers");
        }
        private void RemoveOldWord() {
            if (OldWord != null) {
                OldWord.GoToStat();
                CurrentDayItem.GoToStat(OldWord);
                if (!OldWord.IsRightAnswer)
                    ListWrongAnsweredWords.Add(OldWord);
                CurrentWordsCount++;
                ListWordsForWork.Remove(OldWord);
                OldWord = null;

            }
        }
        private void AssignNewWordToOldWord() {
            if (CurrentWord != null) {
                OldWord = CurrentWord;
                if (ListWordsForWork.Count == (COUNTWORKFORONECYCLE - 1)) { //говнокод
                    CreateNewDuration();
                }

            }
            else {

                CountApproachesCount();
                CloseDuration();
                ShowWrongWords();
                GetWordsForWork();
            }
        }
        private void CreateNewWord() {
            if (ListWordsForWork.Count > 0) {
                int cur = rnd.Next(ListWordsForWork.Count);
                //  cur = 5;
                CurrentWord = ListWordsForWork[cur];
                CurrentWord.IsRightAnswer = true;
                ListWordsForWork.RemoveAt(cur);

            }
            else {
                CurrentWord = null;

            }
            IsCurrentWordExampleVisible = false;
        }
        private void CreateNewDuration() {
            CurrentDuration = generalEntity.Durations.Create();
            CurrentDuration.StartDateTime = DateTime.Now;
        }
        private void CloseDuration() {
            if (CurrentDuration == null)//говнокод
                return;
            CurrentDuration.DurationSeconds = (long)(DateTime.Now - CurrentDuration.StartDateTime).TotalSeconds;

            CurrentDuration.AnsweredWrong = ListWrongAnsweredWords.Count;
            generalEntity.Durations.Add(CurrentDuration);
        }
        void WorkCycle() {
            // GlobalSaveChanges();
            RemoveOldWord();
            AssignNewWordToOldWord();
            CreateNewWord();
        }






        void TestMethod() {
            CurrentWord.IsRightAnswer = false;
        }



        ObservableCollection<MyWord> WordsBeforeEndListCound() {
            var v0 = ListWordsForWork.Where(w => w.Complexity == 2 && w.AllAnswers == w.LastRightAnswers && w.AllAnswers == MyWord.FIRSTRIGHTANSWERSTOCOMPLETE - 1);
            // var v = ListWordsForWork.Where(w => w.Complexity == 2 && w.LastRightAnswers >= MyWord.RIGHTANSWERSTOCOMPLETE);
            var v1 = ListWordsForWork.Where(w => w.Complexity == 2 && w.LastRightAnswers >= MyWord.RIGHTANSWERSTOCOMPLETE);
            var v2 = v1.Concat(v0);

            return new ObservableCollection<MyWord>(v2);
        }






        void HandleKeyDownMethod(KeyEventArgs e) {
            switch (SelectedTabIndex) {
                case 0:
                    switch (e.Key) {
                        case Key.NumPad0:
                        case Key.D0:
                            if (IsCurrentWordExampleVisible || CurrentWord == null) {
                                WorkCycle();
                            }
                            else {
                                IsCurrentWordExampleVisible = true;
                                CurrentWord.IsRightAnswer = false;
                            }
                            break;
                        case Key.NumPad1:
                        case Key.D1:
                            WorkCycle();
                            break;
                        case Key.NumPad3:
                        case Key.D3:
                            if (OldWord == null)
                                return;
                            OldWord.IsRightAnswer = !OldWord.IsRightAnswer;
                            break;
                        case Key.NumPad4:
                        case Key.D4:
                            if (OldWord == null)
                                return;

                            CurrentWordForAllWordsGrid = OldWord;
                            SelectedTabIndex = 1;

                            break;

                        case Key.NumPad5:
                        case Key.D5:
                            if (OldWord != null)
                                OldWord.IsEasy = !OldWord.IsEasy;
                            break;
                        case Key.NumPad7:
                        case Key.D7:
                            if (OldWord == null)
                                return;
                            OldWord.OpenInGoogleTranslate();
                            break;
                        case Key.NumPad8:
                        case Key.D8:
                            CurrentWord.IsEasy = !CurrentWord.IsEasy;
                            WorkCycle();
                            break;
                        case Key.Add:
                        case Key.OemPlus:
                            IsCurrentWordExampleVisible = !IsCurrentWordExampleVisible;
                            if (IsCurrentWordExampleVisible)
                                CurrentWord.IsRightAnswer = false;
                            break;
                    }
                    break;
                case 1:
                    if ((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                        (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                        e.Key == Key.Decimal) {
                        e.Handled = true;
                    }

                    if (e.Key == Key.NumPad4 || e.Key == Key.D4) {
                        SelectedTabIndex = 0;
                    }
                    if (e.Key == Key.NumPad7 || e.Key == Key.D7) {
                        CurrentWordForAllWordsGrid.OpenInGoogleTranslate();
                    }
                    break;
            }
        }
        private void OnSelectedTabIndexChanged(int value) {
            if (ManageGridControlService == null)
                return;
            if (value == 1) {
                ManageGridControlService.SetSearchPanelFocus();
            }
            if (value == 0) {
                ManageGridControlService.ClearSearchString();
                ManageGridControlService.ClearFilter();
            }
        }

        private void CountApproachesCount() {
            if (CurrentWordsCount > 20) {
                CurrentDayItem.CompleteApproaches++;
            }

            CurrentWordsCount = 0;
        }

        private void ShowWrongWords() {
            if (ListWrongAnsweredWords != null && ListWrongAnsweredWords.Count > 0) {
                WrongWordsWindow WrongWordsWindow = new WrongWordsWindow();
                WrongWordsWindow.DataContext = this;
                WrongWordsWindow.ShowDialog();
            }
        }

        public static void GlobalSaveChanges() {
            try {
                ViewModel.generalEntity.SaveChanges();
            }
            catch (Exception e) {
                string st = e.Message;
                MessageBox.Show("Not saved!  " + e.Message);
            }
        }

        void EnterNewWord() {
            MyWord wrd = new MyWord(NewWordToEnter);
            ListAllWords.Add(wrd);
            ListWordsForWork.Add(wrd);
            CurrentWordForAllWordsGrid = wrd;
            NewWordToEnter = new NewWordTemplate();
        }
        void OpenCurrentItemOnWeb() {
            CurrentWordForAllWordsGrid.OpenInGoogleTranslate();
        }
        void SaveChangesInWords() {
            GlobalSaveChanges();
        }

        void ResetCurrentWordsCount() {
            CurrentDayItem.CompleteApproaches++;
            CurrentWordsCount = 0;
        }
        void DeleteWord() {
            if (DXMessageBox.Show(string.Format("Do you really want to delete-' {0} ' word?", CurrentWordForAllWordsGrid.Word), "delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                CurrentWordForAllWordsGrid.Del();
                ListWordsForWork.Remove(CurrentWordForAllWordsGrid);
                ListAllWords.Remove(CurrentWordForAllWordsGrid);
            }
        }




        void PastingNewWords() {
            ListWordsFromClipboard = GetWordsFromClipboard();
            RaisePropertiesChanged("ListWordsFromClipboard");
        }
        void EnterPastedWordsToBase() {
            if (ListWordsFromClipboard == null)
                return;
            foreach (NewWordTemplate wrdTemplate in ListWordsFromClipboard) {
                MyWord wrd = new MyWord(wrdTemplate);
            }
            ListWordsFromClipboard.Clear();
            GlobalSaveChanges();

            // StartWork();
            UpdateAllData();
        }


        private ObservableCollection<NewWordTemplate> GetWordsFromClipboard() {
            string clipboardData = Clipboard.GetText();
            ObservableCollection<NewWordTemplate> results = new ObservableCollection<NewWordTemplate>();
            string[] listString = clipboardData.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string oneline in listString) {
                string[] wordMembers = oneline.Split(new string[] { "\t" }, StringSplitOptions.None);
                if (wordMembers.Count() == 3) {
                    string word = wordMembers[0];
                    string translate = wordMembers[1];
                    string example = wordMembers[2];

                    NewWordTemplate wrdTemplate = new NewWordTemplate();
                    wrdTemplate.Word = word;
                    wrdTemplate.Translate = translate;
                    wrdTemplate.Example = example;

                    results.Add(wrdTemplate);
                }
            }
            return results;
        }

        private void UpdateIndexes() {
            Random r = new Random(DateTime.Now.Millisecond);
            var dts = generalEntity.Data.Where(x => x.Complexity == 2).ToList();
            dts.Select(x => x.RandomNumber = r.Next(1, 10000)).ToList();
            generalEntity.SaveChanges();
        }
        private void CreateNewCircle() {
            var lst = ListAllWords.Where(x => x.Complexity == 2).ToList();
            foreach (var wrd in lst) {
                wrd.IsAnswered = false;
                wrd.LastRightAnswers = 0;
                wrd.AllAnswers = 0;
                var str = "complon" + DateTime.Now.ToShortDateString();
                wrd.Tag = str;
                wrd.AnswerHistory = null;
            }
            GlobalSaveChanges();
        }



    }





}
