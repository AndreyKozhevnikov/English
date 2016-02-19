using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpf.Grid;
using DevExpress.Mvvm.UI;
using System.Windows;
using System.Windows.Threading;

namespace EnglishDX {
    interface IManageGridControlService {
        void ClearFilter();
        void ClearSearchString();
        void SetSearchPanelFocus();
    }

    public class ManageGridControlService :ServiceBase, IManageGridControlService {


        public GridControl MyGridControl {
            get { return (GridControl)GetValue(MyGridControlProperty); }
            set { SetValue(MyGridControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyGridControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyGridControlProperty =
            DependencyProperty.Register("MyGridControl", typeof(GridControl), typeof(ManageGridControlService), new PropertyMetadata(null));

        

        public void ClearFilter() {
            MyGridControl.FilterString = null;
        }



        public void ClearSearchString() {
            MyGridControl.View.SearchString = null;
        }

        public void SetSearchPanelFocus() {
            Dispatcher.BeginInvoke((Action)(() => {
                MyGridControl.View.SearchControl.Focus();
            }), DispatcherPriority.Input);
        }
    }
}
