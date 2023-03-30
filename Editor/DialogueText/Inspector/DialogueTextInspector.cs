using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor {

    public class DialogueTextInspector : VisualElement
    {
        public DialogueTextInspector(DialogueTextNode node)
        {
            this.AddToClassList("DialogueTextInspector");
            styleSheets.Add(Resources.Load<StyleSheet>("Uss/DialogueVisualElement"));

            this.node = node;

            //����������
            VisualElement dialogueNameVisualElement = new VisualElement();
            Label dialogueNameLabel = new Label(ManagerSettingWindow.LanguageObj.Name);
            DialogueNameField = new TextField();
            dialogueNameVisualElement.Add(dialogueNameLabel);
            dialogueNameVisualElement.Add(DialogueNameField);
            dialogueNameVisualElement.AddToClassList("DialogueContainer");

            VisualElement dialogueTimeVisualElement = new VisualElement();
            Label dialogueTimeLabel = new Label(ManagerSettingWindow.LanguageObj.Time);
            DialogueTimeField = new TextField();
            dialogueTimeVisualElement.Add(dialogueTimeLabel);
            dialogueTimeVisualElement.Add(DialogueTimeField);
            dialogueTimeVisualElement.AddToClassList("DialogueContainer");

            VisualElement dialogueSceneVisualElement = new VisualElement();
            Label dialogueSceneLabel = new Label(ManagerSettingWindow.LanguageObj.Scene);
            DialogueSceneField = new TextField();
            dialogueSceneVisualElement.Add(dialogueSceneLabel);
            dialogueSceneVisualElement.Add(DialogueSceneField);
            dialogueSceneVisualElement.AddToClassList("DialogueContainer");

            VisualElement dialogueContentVisualElement = new VisualElement();
            Label dialogueContentLabel = new Label(ManagerSettingWindow.LanguageObj.Content);
            DialogueContentField = new TextField();
            DialogueContentField.AddToClassList("DialogueMultilineTextField");
            DialogueContentField.multiline = true;
            dialogueContentVisualElement.Add(dialogueContentLabel);
            dialogueContentVisualElement.Add(DialogueContentField);
            dialogueContentVisualElement.AddToClassList("DialogueMultilineTextField");
            dialogueContentVisualElement.AddToClassList("DialogueContainer");

            VisualElement dialogueAction = new VisualElement();

            Label DiaogueActionTitle = new Label(ManagerSettingWindow.LanguageObj.DialogueAction);

            VisualElement dialogueActionButtonContainer = new VisualElement();
            dialogueActionButtonContainer.AddToClassList("DialogueContainer");
            Button AddAction = new Button();
            AddAction.text = "��Ӷ���";
            AddAction.clicked += OnDialogueActionClick;
            

            dialogueActionButtonContainer.Add(DiaogueActionTitle);
            dialogueActionButtonContainer.Add(AddAction);

            dialogueActionContainer = new VisualElement();
            dialogueActionContainer.AddToClassList("DialogueActionContainer");
            for (int i = 0; i < this.node.dialogueActions.list.Count; i++)
            {
                Type type = GetDialogueActionType(this.node.dialogueActions.list[i].actionType);
                DialogueActionInspector action = Activator.CreateInstance(type) as DialogueActionInspector;
                action.SetDialogueAction(this.node.dialogueActions.list[i]);
                dialogueActionContainer.Add(action);
                action.Init();
                action.RemoveButton.clicked += () => {
                    this.node.dialogueActions.list.Remove(action.dialogueAction);
                    dialogueActionContainer.Remove(action);
                };
            }      

            dialogueAction.Add(dialogueActionButtonContainer);
            dialogueAction.Add(dialogueActionContainer);

            Add(dialogueNameVisualElement);
            Add(dialogueTimeVisualElement);
            Add(dialogueSceneVisualElement);
            Add(dialogueContentVisualElement);
            Add(dialogueAction);

            //��Ӱ�ť�¼�
            DialogueNameField.RegisterValueChangedCallback(OnDialogueNameVeluaChanged);
            DialogueTimeField.RegisterValueChangedCallback(OnDialogueTimeVeluaChanged);
            DialogueSceneField.RegisterValueChangedCallback(OnDialogueSceneVeluaChanged);
            DialogueContentField.RegisterValueChangedCallback(OnDialogueContentVeluaChanged);

            DialogueNameField.value = this.node.dialogueName;
            DialogueTimeField.value = this.node.time;
            DialogueSceneField.value = this.node.scene;
            DialogueContentField.value = this.node.content;

            AddToClassList("DialogueTextInspector");

            menuWindow = ScriptableObject.CreateInstance<DialogueActionSearchWiodowProvider>();
            menuWindow.OnDialogueMenuWindowProviderDelegate = OnSearchTreeEntry;

        }

        DialogueActionSearchWiodowProvider menuWindow;
        TextField DialogueNameField;
        TextField DialogueTimeField;
        TextField DialogueSceneField;
        TextField DialogueContentField;
        VisualElement dialogueActionContainer;

        /// <summary>
        /// ��ǰ�������Ľڵ�
        /// </summary>
        public DialogueTextNode node;

        /// <summary>
        /// �Ի������˵����ڲ˵����������
        /// </summary>
        /// <param name="SearchTreeEntry"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool OnSearchTreeEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var type = SearchTreeEntry.userData as Type;
            DialogueActionInspector action = Activator.CreateInstance(type) as DialogueActionInspector;
            dialogueActionContainer.Add(action);
            this.node.dialogueActions.list.Add(action.dialogueAction);
            action.RemoveButton.clicked += () => {
                this.node.dialogueActions.list.Remove(action.dialogueAction);
                dialogueActionContainer.Remove(action);
            };
            return true;
        }


        //���ݸ����¼�
        void OnDialogueNameVeluaChanged(ChangeEvent<string> changeEvent)
        {
            node.dialogueName = DialogueNameField.value;
        }

        void OnDialogueTimeVeluaChanged(ChangeEvent<string> changeEvent)
        {
            node.time = DialogueTimeField.value;
        }

        void OnDialogueSceneVeluaChanged(ChangeEvent<string> changeEvent)
        {
            node.scene = DialogueNameField.value;
        }

        void OnDialogueContentVeluaChanged(ChangeEvent<string> changeEvent)
        {
            node.content = DialogueContentField.value;
        }
        
        /// <summary>
        /// �򿪶Ի������˵���ť
        /// </summary>
        void OnDialogueActionClick()
        {
            SearchWindow.Open(new SearchWindowContext(WindowManager.NowWindow.position.position+Event.current.mousePosition), menuWindow);
        }

        public Type GetDialogueActionType(string typeName) {
            switch (typeName)
            {
                case "BackgroundImage": return typeof (BackgroundImageInspector) ;
                case "BackgroundMusic": return typeof(BackgroundMusicInspector);
                case "DialogueFigureImage": return typeof(DialogueFigureImageInspector);
                case "DialogueVoice": return typeof(DialogueVoiceInspector);
            }
            return null;
        }
    }
}
