using System;
using Xamarin.Forms;

namespace HorizontalListView
{
    public class HorizontalListView:ListView
    {
        public HorizontalListView(ListViewCachingStrategy strategy):base(strategy)
        {
            HasUnevenRows = true; //falseだと高さをいじられるのでtrue固定にする
        }

        public static BindableProperty ColumnWidthProperty =
            BindableProperty.Create(
                nameof(ColumnWidth),
                typeof(double),
                typeof(HorizontalListView),
                -1d,
                defaultBindingMode: BindingMode.OneWay
            );

        public double ColumnWidth
        {
            get { return (double)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

    }
}
