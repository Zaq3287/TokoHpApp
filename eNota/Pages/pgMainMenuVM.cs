using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace eNota.Pages
{
    public class pgMainMenuVM : BindProperty
    {
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // fields
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        private string _strTitle;



        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // properties
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public ICommand comEntry { get; set; }
        public ICommand comSettings { get; set; }
        public ICommand comSold { get; set; }
        public ICommand comStock { get; set; }
        public ICommand comTest { get; set; }
        public ICommand comAbout { get; set; }

        public string strTitle { get { return _strTitle; } set { _strTitle = value; OnPropertyChanged("strTitle"); } }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public pgMainMenuVM()
        {
            initCommands();


            if (Global.dbStore.recordExists("tbl_settings"))
            {
                tbl_settings tmp = Global.dbStore.getSettings();
                strTitle = tmp.strName;
            }
            else
            {
                strTitle = "Welcome to e-Nota";
            }
        }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        private void initCommands()
        {
            comEntry = new Command(() => { doEntry(); });
            comSettings = new Command(() => { doSettings(); });
            comSold = new Command(() => { doSold(); });
            comStock = new Command(() => { doStock(); });
            comTest = new Command(() => { doTest(); });
            comAbout = new Command(() => { doAbout(); });
        }

        private void doEntry()
        {
            if (Global.dbStore.recordExists("tbl_settings"))
            {
                var page = new pgNota();
                page.BindingContext = new pgNotaVM();

                App.Current.MainPage.Navigation.PushAsync(page, false);
            }
            else
            {
                Global.showMessage("Please fill in the data settings first!");
            }
            
        }

        private void doSettings()
        {
            var page = new pgSettings();
            page.BindingContext = new pgSettingsVM();

            App.Current.MainPage.Navigation.PushAsync(page, false);
        }

        private void doSold()
        {
            if (Global.dbStore.recordExists("tbl_nota", "strStatus = 'Sold'"))
            {
                var page = new pgData("Sold");

                App.Current.MainPage.Navigation.PushAsync(page, false);
            }
            else
            {
                Global.showMessage("No data to display!");
            }
        }

        private void doStock()
        {
            if (Global.dbStore.recordExists("tbl_nota", "strStatus = 'Stock'"))
            {
                var page = new pgData("Stock");

                App.Current.MainPage.Navigation.PushAsync(page, false);
            }
            else
            {
                Global.showMessage("No data to display!");
            }
        }

        private void doTest()
        {
            try
            {
                Global.dbStore.updateNotaStatus();
                Global.showMessage("Success1");

                var tmp = Global.dbStore.getNotaAll();
                string tmpIMEI = "";
                string tmpBarang = "";
                int intIdxIMEI = 0;

                foreach (tbl_nota nota in tmp)
                {
                    intIdxIMEI = nota.strBarang.IndexOf("IMEI");
                    if (intIdxIMEI >= 0)
                    {
                        tmpIMEI = nota.strBarang.Substring(nota.strBarang.IndexOf("IMEI") + 5);
                        tmpBarang = nota.strBarang.Substring(0, nota.strBarang.IndexOf("IMEI")).Trim();
                        nota.strIMEI = tmpIMEI;
                        nota.strBarang = tmpBarang;
                        Global.dbStore.updateTable(nota);
                    }
                }

                Global.showMessage("Success2");
            }
            catch (Exception ex)
            {
                Global.showMessage("Error: " + ex.Message);
            }
            
        }

        private void doAbout()
        {
            Global.showMessage("Version: " + Global.strVersion + "\n" + Global.strWeb);
        }
    }
}
