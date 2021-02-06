using System;
using System.ComponentModel;

namespace ToDoWpf.Common
{
    /// <summary>
    /// 優先度
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// 低
        /// </summary>
        [Description("低")]
        Low,
        /// <summary>
        /// 中
        /// </summary>
        [Description("中")]
        Medium,
        /// <summary>
        /// 高
        /// </summary>
        [Description("高")]
        High
    }

    /// <summary>
    /// タスクをモデル化したクラス
    /// </summary>
    /// <remarks>分かりやすく Task という名前にすべきだったが、 System.Threading.Tasks 名前空間に Task があってややこしいのでこういう名前にする</remarks>
    public class ToDoTask : IComparable<ToDoTask>, IEquatable<ToDoTask>
    {
        /// <summary>
        /// 一意な識別子
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// タスク名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 期限
        /// </summary>
        public DateTime DueDate { get; set; }
        /// <summary>
        /// 優先度
        /// </summary>
        public Priority Priority { get; set; }
        /// <summary>
        /// 詳細メモ
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ToDoTask()
        {
            Guid = Guid.NewGuid();
            DueDate = DateTime.Now;
            Priority = Priority.Medium;
        }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="source">コピー元オブジェクト</param>
        public ToDoTask(ToDoTask source)
        {
            Guid = source.Guid;
            Name = source.Name;
            DueDate = source.DueDate;
            Priority = source.Priority;
            Detail = source.Detail;
        }

        /// <summary>
        /// このクラスのオブジェクト同士を比較する
        /// </summary>
        /// <param name="other">他のオブジェクト</param>
        /// <returns>自分自身の方が小さければ0未満の値、大きければ0より大きい値、等しければ0</returns>
        public int CompareTo(ToDoTask other)
        {
            // (1) 期限の古い順
            if (!DueDate.Date.Equals(other.DueDate.Date))
            {
                // 自分の DueDate の方が古ければ、負の値になる
                return (int)(DueDate.Date - other.DueDate.Date).TotalMinutes;
            }

            // (2) 期限が同じ場合、優先度の高い順
            if (!Priority.Equals(other.Priority))
            {
                // 自分の Priority の方が高ければ、負の値になる
                return other.Priority - Priority;
            }

            // (3) 優先度も同じなら、名前順
            return Name.CompareTo(other.Name);
        }

        /// <summary>
        /// このクラスのオブジェクト同士が等価かどうかを判定する
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ToDoTask other)
        {
            // Guidが等しければ同一のインスタンス
            return Guid.Equals(other.Guid);
        }
    }
}
