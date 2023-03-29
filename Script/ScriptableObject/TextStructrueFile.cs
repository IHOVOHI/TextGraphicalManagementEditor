using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextGraphicalManagementEditor
{
    /// <summary>
    /// 文本结构文件 插件基础文件 用于保存文本文件之间的结构
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
        /// 节点对应ID 不固定 只作为生成时参考使用
        /// </summary>
        public int NodeId;
        /// <summary>
        /// 节点类型
        /// </summary>
        public string NodeType;
        /// <summary>
        /// 节点位置
        /// </summary>
        [HideInInspector]
        public Rect NodePosition;
        /// <summary>
        /// 后续节点信息
        /// </summary>
        [HideInInspector]
        public List<int> TrailingNodes = new List<int>();
        /// <summary>
        /// 本节点使用的文件
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
        /// 选项名称
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
        /// 选项名称
        /// </summary>
        public List<string> branchName;

        /// <summary>
        /// 选项名称
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

