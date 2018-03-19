using System;
using HorizontalListView.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;

[assembly: ExportRenderer(typeof(HorizontalListView.HorizontalListView), typeof(HorizontalListViewRenderer))]
namespace HorizontalListView.iOS
{
    public class HorizontalListViewRenderer : ViewRenderer<HorizontalListView, UICollectionView>
    {
        UICollectionViewFlowLayout _viewLayout;
        UICollectionView _collectionView;
        HorizontalListViewSource _source;

        protected override void OnElementChanged(ElementChangedEventArgs<HorizontalListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _viewLayout = new UICollectionViewFlowLayout();
                _viewLayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
                _viewLayout.SectionInset = new UIEdgeInsets(0, 0, 0, 0);
                _viewLayout.MinimumLineSpacing = 0;
                _viewLayout.MinimumInteritemSpacing = 0;
                _viewLayout.EstimatedItemSize = UICollectionViewFlowLayout.AutomaticSize;

                _collectionView = new UICollectionView(CGRect.Empty, _viewLayout);
                _collectionView.RegisterClassForCell(typeof(UICollectionViewCell), "WrapperCell");       

                SetNativeControl(_collectionView);

                _source = new HorizontalListViewSource(e.NewElement, _collectionView);
                _collectionView.Source = _source;

                _collectionView.ReloadData();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                foreach (UIView subview in Subviews)
                {
                    DisposeSubviews(subview);
                }

                _collectionView = null;

                _viewLayout.Dispose();
                _viewLayout = null;

                _source.Dispose();
                _source = null;
            }
            base.Dispose(disposing);
        }

        void DisposeSubviews(UIView view)
        {
            foreach (UIView subView in view.Subviews)
            {
                DisposeSubviews(subView);
            }

            view.RemoveFromSuperview();
            view.Dispose();
        }

    }
}
