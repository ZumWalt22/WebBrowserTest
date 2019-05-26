using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class AdoDbMng : IfDbMng
    {
        //http://jeanne.wankuma.com/tips/csharp/sqlserver/

        public AdoDbMng()
        {
            // 接続文字列を生成する
            string stConnectionString = string.Empty;
            stConnectionString += "Data Source         = XXX;";
            //stConnectionString += "Initial Catalog     = Northwind;";
            //stConnectionString += "Integrated Security = SSPI;";

            stConnectionString += "User ID = xxx;";
            stConnectionString += "Password = xxxxx;";

            // SqlConnection の新しいインスタンスを生成する (接続文字列を指定)
            System.Data.SqlClient.SqlConnection cSqlConnection = (
                new System.Data.SqlClient.SqlConnection(stConnectionString)
            );

            // データベース接続を開く
            cSqlConnection.Open();

            // 接続に成功した旨を表示する
            //MessageBox.Show("Microsoft SQL Server に接続されました");

            // データベース接続を閉じる (正しくは オブジェクトの破棄を保証する を参照)
            cSqlConnection.Close();
            cSqlConnection.Dispose();
        }

        public TableInfoVO SelectData(string sql)
        {
            throw new NotImplementedException();
        }

        public void InsertData(string sql)
        {
            throw new NotImplementedException();
        }
    }
}
