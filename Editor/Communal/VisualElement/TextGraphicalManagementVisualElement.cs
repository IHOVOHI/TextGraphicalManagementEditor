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
        /// ��ǰ�Ĵ���
        /// </summary>
        public TextGraphicalManagementEditorWindow window;

        public List<TextGraphicalManagementEditorNode> nowCheckedNode;
        /// <summary>
        /// �ڵ㱻ѡ�к����
        /// </summary>
        /// <param name="node"></param>
        public void AddNowCheckedNode(TextGraphicalManagementEditorNode node)
        {
            window.SetHasUnsavedChanges(true);
            nowCheckedNode.Add(node);
            SetNodeInspector();
        }

        /// <summary>
        /// �ڵ㱻ȡ��ѡ��ʱ����
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNowCheckedNode(TextGraphicalManagementEditorNode node)
        {
            nowCheckedNode.Remove(node);
        }

        /// <summary>
        /// д��ڵ��������
        /// </summary>
        public virtual void SetNodeInspector()
        {
        }
    }
}
