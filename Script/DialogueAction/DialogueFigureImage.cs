using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    [Serializable]
    public class DialogueFigureImage : DialogueAction
    {
        public Sprite dialogueFigureImage;
        public int location = 0;

        public DialogueFigureImage()
        {
            actionType = "DialogueFigureImage";
        }

        public override DialogueActionInspctor SetInspctor()
        {
            DialogueFigureImageInspector thisInspector = new DialogueFigureImageInspector();
            thisInspector.objectField.RegisterValueChangedCallback(OnDialogueFigureImageVeluaChanged);
            thisInspector.locationField.RegisterValueChangedCallback(OnDialogueVeluaChanged);
            inspector = thisInspector;
            if (dialogueFigureImage != null)
            {
                thisInspector.objectField.value = dialogueFigureImage;
                thisInspector.locationField.value = location.ToString();
            }
            return inspector;
        }

        public void OnDialogueFigureImageVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            dialogueFigureImage = changeEvent.newValue as Sprite;
        }
        public void OnDialogueVeluaChanged(ChangeEvent<string> changeEvent)
        {
            location = int.Parse(changeEvent.newValue);
        }
    }

    public class DialogueFigureImageInspector : DialogueActionInspctor
    {
        public ObjectField objectField;
        public TextField locationField;

        public DialogueFigureImageInspector()
        {
            this.AddToClassList("DialogueAction");

            VisualElement objectVisualElement = new VisualElement();
            Label label = new Label("对话人物图片：");
            objectField = new ObjectField();
            objectField.objectType = typeof(Sprite);

            VisualElement locationVisualElement = new VisualElement();
            Label locationLabel = new Label("对话人物位置：");
            locationField = new TextField();
            RemoveButton = new Button();
            RemoveButton.text = "删除";

            objectVisualElement.Add(label);
            objectVisualElement.Add(objectField);
            objectVisualElement.AddToClassList("DialogueContainer");

            locationVisualElement.Add(locationLabel);
            locationVisualElement.Add(locationField);
            locationVisualElement.Add(RemoveButton);
            locationVisualElement.AddToClassList("DialogueContainer");

            this.Add(objectVisualElement);
            this.Add(locationVisualElement);

        }
    }

}