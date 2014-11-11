using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace CardBoard.Project.Views
{
    public sealed partial class NewProjectControl : UserControl
    {
        public event RoutedEventHandler Ok;
        public event RoutedEventHandler Cancel;

        public NewProjectControl()
        {
            this.InitializeComponent();
        }

        private void NewProject_Loaded(object sender, RoutedEventArgs e)
        {
            IdentifierTextBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            IdentifierTextBox.SelectAll();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Ok != null)
                Ok(sender, e);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel != null)
                Cancel(sender, e);
        }
    }
}
