using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// HTML内のformタグ情報を設定　構造体
    /// </summary>
    public class FormElement
    {
        public enum ElementType { TextBox, DropDownList, RadioButton, CheckBox, Label, HiddenValue};

        public ElementType elementType;
        
        /// <summary>
        /// Id
        /// </summary>
        public string id;

        /// <summary>
        /// 値
        /// </summary>
        public string val;

        /// <summary>
        /// Id項目（Name要素）が複数ある場合の識別番号
        /// </summary>
        public int index;

        /// <summary>
        /// 入出力フラグ 0:In 1:Out
        /// </summary>
        public int inOutFlag;

        /// <summary>
        /// 結果正誤チェック結果
        /// </summary>
        public bool valCheck;


        /// <summary>
        /// フォームに値を設定
        /// </summary>
        /// <param name="id">要素のID</param>
        /// <param name="val">設定する値</param>
        /// <param name="inOut">入出力フラグ</param>
        public FormElement(String id, String val, ElementType elementType, int inOutFlag,int index)
        {
            this.elementType = elementType;
            this.id = id;
            this.val = val;
            this.inOutFlag = inOutFlag;
            this.index = index;
        }
    }
}
