using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoWpf.Common;

namespace ToDoWpf.ViewModels
{
    /// <summary>
    /// MainWindowのViewModelクラス
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region プロパティ
        private ObservableCollection<ToDoTask> _tasks = new ObservableCollection<ToDoTask>();
        /// <summary>
        /// タスク一覧
        /// </summary>
        public ObservableCollection<ToDoTask> Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                if (_tasks != value)
                {
                    _tasks = value;
                    RaisePropertyChanged(nameof(Tasks));
                }
            }
        }

        private ToDoTask _inputTask = new ToDoTask();
        /// <summary>
        /// 入力されたタスク
        /// </summary>
        public ToDoTask InputTask
        {
            get
            {
                return _inputTask;
            }
            set
            {
                if (_inputTask != value)
                {
                    _inputTask = value;
                    RaisePropertyChanged(nameof(InputTask));
                }
            }
        }

        private ToDoTask _selectedTask;
        /// <summary>
        /// タスク一覧から選択されたタスク
        /// </summary>
        public ToDoTask SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                if (_selectedTask != value)
                {
                    _selectedTask = value;
                    RaisePropertyChanged(nameof(SelectedTask));
                }
            }
        }
        #endregion

        #region コマンド
        /// <summary>
        /// 追加コマンド
        /// </summary>
        public ICommand AddCommand { get; private set; }
        /// <summary>
        /// 削除コマンド
        /// </summary>
        public ICommand RemoveCommand { get; private set; }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            // コマンドを作成する
            // 今回はViewModelBaseの仕様でExecute/CanExecuteともに
            // object型の引数を1つ取るメソッドでなければならないが（ラムダ式も可能）
            // Prismなど著名なライブラリを使えばこの辺りの不便さは解決されている
            AddCommand = CreateCommand(ExecuteAddCommand, CanExecuteAddCommand);
            RemoveCommand = CreateCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand);

            // アプリケーション設定からタスク一覧を読み込む
            Tasks = Properties.Settings.Default.Tasks ?? new ObservableCollection<ToDoTask>();
        }

        /// <summary>
        /// 追加コマンドを実行する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        private void ExecuteAddCommand(object parameter)
        {
            Tasks.Add(InputTask);
            InputTask = new ToDoTask();

            // アプリケーション設定にタスク一覧を保存する
            Properties.Settings.Default.Tasks = Tasks;
        }

        /// <summary>
        /// 追加コマンドが実行可能かどうか判定する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private bool CanExecuteAddCommand(object parameter)
        {
            return !string.IsNullOrEmpty(InputTask.Name);
        }

        /// <summary>
        /// 削除コマンドを実行する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        private void ExecuteRemoveCommand(object parameter)
        {
            Tasks.Remove(SelectedTask);
            SelectedTask = null;

            // アプリケーション設定にタスク一覧を保存する
            Properties.Settings.Default.Tasks = Tasks;
        }

        /// <summary>
        /// 削除コマンドが実行可能かどうか判定する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private bool CanExecuteRemoveCommand(object parameter)
        {
            return SelectedTask != null && Tasks.Contains(SelectedTask);
        }
    }
}
