using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    [Serializable]
    public class BackgroundMusic : DialogueAction
    {
        public AudioClip backgroundMusic;

        public BackgroundMusic()
        {
            actionType = "BackgroundMusic";
        }

        public override DialogueActionInspctor SetInspctor()
        {
            BackgroundMusicInspector thisInspector = new BackgroundMusicInspector();
            thisInspector.objectField.RegisterValueChangedCallback(OnBackgroundMusicVeluaChanged);
            inspector = thisInspector;
            if (backgroundMusic != null)
            {
                thisInspector.objectField.value = backgroundMusic;
            }
            return inspector;
        }

        public void OnBackgroundMusicVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            backgroundMusic = changeEvent.newValue as AudioClip;
        }
    }

    public class BackgroundMusicInspector : DialogueActionInspctor
    {
        public ObjectField objectField;


        public BackgroundMusicInspector()
        {
            this.AddToClassList("DialogueAction");

            Label label = new Label("±≥æ∞“Ù¿÷£∫");
            objectField = new ObjectField();
            objectField.objectType = typeof(AudioClip);
            RemoveButton = new Button();
            RemoveButton.text = "…æ≥˝";
            this.Add(label);
            this.Add(objectField);
            this.Add(RemoveButton);
            AddToClassList("DialogueContainer");

        }
    }


}