using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpf.Grid;
using DevExpress.Mvvm.UI;
using System.Windows;

namespace EnglishDX {
    interface IClearFilterService {
        void ClearFilter();
    }

    public class ClearFilterService :ServiceBase, IClearFilterService {


        public GridControl MyGridControl {
            get { return (GridControl)GetValue(MyGridControlProperty); }
            set { SetValue(MyGridControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyGridControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyGridControlProperty =
            DependencyProperty.Register("MyGridControl", typeof(GridControl), typeof(ClearFilterService), new PropertyMetadata(null));

        

        public void ClearFilter() {
            MyGridControl.FilterString = null;
        }
    }
}
