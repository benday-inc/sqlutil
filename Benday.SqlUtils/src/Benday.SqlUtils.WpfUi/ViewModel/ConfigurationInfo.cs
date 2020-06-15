/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Benday.SqlUtils.WpfUi"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

namespace Benday.SqlUtils.WpfUi.ViewModel
{
    public class ConfigurationInfo
    {

        public ConfigurationInfo()
        {

        }
    }
}