using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls
{
    public class ButtonClickEventArgs : EventArgs
    {
        public object SelectedItem { get; internal set; }
    }
}
