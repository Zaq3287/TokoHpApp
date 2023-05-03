using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eNota.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pgData : ContentPage
    {
        pgDataVM pgDataVM;
        string strStatus;

        public pgData(string status)
        {
            InitializeComponent();
            strStatus = status;
            pgDataVM = new pgDataVM(strStatus);
            BindingContext = pgDataVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (strStatus == "Sold")
            {
                pgDataVM.getNota();
                dateHeight = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            }
            else
            {
                pgDataVM.getStock();
                dateHeight.Height = 0;
            }
        }
    }
}