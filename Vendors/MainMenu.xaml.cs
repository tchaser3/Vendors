/* Title:           Main Menu
 * Date:            7-17-17
 * Author:          Terry Holmes */

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
using System.Windows.Shapes;

namespace Vendors
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();

        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //this will close the program
            TheMessagesClass.CloseTheProgram();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnCreateVendor_Click(object sender, RoutedEventArgs e)
        {
            CreateVendor CreateVendor = new CreateVendor();
            CreateVendor.Show();
            Close();
        }

        private void btnEditVendor_Click(object sender, RoutedEventArgs e)
        {
            SelectVendor SelectVendor = new SelectVendor();
            SelectVendor.Show();
            Close();
        }
    }
}
