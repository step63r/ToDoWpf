using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// 優先度（コンボボックスにバインドする）
        /// </summary>
        public IEnumerable<Priority> Priorities
        {
            get
            {
                return Enum.GetValues(typeof(Priority)).Cast<Priority>();
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

        /// <summary>
        /// OKボタンが押下された場合、ここにタスクオブジェクトが入る
        /// </summary>
        public ToDoTask Result { get; set; } = null;
        #endregion

        #region コマンド
        /// <summary>
        /// OKコマンド
        /// </summary>
        public ICommand OkCommand { get; private set; }
        /// <summary>
        /// Cancelコマンド
        /// </summary>
        public ICommand CancelCommand { get; private set; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskDialogViewModel()
        {
            // コマンドを作成する
            OkCommand = CreateCommand(ExecuteOkCommand, CanExecuteOkCommand);
            CancelCommand = CreateCommand(ExecuteCancelCommand, CanExecuteCancelCommand);
        }
        #endregion

        #region OkCommand
        /// <summary>
        /// OKコマンドを実行する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        private void ExecuteOkCommand(object parameter)
        {
            Result = Task;
            CloseWindow = true;
        }
        /// <summary>
        /// OKコマンドが実行可能かどうか判定する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private bool CanExecuteOkCommand(object parameter)
        {
            return !(string.IsNullOrEmpty(Task.Name) || Task.DueDate == null);
        }
        #endregion

        #region CancelCommand
        /// <summary>
        /// Cancelコマンドを実行する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        private void ExecuteCancelCommand(object parameter)
        {
            CloseWindow = true;
        }
        /// <summary>
        /// Cancelコマンドが実行可能かどうか判定する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private bool CanExecuteCancelCommand(object parameter)
        {
            // 常に実行可能
            return true;
        }
        #endregion
    }
}
