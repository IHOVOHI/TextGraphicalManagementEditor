using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    /// <summary>
    /// 对话节点 TextStructrue图形窗口中使用
    /// </summary>
    public class DialogueNode : TextGraphicalManagementEditorNode
    {
        private TextStructrueGraphView parentVisualElement;

        public DialogueNode()
        {
            var inputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(Edge));
            //inputPort.RemoveAt(1);
            InputContainer.contentContainer.Add(inputPort);

            var outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(Edge));
            //outputPort.RemoveAt(1);
            outputContainer.Add(outputPort);

            textObj = new ObjectField();

            textObj.objectType = typeof(DialogueTextFile);
            textObj.RegisterValueChangedCallback(OnTextObjVeluaChanged);
            VisualElement visualElement = new VisualElement();
            visualElement.contentContainer.Add(textObj);
            mainContainer.Insert(1, visualElement);
            mainContainer.Insert(0, InputContainer);
            visualElement.AddToClassList("Content");
            RefreshExpandedState();

            this.elementTypeColor = new Color(0, .25f, .36f, .8f);
            this.titleContainer.style.backgroundColor = new Color(0, .25f, .36f, .8f);

            this.AddToClassList("DialogueNode");
        }
        public void SetTitle() {
            DialogueTextFile file = textObj.value as DialogueTextFile;
            title = file.dialogueTitle;
        }


        public void OnTextObjVeluaChanged(ChangeEvent<Object> changeEvent)
        {
            if (textObj.value == null) return;
            SetTitle();
        }

        /// <summary>
        /// 节点选中时调用
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
        /// 节点取消选中时调用
        /// </summary>
        public override void OnUnselected()
        {
            parentVisualElement.RemoveNowCheckedNode(this);
        }

    }
}
