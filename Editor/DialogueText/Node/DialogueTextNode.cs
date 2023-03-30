using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor {

    public class DialogueTextNode : TextGraphicalManagementEditorNode
    {

        public TextField DialogueNameField;

        public TextField DialogueContentField;

        TextGraphicalManagementVisualElement parentVisualElement;

        /// <summary>
        /// 当前说话人名字
        /// </summary>
        public string dialogueName;
        /// <summary>
        /// 当前说话内容
        /// </summary>
        public string content;
        /// <summary>
        /// 当前说话时间
        /// </summary>
        public string time;
        /// <summary>
        /// 当前说话场景
        /// </summary>
        public string scene;

        public SerializationDialogueAction dialogueActions;

        public Port inputPort; 
        public Port outputPort;

        public DialogueTextNode()
        {
            inputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(Edge));
            //inputPort.RemoveAt(1);
            InputContainer.contentContainer.Add(inputPort);

            outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(Edge));
            //outputPort.RemoveAt(1);
            outputContainer.Add(outputPort);

            dialogueActions = new SerializationDialogueAction() ;
            //textObj = new ObjectField();

            //textObj.objectType = typeof(DialogueTextFile);
            //textObj.RegisterValueChangedCallback(OnTextObjVeluaChanged);

            VisualElement visualElement = new VisualElement();
            mainContainer.Insert(1, visualElement);
            mainContainer.Insert(0, InputContainer);
            visualElement.AddToClassList("Content");

            VisualElement dialogueNameVisualElement = new VisualElement();
            Label dialogueNameLabel = new Label(ManagerSettingWindow.LanguageObj.Name);
            DialogueNameField = new TextField();
            DialogueNameField.RegisterValueChangedCallback(OnNameTextVeluaChanged);
            dialogueNameVisualElement.Add(dialogueNameLabel);
            dialogueNameVisualElement.Add(DialogueNameField);
            dialogueNameVisualElement.AddToClassList("DialogueContainer");

            VisualElement dialogueContentVisualElement = new VisualElement();
            Label dialogueContentLabel = new Label(ManagerSettingWindow.LanguageObj.Content);
            DialogueContentField = new TextField();
            DialogueContentField.RegisterValueChangedCallback(OnContentTextVeluaChanged);
            dialogueContentVisualElement.Add(dialogueContentLabel);
            dialogueContentVisualElement.Add(DialogueContentField);
            dialogueContentVisualElement.AddToClassList("DialogueContainer");

            //titleContainer.Add(dialogueNameVisualElement);

            this.titleContainer.AddToClassList("NodeTitle");
            visualElement.contentContainer.AddToClassList("NodeContainer");

            this.titleContainer.Insert(0,dialogueNameVisualElement);
            visualElement.contentContainer.Add(dialogueContentVisualElement);

            RefreshExpandedState();
            this.AddToClassList("DialogueTextNode");

            styleSheets.Add(Resources.Load<StyleSheet>("Uss/DialogueVisualElement"));

        }

        private void OnNameTextVeluaChanged(ChangeEvent<string> evt)
        {
            dialogueName = DialogueNameField.value;
        }

        private void OnContentTextVeluaChanged(ChangeEvent<string> evt)
        {
            content = DialogueContentField.value;
        }

        /// <summary>
        /// 节点选中时调用
        /// </summary>
        public override void OnSelected()
        {
            if (parentVisualElement == null)
            {
                parentVisualElement = this.GetFirstOfType<TextGraphicalManagementVisualElement>();
            }
            parentVisualElement.AddNowCheckedNode(this);
        }

        /// <summary>
        /// 节点取消选中时调用
        /// </summary>
        public override void OnUnselected()
        {
            parentVisualElement.RemoveNowCheckedNode(this);
            DialogueNameField.value = dialogueName;
            DialogueContentField.value = content;
            //if(dialogueName != null) SetTitleColor(parentVisualElement.GetNameColor(dialogueName));
        }

        public void SetTitleColor(Color color) {

            this.titleContainer.style.backgroundColor = color;
            this.elementTypeColor = color;
        }

    }
}
