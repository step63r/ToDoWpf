using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoWpf.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// btnInputTaskをクリックしたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInputTask_Click(object sender, RoutedEventArgs e)
        {
            // テキストボックスに文字が入力されているかチェック
            if (!string.IsNullOrEmpty(tbInputTask.Text))
            {
                // リストボックスに追加
                lbTasks.Items.Add(tbInputTask.Text);

                // テキストボックスを空にしておく
                tbInputTask.Text = string.Empty;
            }
            else
            {
                Debug.WriteLine("テキストボックスが空です");
            }
        }
    }
}
