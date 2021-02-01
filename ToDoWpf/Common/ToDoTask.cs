using System;

namespace ToDoWpf.Common
{
    /// <summary>
    /// タスクをモデル化したクラス
    /// </summary>
    /// <remarks>分かりやすく Task という名前にすべきだったが、 System.Threading.Tasks 名前空間に Task があってややこしいのでこういう名前にする</remarks>
    public class ToDoTask
    {
        /// <summary>
        /// 一意な識別子
        /// </summary>
        public Guid Guid { get; private set; }
        /// <summary>
        /// タスク名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ToDoTask()
        {
            Guid = Guid.NewGuid();
        }
    }
}
