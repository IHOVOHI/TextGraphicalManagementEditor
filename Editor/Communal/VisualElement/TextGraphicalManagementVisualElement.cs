using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{

    public class TextGraphicalManagementVisualElement : GraphView
    {

        /// <summary>
        /// 当前的窗口
        /// </summary>
        public TextGraphicalManagementEditorWindow window;

        public List<TextGraphicalManagementEditorNode> nowCheckedNode;
        /// <summary>
        /// 节点被选中后调用
        /// </summary>
        /// <param name="node"></param>
        public void AddNowCheckedNode(TextGraphicalManagementEditorNode node)
        {
            window.SetHasUnsavedChanges(true);
            nowCheckedNode.Add(node);
            SetNodeInspector();
        }

        /// <summary>
        /// 节点被取消选中时调用
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNowCheckedNode(TextGraphicalManagementEditorNode node)
        {
            nowCheckedNode.Remove(node);
        }

        /// <summary>
        /// 写入节点属性面板
        /// </summary>
        public virtual void SetNodeInspector()
        {
        }
    }
}
