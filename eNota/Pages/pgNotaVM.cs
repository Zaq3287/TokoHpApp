using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace eNota.Pages
{
    public class pgNotaVM : BindProperty
    {
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // fields
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        private DateTime _dtOrder;
        private string _strBarang;
        private string _strIMEI;
        private string _strModal;
        private string _strHarga;
        private string _strTelephone;
        private string _strGaransiToko;
        private string _strGaransiResmi;
        private string _strPayment;
        private string _strMode;
        private string _strStatus;
        private string _strEntry = "";
        private int _intID = 0;
        public ObservableCollection<string> _lstMode;
        public ObservableCollection<string> _lstStatus;
        public ObservableCollection<string> _lstPayment;



        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // properties
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public ICommand comPrint { get; set; }
        public ICommand comClear { get; set; }
        public ICommand comSave { get; set; }

        public DateTime dtOrder { get { return _dtOrder; } set { _dtOrder = value; OnPropertyChanged("dtOrder"); } }
        public string strBarang { get { return _strBarang; } set { _strBarang = value; OnPropertyChanged("strBarang"); } }
        public string strIMEI { get { return _strIMEI; } set { _strIMEI = value; OnPropertyChanged("strIMEI"); } }
        public string strModal { get { return _strModal; } set { _strModal = value; OnPropertyChanged("strModal"); } }
        public string strHarga { get { return _strHarga; } set { _strHarga = value; OnPropertyChanged("strHarga"); } }
        public string strTelephone { get { return _strTelephone; } set { _strTelephone = value; OnPropertyChanged("strTelephone"); } }
        public string strGaransiToko { get { return _strGaransiToko; } set { _strGaransiToko = value; OnPropertyChanged("strGaransiToko"); } }
        public string strGaransiResmi { get { return _strGaransiResmi; } set { _strGaransiResmi = value; OnPropertyChanged("strGaransiResmi"); } }
        public string strPayment { get { return _strPayment; } set { _strPayment = value; OnPropertyChanged("strPayment"); } }
        public string strMode { get { return _strMode; } set { _strMode = value; OnPropertyChanged("strMode"); } }
        public string strStatus { get { return _strStatus; } set { _strStatus = value; OnPropertyChanged("strStatus"); } }
        public ObservableCollection<string> lstPayment { get { return _lstPayment; } set { _lstPayment = value; OnPropertyChanged("lstPayment"); } }
        public ObservableCollection<string> lstMode { get { return _lstMode; } set { _lstMode = value; OnPropertyChanged("lstMode"); } }
        public ObservableCollection<string> lstStatus { get { return _lstStatus; } set { _lstStatus = value; OnPropertyChanged("lstStatus"); } }

        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public pgNotaVM()
        {
            initCommands();
            initList();

            _strEntry = "New";
            getMode();
            getStatus();
            getPayment();
            doClear();
        }

        public pgNotaVM(int intID)
        {
            initCommands();
            initList();

            _strEntry = "Edit";
            _intID = intID;
            getMode();
            getStatus();
            getPayment();
            editData(intID);
        }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        private void initCommands()
        {
            comPrint = new Command(() => { doPrint(); });
            comClear = new Command(() => { doClear(); });
            comSave = new Command(() => { doSave(); });
        }

        private void initList()
        {
            lstMode = new ObservableCollection<string>();
            lstPayment = new ObservableCollection<string>();
            lstStatus = new ObservableCollection<string>();
        }

        private void getMode()
        {
            lstMode.Clear();
            lstMode.Add("New");
            lstMode.Add("Second");
            lstMode.Add("No warranty");
        }

        private void getStatus()
        {
            lstStatus.Clear();
            lstStatus.Add("Stock");
            lstStatus.Add("Sold");
        }

        private void getPayment()
        {
            lstPayment.Clear();
            lstPayment.Add("Tunai");
            lstPayment.Add("Transfer Bank");
            lstPayment.Add("E-Wallet");
        }

        private bool bolCheck()
        {
            if (strBarang.Length > 0 && strHarga.Length > 0 && strModal.Length > 0 && strHarga.Length > 0 && strGaransiResmi.Length > 0 && strGaransiToko.Length > 0)
            {
                try
                {
                    int number = 0;
                    number = Convert.ToInt32(strHarga);
                    number = Convert.ToInt32(strModal);
                    number = Convert.ToInt32(strGaransiToko);
                    number = Convert.ToInt32(strGaransiResmi);
                }
                catch
                {
                    Global.showMessage("Make sure the data is filled in correctly!");
                    return false;
                }
                return true;
            }
            else
            {
                Global.showMessage("Please fill in the data first!");
                return false;
            }
        }

        private async void blueToothPrint()
        {
            string strText = "";
            string strWarranty = "";
            
            strText += "\n" + dtOrder.ToString("dd-MMM-yyyy");
            strText += "\n\nBarang:";
            strText += "\n" + strBarang;
            strText += "\n\nIMEI:";
            strText += "\n" + strIMEI;
            strText += "\n\nTotal: Rp. " + Convert.ToInt32(strHarga).ToString("#,#") + ",-";
            strText += "\n\nPembayaran: " + strPayment;
            if (strMode == "New")
            {
                strText += "\n\nGaransi Toko*: " + strGaransiToko + " hari";
                strText += "\nGaransi Resmi**: " + strGaransiResmi + " bulan";
            }
            else if(strMode == "Second")
            {
                strText += "\n\nGaransi Toko*: " + strGaransiToko + " hari";
            }
            strText += "\n--------------------------------";
            strText += "\n" + Global.centerString("Thank You");
            strText += "\n" + Global.centerString("We hope you enjoy your purchase");
            strText += "\n" + Global.centerString("Have a good day :)");
            strText += "\n--------------------------------";
            if (strMode == "New")
            {
                strWarranty += "\n*Garansi toko tidak berlaku jika terjadi kerusakan atas kesalahan pengguna, garansi hanya berlaku jika ada cacat dari pabrik.";
                strWarranty += "\n**Garansi resmi bisa langsung menuju pusat servis sesuai dari merk barang tersebut.";
            }
            else if (strMode == "Second")
            {
                strWarranty += "\n*Garansi toko tidak berlaku jika terjadi kerusakan atas kesalahan pengguna, garansi hanya berlaku jika ada cacat dari pabrik.";
            }

            await Global.blueToothService.Print(Global.strDevice, strText, strWarranty);
        }

        private void saveData()
        {
            if (_strEntry == "New")
            {
                Global.dbStore.insertTable(new tbl_nota
                {
                    dtOrder = dtOrder,
                    strMode = strMode,
                    strStatus = strStatus,  
                    strBarang = strBarang,
                    strIMEI = strIMEI,  
                    strHarga = strHarga,
                    strModal = strModal,
                    strTelephone = strTelephone,
                    strPayment = strPayment,
                    strGaransiToko = strGaransiToko,
                    strGaransiResmi = strGaransiResmi
                });
            }
            else
            {
                Global.dbStore.updateTable(new tbl_nota
                {
                    intID = _intID,
                    dtOrder = dtOrder,
                    strMode = strMode,
                    strStatus=strStatus,    
                    strBarang = strBarang,
                    strIMEI = strIMEI,
                    strHarga = strHarga,
                    strModal = strModal,
                    strTelephone = strTelephone,
                    strPayment = strPayment,
                    strGaransiToko = strGaransiToko,
                    strGaransiResmi = strGaransiResmi
                });
            }
            
        }

        private void editData(int intID)
        {
            var tmp = Global.dbStore.getNota(intID);

            dtOrder = tmp.dtOrder;
            strBarang = tmp.strBarang;
            strIMEI = tmp.strIMEI;
            strHarga = tmp.strHarga;
            strModal = tmp.strModal;
            strStatus = tmp.strStatus;
            strTelephone = tmp.strTelephone;
            strGaransiToko = tmp.strGaransiToko;
            strGaransiResmi = tmp.strGaransiResmi;
            strMode = tmp.strMode;
            strPayment = tmp.strPayment;
        }

        private void doPrint()
        {
            if (Global.blueToothService.bolBlueToothEnable())
            {
                if (bolCheck())
                {
                    saveData();
                    blueToothPrint();
                    Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            else
            {
                Global.showMessage("Please turn on the bluetooth device first!");
            }
        }

        private void doSave()
        {
            if (bolCheck())
            {
                saveData();
                Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        private void doClear()
        {
            dtOrder = DateTime.Now;
            strBarang = "";
            strIMEI = "";
            strHarga = "";
            strModal = "";
            strTelephone = "";
            strGaransiToko = "3";
            strGaransiResmi = "12";
            strMode = "New";
            strStatus = "Stock";
            strPayment = "Tunai";
            _strEntry = "New";
        }

    }
}
