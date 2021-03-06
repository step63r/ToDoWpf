﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        private bool _exitAsMinimized = false;
        /// <summary>
        /// 通知領域に格納する
        /// </summary>
        public bool ExitAsMinimized
        {
            get
            {
                return _exitAsMinimized;
            }
            set
            {
                _exitAsMinimized = value;
                RaisePropertyChanged(nameof(ExitAsMinimized));

                // アプリケーション設定に保存
                Properties.Settings.Default.ExitAsMinimized = value;
            }
        }

        private TaskDialogViewModel _dialogViewModel;
        /// <summary>
        /// ダイアログViewModel
        /// </summary>
        public TaskDialogViewModel DialogViewModel
        {
            get
            {
                return _dialogViewModel;
            }
            set
            {
                if (_dialogViewModel != value)
                {
                    _dialogViewModel = value;
                    RaisePropertyChanged(nameof(DialogViewModel));
                }
            }
        }
        #endregion

        #region コマンド
        /// <summary>
        /// 削除コマンド
        /// </summary>
        public ICommand RemoveCommand { get; private set; }
        /// <summary>
        /// 新規作成コマンド
        /// </summary>
        public ICommand CreateNewCommand { get; private set; }
        /// <summary>
        /// タスク詳細表示コマンド
        /// </summary>
        public ICommand ShowDetailCommand { get; private set; }
        /// <summary>
        /// ダイアログ閉じるコマンド
        /// </summary>
        public ICommand CloseDialogCommand { get; private set; }
        #endregion

        #region メンバ変数
        /// <summary>
        /// 設定ファイルパス
        /// </summary>
        private static readonly string _filePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\ToDoWpf\ToDoTasks.xml";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            // アプリケーション設定をロード
            ExitAsMinimized = Properties.Settings.Default.ExitAsMinimized;

            // コマンドを作成する
            // 今回はViewModelBaseの仕様でExecute/CanExecuteともに
            // object型の引数を1つ取るメソッドでなければならないが（ラムダ式も可能）
            // Prismなど著名なライブラリを使えばこの辺りの不便さは解決されている
            RemoveCommand = CreateCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand);
            CreateNewCommand = CreateCommand(ExecuteCreateNewCommand, CanExecuteCreateNewCommand);
            ShowDetailCommand = CreateCommand(ExecuteShowDetailCommand, CanExecuteShowDetailCommand);
            CloseDialogCommand = CreateCommand(ExecuteCloseDialogCommand, CanExecuteCloseDialogCommand);

            // アプリケーション設定からタスク一覧を読み込む
            CreateSettingsIfNotExists();
            var ret = XmlConverter.DeSerialize<ObservableCollection<ToDoTask>>(_filePath);

            Tasks = ret ?? new ObservableCollection<ToDoTask>();

            // ソートしとく
            Tasks = SortTasks(Tasks);
        }
        #endregion

        #region RemoveCommand
        /// <summary>
        /// 削除コマンドを実行する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        private void ExecuteRemoveCommand(object parameter)
        {
            Tasks.Remove(SelectedTask);
            SelectedTask = null;

            // ソートしとく
            Tasks = SortTasks(Tasks);

            // アプリケーション設定にタスク一覧を保存する
            XmlConverter.Serialize(Tasks, _filePath);
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
        #endregion

        #region CreateNewCommand
        /// <summary>
        /// 新規作成コマンドを実行する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        private void ExecuteCreateNewCommand(object parameter)
        {
            DialogViewModel = new TaskDialogViewModel()
            {
                Task = new ToDoTask()
            };
        }
        /// <summary>
        /// 新規作成コマンドが実行可能かどうか判定する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private bool CanExecuteCreateNewCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region ShowDetailCommand
        /// <summary>
        /// タスク詳細表示コマンドを実行する
        /// </summary>
        /// <param name="paramenter">パラメータ</param>
        private void ExecuteShowDetailCommand(object paramenter)
        {
            DialogViewModel = new TaskDialogViewModel()
            {
                Task = new ToDoTask(SelectedTask)
            };
        }
        /// <summary>
        /// タスク詳細表示コマンドが実行可能かどうか判定する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private bool CanExecuteShowDetailCommand(object parameter)
        {
            return SelectedTask != null;
        }
        #endregion

        #region CloseDialogCommand
        /// <summary>
        /// ダイアログ閉じるコマンドを実行する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        private void ExecuteCloseDialogCommand(object parameter)
        {
            if (parameter is TaskDialogViewModel viewModel)
            {
                if (viewModel.Result != null)
                {
                    _ = UpdateTasks(viewModel.Result);
                }
            }
        }
        /// <summary>
        /// ダイアログ閉じるコマンドが実行可能かどうか判定する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private bool CanExecuteCloseDialogCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region 内部メソッド
        /// <summary>
        /// 設定ファイルが存在しない場合、作成する
        /// </summary>
        private static void CreateSettingsIfNotExists()
        {
            // ディレクトリ取得
            string dirInfo = Path.GetDirectoryName(_filePath);
            Directory.CreateDirectory(dirInfo);

            // ファイルが存在しなければ作る
            if (!File.Exists(_filePath))
            {
                var obj = new ObservableCollection<ToDoTask>();
                XmlConverter.Serialize(obj, _filePath);
            }
        }

        /// <summary>
        /// タスク一覧をソートする
        /// </summary>
        /// <param name="source">元コレクション</param>
        /// <returns>ソートされたコレクション</returns>
        /// <remarks>一度 List に変換しないとソートできないため、仕方なくこうする</remarks>
        private ObservableCollection<ToDoTask> SortTasks(ObservableCollection<ToDoTask> source)
        {
            var taskList = source.ToList();
            taskList.Sort();
            return new ObservableCollection<ToDoTask>(taskList);
        }

        /// <summary>
        /// タスク一覧を更新する
        /// </summary>
        /// <param name="targetTask">変更後のインスタンス</param>
        /// <returns>成功/失敗</returns>
        private bool UpdateTasks(ToDoTask targetTask)
        {
            bool ret = false;

            var previousTask = Tasks.Where(item => item.Equals(targetTask)).FirstOrDefault();
            if (previousTask != null)
            {
                // 古いオブジェクトを削除する
                Tasks.Remove(previousTask);
            }

            // 新しいオブジェクトを追加する
            Tasks.Add(targetTask);

            // ソートしとく
            Tasks = SortTasks(Tasks);

            // アプリケーション設定にタスク一覧を保存する
            XmlConverter.Serialize(Tasks, _filePath);

            return ret;
        }
        #endregion
    }
}
