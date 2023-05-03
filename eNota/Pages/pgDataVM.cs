using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace eNota.Pages
{
    public class pgDataVM : BindProperty
    {
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // fields
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        private ObservableCollection<Nota> _lstNota;
        private DateTime _dtStart;
        private DateTime _dtEnd;


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // properties
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public ICommand comDetail { get; set; }
        public ICommand comDelete { get; set; }
        public ICommand comEdit { get; set; }
        public ObservableCollection<Nota> lstNota { get { return _lstNota; } set { _lstNota = value; OnPropertyChanged("lstNota"); } }
        public DateTime dtStart { get { return _dtStart; } set { _dtStart = value; getNota(); OnPropertyChanged("dtStart"); } }
        public DateTime dtEnd { get { return _dtEnd; } set { _dtEnd = value; getNota(); OnPropertyChanged("dtEnd"); } }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public pgDataVM(string strStatus)
        {
            initCommands();
            initList();
            
            if (strStatus == "Sold")
            {
                dtStart = DateTime.Now.Date.AddDays(-90);
                dtEnd = DateTime.Now.Date;
            }
            else //stock
            {
                getStock();
            }
        }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        private void initCommands()
        {
            comDetail = new Command(doDetail);
            comDelete = new Command(doDelete);
            comEdit = new Command(doEdit);
        }

        private void initList()
        {
            lstNota = new ObservableCollection<Nota>();
        }

        public void getNota()
        {
            lstNota.Clear();
            var tmp = Global.dbStore.getNotaSold(dtStart, dtEnd);
            int intLaba = 0;
            string strLabaColor = "";

            foreach(tbl_nota nota in tmp)
            {
                intLaba = Convert.ToInt32(nota.strHarga) - Convert.ToInt32(nota.strModal);
                if (intLaba > 0)
                {
                    strLabaColor = "#00FF00";
                }
                else if (intLaba < 0)
                {
                    strLabaColor = "#FF0000";
                }
                else if (intLaba == 0)
                {
                    strLabaColor = "#808080";
                }

                lstNota.Add(new Nota
                {
                    intID = nota.intID,
                    strBarang = nota.strBarang,
                    strHarga = Convert.ToInt32(nota.strHarga).ToString("#,#"),
                    strLaba = (intLaba > 0 ? "+" : "-") + (intLaba == 0 ? "" : " " + Math.Abs(intLaba).ToString("#,#")),
                    strLabaColor = strLabaColor,
                    strDate = nota.dtOrder.ToString("dd MMMM yyyy")
                });
            }
        }

        public void getStock()
        {
            lstNota.Clear();
            var tmp = Global.dbStore.getNotaStock();
            int intLaba = 0;
            string strLabaColor = "";

            foreach (tbl_nota nota in tmp)
            {
                //intLaba = Convert.ToInt32(nota.strHarga) - Convert.ToInt32(nota.strModal);
                if (intLaba > 0)
                {
                    strLabaColor = "#00FF00";
                }
                else if (intLaba < 0)
                {
                    strLabaColor = "#FF0000";
                }
                else if (intLaba == 0)
                {
                    strLabaColor = "#808080";
                }

                lstNota.Add(new Nota
                {
                    intID = nota.intID,
                    strBarang = nota.strBarang,
                    strHarga = Convert.ToInt32(nota.strHarga).ToString("#,#"),
                    strLaba = (intLaba > 0 ? "+" : "-") + (intLaba == 0 ? "" : " " + Math.Abs(intLaba).ToString("#,#")),
                    strLabaColor = strLabaColor,
                    strDate = nota.dtOrder.ToString("dd MMMM yyyy")
                });
            }
        }

        private void doDetail(object sender)
        {
            int intID = Convert.ToInt32(sender);
            var tmp = Global.dbStore.getNota(intID);
            string strMessage = "Tanggal: " + tmp.dtOrder.ToString("dd MMMM yyyy");
            strMessage += "\nTelephone: " + tmp.strTelephone;
            strMessage += "\nBarang: " + tmp.strBarang;
            strMessage += "\nIMEI: " + tmp.strIMEI;
            strMessage += "\nJual: " + "Rp. " + Convert.ToInt32(tmp.strHarga).ToString("#,#") + ",-";
            strMessage += "\nModal: " + "Rp. " + Convert.ToInt32(tmp.strModal).ToString("#,#") + ",-";
            strMessage += "\nPembayaran: " + tmp.strPayment;
            if (tmp.strMode == "New")
            {
                strMessage += "\nGaransi Toko: " + tmp.strGaransiToko + " hari";
                strMessage += "\nGaransi Resmi: " + tmp.strGaransiResmi + " bulan";
            }
            else if (tmp.strMode == "Second")
            {
                strMessage += "\nGaransi Toko: " + tmp.strGaransiToko + " hari";
            }

            Global.showMessage(strMessage);
        }

        private async void doDelete(object sender)
        {
            var action = await App.Current.MainPage.DisplayAlert(Global.strTitle, "Are you sure to delete?", "Yes", "No");
            if (action)
            {
                int intID = Convert.ToInt32(sender);

                Global.dbStore.deleteTableNota(intID);

                getNota();

                Global.showMessage("Data deleted successfully");
            }
        }

        private void doEdit(object sender)
        {
            int intID = Convert.ToInt32(sender);

            var page = new pgNota();
            page.BindingContext = new pgNotaVM(intID);

            App.Current.MainPage.Navigation.PushAsync(page, false);
        }

        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // class
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public class Nota
        {
            public long intID { get; set; }
            public string strBarang { get; set; }
            public string strHarga { get; set; }
            public string strLaba { get; set; }
            public string strLabaColor { get; set; }
            public string strDate { get; set; }
        }
    }
}
