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
using ZdravoCorp.Doktor;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        //just for simulation for now...should be log IN here
        public MainWindow()
        {
            //commented part is used for simulation that i already logged in just so i dont type user and password always
            /*Doctor doc1 = new Doctor("doctor1", "doc1");  //logIn simulation
            InitializeComponent();
            //docInfo.Text = doc1.ToString(); //check if i loaded everything right*/
            LoginWindow win = new LoginWindow();
            win.Show();
        }

    }
}
