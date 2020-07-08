using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Automation;
using System.Windows.Forms;

namespace IPChecker
{
    public class Program
    {
        public static FlashWindow flashWindow;
        public static string lastIP;

        static void Main(string[] args)
        {
            InitTimer();
            flashWindow = new FlashWindow();
            AutomationFocusChangedEventHandler focusHandler = OnFocusChange;
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);
            Automation.RemoveAutomationFocusChangedEventHandler(focusHandler);
            lastIP = GetIP();
            Application.Run();
        }

        public static void InitTimer()
        {
            Console.WriteLine("Starting Application");
            Timer timer1;
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            int seconds = 30; //In Seconds
            timer1.Interval = seconds * 1000;
            timer1.Start();
        }

        public static string GetIP()
        {
            string ipV4 = NetworkInterface.GetAllNetworkInterfaces()
                .Select(i => i.GetIPProperties().UnicastAddresses)
                .SelectMany(u => u)
                .Where(u => u.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(i => i.Address)
                .First() //I only need the first address as my primary network interface is the first one
                .ToString();
            return ipV4;
        }

        public static void OnFocusChange(object source, AutomationFocusChangedEventArgs e)
        {
            var focusedHandle = new IntPtr(AutomationElement.FocusedElement.Current.NativeWindowHandle);
            var myConsoleHandle = Process.GetCurrentProcess().MainWindowHandle;

            if (focusedHandle == myConsoleHandle)
            {
                flashWindow.StopFlash();
            }
        }

        public static void timer1_Tick(object sender, EventArgs e)
        {
            string currentIP = GetIP();
            if (currentIP != lastIP)
            {
                Console.WriteLine("New IP: " + currentIP);
                flashWindow.FlashWindowNow(Process.GetCurrentProcess().MainWindowHandle);
            }
        }
    }
}
