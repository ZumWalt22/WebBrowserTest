using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class TableInfoVO
    {
        //テーブル名称
        public string talbeName = null;

        //カラム名称リスト
        public List<string> columnNameList = new List<string>();

        //値リスト
        public List<List<string>> rowValList = new List<List<string>>();
    }
}
