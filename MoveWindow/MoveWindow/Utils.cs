using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG
{
    public class Utils
    {
        /// <summary>
        /// 窗体区域
        /// </summary>
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        

        /// <summary>
        /// 改变指定窗口的位置和大小
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="bRepaint">是否要重画CWnd</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "MoveWindow")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        /// <summary>
        /// 改变一个子窗口，弹出式窗口或顶层窗口的尺寸，位置和Z序。子窗口，弹出式窗口，及顶层窗口根据它们在屏幕上出现的顺序排序、顶层窗口设置的级别最高，并且被设置为Z序的第一个窗口。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);


        /// <summary>
        /// 返回指定窗口的边框矩形的尺寸。该尺寸以相对于屏幕坐标左上角的屏幕坐标给出。
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="lpRect">指向一个RECT结构的指针，该结构接收窗口的左上角和右下角的屏幕坐标</param>
        /// <returns>如果函数成功，返回值为非零：如果函数失败，返回值为零</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);



        /// <summary>
        /// 获得一个顶层窗口的句柄，该窗口的类名和窗口名与给定的字符串相匹配。这个函数不查找子窗口。在查找时不区分大小写。
        /// </summary>
        /// <param name="lpClassName">指向一个指定了类名的空结束字符串，或一个标识类名字符串的成员的指针。如果该参数为一个成员，则它必须为前次调用theGlobafAddAtom函 数产生的全局成员。该成员为16位，必须位于IpClassName的低 16位，高位必须为 0</param>
        /// <param name="lpWindowName">指向一个指定了窗口名（窗口标题）的空结束字符串。如果该参数为空，则为所有窗口全匹配。</param>
        /// <returns>如果函数成功，返回值为具有指定类名和窗口名的窗口句柄；如果函数失败，返回值为NULL。</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);


        /// <summary>
        /// 函数返回桌面窗口的句柄。
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public extern static IntPtr GetDesktopWindow();
        

        /// <summary>
        /// 该函数获得指定窗口所属的类的类名
        /// </summary>
        /// <param name="hWnd">窗口的句柄及间接给出的窗口所属的类</param>
        /// <param name="lpClassName">指向接收窗口类名字符串的缓冲区的指针</param>
        /// <param name="nMaxCount">指定由参数lpClassName指示的缓冲区的字节数。如果类名字符串大于缓冲区的长度，则多出的部分被截断</param>
        /// <returns>如果函数成功，返回值为拷贝到指定缓冲区的字符个数：如果函数失败，返回值为0。</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern long GetLastError();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="title"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int nMaxCount);
        

        /// <summary>
        /// 获取一个前台窗口的句柄（窗口与用户当前的工作）。该系统分配给其他线程比它的前台窗口的线程创建一个稍微更高的优先级。
        /// </summary>
        /// <returns>返回值是一个前台窗口的句柄。在某些情况下，如一个窗口失去激活时，前台窗口可以是NULL。</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetForegroundWindow();


        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="hWnd">信息发往的窗口的句柄</param>
        /// <param name="Msg">消息ID</param>
        /// <param name="wParam">参数1</param>
        /// <param name="lParam">参数2</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(IntPtr hWnd, int Msg,int wParam, int lParam);


        /// <summary>
        /// 修改注册表，设置开机启动项目
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isAutoRun"></param>       
        public static void SetAutoRun(string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                    throw new Exception("该文件不存在!");

                fileName = fileName.Replace("/", "\\");
                String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);

                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                Console.WriteLine("{0} >>>{1}", name, fileName);

                if (isAutoRun)
                    reg.SetValue(name, fileName);
                else
                    reg.SetValue(name, false);
            }
            catch
            {
                Console.WriteLine("写入注册表失败");
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }

    }
}
