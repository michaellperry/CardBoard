﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CardBoard.Project.Views
{
    public sealed partial class NewProjectControl : UserControl, IDialogControl
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
