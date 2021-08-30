using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Benday.Presentation;
using System.Windows;

namespace Benday.SqlUtils.WpfUi.Controls
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

        public Orientation ItemsOrientation
        {
            get
            {
                var temp = (Orientation)this.GetValue(ItemsOrientationProperty);
                Console.WriteLine($"*** ITEMSORIENTATION.GET ****: {temp}");
                return temp;
            }
            set
            {
                Console.WriteLine($"*** ITEMSORIENTATION.SET ****: {value}");
                this.SetValue(ItemsOrientationProperty, value);
            }
        }

        public static readonly DependencyProperty ItemsOrientationProperty = DependencyProperty.Register(
          "ItemsOrientation", typeof(Orientation), typeof(RadioButtonList), new PropertyMetadata(Orientation.Vertical));
    }
}
