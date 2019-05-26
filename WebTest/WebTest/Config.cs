using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace TaskTop
{
    class Config
    {
        //コンフィグファイルの配置場所
        private string baseFilePass = "Conf";

        //コンフィグファイルの配置場所
        private string fileName = "setting.ini";

        //Config情報格納テーブル
        Hashtable configInfoTable = new Hashtable();

        /// <summary>
        /// Configファイルから読み取り
        /// </summary>
        public Config()
        {
            string line = "";

            using (StreamReader sr = new StreamReader(baseFilePass + "\\" + fileName,
                Encoding.GetEncoding("Shift_JIS")))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    //イコールで分離
                    string[] splitStr = line.Split('=');

                    //ハッシュテーブルに格納
                    if (splitStr.Length == 2){
                        configInfoTable.Add(splitStr[0], splitStr[1]);
                    }
                }
            }
        }

        /// <summary>
        /// Config情報取得
        /// </summary>
        /// <returns></returns>
        public string getdefaultItem(string key){

            return configInfoTable[key].ToString();
        }
    }
}
