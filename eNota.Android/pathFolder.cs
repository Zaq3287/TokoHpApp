using eNota.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(pathFolder))]
namespace eNota.Droid
{
   
    public class pathFolder : IPath
    {
        public string getPathFolder()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        }

    }
}