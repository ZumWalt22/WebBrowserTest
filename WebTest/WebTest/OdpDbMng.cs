using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* 環境依存になるため使用しない
namespace WindowsFormsApplication1
{
    //-----以下を追加
    using System.Data;
    using System.Data.Common;
    //using Oracle.DataAccess.Client;
    //using Oracle.DataAccess.Types;
    using System.Text;
    using System.Windows.Forms;


    /// <summary>
    /// DB管理クラス
    /// </summary>
    class OdpDbMng : IfDbMng
    {
        DbProviderFactory factory;
        DbConnectionStringBuilder ocsb;
        DbConnection conn;
        DbCommand cmd;
        DbDataAdapter da;
        DataSet ds;

        public class DataAccess
        {
            //DB接続関連はクラスで保有
            DbProviderFactory factory;
            DbConnection conn;
            DbConnectionStringBuilder ocsb;
            DbCommand cmd;
            DbDataAdapter da;
            DataSet ds;
        }

        #region 接続処理
        /// <summary>
        /// 接続処理
        /// </summary>
        /// <returns></returns>
        public OdpDbMng(string dataSource,string userId,string password)
        {
            //ローカルのOracleTNSに接続にいく Oracleクライアントインストール後 tnsnames.oraに以下を追記

            //JOO =
            //  (DESCRIPTION =
            //    (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.0.35)(PORT = 1521))
            //    (CONNECT_DATA =
            //      (SERVER = DEDICATED)
            //      (SERVICE_NAME = XE)
            //    )
            //  )

            try
            {

                //「.net framework データ プロバイダーが見つかりません」のエラーがでる場合「x86」でコンパイルするように設定する
                factory = DbProviderFactories.GetFactory("Oracle.DataAccess.Client");
                ocsb = factory.CreateConnectionStringBuilder();

                //接続文字列の設定
                ocsb["Data Source"] = dataSource;
                ocsb["User ID"] = userId;
                ocsb["Password"] = password;
                conn = factory.CreateConnection();
                conn.ConnectionString = ocsb.ConnectionString;

                //データベース接続
                conn.Open();
            }
            catch (Exception e)
            {
                //メッセージボックスを表示する
                MessageBox.Show(e.Message,
                    "DB接続エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// 切断処理
        /// </summary>
        /// <returns></returns>
        public void DbDisConnect()
        {

            if (conn != null)
            {
                //データベース切断
                conn.Close();
            }
        }
        #endregion

        #region ＤＢ操作
        /// <summary>
        /// 情報登録
        /// </summary>
        /// <param name="sql"></param>
        public void InsertData(string sql)
        {
            try
            {
                cmd = conn.CreateCommand();
                //cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                //Insert実行
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //メッセージボックスを表示する
                MessageBox.Show(e.Message,
                    "DB接続エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// テーブル情報取得
        /// </summary>
        /// <param name="sql">取得SQL</param>
        /// <returns>テーブル情報</returns>
        public TableInfoVO SelectData(string sql)
        {
            //テーブル情報格納
            TableInfoVO tableInfoVO = new TableInfoVO();

            //行情報格納
            List<string> rowInfoList = null;

            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                da = factory.CreateDataAdapter();
                da.SelectCommand = cmd;

                //SELECTの実行
                ds = new DataSet();
                da.Fill(ds);


                //データセットを取得
                foreach (DataTable table in ds.Tables)
                {

                    //テーブル名称を設定
                    tableInfoVO.talbeName = table.TableName;

                    //カラム情報を設定
                    foreach (DataColumn column in table.Columns)
                    {
                        tableInfoVO.columnNameList.Add(column.ColumnName);
                    }

                    //列情報を設定
                    List<string> rowInfo = new List<string>();
                    foreach (DataRow row in table.Rows)
                    {
                        //行情報を設定
                        rowInfoList = new List<string>();
                        foreach (Object cell in row.ItemArray)
                        {
                            rowInfoList.Add(cell.ToString());
                        }
                        tableInfoVO.rowValList.Add(rowInfoList);
                    }
                }
            }
            catch
            {

            }
            finally
            {
                this.DbDisConnect();
            }

            return tableInfoVO;
        }
        #endregion
    }
}

*/
