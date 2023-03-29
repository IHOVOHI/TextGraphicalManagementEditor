using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{

    public class DialogueInspector : VisualElement
    {

        private DialogueNode node;
        private ObjectField objectField;
        private TextField dialogueTitleField;
        private TextField dialogueTimeField;
        private TextField dialogueSceneField;

        public DialogueInspector(DialogueNode node)
        {
            this.AddToClassList("DialogueTextInspector");

            this.node = node;
            this.AddToClassList("DialogueInspector");

            ///新建并添加当前对话文件的输入框组
            VisualElement objectFieldVisualElement = new VisualElement();
            Label objectLabel = new Label(ManagerSettingWindow.LanguageObj.CurrentDialogueFile);
            objectField = new ObjectField();
            objectField.objectType = typeof(DialogueTextFile);
            objectField.RegisterValueChangedCallback(OnTextObjVeluaChanged);
            objectFieldVisualElement.Add(objectLabel);
            objectFieldVisualElement.Add(objectField);
            objectFieldVisualElement.AddToClassList("row");

            //新建并添加当前对话标题、时间、场景的输入框组
            VisualElement dialogueTitleVisualElement = new VisualElement();
            Label dialogueTitleLabel = new Label(ManagerSettingWindow.LanguageObj.CurrentDialogueTitle);
            dialogueTitleField = new TextField();
            dialogueTitleField.RegisterValueChangedCallback(OnDialogueTitleVeluaChanged);
            dialogueTitleVisualElement.Add(dialogueTitleLabel);
            dialogueTitleVisualElement.Add(dialogueTitleField);
            dialogueTitleVisualElement.AddToClassList("row");

            VisualElement dialogueTimeVisualElement = new VisualElement();
            Label dialogueTimeLabel = new Label(ManagerSettingWindow.LanguageObj.CurrentDialogueTime);
            dialogueTimeField = new TextField();
            dialogueTimeField.RegisterValueChangedCallback(OnDialogueTimeVeluaChanged);
            dialogueTimeVisualElement.Add(dialogueTimeLabel);
            dialogueTimeVisualElement.Add(dialogueTimeField);
            dialogueTimeVisualElement.AddToClassList("row");

            VisualElement dialogueSceneVisualElement = new VisualElement();
            Label dialogueSceneLabel = new Label(ManagerSettingWindow.LanguageObj.CurrentDialogueScenes);
            dialogueSceneField = new TextField();
            dialogueSceneField.RegisterValueChangedCallback(OnDialogueSceneVeluaChanged);
            dialogueSceneVisualElement.Add(dialogueSceneLabel);
            dialogueSceneVisualElement.Add(dialogueSceneField);
            dialogueSceneVisualElement.AddToClassList("row");

            Add(objectFieldVisualElement);
            Add(dialogueTitleVisualElement);
            Add(dialogueTimeVisualElement);
            Add(dialogueSceneVisualElement);

            SetVelua();
        }

        private void OnTextObjVeluaChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            node.textObj.value = objectField.value;
            SetVelua();
        }

        public void SetVelua() {
            objectField.value = node.textObj.value;
            if (objectField.value != null)
            {

                DialogueTextFile file = (DialogueTextFile)objectField.value;
                dialogueTitleField.value = file.dialogueTitle;
                dialogueTimeField.value = file.dialogueTime;
                dialogueSceneField.value = file.dialogueScene;
            }
        }
        void OnDialogueTitleVeluaChanged(ChangeEvent<string> changeEvent)
        {
            DialogueTextFile file = (DialogueTextFile)objectField.value;
            file.dialogueTitle = dialogueTitleField.value;
            node.SetTitle(); 
            EditorUtility.SetDirty(file);
        }

        void OnDialogueTimeVeluaChanged(ChangeEvent<string> changeEvent)
        {
            DialogueTextFile file = (DialogueTextFile)objectField.value;
            file.dialogueTime = dialogueTimeField.value;
            EditorUtility.SetDirty(file);

        }

        void OnDialogueSceneVeluaChanged(ChangeEvent<string> changeEvent)
        {
            DialogueTextFile file = (DialogueTextFile)objectField.value;
            file.dialogueScene = dialogueSceneField.value;
            EditorUtility.SetDirty(file);
        }
    }
}