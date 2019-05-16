using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceCG
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents m_GlobalHook;

        public Form1()
        {
            InitializeComponent();

            this.notifyIcon.Visible = true;
            this.Visible = false;
            this.ShowInTaskbar = false;
            this.Icon = this.notifyIcon.Icon;
            this.WindowState = FormWindowState.Minimized;
        }

        #region Object Events
        private void Form1_Load(object sender, EventArgs e)
        {
            Utils.SetAutoRun(Application.ExecutablePath.ToString(), true);

            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyDown += M_GlobalHook_KeyDown;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (m_GlobalHook != null)
            {
                m_GlobalHook.KeyDown -= M_GlobalHook_KeyDown;
                m_GlobalHook.Dispose();
            }
        }

        private void M_GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine(e.KeyData);
            switch(e.KeyData)
            {
                case Keys.Alt | Keys.F2:
                    GetWindow();
                    break;

                default:
                    e.Handled = false;
                    break;
            }            
        }
        #endregion

        /// <summary>
        /// 获取窗体
        /// </summary>
        public void GetWindow()
        {
            IntPtr hwnd = Utils.GetForegroundWindow();
            if(hwnd == IntPtr.Zero)
            {
                MessageBox.Show("未找到活动的前台窗口句柄。", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            else
            {
                try
                {
                    //获取原窗体的大小
                    Utils.Rect wRect = new Utils.Rect();
                    Utils.GetWindowRect(hwnd, out wRect);
                    Rectangle rect = new Rectangle(wRect.Left, wRect.Top, wRect.Right - wRect.Left, wRect.Bottom - wRect.Top);

                    //还原最大化窗体
                    //#define WM_SYSCOMMAND 0x0112  
                    //SC_RESTORE    0xF120
                    //https://msdn.microsoft.com/en-us/library/windows/desktop/ms646360%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
                    //if (wRect.Left < 0 || wRect.Top < 0)
                    //    Utils.PostMessage(hwnd, 0x0112, 0xF120, 0);

                    //获取窗体类名
                    StringBuilder cn = new StringBuilder(128);
                    Utils.GetClassName(hwnd, cn, cn.Capacity);
                    //获取窗体标题
                    StringBuilder sn = new StringBuilder(256);
                    Utils.GetWindowText(hwnd, sn, sn.Capacity);

                    //如果是设置窗体本身就不要在显示了
                    if (cn.ToString().IndexOf("HwndWrapper") != -1 || sn.ToString().IndexOf("Setting Window Size") != -1) return;

                    //SettingDisplay sd = new SettingDisplay();
                    using (SettingDisplay sd = new SettingDisplay())
                    {                        
                        //设置显示窗口
                        sd.Resolution = rect;
                        //sd.ShowIcon = false;
                        sd.Icon = this.notifyIcon.Icon;
                        sd.Text = String.Format("Setting Window Size  -  Hwnd:{0}    Title:{1}    ClassName:{2}", hwnd.ToString(), sn.ToString(), cn.ToString());
                        sd.ShowDialog(new WindowWrapper(hwnd));
                        sd.Activate();

                        //Console.WriteLine(sd.Text);

                        //设置窗体新的大小及位置
                        if (sd.DialogResult == DialogResult.OK)
                        {
                            rect = sd.Resolution;
                            Console.WriteLine(rect);
                            Utils.MoveWindow(hwnd, rect.X, rect.Y, rect.Width, rect.Height, true);
                            //Utils.SetWindowPos(hwnd, -1, rect.X, rect.Y, rect.Width, rect.Height, 0x04 | 0x20);
                        }
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show(new WindowWrapper(hwnd), "窗口设置错误：" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
