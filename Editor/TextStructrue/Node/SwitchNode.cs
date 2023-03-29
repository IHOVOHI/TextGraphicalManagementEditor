using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    /// <summary>
    /// �л��ڵ� ���ڲ�ͬ�ı��ṹ�ļ�����ת
    /// </summary>
    public class SwitchNode : TextGraphicalManagementEditorNode
    {
        private TextStructrueGraphView parentVisualElement;

        //���õ� ����¼�
        //public Clickable clickEvent;
        //public float ClickTime;

        public SwitchNode()
        {
            title = "";

            var inputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(Edge));
            InputContainer.Add(inputPort);

            textObj = new ObjectField();
            textObj.objectType = typeof(TextStructrueFile);
            textObj.RegisterValueChangedCallback(OnTextObjVeluaChanged);
            VisualElement visualElement = new VisualElement();
            visualElement.contentContainer.Add(textObj);
            Button button = new Button(NodeClick);
            titleButtonContainer.Add(button);
            button.text = ManagerSettingWindow.LanguageObj.Open;
            mainContainer.Insert(1, visualElement);
            mainContainer.Insert(0, InputContainer);
            RefreshExpandedState();
            this.AddToClassList("SwitchNode");

            this.elementTypeColor = new Color(0, .44f, .3f, .8f);
            this.titleContainer.style.backgroundColor = new Color(0, .44f, .3f, .8f);

            //clickEvent = new Clickable(NodeClick);
            //clickEvent.target = (VisualElement)this;
            //ClickTime = System.DateTime.Now.Second;
        }

        public void OnTextObjVeluaChanged(ChangeEvent<Object> changeEvent)
        {
            title = textObj.value.name;
        }

        public void NodeClick()
        {
            if (this.textObj.value != null)
            {
                //TextStructrueWindow window = TextStructrueWindow.OpenTextManagerWindow(this.textObj.value.name);
                //window.fileObj = this.textObj.value as TextStructrueFile;
            }
        }

        /// <summary>
        /// �ڵ�ѡ��ʱ����
        /// </summary>
        public override void OnSelected()
        {
            if (parentVisualElement == null)
            {
                parentVisualElement = this.GetFirstOfType<TextStructrueGraphView>();
            }
            parentVisualElement.AddNowCheckedNode(this);
        }

        /// <summary>
        /// �ڵ�ȡ��ѡ��ʱ����
        /// </summary>
        public override void OnUnselected()
        {
            parentVisualElement.RemoveNowCheckedNode(this);
        }
    }
}
