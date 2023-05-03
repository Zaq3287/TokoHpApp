using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace eNota.Pages
{
    public class pgSettingsVM : BindProperty
    {
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // fields
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        private IList<string> _lstDevice;
        private string _strName = "";
        private string _strAddress = "";
        private string _strCity = "";
        private string _strTelephone = "";
        private string _strDevice = "";


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // properties
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public ICommand comRefresh { get; set; }
        public ICommand comSave { get; set; }
        public ICommand comBackup { get; set; }
        public ICommand comRestore { get; set; }

        public IList<string> lstDevice { get { return _lstDevice; } set { _lstDevice = value; OnPropertyChanged("lstDevice"); } }
        public string strName { get { return _strName; } set { _strName = value; OnPropertyChanged("strName"); } }
        public string strAddress { get { return _strAddress; } set { _strAddress = value; OnPropertyChanged("strAddress"); } }
        public string strCity { get { return _strCity; } set { _strCity = value; OnPropertyChanged("strCity"); } }
        public string strTelephone { get { return _strTelephone; } set { _strTelephone = value; OnPropertyChanged("strTelephone"); } }
        public string strDevice { get { return _strDevice; } set { _strDevice = value; OnPropertyChanged("strDevice"); } }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public pgSettingsVM()
        {
            initList();
            initCommands();

            initData();
        }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        private void initCommands()
        {
            comRefresh = new Command(() => { doRefresh(); });
            comSave = new Command(() => { doSave(); });
            comBackup = new Command(() => { doBackup(); });
            comRestore = new Command(() => { doRestore(); });
        }

        private void initList()
        {
            lstDevice = new ObservableCollection<string>();
        }

        private void initData()
        {
            if (Global.dbStore.recordExists("tbl_settings"))
            {
                tbl_settings tbl_Settings = Global.dbStore.getSettings();

                strName = tbl_Settings.strName;
                strAddress = tbl_Settings.strAddress;
                strCity = tbl_Settings.strCity;
                strTelephone = tbl_Settings.strTelephone;
            }
            else
            {
                strName = "";
                strAddress = "";
                strCity = "";
                strTelephone = "";
                strDevice = "";
            }
            getDeviceList(false);
        }

        private bool saveData()
        {
            bool bolResult = false;

            if (strName.Length == 0 || strAddress.Length == 0 || strCity.Length == 0 || strTelephone.Length == 0)
            {
                Global.showMessage("Please fill in the data first!");
            }
            else
            {
                if (strName.Length > 15)
                {
                    Global.showMessage("Name cannot exceed 15 characters!");
                    return false;
                }
                if (strAddress.Length > 32)
                {
                    Global.showMessage("Address cannot exceed 32 characters!");
                    return false;
                }
                if (strCity.Length > 32)
                {
                    Global.showMessage("City cannot exceed 32 characters!");
                    return false;
                }
                if (strTelephone.Length > 20)
                {
                    Global.showMessage("Telephone cannot exceed 20 characters!");
                    return false;
                }

                tbl_settings tbl_Settings = new tbl_settings
                {
                    intID = 0,
                    strName = strName,
                    strAddress = strAddress,
                    strCity = strCity,
                    strTelephone = strTelephone,
                    strDevice = strDevice
                };

                if (Global.dbStore.recordExists("tbl_settings"))
                {
                    Global.dbStore.updateTable(tbl_Settings);
                }
                else
                {
                    Global.dbStore.insertTable(tbl_Settings);
                }

                bolResult = true;
            }

            return bolResult;
        }

        private void getDeviceList(bool bolShowMessage = true)
        {
            if (Global.blueToothService.bolBlueToothEnable())
            {
                var list = Global.blueToothService.GetDeviceList();
                lstDevice.Clear();
                foreach (var item in list)
                {
                    lstDevice.Add(item);
                }
                strDevice = Global.strDevice;
            }
            else
            {
                if (bolShowMessage) Global.showMessage("Please turn on the bluetooth device first!");
            }
        }

        private void doRefresh()
        {
            getDeviceList();
        }

        private void doSave()
        {
            if (saveData())
            {
                Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        private void doBackup()
        {
            try
            {
                if (saveData())
                {
                    Global.dbStore.closeDB();
                    string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "eNota.db3");
                    string backupfile = Path.Combine(Global.path.getPathFolder(), "eNota.db3");
                    File.Copy(fileName, backupfile, true);
                    File.SetAttributes(backupfile, FileAttributes.Normal);

                    Global.dbStore.reConnectDB();
                    Application.Current.MainPage.Navigation.PopAsync();
                    Global.showMessage("Database backup successful in folder download!");
                }
            }
            catch
            {
                Global.showMessage("Backup failed, please allow access to storage!");
                Global.dbStore.reConnectDB();
            }
        }

        private async void doRestore()
        {
            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "eNota.db3");
                string sourcefile = Path.Combine(Global.path.getPathFolder(), "eNota.db3");
                if (File.Exists(sourcefile))
                {
                    var action = await App.Current.MainPage.DisplayAlert(Global.strTitle, "Are you sure to restore?", "Yes", "No");
                    if (action)
                    {
                        Global.dbStore.closeDB();
                        File.Copy(sourcefile, fileName, true);
                        File.SetAttributes(fileName, FileAttributes.Normal);

                        Global.dbStore.reConnectDB();
                        await Application.Current.MainPage.Navigation.PopAsync();
                        Global.showMessage("Database restore successful!");
                    }
                }
                else
                {
                    Global.showMessage("Backup file not found in folder download!");
                    Global.dbStore.reConnectDB();
                }
            }
            catch
            {
                Global.showMessage("Restore failed, please allow access to storage!");
                Global.dbStore.reConnectDB();
            }
        }
    }
}
