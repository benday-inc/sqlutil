using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Benday.Presentation;

namespace Benday.SqlUtils.WpfUi.Controls
{
    public partial class CheckboxList : UserControl
    {
        public CheckboxList()
        {
            InitializeComponent();
        }

        public ObservableCollection<ISelectableItem> Items
        {
            get
            {
                ObservableCollection<ISelectableItem> viewModel = this.DataContext as ObservableCollection<ISelectableItem>;

                if (viewModel == null)
                {
                    viewModel = new ObservableCollection<ISelectableItem>();

                    this.DataContext = viewModel;
                }

                return viewModel;
            }
        }

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
          "LabelText", typeof(string), typeof(CheckboxListField), new PropertyMetadata(String.Empty));        
    }
}
