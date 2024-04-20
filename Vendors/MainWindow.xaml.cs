/* Title:           Main Window - Vendors Program
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NewEventLogDLL;
using NewEmployeeDLL;
using VendorsDLL;
using DataValidationDLL;

namespace Vendors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        public static VerifyLogonDataSet TheVerifyLogonDataSet = new VerifyLogonDataSet();
        public static FindVendorByVendorNameDataSet TheFindVendorByVendorNameDataSet = new FindVendorByVendorNameDataSet();
        public static FindVendorByVendorIDDataSet TheFindVendorByVendorIDDataSet = new FindVendorByVendorIDDataSet();

        int gintNoOfMisses;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strValueForValidation;
            int intEmployeeID = 0;
            string strLastName;
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            string strErrorMessage = "";
            int intRecordsReturned;

            try
            {
                //data validation
                strValueForValidation = pbxEmployeeID.Password.ToUpper();
                strLastName = txtLastName.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Employee ID is not an Integer\n";
                }
                else
                {
                    intEmployeeID = Convert.ToInt32(strValueForValidation);
                }
                if (strLastName == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Last Name Was Not Entered\n";
                }
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                //getting the employee
                TheVerifyLogonDataSet = TheEmployeeClass.VerifyLogon(intEmployeeID, strLastName);

                intRecordsReturned = TheVerifyLogonDataSet.VerifyLogon.Rows.Count;

                if (intRecordsReturned == 0)
                {
                    LogonFailed();
                }
                else
                {
                    if (TheVerifyLogonDataSet.VerifyLogon[0].EmployeeGroup == "USERS")
                    {
                        LogonFailed();
                    }
                    else
                    {
                        MainMenu MainMenu = new MainMenu();
                        MainMenu.Show();
                        Hide();
                    }

                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Data Entry // Main Window // Sign In Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }
        private void LogonFailed()
        {
            gintNoOfMisses++;

            if (gintNoOfMisses == 3)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "There Have Been Three Attempts to Login Into Vehicle Data Entry");

                TheMessagesClass.ErrorMessage("There Have Been Three Attempts To Sign In\nThe Application Will Now Close");

                Application.Current.Shutdown();
            }
            else
            {
                TheMessagesClass.InformationMessage("You Have Failed the Sign In Process");
            }
        }
    }
}
