/* Title:           Create Vendor
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
using VendorsDLL;
using NewEventLogDLL;
using DataValidationDLL;

namespace Vendors
{
    /// <summary>
    /// Interaction logic for CreateVendor.xaml
    /// </summary>
    public partial class CreateVendor : Window
    {
        //setting the classes
        VendorsClass TheVendorsClass = new VendorsClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        ShowVendorNames ShowVendorName = new ShowVendorNames();

        //setting the data
        FindVendorByVendorPhoneNumberDataSet TheFindVendorByVendorPhoneNumberDataSet = new FindVendorByVendorPhoneNumberDataSet();

        public CreateVendor()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            ShowVendorName.Close();
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //setting up the local variables
            string strVendorName;
            string strPhoneNumber;
            string strContactName;
            string strErrorMessage = "";
            bool blnFatalError = false;
            int intRecordsReturned;
            bool blnThereIsAProblem;

            strVendorName = txtVendorName.Text;
            if(strVendorName == "")
            {
                blnFatalError = true;
                strErrorMessage += "Vendor Name Was Not Entered\n";
            }
            else
            {
                MainWindow.TheFindVendorByVendorNameDataSet = TheVendorsClass.FindVendorByVendorName(strVendorName);

                intRecordsReturned = MainWindow.TheFindVendorByVendorNameDataSet.FindVendorByVendorName.Rows.Count;

                if(intRecordsReturned > 0)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Vendor Exists\n";
                }
            }
            strPhoneNumber = txtPhoneNumber.Text;
            if(strPhoneNumber == "")
            {
                blnFatalError = true;
                strErrorMessage += "The Phone Number Was Not Entered\n";
            }
            else
            {
                blnThereIsAProblem = TheDataValidationClass.VerifyPhoneNumberFormat(strPhoneNumber);

                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Phone Number Is Not The Correct Format\nShould Be (xxx) xxx-xxx\n";
                }
                else
                {
                    TheFindVendorByVendorPhoneNumberDataSet = TheVendorsClass.FindVendorByVendorPhoneNumber(strPhoneNumber);

                    intRecordsReturned = TheFindVendorByVendorPhoneNumberDataSet.FindVendorByVendorPhoneNumber.Rows.Count;

                    if (intRecordsReturned > 0)
                    {
                        strErrorMessage += "The Phone Number Exists with Another Vendor\n";
                        blnFatalError = true;
                    }
                }
            }
            strContactName = txtContactName.Text;
            if(strContactName == "")
            {
                blnFatalError = true;
                strErrorMessage += "Contact Name Was Not Entered\n";
            }
            if(blnFatalError == true)
            {
                TheMessagesClass.ErrorMessage(strErrorMessage);
                return;
            }

            blnFatalError = TheVendorsClass.InsertNewVendor(strVendorName, strContactName, strPhoneNumber);

            if(blnFatalError == true)
            {
                TheMessagesClass.ErrorMessage("There Was a Massive Problem, Contact IT");
                return;
            }
            else
            {
                TheMessagesClass.InformationMessage("The Vendor Has Been Saved");
                txtContactName.Text = "";
                txtPhoneNumber.Text = "";
                txtVendorName.Text = "";
            }
        }

        private void txtVendorName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strVendorName;
            int intLength;

            strVendorName = txtVendorName.Text;
            intLength = strVendorName.Length;

            if(intLength > 2)
            {
                MainWindow.TheFindVendorByVendorNameDataSet = TheVendorsClass.FindVendorByVendorName(strVendorName);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowVendorName.Show();

            txtVendorName.Focus();
        }
    }
}
