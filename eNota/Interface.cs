using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace eNota
{
    public interface IBlueToothService
    {
        IList<string> GetDeviceList();
        bool bolBlueToothEnable();
        Task Print(string strDevice, string strMessage, string strStoreName);
    }

    public interface IPath
    {
        string getPathFolder();
    }
}
