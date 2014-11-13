using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace CardBoard.Project.Views
{
    public interface IDialogControl
    {
        event RoutedEventHandler Ok;
        event RoutedEventHandler Cancel;

        double Width { get; set; }
        double Height { get; set; }
        object DataContext { get; set; }
    }
}
