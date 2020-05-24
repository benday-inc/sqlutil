using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Benday.Presentation;

namespace Benday.SqlServerUtilities.WpfUi.Controls
{
    public partial class RadioButtonList : UserControl
    {
        public RadioButtonList()
        {
            InitializeComponent();
        }

        public ObservableCollection<ISelectableItem> Items
        {
            get
            {
                ObservableCollection<ISelectableItem> viewModel =
                    this.DataContext as ObservableCollection<ISelectableItem>;

                if (viewModel == null)
                {
                    viewModel = new ObservableCollection<ISelectableItem>();

                    this.DataContext = viewModel;
                }

                return viewModel;
            }
        }

        private void RadioButton_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var senderAsRadioButton = sender as RadioButton;

            if (senderAsRadioButton != null)
            {
                senderAsRadioButton.IsChecked = true;
            }
        }
    }
}
