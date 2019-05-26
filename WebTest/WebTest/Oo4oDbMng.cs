using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using OracleInProcServer;

using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace WindowsFormsApplication1
{

/* 環境依存になるため使用しない
    /// <summary>
    /// Oo4o接続DB管理クラス
    /// </summary>
    class Oo4oDbMng : IfDbMng
    {
        //参照の「COM」タブで「Oracle InProc Server 5.0 Type Library」を選択することで、
        //「Bin」フォルダ直下に「Interop.OracleInProcServer.dll」が追加されます

        public Oo4oDbMng()
        {
            ////SQL文
            //StringBuilder sql = default(StringBuilder);
            ////以下、ASP+oo4oで使用していた変数
            ////OraDatabase
            //OraDatabase OraDatabase = default(OraDatabase);
            ////RecordSet
            //OraDynaset rs = null;
            ////RecordSetより取得するFieldsコレクション
            //OraFields fields = default(OraFields);
            ////RecordSetのFieldsコレクションより取得するフィールド
            //OraField field = default(OraField);

            ////SQL文を編集
            ////ここでは、CURRENT_TIMESTAMP関数で年～ミリ秒までを取得するだけ
            //sql = new StringBuilder();
            //sql.Append("SELECT TO_CHAR(CURRENT_TIMESTAMP, 'YYYY/MM/DD HH24:MI:SS.FF3') FROM DUAL");

            //try {
            //    //SQL文を実行し、RecordSetに格納
            //    rs = (OraDynaset)OraDatabase.CreateDynaset(sql.ToString(),0,ref 
            //    //データが存在する場合
            //    //（ここでは必ず存在するが、一応判断は行っておく）
            //    if (rs.EOF == false) {
            //        //------------------------------------------------------------
            //        //遅延バインディングを使用しない場合、このようなコードとなる
            //        //------------------------------------------------------------
            //        //RecordSet.FieldsをOraFieldsにキャスト
            //        fields = (OraFields)rs.Fields;
            //        //RecordSet.Fields(0)をOraFieldにキャスト
            //        //field = (OraField)fields(0);
            //        //------------------------------------------------------------

            //    }
            //} finally {
            //    //Finally処理でRecordSetを閉じる
            //    rs.Close();
            //    rs = null;
            //}
        }

        public TableInfoVO SelectData(string sql)
        {
            throw new System.NotImplementedException();
        }

        public void InsertData(string sql)
        {
            throw new System.NotImplementedException();
        }
    }
*/

}


