using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EnglishDX {
    public partial class ViewModel {
        Random rnd;

        MyWord _currentWord;
        MyWord _oldWord;
        NewWordTemplate _newWordToEnter;
        MyWord _currentWordForAllWordsGrid;
        bool _isCurrentWordExampleVisible;


        public static EngBaseEntities1 generalEntity;

        int _currentWordCount;
        int _selecteTabIndex;
        int _hardWordsCount;
        int _wordsBeforeEnd;


        Duration _currentDuration;

      



        DayStatItem _currentDayItem;

        ICommand _createNewWordCommand;
        ICommand _hadleKeyDownCommand;
        ICommand _myTestCommand;
        ICommand _updateDataCommand;
        ICommand _enterNewWordCommand;
        ICommand _openCurrentItemOnWeb;
        ICommand _saveChangesInWordsCommand;
        ICommand _resetCurrentWordsCountCommand;
        ICommand _deleteWordCommand;
        ICommand _pastingNewWordsCommand;
        ICommand _enterPastedWordsToBaseCommand;
        ICommand _updateIndexesCommand;
        ICommand _createNewCircleCommand;

    
  
      
        



        public ObservableCollection<MyWord> ListAllWords { get; set; }
        public ObservableCollection<MyWord> ListWordsForWork { get; set; }
        public ObservableCollection<DayStatItem> ListStatItems { get; set; }
        public List<string> ListLastAnswers { get; set; }
        public ObservableCollection<MyWord> ListWrongAnsweredWords { get; set; }
        public ObservableCollection<NewWordTemplate> ListWordsFromClipboard { get; set; }
        public ObservableCollection<DayStatItem> DayStatItems { get; set; }
        public ObservableCollection<MyWord> WordsBeforeEndList { get; set; }

        public MyWord CurrentWord {
            get { return _currentWord; }
            set {
                _currentWord = value;
                RaisePropertyChanged("CurrentWord");
            }
        }
        public MyWord OldWord {
            get { return _oldWord; }
            set {
                _oldWord = value;
                RaisePropertyChanged("OldWord");
            }
        }
        public NewWordTemplate NewWordToEnter {
            get { return _newWordToEnter; }
            set {
                _newWordToEnter = value;
                RaisePropertyChanged("NewWordToEnter");
            }
        }
        public MyWord CurrentWordForAllWordsGrid {
            get { return _currentWordForAllWordsGrid; }
            set {
                _currentWordForAllWordsGrid = value;
                RaisePropertyChanged("CurrentWordForAllWordsGrid");
            }
        }
        public DayStatItem CurrentDayItem {
            get { return _currentDayItem; }
            set {
                _currentDayItem = value;
                RaisePropertyChanged("CurrentDayItem");
            }
        }

        public int CurrentWordsCount {
            get { return _currentWordCount; }
            set {
                _currentWordCount = value;
                RaisePropertyChanged("CurrentWordsCount");
            }
        }
        public int SelectedTabIndex {
            get { return _selecteTabIndex; }
            set {
                _selecteTabIndex = value;
                RaisePropertyChanged("SelectedTabIndex");
            }
        }
        public bool IsCurrentWordExampleVisible {
            get { return _isCurrentWordExampleVisible; }
            set {
                _isCurrentWordExampleVisible = value;
                RaisePropertiesChanged("IsCurrentWordExampleVisible");
            }
        }
        public int HardWordsCount {
            get { return _hardWordsCount; }
            set { _hardWordsCount = value;
            RaisePropertiesChanged("HardWordsCount");
            }
        }



        public int WordsBeforeEnd {
            get { return _wordsBeforeEnd; }
            set { _wordsBeforeEnd = value;
            RaisePropertyChanged("WordsBeforeEnd");
            }
        }
        public Duration CurrentDuration {
            get { return _currentDuration; }
            set { _currentDuration = value;
            RaisePropertyChanged("CurrentDuration");
            }
        }


        public ICommand CreateNewWordCommand {
            get {
                if (_createNewWordCommand == null) {
                    _createNewWordCommand = new DelegateCommand(WorkCycle);
                }
                return _createNewWordCommand;
            }
        }
        public ICommand HandleKeyDownCommand {
            get {

                if (_hadleKeyDownCommand == null)
                    _hadleKeyDownCommand = new DelegateCommand<KeyEventArgs>(HandleKeyDownMethod);
                return _hadleKeyDownCommand;

            }

        }
        public ICommand UpdateDataCommand {
            get {
                if (_updateDataCommand == null)
                    _updateDataCommand = new DelegateCommand(UpdateAllData);
                return _updateDataCommand;
            }
        }
        public ICommand EnterNewWordCommand {
            get {
                if (_enterNewWordCommand == null)
                    _enterNewWordCommand = new DelegateCommand(EnterNewWord);
                return _enterNewWordCommand;
            }
        }
        public ICommand OpenCurrentItemOnWebCommand {
            get {
                if (_openCurrentItemOnWeb == null)
                    _openCurrentItemOnWeb = new DelegateCommand(OpenCurrentItemOnWeb);
                return _openCurrentItemOnWeb;
            }
        }

        public ICommand SaveChangesInWordsCommand {
            get {
                if (_saveChangesInWordsCommand == null)
                    _saveChangesInWordsCommand = new DelegateCommand(SaveChangesInWords);
                return _saveChangesInWordsCommand;
            }
        }
        public ICommand ResetCurrentWordsCommand {
            get {
                if (_resetCurrentWordsCountCommand == null)
                    _resetCurrentWordsCountCommand = new DelegateCommand(ResetCurrentWordsCount);
                return _resetCurrentWordsCountCommand;
            }
        }
        public ICommand DeleteWordCommand {
            get {
                if (_deleteWordCommand == null)
                    _deleteWordCommand = new DelegateCommand(DeleteWord);
                return _deleteWordCommand;
            }
        }
        public ICommand TestCommand {
            get {
                if (_myTestCommand == null)
                    _myTestCommand = new DelegateCommand(TestMethod);
                return _myTestCommand;
            }
        }
        public ICommand PastingNewWordsCommand {
            get {
                if (_pastingNewWordsCommand == null)
                    _pastingNewWordsCommand = new DelegateCommand(PastingNewWords);
                return _pastingNewWordsCommand; }
        }


        public ICommand EnterPastedWordsToBaseCommand {
            get {
                if (_enterPastedWordsToBaseCommand == null)
                    _enterPastedWordsToBaseCommand = new DelegateCommand(EnterPastedWordsToBase);
                return _enterPastedWordsToBaseCommand; }
        
        }
        public ICommand UpdateIndexesCommand {
            get {
                if (_updateIndexesCommand == null)
                    _updateIndexesCommand = new DelegateCommand(UpdateIndexes);
                return _updateIndexesCommand; }
            set {
                _updateIndexesCommand = value; }
        }

        public ICommand CreateNewCircleCommand {
            get {
                if (_createNewCircleCommand == null)
                    _createNewCircleCommand = new DelegateCommand(CreateNewCircle);
                return _createNewCircleCommand; }
            set { _createNewCircleCommand = value; }
        }

        IServiceContainer serviceContainer = null;
        protected IServiceContainer ServiceContainer {
            get {
                if (serviceContainer == null)
                    serviceContainer = new ServiceContainer(this);
                return serviceContainer;
            }
        }
        IServiceContainer ISupportServices.ServiceContainer { get { return ServiceContainer; } }
        IClearFilterService ClearFilterService { get { return ServiceContainer.GetService<IClearFilterService>(); } }


    }
}
