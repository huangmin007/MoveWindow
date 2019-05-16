using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG
{
    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        private IntPtr _hwnd;

        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        
    }
}
