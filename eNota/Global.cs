using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eNota
{
    public class Global
    {
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // constanta
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public const string strTitle = "e-Nota";
        public const string strVersion = "1.0.1";
        public const string strWeb = "www.zaqstore.com";
        
        

        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // fields
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        private static Database _dbStore;
        private static string _strDevice = "";
        private static IBlueToothService _blueToothService;
        private static IPath _path;

        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // properties
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public static Database dbStore
        {
            get
            {
                if (_dbStore == null)
                {
                    _dbStore = new Database();
                }
                return _dbStore;
            }
        }


        public static string strDevice
        {
            get
            {
                if (dbStore.recordExists("tbl_settings"))
                {
                    tbl_settings tmp = dbStore.getSettings();
                    _strDevice = tmp.strDevice;
                    return _strDevice;
                }
                else
                {
                    return "";
                }
            }
        }

        public static IBlueToothService blueToothService
        {
            get
            {
                if (_blueToothService == null)
                {
                    _blueToothService = DependencyService.Get<IBlueToothService>();
                }
                return _blueToothService;
            }
        }

        public static IPath path
        {
            get
            {
                if (_path == null)
                {
                    _path = DependencyService.Get<IPath>();
                }
                return _path;
            }
        }


        // -------------------------------------------------------------
        // -------------------------------------------------------------
        // function
        // -------------------------------------------------------------
        // -------------------------------------------------------------
        public static void showMessage(string strMessage)
        {
            Device.BeginInvokeOnMainThread(async () => 
            {
                await App.Current.MainPage.DisplayAlert(strTitle, strMessage, "OK");
            });
        }

        public static string centerString(string strText, int length = 32)
        {
            return strText.PadLeft(((length - strText.Length) / 2) + strText.Length).PadRight(length);
        }

        public static async Task<PermissionStatus> tryRequestStoragePermission()
        {
            var permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
            if (permissionStatus != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {

                }
            }
            return permissionStatus;
        }
    }
}
