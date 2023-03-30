using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor {

    public class BackgroundImageInspector : DialogueActionInspector
    {
        public BackgroundImage backgroundImage { get{ return dialogueAction as BackgroundImage; } }

        public ObjectField objectField;


        public BackgroundImageInspector()
        {
            dialogueAction = new BackgroundImage();
            SetVisualElement();
        }

        public BackgroundImageInspector(BackgroundImage backgroundImage)
        {
            SetVisualElement();
        }

        public void OnDialogueImageVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            backgroundImage.backgroundImage = changeEvent.newValue as Sprite;
        }

        public override void SetVisualElement()
        {
            this.AddToClassList("DialogueAction");
            Label label = new Label("±³¾°Í¼Æ¬£º");

            objectField = new ObjectField();
            objectField.objectType = typeof(Sprite);
            objectField.RegisterValueChangedCallback(OnDialogueImageVeluaChanged);
            RemoveButton = new Button();
            RemoveButton.text = "É¾³ý";
            this.Add(label);
            this.Add(objectField);
            this.Add(RemoveButton);

            AddToClassList("DialogueContainer");
        }

        public override void Init()
        {
            if (backgroundImage.backgroundImage != null)
            {
                objectField.value = backgroundImage.backgroundImage;
            }
        }
    }
}
