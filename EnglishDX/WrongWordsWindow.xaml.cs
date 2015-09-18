
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EnglishDX {
    /// <summary>
    /// Interaction logic for WrongWordsWindow.xaml
    /// </summary>
    public partial class WrongWordsWindow : Window {
        public WrongWordsWindow() {
            InitializeComponent();
            this.Loaded += WrongWordsWindow_Loaded;
            
        }

        void WrongWordsWindow_Loaded(object sender, RoutedEventArgs e) {
            ViewModel vm = this.DataContext as ViewModel;
            string st = string.Format("WrongWordsWindow - {0}, AllSecond - {1}",vm.ListWrongAnsweredWords.Count,vm.CurrentDuration.DurationSeconds);
            this.Title = st;
        }
    }
}
