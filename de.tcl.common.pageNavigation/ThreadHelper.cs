using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace de.tcl.common.pageNavigation
{
    public static class ThreadHelper
    {
        public static int MainThreadId { get; private set; }

        public static void Initialize(int mainThreadId)
        {
            MainThreadId = mainThreadId;
        }

        public static bool IsOnMainThread
        {
            get { return Environment.CurrentManagedThreadId == MainThreadId; }
        }

        public static void InvokeOnMainThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        public static void InvokeOnMainThreadIfRequired(Action action)
        {
            // Check if we need to marshal to GUI thread
            if (!IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(action);
            }
            else
            {
                // Just call the method (no marshaling required)
                action();
            }
        }
    }
}
