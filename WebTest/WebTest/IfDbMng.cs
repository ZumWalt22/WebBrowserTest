using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// DB管理インターフェース
    /// </summary>
    interface IfDbMng
    {
        /// <summary>
        /// 取得処理
        /// </summary>
        /// <param name="sql">取得SQL</param>
        /// <returns>テーブル情報</returns>
        TableInfoVO SelectData(string sql);

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="sql"></param>
        void InsertData(string sql);
    }
}
