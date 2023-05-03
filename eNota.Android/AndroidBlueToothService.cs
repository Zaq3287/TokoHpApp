using Android.Bluetooth;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eNota.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidBlueToothService))]
namespace eNota.Droid
{
    class AndroidBlueToothService : IBlueToothService
    {
        /// <summary>
        /// We have to use local device Bluetooth adapter.
        /// BondedDevices returns BluetoothDevice collection anyway I need to take just device name.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetDeviceList()
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                var lstDevice = bluetoothAdapter?.BondedDevices.Select(i => i.Name).ToList();
                return lstDevice;
            }
        }

        public bool bolBlueToothEnable()
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                return bluetoothAdapter.IsEnabled;
            }
        }

        /// <summary>
        /// We have to use local device Bluetooth adapter.
        /// We need to find Bluetooth Device with selected device name.
        /// Now, we use BluetoothSocket class with most common UUID
        /// Try to connect BluetoothSocket then convert your text-message to bytearray
        /// Last step write your bytearray by way of bluetoothSocket
        /// </summary>
        /// <param name="deviceName">Selected deviceName</param>
        /// <param name="text">My printed text-message</param>
        /// <returns></returns>
        public Task Print(string strDevice, string strMessage, string strWarranty)
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                          where bd?.Name == strDevice
                                          select bd).FirstOrDefault();
                try
                {
                    using (BluetoothSocket bluetoothSocket = device?.
                        CreateRfcommSocketToServiceRecord(
                        UUID.FromString("00001101-0000-1000-8000-00805f9b34fb")))
                    {
                        bluetoothSocket?.Connect();

                        //byte[] c1 = new byte[] { 0x1B, 0x21, 0x00 };  // 0- normal size text
                        //bluetoothSocket?.OutputStream.Write(c1);
                        //bluetoothSocket?.OutputStream.Write(Encoding.ASCII.GetBytes("\nTest aja ya ini doang 1"));
                        //byte[] c2 = new byte[] { 0x1B, 0x21, 0x01 };  // 0- normal size text
                        //bluetoothSocket?.OutputStream.Write(c2);
                        //bluetoothSocket?.OutputStream.Write(Encoding.ASCII.GetBytes("\nTest aja ya ini doang 2"));
                        //byte[] c3 = new byte[] { 0x1B, 0x21, 0x02 };  // 0- normal size text
                        //bluetoothSocket?.OutputStream.Write(c3);
                        //bluetoothSocket?.OutputStream.Write(Encoding.ASCII.GetBytes("\nTest aja ya ini doang 3"));
                        //byte[] c4 = new byte[] { 0x1B, 0x21, 0x03 };  // 0- normal size text
                        //bluetoothSocket?.OutputStream.Write(c4);
                        //bluetoothSocket?.OutputStream.Write(Encoding.ASCII.GetBytes("\nTest aja ya ini doang 4"));
                        //bluetoothSocket?.OutputStream.Write(Encoding.ASCII.GetBytes("\n\n\n\n"));

                        tbl_settings tmp = Global.dbStore.getSettings();

                        string strStoreDetail = "";
                        strStoreDetail += "\n" + Global.centerString(tmp.strAddress);
                        strStoreDetail += "\n" + Global.centerString(tmp.strCity);
                        strStoreDetail += "\n" + Global.centerString("WA " + tmp.strTelephone);

                        byte[] sizeNormal = new byte[] { 0x1B, 0x21, 0x00 };  //normal size text
                        byte[] sizeSmall = new byte[] { 0x1B, 0x21, 0x01 };  //normal size text
                        byte[] sizeBold = new byte[] { 0x1B, 0x21, 0x08 };  //only bold text
                        byte[] sizeBoldMedium = new byte[] { 0x1B, 0x21, 0x20 }; //bold with medium text
                        byte[] sizeBoldLarge = new byte[] { 0x1B, 0x21, 0x10 }; //bold with large text
                        byte[] message = Encoding.ASCII.GetBytes(strMessage);
                        byte[] line = Encoding.ASCII.GetBytes("\n--------------------------------");
                        byte[] storeName = Encoding.ASCII.GetBytes("\n" + Global.centerString(tmp.strName, 16));
                        byte[] storeDetail = Encoding.ASCII.GetBytes(strStoreDetail);
                        byte[] appName = Encoding.ASCII.GetBytes("\n\n" + Global.centerString(Global.strTitle + " Version " + Global.strVersion) + "\n" + Global.centerString(Global.strWeb) + "\n\n\n");
                        byte[] warranty = Encoding.ASCII.GetBytes(strWarranty);

                        bluetoothSocket?.OutputStream.Write(sizeNormal);
                        bluetoothSocket?.OutputStream.Write(line);
                        bluetoothSocket?.OutputStream.Write(sizeBoldMedium);
                        bluetoothSocket?.OutputStream.Write(storeName);
                        bluetoothSocket?.OutputStream.Write(sizeNormal);
                        bluetoothSocket?.OutputStream.Write(storeDetail);
                        bluetoothSocket?.OutputStream.Write(line);

                        bluetoothSocket?.OutputStream.Write(message);

                        if (strWarranty.Length > 0)
                        {
                            bluetoothSocket?.OutputStream.Write(sizeSmall);
                            bluetoothSocket?.OutputStream.Write(warranty);
                        }

                        bluetoothSocket?.OutputStream.Write(sizeNormal);
                        bluetoothSocket?.OutputStream.Write(appName);

                        bluetoothSocket.Close();
                    }
                }
                catch (Exception ex)
                {
                    Global.showMessage("Failed to print!");
                }
            }

            return Task.CompletedTask;
        }
    }
}