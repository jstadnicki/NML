using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace NML.Utils
{
    public class KeyboardHandler : IDisposable
    {
        // Private classes ----------------------------------------------------

        private static class Native
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        }

        // Public const values ------------------------------------------------

        public const int WM_HOTKEY = 0x0312;
        public const int Modifiers = 0x0008; // WinKey
        public const int Key = 0x41; // A

        // Private fields -----------------------------------------------------

        private readonly Window _mainWindow;
        private WindowInteropHelper _host;

        // Private methods ----------------------------------------------------

        private void SetupHotKey(IntPtr handle)
        {
            Native.RegisterHotKey(handle, GetType().GetHashCode(), Modifiers, Key);
        }

        void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message == WM_HOTKEY)
            {
                if (ShortcutPressed != null)
                    ShortcutPressed(this, EventArgs.Empty);

                handled = true;
            }
            else
                handled = false;
        }

        // Public classes -----------------------------------------------------

        public KeyboardHandler(Window mainWindow)
        {
            _mainWindow = mainWindow;
            _host = new WindowInteropHelper(_mainWindow);

            SetupHotKey(_host.Handle);
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        public void Dispose()
        {
            Native.UnregisterHotKey(_host.Handle, GetType().GetHashCode());
        }

        // Public events ------------------------------------------------------

        public event EventHandler ShortcutPressed;
    }
}
