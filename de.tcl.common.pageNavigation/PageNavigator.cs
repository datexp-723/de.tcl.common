using de.tcl.common.entities.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace de.tcl.common.pageNavigation
{
    /// <summary>
    /// Provides page navigation for the mobile app.
    /// </summary>
    public static class PageNavigator
    {

        #region =============== fields & constants =========================

        private static readonly IDictionary<Type, Type> _viewsByViewModels = new Dictionary<Type, Type>();
        private static readonly object _syncInstance = new object();

        #endregion ============ fields & constants =========================

        #region =============== events & delegates  ========================

        /// <summary>
        /// Raised, if the active page has changed.
        /// </summary>
        public static event EventHandler<EventArgs> PageChanged;

        #endregion ============ events & delegates  ========================

        #region =============== properties =================================

        public static INavigation XamarinFormsNavigation { get; set; }

        #endregion ============ properties =================================

        #region =============== public & internal methods ==================

        public static void RegisterViewMapping(Type viewModel, Type view)
        {
            lock (_syncInstance)
            {
                if (!_viewsByViewModels.ContainsKey(viewModel))
                {
                    _viewsByViewModels.Add(viewModel, view);
                }
            }
        }

        public static async Task RemoveLastPagesAsync(int numberOfPagesToRemove)
        {
            try
            {
                if (ThreadHelper.IsOnMainThread)
                {
                    if (XamarinFormsNavigation.NavigationStack.Count() >= numberOfPagesToRemove)
                    {
                        Page lastView = XamarinFormsNavigation.NavigationStack[XamarinFormsNavigation.NavigationStack.Count - numberOfPagesToRemove];
                        XamarinFormsNavigation.RemovePage(lastView);
                        await PopLastPageAsync();
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (XamarinFormsNavigation.NavigationStack.Count() >= numberOfPagesToRemove)
                        {
                            Page lastView = XamarinFormsNavigation.NavigationStack[XamarinFormsNavigation.NavigationStack.Count - numberOfPagesToRemove];
                            XamarinFormsNavigation.RemovePage(lastView);
                            await PopLastPageAsync();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }
        }

        public static async Task PopLastPageAsync()
        {
            try
            {
                if (ThreadHelper.IsOnMainThread)
                {
                    if (XamarinFormsNavigation.NavigationStack.Count() > 0)
                    {
                        await XamarinFormsNavigation.PopAsync();
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (XamarinFormsNavigation.NavigationStack.Count() > 0)
                        {
                            await XamarinFormsNavigation.PopAsync();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }
        }

        public static async Task<ViewModelBase> NavigateToAsync<TVM>() where TVM : ViewModelBase
        {
            await NavigateToViewAsync(typeof(TVM));

            Page lastPageInStack = XamarinFormsNavigation.NavigationStack.Last();
            if (lastPageInStack != null
                && lastPageInStack.BindingContext is ViewModelBase)
            {
                ViewModelBase viewModel = (ViewModelBase)lastPageInStack.BindingContext;
                await viewModel.InitAsync();

                return viewModel;
            }

            return null;
        }

        public static void NavigateTo<TVM, TParameter>(TParameter parameter) where TVM : ViewModelBase
        {
            NavigateToView(typeof(TVM));

            Page lastPageInStack = XamarinFormsNavigation.NavigationStack.Last();
            if (lastPageInStack != null
                && lastPageInStack.BindingContext is ViewModelBase<TParameter>)
            {
                ViewModelBase<TParameter> viewModel = (ViewModelBase<TParameter>)lastPageInStack.BindingContext;
                viewModel.InitAsync(parameter);
            }
        }

        public static async Task NavigateToAsync<TVM, TParameter>(TParameter parameter) where TVM : ViewModelBase
        {
            await NavigateToViewAsync(typeof(TVM));

            Page lastPageInStack = XamarinFormsNavigation.NavigationStack.Last();
            if (lastPageInStack != null
                && lastPageInStack.BindingContext is ViewModelBase<TParameter>)
            {
                ViewModelBase<TParameter> viewModel = (ViewModelBase<TParameter>)lastPageInStack.BindingContext;
                await viewModel.InitAsync(parameter);
            }
        }

        #endregion ============ public & internal methods ==================

        #region =============== private & protected methods ================

        private static void NavigateToView(Type viewModelType)
        {
            Page view = CreatePageByViewModelType(viewModelType);
            if (view != null)
            {
                if (ThreadHelper.IsOnMainThread)
                {
                    XamarinFormsNavigation.PushAsync(view);
                    RaisePageChanged();
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        XamarinFormsNavigation.PushAsync(view);
                        RaisePageChanged();
                    });
                }
            }
        }

        private static async Task NavigateToViewAsync(Type viewModelType)
        {
            Page view = CreatePageByViewModelType(viewModelType);
            if (view != null)
            {
                if (ThreadHelper.IsOnMainThread)
                {
                    await XamarinFormsNavigation.PushAsync(view);
                    RaisePageChanged();
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await XamarinFormsNavigation.PushAsync(view);
                        RaisePageChanged();
                    });
                }
            }
        }

        private static Page CreatePageByViewModelType(Type viewModelType)
        {
            Type viewType;
            lock (_syncInstance)
            {
                if (!_viewsByViewModels.TryGetValue(viewModelType, out viewType))
                {
                    throw new ArgumentException($"No view found in ViewMapping for {viewModelType.FullName}.");
                }
            }

            ConstructorInfo constructor = viewType.GetTypeInfo()
                .DeclaredConstructors
                .FirstOrDefault(dc => dc.GetParameters().Count() <= 0);

            return constructor.Invoke(null) as Page;
        }

        private static void RaisePageChanged()
        {
            PageChanged?.Invoke(typeof(PageNavigator), EventArgs.Empty);
        }

        #endregion ============ private & protected methods ================
    }
}
