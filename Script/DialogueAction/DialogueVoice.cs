using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    [Serializable]
    public class DialogueVoice : DialogueAction
    {
        public AudioClip backgroundVoice;

        public DialogueVoice()
        {
            actionType = "DialogueVoice";
        }
        public override DialogueActionInspctor SetInspctor()
        {
            DialogueVoiceInspector thisInspector = new DialogueVoiceInspector();
            thisInspector.objectField.RegisterValueChangedCallback(OnBackgroundMusicVeluaChanged);
            inspector = thisInspector;
            if (backgroundVoice != null)
            {
                thisInspector.objectField.value = backgroundVoice;
            }
            return inspector;
        }

        public void OnBackgroundMusicVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            backgroundVoice = changeEvent.newValue as AudioClip;
        }
    }

    public class DialogueVoiceInspector : DialogueActionInspctor
    {
        public ObjectField objectField;

        public DialogueVoiceInspector()
        {
            this.AddToClassList("DialogueAction");
            AddToClassList("DialogueContainer");


            Label label = new Label("∂‘ª∞”Ô“Ù£∫");
            objectField = new ObjectField();
            objectField.objectType = typeof(AudioClip);
            RemoveButton = new Button();
            RemoveButton.text = "…æ≥˝";
            this.Add(label);
            this.Add(objectField);
            this.Add(RemoveButton);
        }
    }


}