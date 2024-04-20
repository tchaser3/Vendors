/* Title:           Edit Vendor
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
using DataValidationDLL;

namespace Vendors
{
    /// <summary>
    /// Interaction logic for EditVendor.xaml
    /// </summary>
    public partial class EditVendor : Window
    {
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        VendorsClass TheVendorsClass = new VendorsClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        public EditVendor()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //loading the combo box
                cboSelectActive.Items.Add("Select Active");
                cboSelectActive.Items.Add("Yes");
                cboSelectActive.Items.Add("No");

                //loading the controls
                txtVendorID.Text = Convert.ToString(MainWindow.TheFindVendorByVendorIDDataSet.FindVendorByVendorID[0].VendorID);
                txtVendorName.Text = MainWindow.TheFindVendorByVendorIDDataSet.FindVendorByVendorID[0].VendorName;
                txtPhoneNumber.Text = MainWindow.TheFindVendorByVendorIDDataSet.FindVendorByVendorID[0].PhoneNumber;
                txtContactName.Text = MainWindow.TheFindVendorByVendorIDDataSet.FindVendorByVendorID[0].ContactName;
                if (MainWindow.TheFindVendorByVendorIDDataSet.FindVendorByVendorID[0].Active == true)
                    cboSelectActive.SelectedIndex = 1;
                else
                    cboSelectActive.SelectedIndex = 2;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vendors // Edit Vendor // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int intVendorID;
            string strVendorName;
            string strPhoneNumber;
            string strContactName;
            bool blnActive = true;
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            string strErrorMessage = "";

            //data validation
            try
            {
                intVendorID = Convert.ToInt32(txtVendorID.Text);

                strVendorName = txtVendorName.Text;
                if(strVendorName == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "Vendor Name Not Entered\n";
                }
                strPhoneNumber = txtPhoneNumber.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyPhoneNumberFormat(strPhoneNumber);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Phone Number Is Not The Correct Format\n";
                }
                strContactName = txtContactName.Text;
                if(strContactName == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Contact Name Was Not Entered\n";
                }
                if(cboSelectActive.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Vendor is not Set for Active Or Not\n";
                }
                else
                {
                    if (cboSelectActive.SelectedIndex == 1)
                        blnActive = true;
                    else
                        blnActive = false;
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                blnFatalError = TheVendorsClass.UpdateVendor(intVendorID, strVendorName, strContactName, strPhoneNumber, blnActive);

                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage("There Has Been A Problem, Contact IT");
                }
                else
                {
                    TheMessagesClass.InformationMessage("The Vendor Has Been Updated");
                    Close();
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vendors // Edit Vendor //Save Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
