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
        private ObservableCollection<string> _tasks = new ObservableCollection<string>();
        /// <summary>
        /// タスク一覧
        /// </summary>
        public ObservableCollection<string> Tasks
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

        private string _inputTask = "";
        /// <summary>
        /// 入力されたタスク
        /// </summary>
        public string InputTask
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
        #endregion

        #region コマンド
        /// <summary>
        /// 追加コマンド
        /// </summary>
        public ICommand AddCommand { get; private set; }
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
        }

        /// <summary>
        /// 追加コマンドを実行する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        private void ExecuteAddCommand(object parameter)
        {
            Tasks.Add(InputTask);
            InputTask = string.Empty;
        }

        /// <summary>
        /// 追加コマンドが実行可能かどうか判定する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private bool CanExecuteAddCommand(object parameter)
        {
            return !string.IsNullOrEmpty(InputTask);
        }
    }
}
