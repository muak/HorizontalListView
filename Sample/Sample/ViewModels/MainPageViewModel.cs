using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Sample.ViewModels
{
	public class MainPageViewModel : BindableBase
	{
        public ObservableCollection<Hoge> ItemsSource { get; set; }

		public MainPageViewModel()
		{
            ItemsSource = new ObservableCollection<Hoge>(Shuffle());
		}

        List<Hoge> Shuffle()
        {
            var list = new List<Hoge>();

            var rand = new Random();
            for (var i = 0; i < 30; i++)
            {

                var r = rand.Next(10, 245);
                var g = rand.Next(10, 245);
                var b = rand.Next(10, 245);
                var color = Color.FromRgb(r, g, b);
                var w = rand.Next(80, 150);
                var h = rand.Next(30, 60);

                list.Add(new Hoge
                {
                    Name = $"#{r:X2}{g:X2}{b:X2}",
                    Color = color,
                    Width = w,
                    Height = h,
                });
            }

            return list;
        }

        Hoge GetNextItem()
        {
            var rand = new Random();
            var r = rand.Next(10, 245);
            var g = rand.Next(10, 245);
            var b = rand.Next(10, 245);
            var color = Color.FromRgb(r, g, b);
            var w = rand.Next(30, 100);
            var h = rand.Next(30, 60);

            return new Hoge
            {
                Name = $"#{r:X2}{g:X2}{b:X2}",
                Color = color,
                Width = w,
                Height = h,
            };
        }
	}

    public class Hoge
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}

