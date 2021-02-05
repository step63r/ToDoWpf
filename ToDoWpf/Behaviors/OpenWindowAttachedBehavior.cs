using System.Windows;
using System.Windows.Input;

namespace ToDoWpf.Behaviors
{
    /// <summary>
    /// 子ウィンドウを表示する添付ビヘイビア
    /// </summary>
    /// <remarks>http://sourcechord.hatenablog.com/entry/2014/04/06/234443</remarks>
    internal class OpenWindowAttachedBehavior
    {
        #region IsModal
        public static bool GetIsModal(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsModalProperty);
        }

        public static void SetIsModal(DependencyObject obj, bool value)
        {
            obj.SetValue(IsModalProperty, value);
        }

        private static readonly DependencyProperty IsModalProperty =
            DependencyProperty.RegisterAttached("IsModal", typeof(bool), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(true));
        #endregion

        #region HasOwner
        public static bool GetHasOwner(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasOwnerProperty);
        }

        public static void SetHasOwner(DependencyObject obj, bool value)
        {
            obj.SetValue(HasOwnerProperty, value);
        }

        private static readonly DependencyProperty HasOwnerProperty =
            DependencyProperty.RegisterAttached("HasOwner", typeof(bool), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(true));
        #endregion

        #region CloseCommand
        public static ICommand GetCloseCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CloseCommandProperty);
        }

        public static void SetCloseCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CloseCommandProperty, value);
        }

        private static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.RegisterAttached("CloseCommand", typeof(ICommand), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(null));
        #endregion

        #region WindowTemplate
        public static DataTemplate GetWindowTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(WindowTemplateProperty);
        }

        public static void SetWindowTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(WindowTemplateProperty, value);
        }

        private static readonly DependencyProperty WindowTemplateProperty =
            DependencyProperty.RegisterAttached("WindowTemplate", typeof(DataTemplate), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(null));
        #endregion

        #region WindowViewModel
        public static object GetWindowViewModel(DependencyObject obj)
        {
            return obj.GetValue(WindowViewModelProperty);
        }

        public static void SetWindowViewModel(DependencyObject obj, object value)
        {
            obj.SetValue(WindowViewModelProperty, value);
        }

        private static readonly DependencyProperty WindowViewModelProperty =
            DependencyProperty.RegisterAttached("WindowViewModel", typeof(object), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(null, OnWindowViewModelChanged));

        private static void OnWindowViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                var template = GetWindowTemplate(d);
                if (template != null)
                {
                    var viewModel = GetWindowViewModel(d);
                    if (viewModel != null)
                    {
                        OpenWindow(element);
                    }
                    else
                    {
                        CloseWindow(element);
                    }
                }
            }
        }
        #endregion

        private static void OpenWindow(FrameworkElement element)
        {
            var isModal = GetIsModal(element);
            var win = GetWindow(element);
            var cmd = GetCloseCommand(element);
            var template = GetWindowTemplate(element);
            var vm = GetWindowViewModel(element);
            var owner = Window.GetWindow(element);
            var hasOwner = GetHasOwner(element);

            if (win == null)
            {
                win = new Window()
                {
                    ContentTemplate = template,
                    Content = vm,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    Owner = hasOwner ? owner : null,
                    ResizeMode = ResizeMode.NoResize,
                    ShowInTaskbar = false,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                win.Closed += (s, e) =>
                {
                    if (cmd != null)
                    {
                        if (cmd.CanExecute(vm))
                        {
                            cmd.Execute(vm);
                        }
                        SetWindow(element, null);
                    }
                };

                SetWindow(element, win);
                if (isModal)
                {
                    win.ShowDialog();
                }
                else
                {
                    win.Show();
                }
            }
            else
            {
                win.Activate();
            }
        }

        private static void CloseWindow(FrameworkElement element)
        {
            var win = GetWindow(element);
            if (win != null)
            {
                win.Close();
                SetWindow(element, null);
            }
        }

        #region Window
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static Window GetWindow(DependencyObject obj)
        {
            return (Window)obj.GetValue(WindowProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        private static void SetWindow(DependencyObject obj, Window value)
        {
            obj.SetValue(WindowProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly DependencyProperty WindowProperty =
            DependencyProperty.RegisterAttached("Window", typeof(Window), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(null));
        #endregion
    }
}
