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
using DevExpress.Xpf.Core;
using DevExpress.Xpf.NavBar;
using System.Diagnostics;
using DevExpress.Xpf.Grid;
using System.Windows.Threading;


namespace EnglishDX
{
    public partial class MainWindow : DXWindow
    {
        public MainWindow()
        {
            InitializeComponent();
          



            v = new ViewModel();
          

            this.DataContext = v;
             this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            layoutControl.Focus();
        }
        ViewModel v;

        private void LayoutGroup_PreviewKeyDown_1(object sender, KeyEventArgs e) {
            Debug.Print("temptemp");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
       //     gr2.ItemsSource = v.ListAllWords;
        }

        private void gr2_CurrentItemChanged_1(object sender, CurrentItemChangedEventArgs e) {
            GridControl gc = sender as GridControl;
            TableView tv = gc.View as TableView;
            int rH = tv.FocusedRowHandle+6;
            rH = Math.Min(gc.VisibleRowCount-1, rH);
            tv.ScrollIntoView(0);

            Dispatcher.BeginInvoke((Action)(() => {
                tv.ScrollIntoView(rH);
            }), DispatcherPriority.Input);

            //MyWord wrd = gc.GetRow(rH) as MyWord;
            //Debug.Print(wrd + " " + rH);

        }

        private void DXWindow_PreviewKeyDown_1(object sender, KeyEventArgs e) {

        }

   
    }


}
