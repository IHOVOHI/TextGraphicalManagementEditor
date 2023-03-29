
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TextGraphicalManagementEditor {
    /// <summary>
    /// �Ի��ļ� �洢�Ի�
    /// </summary>
    [CreateAssetMenu(menuName = "Text Graphical Marager/Dialogue Text")]
    public class DialogueTextFile : TextFile
    {
        [HideInInspector]
        public Vector3 Position;

        /// <summary>
        /// ���ֶ�Ӧ��ɫ�б�
        /// </summary>
        [HideInInspector]
        public SerializationDictionary<string, Color> nameColors;

        /// <summary>
        /// �Ի��ļ�·��
        /// </summary>
        public string dialogueTextFileUrl;
        /// <summary>
        /// �Ի��ļ�·��
        /// </summary>
        public TextAsset TextFile;
        /// <summary>
        /// �Ի�����
        /// </summary>
        public string dialogueTitle;
        /// <summary>
        /// �Ի�ʱ��
        /// </summary>
        public string dialogueTime;
        /// <summary>
        /// �Ի�����
        /// </summary>
        public string dialogueScene;
        /// <summary>
        /// �Ի�����
        /// </summary>
        [HideInInspector]
        public List<DialogueDate> dialogueDates;
         
        
    }

    /// <summary>
    /// �Ի�����
    /// </summary>
    [Serializable]
    public class DialogueDate : NodeData {

        public DialogueDate(NodeData nodeData) : base(nodeData)
        {
            
        }

        public DialogueDate()
        {
        }

        /// <summary>
        /// �Ի�ʱ��
        /// </summary>
        public string time;
        /// <summary>
        /// �Ի�����
        /// </summary>
        public string scene;
        /// <summary>
        /// ˵������
        /// </summary>
        public string name;
        /// <summary>
        /// ˵������
        /// </summary>
        public string content;
        /// <summary>
        /// �Ի�����
        /// </summary>
        public SerializationDialogueAction actions;
        /// <summary>
        /// ����ǰ�ڵ�����
        /// </summary>
        [HideInInspector]
        public Rect savePosition;
    }
}