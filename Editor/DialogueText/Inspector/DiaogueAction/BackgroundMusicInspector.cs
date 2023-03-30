using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    
    public class BackgroundMusicInspector : DialogueActionInspector
    {
        private BackgroundMusic backgroundMusic { get { return dialogueAction as BackgroundMusic; } }

        public ObjectField objectField;

        public BackgroundMusicInspector()
        {
            dialogueAction = new BackgroundMusic();
            SetVisualElement();
        }

        public override void SetVisualElement()
        {
            this.AddToClassList("DialogueAction");

            Label label = new Label("±≥æ∞“Ù¿÷£∫");

            objectField = new ObjectField();
            objectField.objectType = typeof(AudioClip);
            objectField.RegisterValueChangedCallback(OnBackgroundMusicVeluaChanged);

            RemoveButton = new Button();
            RemoveButton.text = "…æ≥˝";
            this.Add(label);
            this.Add(objectField);
            this.Add(RemoveButton);
            AddToClassList("DialogueContainer");
        }

        public void OnBackgroundMusicVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            backgroundMusic.backgroundMusic = changeEvent.newValue as AudioClip;
        }

        public override void Init()
        {
            if (backgroundMusic.backgroundMusic != null)
            {
                objectField.value = backgroundMusic.backgroundMusic;
            }
        }
    }


}