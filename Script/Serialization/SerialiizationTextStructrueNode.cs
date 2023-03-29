using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextGraphicalManagementEditor
{
    [Serializable]
    public class SerialiizationTextStructrueNode : ISerializationCallbackReceiver
    {
        /// <summary>
        /// 用于外部调用的列表
        /// </summary>
        [SerializeField]
        public List<NodeData> list;

        /// <summary>
        /// 用于序列化的列表 在面板中隐藏
        /// </summary>
        [SerializeField, HideInInspector]
        private List<int> nodeIds;
        [SerializeField]
        private List<BranchNodeData> branchNodeDatas;
        [SerializeField, HideInInspector]
        private List<AutomaticBranchNodeData> automaticBranchNodeDatas;
        [SerializeField]
        private List<DialogueDate> dialogueNodeDatas;


        /// <summary>
        /// 初始化用于序列化的列表
        /// </summary>
        public void Initialize()
        {
            nodeIds = new List<int>();
            branchNodeDatas = new List<BranchNodeData>();
            automaticBranchNodeDatas = new List<AutomaticBranchNodeData>();
            dialogueNodeDatas = new List<DialogueDate>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dialogueActions"></param>
        public SerialiizationTextStructrueNode(List<NodeData> nodeData)
        {
            list = nodeData;
            Initialize();
        }

        public SerialiizationTextStructrueNode()
        {
            list = new List<NodeData>();
            Initialize();
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].NodeType)
                {
                    case "TextGraphicalManagementEditor.BranchNode":
                        list[i] = branchNodeDatas[nodeIds[i]]; break;
                    case "TextGraphicalManagementEditor.AutomaticBranchNode":
                        list[i] = automaticBranchNodeDatas[nodeIds[i]]; break;
                    case "TextGraphicalManagementEditor.DialogueTextNode":
                        list[i] = dialogueNodeDatas[nodeIds[i]]; break;
                    default:
                        break;
                }
            }
        }

        //此处可能存在问题 可在时间充裕时研究优化 此事件在运行时会重复调用
        public void OnBeforeSerialize()
        {
            Initialize();
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].NodeType)
                {
                    case "TextGraphicalManagementEditor.BranchNode":
                        branchNodeDatas.Add(list[i] as BranchNodeData);
                        nodeIds.Add(branchNodeDatas.Count - 1);
                        break;
                    case "TextGraphicalManagementEditor.AutomaticBranchNode":
                        automaticBranchNodeDatas.Add(list[i] as AutomaticBranchNodeData);
                        nodeIds.Add(automaticBranchNodeDatas.Count - 1);
                        break;
                    case "TextGraphicalManagementEditor.DialogueTextNode":
                        dialogueNodeDatas.Add(list[i] as DialogueDate);
                        nodeIds.Add(dialogueNodeDatas.Count - 1);
                        break;
                    default:
                        nodeIds.Add(i); 
                        break;
                }
            }
        }
    }
}