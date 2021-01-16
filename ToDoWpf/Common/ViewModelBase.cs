using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace ToDoWpf.Common
{
    /// <summary>
    /// ViewModelの基底クラス
    /// </summary>
    /// <remarks>
    /// <para>MVVMフレームワークのライブラリが豊富に提供されているため、普段の開発でここまで自作することはない。</para>
    /// <para>今回は理解のためフルスクラッチで実装してみる。</para>
    /// <para>実装内容はほぼお決まりパターンがあるが、開発者やC#のバージョンによって微妙に表記が異なる。</para>
    /// <para>[参考URL] https://qiita.com/tricogimmick/items/f07ef53dea817d198475</para>
    /// </remarks>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        #region INotifyPropertyChangedの実装
        /// <summary>
        /// INotifyPropertyChanged.PropertyChangedの実装
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// INotifyPropertyChanged.PropertyChangedイベントを発生させる
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region IDataErrorInfoの実装
        /// <summary>
        /// IDataErrorInfo用のエラーメッセージを保持する辞書
        /// </summary>
        private Dictionary<string, string> _errorMessage = new Dictionary<string, string>();

        /// <summary>
        /// IDataErrorInfo.Error の実装
        /// </summary>
        public string Error
        {
            get
            {
                return (_errorMessage.Count > 0) ? "Has Error" : null;
            }
        }

        /// <summary>
        /// IDataErrorInfo.Itemの実装
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                if (_errorMessage.ContainsKey(columnName))
                {
                    return _errorMessage[columnName];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// エラーメッセージのセット
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        protected void SetError(string propertyName, string errorMessage)
        {
            _errorMessage[propertyName] = errorMessage;
        }

        /// <summary>
        /// エラーメッセージのクリア
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected void ClearError(string propertyName)
        {
            if (_errorMessage.ContainsKey(propertyName))
            {
                _errorMessage.Remove(propertyName);
            }
        }
        #endregion

        /// <summary>
        /// ICommand実装用のヘルパークラス
        /// </summary>
        private class _delegateCommand : ICommand
        {
            /// <summary>
            /// コマンド本体
            /// </summary>
            private Action<object> _command;

            /// <summary>
            /// 実行可否
            /// </summary>
            private Func<object, bool> _canExecute;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="command">コマンド本体</param>
            /// <param name="canExecute">実行可否</param>
            public _delegateCommand(Action<object> command, Func<object, bool> canExecute = null)
            {
                if (command == null)
                {
                    throw new ArgumentNullException();
                }

                _command = command;
                _canExecute = canExecute;
            }

            /// <summary>
            /// ICommand.Executeの実装
            /// </summary>
            /// <param name="parameter">パラメータ</param>
            void ICommand.Execute(object parameter)
            {
                _command(parameter);
            }

            /// <summary>
            /// ICommand.CanExecuteの実装
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns></returns>
            bool ICommand.CanExecute(object parameter)
            {
                if (_canExecute != null)
                {
                    return _canExecute(parameter);
                }
                else
                {
                    return true;
                }
            }

            /// <summary>
            /// ICommand.CanExecuteChangedの実装
            /// </summary>
            event EventHandler ICommand.CanExecuteChanged
            {
                add
                {
                    CommandManager.RequerySuggested += value;
                }
                remove
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// コマンドの生成
        /// </summary>
        /// <param name="command">コマンド本体</param>
        /// <param name="canExecute">実行可否</param>
        /// <returns></returns>
        protected ICommand CreateCommand(Action<object> command, Func<object, bool> canExecute = null)
        {
            return new _delegateCommand(command, canExecute);
        }
    }
}
