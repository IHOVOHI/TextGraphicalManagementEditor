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
        /// �����ⲿ���õ��б�
        /// </summary>
        [SerializeField]
        public List<NodeData> list;

        /// <summary>
        /// �������л����б� �����������
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
        /// ��ʼ���������л����б�
        /// </summary>
        public void Initialize()
        {
            nodeIds = new List<int>();
            branchNodeDatas = new List<BranchNodeData>();
            automaticBranchNodeDatas = new List<AutomaticBranchNodeData>();
            dialogueNodeDatas = new List<DialogueDate>();
        }

        /// <summary>
        /// ���캯��
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

        //�˴����ܴ������� ����ʱ���ԣʱ�о��Ż� ���¼�������ʱ���ظ�����
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