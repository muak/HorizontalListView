using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

namespace HorizontalListView.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class HorizontalListViewAdapter : RecyclerView.Adapter, AView.IOnClickListener
    {
        const int DefaultGroupHeaderTemplateId = 0;
        const int DefaultItemTemplateId = 1;

        Context _context;
        HorizontalListView _listView;
        RecyclerView _recyclerView;
        IList _source;
        ITemplatedItemsView<Cell> TemplatedItemsView => _listView;
        Dictionary<DataTemplate, int> _templateToId = new Dictionary<DataTemplate, int>();
        List<ViewHolder> _viewHolders = new List<ViewHolder>();

        int _dataTemplateIncrementer = 2;

        public HorizontalListViewAdapter(Context context, HorizontalListView listview, RecyclerView recyclerView)
        {
            _context = context;
            _listView = listview;
            _recyclerView = recyclerView;
            _source = _listView.ItemsSource as IList;

            var templatedItems = ((ITemplatedItemsView<Cell>)_listView).TemplatedItems;
            templatedItems.CollectionChanged += OnCollectionChanged;
        }

        public override int ItemCount => _source.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int GetItemViewType(int position)
        {
            DataTemplate itemTemplate = _listView.ItemTemplate;

            if (itemTemplate == null)
            {
                return DefaultItemTemplateId;
            }

            var selector = itemTemplate as DataTemplateSelector;
            if (selector != null)
            {
                object item = TemplatedItemsView.TemplatedItems.ListProxy[position];
                itemTemplate = selector.SelectTemplate(item, _listView);
            }
            int key;
            if (!_templateToId.TryGetValue(itemTemplate, out key))
            {
                _dataTemplateIncrementer++;
                key = _dataTemplateIncrementer;
                _templateToId[itemTemplate] = key;
            }
            return key;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = new LinearLayout(_context) { Orientation = Orientation.Vertical };
            var width = _listView.ColumnWidth >= 0d ?
                                 (int)_context.ToPixels(_listView.ColumnWidth) : ViewGroup.LayoutParams.WrapContent;

            view.LayoutParameters = new LinearLayout.LayoutParams(width, ViewGroup.LayoutParams.MatchParent);

            var viewHolder = new ViewHolder(view);

            _viewHolders.Add(viewHolder);

            return viewHolder;

        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AView nativeCell = null;
            var layout = (Android.Widget.LinearLayout)holder.ItemView;


            nativeCell = layout.GetChildAt(0);
            if (nativeCell != null)
            {
                layout.RemoveViewAt(0);
            }

            var formsCell = _listView.TemplatedItems[position];

            nativeCell = CellFactory.GetCell(formsCell, nativeCell, _recyclerView, _context, _listView);

            var minHeight = (int)_context.ToPixels(40);
            var minWidth = (int)_context.ToPixels(20);

            //it is neccesary to set both
            layout.SetMinimumHeight(minHeight);
            nativeCell.SetMinimumHeight(minHeight);

            layout.SetMinimumWidth(minWidth);
            nativeCell.SetMinimumWidth(minWidth);

            if (formsCell is ViewCell)
            {
                var vCell = formsCell as ViewCell;
                if (_listView.ColumnWidth < 0)
                {
                    var width = (int)_context.ToPixels(vCell.View.WidthRequest);
                    layout.SetMinimumWidth(width);
                    nativeCell.SetMinimumWidth(width);
                }
            }

            layout.AddView(nativeCell, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var templatedItems = ((ITemplatedItemsView<Cell>)_listView).TemplatedItems;
                templatedItems.CollectionChanged -= OnCollectionChanged;

                foreach (var holder in _viewHolders)
                {
                    holder.Dispose();
                }
                _viewHolders.Clear();
                _viewHolders = null;
                _templateToId.Clear();
                _templateToId = null;

                _context = null;
                _listView = null;
                _recyclerView = null;
                _source = null;

            }
            base.Dispose(disposing);
        }


        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_recyclerView != null)
            {
                NotifyDataSetChanged();
            }
        }

        public void OnClick(AView v)
        {
            //throw new NotImplementedException();
        }


    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal class ViewHolder : RecyclerView.ViewHolder
    {
        LinearLayout _body;
        public ViewHolder(AView view) : base(view)
        {
            _body = (Android.Widget.LinearLayout)view;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var child = _body.GetChildAt(0);
                child?.Dispose();
                _body = null;
                ItemView?.Dispose();
                ItemView = null;
            }
            base.Dispose(disposing);
        }
    }
}
