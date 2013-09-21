using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CardBoard.Board.Views
{
    public sealed partial class FullScreenBoardControl : UserControl
    {
        public FullScreenBoardControl()
        {
            this.InitializeComponent();
        }

        private void ToDo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Any())
            //{
            //    Doing.SelectedItem = null;
            //    Done.SelectedItem = null;
            //}
        }

        private void Doing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Any())
            //{
            //    ToDo.SelectedItem = null;
            //    Done.SelectedItem = null;
            //}
        }

        private void Done_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Any())
            //{
            //    ToDo.SelectedItem = null;
            //    Doing.SelectedItem = null;
            //}
        }
    }
}
