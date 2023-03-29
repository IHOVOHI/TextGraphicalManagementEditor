using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextGraphicalManagementEditor
{
    /// <summary>
    /// �ı��ṹ�ļ� ��������ļ� ���ڱ����ı��ļ�֮��Ľṹ
    /// </summary>
    [CreateAssetMenu(menuName = "Text Graphical Marager/Text Structrue")]
    public class TextStructrueFile : TextFile
    {
        [HideInInspector]
        public Vector3 Position;
        [HideInInspector]
        public int roodId;

        [HideInInspector]
        public SerialiizationTextStructrueNode nodeDatas = new SerialiizationTextStructrueNode();

    }

    [Serializable]
    public class NodeData
    {
        /// <summary>
        /// �ڵ��ӦID ���̶� ֻ��Ϊ����ʱ�ο�ʹ��
        /// </summary>
        public int NodeId;
        /// <summary>
        /// �ڵ�����
        /// </summary>
        public string NodeType;
        /// <summary>
        /// �ڵ�λ��
        /// </summary>
        [HideInInspector]
        public Rect NodePosition;
        /// <summary>
        /// �����ڵ���Ϣ
        /// </summary>
        [HideInInspector]
        public List<int> TrailingNodes = new List<int>();
        /// <summary>
        /// ���ڵ�ʹ�õ��ļ�
        /// </summary>
        [HideInInspector]
        public TextFile NodeFile;

        public NodeData()
        {
        }

        public NodeData(NodeData nodeData)
        {
            NodeId = nodeData.NodeId;
            NodeType = nodeData.NodeType;
            NodePosition = nodeData.NodePosition;
            TrailingNodes = nodeData.TrailingNodes;
            NodeFile = nodeData.NodeFile;
        }

        public NodeData(TextFile nodeFile)
        {
            NodeFile = nodeFile;
        }

        public NodeData(int nodeId, string nodeType, Rect nodePosition, List<int> trailingNodes, TextFile nodeFile)
        {
            NodeId = nodeId;
            NodeType = nodeType;
            NodePosition = nodePosition;
            TrailingNodes = trailingNodes;
            NodeFile = nodeFile;
        }
    }

    [Serializable]
    public class BranchNodeData: NodeData
    {

        /// <summary>
        /// ѡ������
        /// </summary>
        public List<string> branchName;
        [SerializeField]
        public List<OptionDataValue> optionDatas;

        public BranchNodeData(NodeData nodeData) : base(nodeData)
        {
            Initialize();
        }

        public void Initialize()
        {
            branchName = new List<string>();
            optionDatas = new List<OptionDataValue>();

        }
    }

    [Serializable]
    public class OptionDataValue
    {
        public List<string> DataName;
        public List<string> DataValue;

        public OptionDataValue()
        {
            DataName = new List<string>();
            DataValue = new List<string>();
        }
    }

    [Serializable]
    public class  AutomaticBranchNodeData : NodeData
    {

        public int compare;
        /// <summary>
        /// ѡ������
        /// </summary>
        public List<string> branchName;

        /// <summary>
        /// ѡ������
        /// </summary>
        public List<float> branchValue;

        public AutomaticBranchNodeData(NodeData nodeData) : base(nodeData)
        {
            Initialize();
        }

        public void Initialize()
        {
            branchName = new List<string>();
            branchValue = new List<float>();
        }
    }
}

