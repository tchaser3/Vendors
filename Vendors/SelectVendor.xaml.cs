/* Title:           Select Vendor
 * Date:            7-18-17
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
using NewEventLogDLL;
using VendorsDLL;

namespace Vendors
{
    /// <summary>
    /// Interaction logic for SelectVendor.xaml
    /// </summary>
    public partial class SelectVendor : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        VendorsClass TheVendorsClass = new VendorsClass();

        FindVendorsSortedByVendorNameDataSet TheFindVendorsSortedByVendorNameDataSet = new FindVendorsSortedByVendorNameDataSet();

        public SelectVendor()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this will load the grid
            TheFindVendorsSortedByVendorNameDataSet = TheVendorsClass.FindVendorsSortedByVendorName();

            dgrVendors.ItemsSource = TheFindVendorsSortedByVendorNameDataSet.FindVendorsSortedByVendorName;
        }

        private void dgrVendors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;
            int intVendorID;

            try
            {
                intSelectedIndex = dgrVendors.SelectedIndex;

                if(intSelectedIndex > -1)
                {
                    intVendorID = TheFindVendorsSortedByVendorNameDataSet.FindVendorsSortedByVendorName[intSelectedIndex].VendorID;

                    MainWindow.TheFindVendorByVendorIDDataSet = TheVendorsClass.FindVendorByVendorID(intVendorID);

                    EditVendor EditVendor = new EditVendor();
                    EditVendor.ShowDialog();

                    TheFindVendorsSortedByVendorNameDataSet = TheVendorsClass.FindVendorsSortedByVendorName();

                    dgrVendors.ItemsSource = TheFindVendorsSortedByVendorNameDataSet.FindVendorsSortedByVendorName;
                }
                
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vendors // Select Vendor // Grid Selection Changed " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
