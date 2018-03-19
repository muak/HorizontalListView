using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;

namespace HorizontalListView.iOS
{
    public class HorizontalListViewSource:UICollectionViewSource
    {
        IList _source;
        HorizontalListView _listView;
        UICollectionView _collectionView;
        ITemplatedItemsView<Cell> TemplatedItemsView => _listView;
        UITableView _dummyTableView;

        public HorizontalListViewSource(HorizontalListView listview,UICollectionView collectionView)
        {
            _listView = listview;
            _source = _listView.ItemsSource as IList;

            _collectionView = collectionView;

            var templatedItems = ((ITemplatedItemsView<Cell>)_listView).TemplatedItems;
            templatedItems.CollectionChanged += OnCollectionChanged;

            _dummyTableView = new UITableView(CGRect.Empty, UITableViewStyle.Grouped);
            _dummyTableView.RowHeight = UITableView.AutomaticDimension;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return _source.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = _listView.TemplatedItems[indexPath.Row];

            var id = cell.GetType().FullName;
            var renderer = (CellRenderer)Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IRegisterable>(cell.GetType());

            var innerReusableCell = _dummyTableView.DequeueReusableCell(id);
            var innerNativeCell = renderer.GetCell(cell, innerReusableCell, _dummyTableView);
           
            var reusableCell = collectionView.DequeueReusableCell("WrapperCell", indexPath) as UICollectionViewCell;
            foreach(var child in reusableCell.ContentView.Subviews){
                child.RemoveFromSuperview();
            }

            reusableCell.ContentView.AddSubview(innerNativeCell);

            innerNativeCell.TranslatesAutoresizingMaskIntoConstraints = false;
            innerNativeCell.TopAnchor.ConstraintEqualTo(reusableCell.ContentView.TopAnchor).Active = true;
            innerNativeCell.LeftAnchor.ConstraintEqualTo(reusableCell.ContentView.LeftAnchor).Active = true;
            innerNativeCell.BottomAnchor.ConstraintEqualTo(reusableCell.ContentView.BottomAnchor).Active = true;
            innerNativeCell.RightAnchor.ConstraintEqualTo(reusableCell.ContentView.RightAnchor).Active = true;
            reusableCell.ContentView.HeightAnchor.ConstraintEqualTo((System.nfloat)_listView.Height).Active = true;
            reusableCell.ContentView.WidthAnchor.ConstraintEqualTo((System.nfloat)_listView.ColumnWidth).Active = true;


            return reusableCell;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                var templatedItems = ((ITemplatedItemsView<Cell>)_listView).TemplatedItems;
                templatedItems.CollectionChanged -= OnCollectionChanged;

                _dummyTableView.Dispose();
                _dummyTableView = null;
                _source = null;
                _listView = null;
                _collectionView = null;
            }
            base.Dispose(disposing);
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_collectionView != null)
            {
                _collectionView.ReloadData();
            }
        }
    }
}
