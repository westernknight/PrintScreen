using AForge.Video;
using AForge.Video.FFMPEG;
using ScreenShotDemo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PrintScreen
{
    class Program
    {

        static BackgroundWorker background = new BackgroundWorker();
        static BackgroundWorker save_background = new BackgroundWorker();

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();


        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]   //找子窗体   
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]   //用于发送信息给窗体   
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        [DllImport("User32.dll", EntryPoint = "ShowWindow")]   //
        private static extern bool ShowWindow(IntPtr hWnd, int type);

        enum bitRate
        {
            _50kbit = 50000,
            _100kbit = 100000,
            _500kbit = 500000,
            _1000kbit = 1000000,
            _2000kbit = 2000000,
            _3000kbit = 3000000,
            _4000kbit = 4000000,
            _5000kbit = 5000000,
        }

        static void Main(string[] args)
        {
            int Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            Rectangle screenArea = new Rectangle(0, 0, Width, Height);
            ScreenCaptureStream streamVideo = new ScreenCaptureStream(screenArea);
            VideoFileWriter writer = new VideoFileWriter();




            IntPtr ParenthWnd = new IntPtr(0);
            IntPtr et = new IntPtr(0);
            ParenthWnd = FindWindow(null, Console.Title);
            ShowWindow(ParenthWnd, 2);//隐藏本dos窗体, 0: 后台执行；1:正常启动；2:最小化到任务栏；3:最大化


            Thread.Sleep(1000);
            writer.Open("a.wmv", Width, Height);


            streamVideo.NewFrame += (sender, eventArgs) =>
                {
                    writer.WriteVideoFrame(eventArgs.Frame);
                };
            streamVideo.Start();



            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.F9)
                {
                    break;
                }
            }


            streamVideo.Stop();
            writer.Close();

#if false
return;


            ScreenCapture sc = new ScreenCapture();
            // capture entire screen, and save it to a file

            // display image in a Picture control named imageDisplay



            // capture this window, and save it
            // sc.CaptureWindowToFile(GetDesktopWindow(), "0.png", ImageFormat.Png);




            Console.WriteLine("press f9 key to start capture.");


            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.F9)
                {
                    break;
                }
            }



            Console.WriteLine();


            IntPtr ParenthWnd = new IntPtr(0);
            IntPtr et = new IntPtr(0);
            ParenthWnd = FindWindow(null, Console.Title);
            ShowWindow(ParenthWnd, 2);//隐藏本dos窗体, 0: 后台执行；1:正常启动；2:最小化到任务栏；3:最大化



            string directoryPath = DateTime.Now.ToString("[yyyy-MM-dd-hh-mm-ss]");
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            if (di.Exists == false)
            {
                di.Create();
            }


            background.DoWork += (sender, e) =>
                {
                    int capCount = 0;
                    while (background.CancellationPending == false)
                    {
                        Image img = sc.CaptureScreen();
                        sc.CaptureWindowToFile(GetDesktopWindow(), string.Format("{0}/{1:D6}.png", directoryPath, capCount), ImageFormat.Png);
                        capCount++;
                    }
                };
            background.WorkerSupportsCancellation = true;

            background.RunWorkerAsync();


            Console.WriteLine("press f9 key to stop capture.");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.F9)
                {
                    break;
                }
            }
            background.CancelAsync();

            while (background.IsBusy)
            {

            }
#endif
        }
    }
}
