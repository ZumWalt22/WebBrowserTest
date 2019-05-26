using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using Microsoft.Office.Interop.Excel; //EXCELコンポーネントを使用するため必要

namespace ExcelMng
{
    //TODO 一時的にCSVで動くように変更
    //本来ならば、FactoryパターンでExcelとCSVを切り替えられるようにするべき？

    /// <summary>
    /// Excel(CSV)コマンドクラス
    /// </summary>
    public class ExcelController
    {
        /// <summary>
        /// ブック名
        /// </summary>
        String bookName;

        /// <summary>
        /// ブック名(Open済み比較用)
        /// </summary>
        String bookActiveName;

        /// <summary>
        /// Excelファイルパス
        /// </summary>
        String excelPass;

        /// <summary>
        /// 作業対象Book Dictionary<SheetName,ColXY>
        /// </summary>
        Dictionary<String,List<List<String>>> workbookDict = new Dictionary<string, List<List<string>>>();

        /// <summary>
        /// Excelファイル設定
        /// </summary>
        /// <param name="excelPass">Excelファイルパス</param>
        public void setExcelFilePass(string excelPass)
        {
            this.excelPass = excelPass;

            //string bookName = Path.GetFileNameWithoutExtension(excelPass);
            //bookName = bookName.Split('.')[0];

            //this.bookName = bookName;

            //Dir名取得　
            this.bookName = Path.GetDirectoryName(excelPass);

        }

        /// <summary>
        /// Book名称設定
        /// </summary>
        /// <returns></returns>
        public string getBookName()
        {
            return bookName;
        }

        /// <summary>
        /// Bookを開く
        /// </summary>
        /// <param name="excelFileName"></param>
        /// <returns>Book名称</returns>
        public void openExcelFile()
        {
            try
            {
                //Dir内のCSVファイル一覧を取得する
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(excelPass);
                System.IO.FileInfo[] files = di.GetFiles("*.csv", System.IO.SearchOption.TopDirectoryOnly);
                foreach (System.IO.FileInfo f in files)
                {
                    //CSVファイル情報
                    List<List<String>> csvColXYList = new List<List<string>>();

                    //CSVパス情報を取得
                    string csvPass = f.FullName;

                    //CSVファイル読み取り
                    using (StreamReader reader = new StreamReader(csvPass, Encoding.Default))
                    {
                        // 読み取り可能文字が存在しない（ファイルの末尾に到着）すると -1 が返される
                        while (reader.Peek() > -1)
                        {
                            //文字列を取得
                            string text = reader.ReadLine();

                            List<String> csvColXList = new List<string>();
                            csvColXList = text.Split(',').ToList<String>();
                            //Col行情報を追加
                            csvColXYList.Add(csvColXList);
                        }

                        // ファイルを閉じる
                        reader.Close();
                    }

                    //CSVファイル名を取得
                    string sheetName = Path.GetFileNameWithoutExtension(csvPass);

                    //CSVファイル名をSheet名とし、設定
                    workbookDict.Add(sheetName, csvColXYList);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Bookを閉じる
        /// </summary>
        public void closeExcelFile()
        {
            //CSVの場合、なにもしない
        }

        /// <summary>
        /// 値を取得
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="sheetName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public string getValue(string sheetName, int x, int y)
        {
            //// 与えられたワークシート名から、Worksheetオブジェクトを得る
            //int index = getSheetIndex(sheetName, workbook.Sheets);
            //if (index > 0)
            //{
            //    Worksheet oSheet = (Worksheet)workbook.Sheets[getSheetIndex(sheetName, workbook.Sheets)];
            //    Range rng = (Range)oSheet.Cells[y, x];
            //    return (string)rng.Text;
            //}

            //シート情報を取得
            List<List<String>> csvColXYList = workbookDict[sheetName];

            string colStr = "";
            if (csvColXYList.Count > y)
            {
                if (csvColXYList[y].Count > x)
                {
                    colStr = csvColXYList[y - 1][x - 1];
                }
            }
            Console.WriteLine("x:" + x + " y:" + y);

            //配列位置の情報を取得
            return colStr;
        }

        /// <summary>
        /// シート名のリストを取得
        /// </summary>
        /// <returns></returns>
        public List<String> getSheetList()
        {
            List<String> sheetList = new List<String>();

            //if (isCloseExcelFile())
            //{
            //    // シート名一覧の取得
            //    foreach (Worksheet sh in workbook.Sheets)
            //    {
            //        sheetList.Add(sh.Name);
            //    }
            //}

            // シート名一覧の取得
            foreach (String sheetName in workbookDict.Keys)
            {
                sheetList.Add(sheetName);
            }

            return sheetList;
        }

    }

        /* 機種依存のため使用しない

        /// <summary>
        /// Excelコマンドクラス
        /// </summary>
        public class ExcelController
        {
            /// <summary>
            /// Excelオブジェクト
            /// </summary>
            Application oXls; 

            /// <summary>
            /// 作業対象Book
            /// </summary>
            Workbook workbook;

            /// <summary>
            /// ブック名
            /// </summary>
            String bookName;

            /// <summary>
            /// ブック名(Open済み比較用)
            /// </summary>
            String bookActiveName;

            /// <summary>
            /// Excelファイルパス
            /// </summary>
            String excelPass;

            //エビデンス設定情報
            public int setX = 1;
            public int setY = 1;
            public int setAddX = 13;
            public int setAddY = 100;
            public int setDbAddY = 50;
            public int setW = 640;
            public int setH = 400;

            //COMコンポーネントへの参照をプロジェクトに追加する。
            //参照の追加］ウィンドウで［COM］タブを選択し、「Microsoft Excel 14.0 Object Library」を選択（EXCEL2010の場合）

            //EXCELファイルをOpenする
            //http://www.atmarkit.co.jp/fdotnet/dotnettips/717excelfile/excelfile.html

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="oXls">Application</param>
            public ExcelController(Application oXls)
            {
                this.oXls = oXls;
            }

            /// <summary>
            /// Excelファイル設定
            /// </summary>
            /// <param name="excelPass">Excelファイルパス</param>
            public void setExcelFilePass(string excelPass)
            {
                this.excelPass = excelPass;

                string bookName = Path.GetFileNameWithoutExtension(excelPass);
                bookName = bookName.Split('.')[0];

                this.bookName = bookName;
            }

            /// <summary>
            /// Book名称設定
            /// </summary>
            /// <returns></returns>
            public string getBookName(){
                return bookName;
            }

            /// <summary>
            /// Bookを開く
            /// </summary>
            /// <param name="excelFileName"></param>
            /// <returns>Book名称</returns>
            public void openExcelFile()
            {

                //oXls = new Application();
                //oXls.Visible = true; // Excelのウィンドウを表示する
                oXls.Visible = false; 

                // Excelファイルをオープンする
                workbook = (Workbook)(oXls.Workbooks.Open(
                    excelPass,  // オープンするExcelファイル名
                    Type.Missing, // （省略可能）UpdateLinks (0 / 1 / 2 / 3)
                    Type.Missing, // （省略可能）ReadOnly (True / False )
                    Type.Missing, // （省略可能）Format
                    // 1:タブ / 2:カンマ (,) / 3:スペース / 4:セミコロン (;)
                    // 5:なし / 6:引数 Delimiterで指定された文字
                    Type.Missing, // （省略可能）Password
                    Type.Missing, // （省略可能）WriteResPassword
                    Type.Missing, // （省略可能）IgnoreReadOnlyRecommended
                    Type.Missing, // （省略可能）Origin
                    Type.Missing, // （省略可能）Delimiter
                    Type.Missing, // （省略可能）Editable
                    Type.Missing, // （省略可能）Notify
                    Type.Missing, // （省略可能）Converter
                    Type.Missing, // （省略可能）AddToMru
                    Type.Missing, // （省略可能）Local
                    Type.Missing  // （省略可能）CorruptLoad
                ));

                bookActiveName = workbook.Name;
            }

            /// <summary>
            /// Bookを閉じる
            /// </summary>
            public void closeExcelFile()
            {
                try
                {
                    workbook.Close(Type.Missing, Type.Missing, Type.Missing);
                    //oXls.Quit();
                }
                catch (Exception)
                {

                }
            }

            /// <summary>
            /// Bookが閉じられたかを確認する
            /// </summary>
            public bool isCloseExcelFile()
            {
                if (bookActiveName != null)
                {
                    foreach (Workbook workbook in oXls.Workbooks)
                    {
                        if (bookActiveName.Equals(workbook.Name))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            // 指定されたワークシート名のインデックスを返すメソッド
            public int getSheetIndex(string sheetName, Sheets shs)
            {
              int i = 0;
              foreach (Worksheet sh in shs)
              {
                if (sheetName == sh.Name)
                {
                  return i + 1;
                }
                i += 1;
              }
              return 1;
            }

            /// <summary>
            /// シートを設定
            /// </summary>
            /// <param name="sheetIndex"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="str"></param>
            /// <returns></returns>
            public void setActiveSheet(string sheetName)
            {
                workbook.Activate();

                // 与えられたワークシート名から、Worksheetオブジェクトを得る
                Worksheet worksheet = (Worksheet)workbook.Sheets[getSheetIndex(sheetName, workbook.Sheets)];

                worksheet.Select();
            }

            /// <summary>
            /// セルを設定
            /// </summary>
            /// <param name="sheetIndex"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="str"></param>
            /// <returns></returns>
            public void setActiveCell(string sheetName, int x, int y)
            {
                // 与えられたワークシート名から、Worksheetオブジェクトを得る
                Worksheet worksheet = (Worksheet)workbook.Sheets[getSheetIndex(sheetName, workbook.Sheets)];

                worksheet.Select();

                Range rng = (Range)worksheet.Cells[y, x];

                //選択Cellをアクティブにする
                rng.Select();
            }

            /// <summary>
            /// 背景色を設定
            /// </summary>
            /// <param name="workbook"></param>
            /// <param name="sheetName"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="str"></param>
            /// <returns></returns>
            public void setBackColor(string sheetName, int x, int y, int r,int g,int b)
            {
                // 与えられたワークシート名から、Worksheetオブジェクトを得る
                Worksheet worksheet = (Worksheet)workbook.Sheets[getSheetIndex(sheetName, workbook.Sheets)];

                Range rng = (Range)worksheet.Cells[y, x];
                rng.Interior.Color = System.Drawing.Color.FromArgb(r, g, b);
            }

            /// <summary>
            /// 値を設定
            /// </summary>
            /// <param name="workbook"></param>
            /// <param name="sheetName"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="str"></param>
            /// <returns></returns>
            public void setValue(string sheetName, int x, int y, string str)
            {
                // 与えられたワークシート名から、Worksheetオブジェクトを得る
                Worksheet worksheet = (Worksheet)workbook.Sheets[getSheetIndex(sheetName, workbook.Sheets)];

                Range rng = (Range)worksheet.Cells[y, x];
                rng.Value2 = str;
            }

            /// <summary>
            /// 値を取得
            /// </summary>
            /// <param name="workbook"></param>
            /// <param name="sheetName"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public string getValue(string sheetName, int x, int y)
            {
                // 与えられたワークシート名から、Worksheetオブジェクトを得る
                int index = getSheetIndex(sheetName, workbook.Sheets);
                if (index > 0){
                    Worksheet oSheet = (Worksheet)workbook.Sheets[getSheetIndex(sheetName, workbook.Sheets)];
                    Range rng = (Range)oSheet.Cells[y, x];
                    return (string)rng.Text;
                }

                return "";
            }

            /// <summary>
            /// シートを追加する
            /// </summary>
            /// <param name="sheetname"></param>
            public void addSheet(string sheetname)
            {
                Worksheet oSheet = (Worksheet)workbook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oSheet.Name = sheetname;
            }

            /// <summary>
            /// シートを削除する
            /// </summary>
            /// <param name="sheetname"></param>
            public void deleteSheet(string sheetname)
            {
                // 与えられたワークシート名から、Worksheetオブジェクトを得る
                int sheetIndex = getSheetIndex(sheetname, workbook.Sheets);
                if (sheetIndex > 0)
                {
                    Worksheet worksheet = (Worksheet)workbook.Sheets[sheetIndex];
                    worksheet.Delete();
                }
            }

            /// <summary>
            /// シート名のリストを取得
            /// </summary>
            /// <returns></returns>
            public List<String> getSheetList()
            {
                List<String> sheetList = new List<String>();

                if (isCloseExcelFile())
                {
                    // シート名一覧の取得
                    foreach (Worksheet sh in workbook.Sheets)
                    {
                        sheetList.Add(sh.Name);
                    }
                }

                return sheetList;
            }

            /// <summary>
            /// 画像を張りつけ
            /// </summary>
            /// <param name="sheetName">シート名称</param>
            /// <param name="imageFileName">画像ファイル名</param>
            /// <param name="x">設定位置　X</param>
            /// <param name="y">設定位置　Y</param>
            /// <param name="w">イメージサイズ 横</param>
            /// <param name="h">イメージサイズ 縦</param>
            public void setImage(string sheetName, string imageFileName, int x, int y, int w, int h)
            {
                // 与えられたワークシート名から、Worksheetオブジェクトを得る
                Worksheet worksheet = null;

                // アクティブシートを設定
                if (!string.IsNullOrEmpty(sheetName))
                {
                    worksheet = (Worksheet)workbook.Sheets[getSheetIndex(sheetName, workbook.Sheets)];
                }
                else
                {
                    worksheet = (Worksheet)workbook.ActiveSheet;
                }

                //worksheet.shapes.AddPicture("e:\\a1.png", Core.MsoTriState.msoFalse, Core.MsoTriState.msoTrue, 72, 19, 126, 55);

                Microsoft.Office.Interop.Excel.Range cells = worksheet.Cells;

                //Cellの位置を指定する
                Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)cells[y, x];

                //指定のCellに移動する
                range.Select();

                Microsoft.Office.Interop.Excel.Shapes shapes = worksheet.Shapes;

                //画像設定位置を割り出し
                float fx = float.Parse(range.Left.ToString());
                float fy = float.Parse(range.Top.ToString());

                //画像をExcelに設定する
                worksheet.Shapes.AddPicture(
                    imageFileName,
                    Microsoft.Office.Core.MsoTriState.msoFalse,
                    Microsoft.Office.Core.MsoTriState.msoTrue,
                    fx, fy, w, h);
            }
        }
    */
    }
