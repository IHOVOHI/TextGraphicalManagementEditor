using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    public class DialogueVoiceInspector : DialogueActionInspector
    {

        private DialogueVoice dialogueVoice { get { return dialogueAction as DialogueVoice; } }

        public ObjectField objectField;


        public DialogueVoiceInspector()
        {
            dialogueAction = new DialogueVoice();
            SetVisualElement();
        }

        public void OnBackgroundMusicVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            dialogueVoice.dialogueVoice = changeEvent.newValue as AudioClip;
        }

        public override void SetVisualElement()
        {
            this.AddToClassList("DialogueAction");
            AddToClassList("DialogueContainer");


            Label label = new Label("∂‘ª∞”Ô“Ù£∫");
            objectField = new ObjectField();
            objectField.objectType = typeof(AudioClip);
            objectField.RegisterValueChangedCallback(OnBackgroundMusicVeluaChanged);

            RemoveButton = new Button();
            RemoveButton.text = "…æ≥˝";
            this.Add(label);
            this.Add(objectField);
            this.Add(RemoveButton);
           
        }

        public override void Init()
        {
            if (dialogueVoice.dialogueVoice != null)
            {
                objectField.value = dialogueVoice.dialogueVoice;
            }
        }
    }


}