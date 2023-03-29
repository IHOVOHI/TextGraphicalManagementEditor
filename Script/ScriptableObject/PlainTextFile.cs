using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextGraphicalManagementEditor {
    /// <summary>
    /// 普通文本文件 保存普通文本
    /// </summary>
    [CreateAssetMenu(menuName = "Text Graphical Marager/Plain Text")]
    public class PlainTextFile : TextFile
    {

        [HideInInspector]
        public List<PlainTextDate> plainTexts = new List<PlainTextDate>();
    }

    [Serializable]
    public class PlainTextDate {
        /// <summary>
        /// 数据分类名字
        /// </summary>
        [HideInInspector]
        public string typeName;
        /// <summary>
        /// 数据集合
        /// </summary>
        [HideInInspector]
        public List<string> contents;
    }
}
