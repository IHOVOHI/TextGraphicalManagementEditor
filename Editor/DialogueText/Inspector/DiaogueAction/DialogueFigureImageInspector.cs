using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    public class DialogueFigureImageInspector : DialogueActionInspector
    {
        private DialogueFigureImage figureImage { get { return dialogueAction as DialogueFigureImage; } }

        public ObjectField objectField;
        public TextField locationField;


        public DialogueFigureImageInspector()
        {
            dialogueAction = new DialogueFigureImage();
            SetVisualElement();
        }

        public void OnDialogueFigureImageVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            figureImage.dialogueFigureImage = changeEvent.newValue as Sprite;
        }

        public void OnDialogueVeluaChanged(ChangeEvent<string> changeEvent)
        {
            figureImage.location = int.Parse(changeEvent.newValue);
        }

        public override void SetVisualElement()
        {
            this.AddToClassList("DialogueAction");

            VisualElement objectVisualElement = new VisualElement();
            Label label = new Label("对话人物图片：");
            objectField = new ObjectField();
            objectField.objectType = typeof(Sprite);
            objectField.RegisterValueChangedCallback(OnDialogueFigureImageVeluaChanged);

            VisualElement locationVisualElement = new VisualElement();
            Label locationLabel = new Label("对话人物位置：");
            locationField = new TextField();
            locationField.RegisterValueChangedCallback(OnDialogueVeluaChanged);

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

        public override void Init()
        {
            if (figureImage.dialogueFigureImage != null)
            {
                objectField.value = figureImage.dialogueFigureImage;
                locationField.value = figureImage.location.ToString();
            }
        }
    }

}