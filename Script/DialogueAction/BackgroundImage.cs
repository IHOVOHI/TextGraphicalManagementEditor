using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace TextGraphicalManagementEditor {
    [Serializable]
    public class BackgroundImage : DialogueAction
    {
        public Sprite backgroundImage;

        public BackgroundImage()
        {
            actionType = "BackgroundImage";
        }

        public override DialogueActionInspctor SetInspctor()
        {
            BackgroundImageInspector thisInspector = new BackgroundImageInspector();
            thisInspector.objectField.RegisterValueChangedCallback(OnDialogueImageVeluaChanged);
            inspector = thisInspector;
            if (backgroundImage != null)
            {
                thisInspector.objectField.value = backgroundImage;
            }
            return inspector;
        }

        public void OnDialogueImageVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            backgroundImage = changeEvent.newValue as Sprite;
        }

    }

    public class BackgroundImageInspector : DialogueActionInspctor
    {
        public ObjectField objectField;

        public BackgroundImageInspector()
        {
            this.AddToClassList("DialogueAction");
            Label label = new Label("±³¾°Í¼Æ¬£º");
            objectField = new ObjectField();
            objectField.objectType = typeof(Sprite);
            RemoveButton = new Button();
            RemoveButton.text = "É¾³ý";
            this.Add(label);
            this.Add(objectField);
            this.Add(RemoveButton);

            AddToClassList("DialogueContainer");

        }
    }
}