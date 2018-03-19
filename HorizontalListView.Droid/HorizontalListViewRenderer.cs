using System;
using Android.Support.V7.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Xamarin.Forms;
using HorizontalListView.Droid;

[assembly: ExportRenderer(typeof(HorizontalListView.HorizontalListView), typeof(HorizontalListViewRenderer))]
namespace HorizontalListView.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class HorizontalListViewRenderer: ViewRenderer<HorizontalListView, RecyclerView>
    {
        LinearLayoutManager _layoutManager;
        HorizontalListViewAdapter _adapter;

        public HorizontalListViewRenderer(Context context):base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<HorizontalListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {

                var recyclerView = new RecyclerView(Context);
                _layoutManager = new LinearLayoutManager(Context);
                _layoutManager.Orientation = LinearLayoutManager.Horizontal;

                SetNativeControl(recyclerView);

                Control.Focusable = false;
                Control.DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
                Control.HorizontalScrollBarEnabled = true; //これやっても出ない

                _adapter = new HorizontalListViewAdapter(Context, e.NewElement, recyclerView);
                Control.SetAdapter(_adapter);

                recyclerView.SetLayoutManager(_layoutManager);

                _adapter?.NotifyDataSetChanged();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _adapter?.Dispose();
                _layoutManager?.Dispose();
                _adapter = null;
                _layoutManager = null;
            }
            base.Dispose(disposing);
        }

    }
}
