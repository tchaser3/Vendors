/* Title:           show Vendor Names
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
using System.Windows.Threading;
using NewEventLogDLL;

namespace Vendors
{
    /// <summary>
    /// Interaction logic for ShowVendorNames.xaml
    /// </summary>
    public partial class ShowVendorNames : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();

        DispatcherTimer MyTimer = new DispatcherTimer();

        public ShowVendorNames()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void BeginTheProcess(object sender, EventArgs e)
        {
            UpdateGrid();
        }
        private void UpdateGrid()
        {
            dgrResults.ItemsSource = MainWindow.TheFindVendorByVendorNameDataSet.FindVendorByVendorName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();

            MyTimer.Tick += new EventHandler(BeginTheProcess);
            MyTimer.Interval = new TimeSpan(0, 0, 1);
            MyTimer.Start();
        }
    }
}
