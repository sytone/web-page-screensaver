using System.Windows.Forms;

namespace WebPageScreensaver
{
    public class GlobalUserEventHandler : IMessageFilter
    {
        public delegate void UserEvent();
        
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;

        public event UserEvent Event;

        public bool PreFilterMessage(ref Message m)
        {
            if ( (m.Msg >= WM_MOUSEMOVE && m.Msg <= WM_MBUTTONDBLCLK) 
                 || m.Msg == WM_KEYDOWN || m.Msg == WM_KEYUP)
            {
                // Ignore the zome calls. 
                if ((m.Msg == WM_KEYDOWN && (Keys)m.WParam == (Keys.LButton | Keys.ShiftKey))
                    || (m.Msg == WM_KEYUP && (Keys)m.WParam == (Keys.LButton | Keys.ShiftKey))
                    || (m.Msg == WM_KEYDOWN && (Keys)m.WParam == (Keys.ShiftKey | Keys.Space))
                    || (m.Msg == WM_KEYUP && (Keys)m.WParam == (Keys.ShiftKey | Keys.Space))
                    || (m.Msg == WM_KEYDOWN && (Keys)m.WParam == (Keys.LButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17))
                    || (m.Msg == WM_KEYUP && (Keys)m.WParam == (Keys.LButton | Keys.MButton | Keys.Back | Keys.ShiftKey | Keys.Space | Keys.F17))
                    || (Keys)m.WParam == Keys.None)
                    
                {
                }
                else
                {
                    if (Event != null)
                    {
                        Event();
                    }
                }
            }
            // Always allow message to continue to the next filter control
            return false;
        }
    }
}