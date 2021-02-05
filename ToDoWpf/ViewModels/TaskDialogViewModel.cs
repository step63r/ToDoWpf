using System.Windows.Input;
using ToDoWpf.Common;

namespace ToDoWpf.ViewModels
{
    /// <summary>
    /// TaskDialogのViewModelクラス
    /// </summary>
    public class TaskDialogViewModel : ViewModelBase
    {
        #region プロパティ
        private ToDoTask _task;
        /// <summary>
        /// このダイアログにバインドされたタスク
        /// </summary>
        public ToDoTask Task
        {
            get
            {
                return _task;
            }
            set
            {
                if (_task != value)
                {
                    _task = value;
                    RaisePropertyChanged(nameof(Task));
                }
            }
        }

        private bool _closeWindow = false;
        /// <summary>
        /// このウィンドウを閉じるか
        /// </summary>
        public bool CloseWindow
        {
            get
            {
                return _closeWindow;
            }
            set
            {
                if (_closeWindow != value)
                {
                    _closeWindow = value;
                    RaisePropertyChanged(nameof(CloseWindow));
                }
            }
        }
        #endregion

        #region コマンド
        /// <summary>
        /// OKコマンド
        /// </summary>
        public ICommand OkCommand { get; private set; }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskDialogViewModel()
        {
            // コマンドを作成する
            OkCommand = CreateCommand(ExecuteOkCommand, CanExecuteOkCommand);

            //Task = new ToDoTask()
            //{
            //    Name = "sample",
            //    DueDate = new System.DateTime(2022, 1, 1),
            //    Priority = Priority.Medium
            //};
        }

        /// <summary>
        /// OKコマンドを実行する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        private void ExecuteOkCommand(object parameter)
        {
            CloseWindow = true;
        }

        /// <summary>
        /// OKコマンド
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private bool CanExecuteOkCommand(object parameter)
        {
            // 常に実行可能
            return true;
        }
    }
}
