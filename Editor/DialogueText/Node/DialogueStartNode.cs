using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor {

    public class DialogueStartNode : TextGraphicalManagementEditorNode
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public DialogueStartNode()
        {

            //�ڵ�����
            title = ManagerSettingWindow.LanguageObj.RootNode;

            //�������˿�
            outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(Edge));
            outputContainer.Add(outputPort);

            //��ǰʹ�õ��ļ�
            textObj = new ObjectField();
            textObj.value = null;
            this.capabilities -= Capabilities.Deletable;
            RefreshExpandedState();
            this.AddToClassList("RootNode");
        }

        public Port outputPort;


        /// <summary>
        /// �ڵ�ѡ��ʱ����
        /// </summary>
        public override void OnSelected()
        {
            this.GetFirstOfType<DialogueVisualElement>().AddNowCheckedNode(this);
        }

        /// <summary>
        /// �ڵ�ȡ��ѡ��ʱ����
        /// </summary>
        public override void OnUnselected()
        {
            this.GetFirstOfType<DialogueVisualElement>().RemoveNowCheckedNode(this);
        }
    }

}