using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZdravoCorp.Doktor.View.UserControls
{
    /// <summary>----------> NOTHING FROM THIS FOLDER IS USED! LEFT JUST IN CASE..SHOULD BE DELETED
    /// Interaction logic for txtBlockWithPlaceholder.xaml
    /// </summary>
    public partial class txtBlockWithPlaceholder : UserControl
    {
        public txtBlockWithPlaceholder()
        {
            InitializeComponent();
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)    //function for appearing and disappearing placeholder text
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                placeholder.Visibility = Visibility.Visible;
                return;
            }
            placeholder.Visibility = Visibility.Hidden;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            txtInput.Focus();
        }
    }
}
