using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace de.tcl.common.entities
{
    public class MenuCommand
    {
        #region =============== properties =================================

        /// <summary>
        /// The real command itself.
        /// </summary>
        public ICommand CommandToExecute { get; set; }

        /// <summary>
        /// Text property for the command (e.g. used by buttons).
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Image for the command (e.g. used by buttons).
        /// </summary>
        public string ImageSource { get; set; }

        #endregion ============ properties =================================
    }
}
