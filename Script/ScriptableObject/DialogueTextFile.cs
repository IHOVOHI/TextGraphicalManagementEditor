
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TextGraphicalManagementEditor {
    /// <summary>
    /// 对话文件 存储对话
    /// </summary>
    [CreateAssetMenu(menuName = "Text Graphical Marager/Dialogue Text")]
    public class DialogueTextFile : TextFile
    {
        [HideInInspector]
        public Vector3 Position;

        /// <summary>
        /// 名字对应颜色列表
        /// </summary>
        [HideInInspector]
        public SerializationDictionary<string, Color> nameColors;

        /// <summary>
        /// 对话文件路径
        /// </summary>
        public string dialogueTextFileUrl;
        /// <summary>
        /// 对话文件路径
        /// </summary>
        public TextAsset TextFile;
        /// <summary>
        /// 对话标题
        /// </summary>
        public string dialogueTitle;
        /// <summary>
        /// 对话时间
        /// </summary>
        public string dialogueTime;
        /// <summary>
        /// 对话场景
        /// </summary>
        public string dialogueScene;
        /// <summary>
        /// 对话数据
        /// </summary>
        [HideInInspector]
        public List<DialogueDate> dialogueDates;
         
        
    }

    /// <summary>
    /// 对话数据
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
        /// 对话时间
        /// </summary>
        public string time;
        /// <summary>
        /// 对话场景
        /// </summary>
        public string scene;
        /// <summary>
        /// 说话人物
        /// </summary>
        public string name;
        /// <summary>
        /// 说话内容
        /// </summary>
        public string content;
        /// <summary>
        /// 对话动作
        /// </summary>
        public SerializationDialogueAction actions;
        /// <summary>
        /// 保存前节点坐标
        /// </summary>
        [HideInInspector]
        public Rect savePosition;
    }
}