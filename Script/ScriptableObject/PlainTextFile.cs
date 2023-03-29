using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextGraphicalManagementEditor {
    /// <summary>
    /// ��ͨ�ı��ļ� ������ͨ�ı�
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
        /// ���ݷ�������
        /// </summary>
        [HideInInspector]
        public string typeName;
        /// <summary>
        /// ���ݼ���
        /// </summary>
        [HideInInspector]
        public List<string> contents;
    }
}
