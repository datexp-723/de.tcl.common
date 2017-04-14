using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace de.tcl.common.entities.Mvvm
{
    /// <summary>
    /// Base abstract class for parameterless ViewModels.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        #region =============== events & delegates  ========================

        /// <summary>
        /// Raised, if a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion ============ events & delegates  ========================

        #region =============== constructors & destructors =================

        /// <summary>
        /// Constructor initiating a new instance of <see cref="ViewModelBase"/>.
        /// </summary>
        protected ViewModelBase()
        {

        }

        #endregion ============ constructors & destructors =================

        #region =============== public & internal methods ==================

        /// <summary>
        /// Initialization routine of the ViewModel.
        /// </summary>
        /// <returns>Task of the initialization routine.</returns>
        public abstract Task InitAsync();

        #endregion ============ public & internal methods ==================

        #region =============== private & protected methods ================

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="name">
        /// Optional: Name of the changed property. 
        /// Pass <c>null</c> for 'all property changed' notification.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion ============ private & protected methods ================
    }

    /// <summary>
    /// Base abstract class for ViewModels which needs to be initialized with parameter.
    /// </summary>
    /// <typeparam name="TParameter">Generic object used for parametrization.</typeparam>
    public abstract class ViewModelBase<TParameter> : ViewModelBase
    {

        #region =============== constructors & destructors =================

        /// <summary>
        /// Initialization routine of the ViewModel.
        /// </summary>
        protected ViewModelBase()
        {

        }

        #endregion ============ constructors & destructors =================

        #region =============== public & internal methods ==================

        /// <summary>
        /// Initialization routine of the ViewModel with default parameter.
        /// </summary>
        /// <returns>Task of the initialization routine.</returns>
        public override async Task InitAsync()
        {
            await InitAsync(default(TParameter));
        }

        /// <summary>
        /// Initialization routine of the ViewModel.
        /// </summary>
        /// <param name="parameter">Paremeter to pass into the ViewModel.</param>
        /// <returns>Task of the initialization routine.</returns>
        public abstract Task InitAsync(TParameter parameter);

        #endregion ============ public & internal methods ==================

    }
}
