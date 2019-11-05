using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowFocusing
{
    internal class Program
    {
        public delegate bool EnumWindowCallback(int hwnd, int lParam);
        [DllImport("user32.dll")] public static extern int EnumWindows(EnumWindowCallback callback, int y);
        [DllImport("user32.dll")] public static extern int GetParent(int hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] public static extern int GetWindowText(int hWnd, StringBuilder text, int count);
        [DllImport("user32.dll")] public static extern long GetWindowLong(int hWnd, int nIndex);
        [DllImport("user32.dll")] private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();

        // #include <winuser.h> 참조
        private const int SW_SHOW = 5;
        private const long WS_VISIBLE = 0x10000000L;
        private const long WS_CAPTION = 0x00C00000L;
        private const int GCL_HMODULE = -16;
        private static readonly List<string> m_titles = new List<string>();
        private static readonly List<string> m_names = new List<string>();

        private static void Main(string[] args)
        {
            GetWindows();
            string windowName;
            if (args.Length > 0) {
                windowName = args[0];
            } else {
                Console.WriteLine("Title 목록");
                foreach (var title in m_titles) {
                    Console.WriteLine(title);
                }
                Console.WriteLine("포커스로 지정할 Title 입력");
                Console.Write("-> ");
                windowName = Console.ReadLine();
                if (string.IsNullOrEmpty(windowName)) {
                    return;
                }
            }

            foreach (var title in m_titles) {
                if (title.IndexOf(windowName, StringComparison.OrdinalIgnoreCase) < 0) {
                    continue;
                }
                m_names.Add(title);
            }

            SetFocus(0);
        }

        private static void SetFocus(int index)
        {
            if (index >= m_names.Count) {
                return;
            }

            var task = Task.Run(() => {
                var hWnd = FindWindow(null, m_names[index]);
                ShowWindowAsync(hWnd, SW_SHOW);
                SetForegroundWindow(hWnd);
                do {
                    var d = Task.Delay(10);
                    d.Wait();
                    var current = GetForegroundWindow();
                    if (hWnd.Equals(current)) {
                        break;
                    }
                } while (true);
            });

            task.Wait();
            SetFocus(++index);
        }

        private static void GetWindows()
        {
            EnumWindowCallback callback = new EnumWindowCallback(EnumWindowsProc);
            EnumWindows(callback, 0);
        }

        private static bool EnumWindowsProc(int hWnd, int lParam)
        {
            var style = GetWindowLong(hWnd, GCL_HMODULE);
            if ((style & WS_VISIBLE) == WS_VISIBLE && (style & WS_CAPTION) == WS_CAPTION) {
                if (GetParent(hWnd) == 0) {
                    StringBuilder Buf = new StringBuilder(256);
                    if (GetWindowText(hWnd, Buf, 256) > 0) {
                        m_titles.Add(Buf.ToString());
                    }
                }
            }
            return true;
        }
    }
}