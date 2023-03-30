using System;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    public class DialogueActionInspector : VisualElement
    {
        public DialogueAction dialogueAction;

        public Button RemoveButton;

        public virtual void SetVisualElement()
        {

        }

        public virtual void Init() {

        }

        public virtual void SetDialogueAction(DialogueAction dialogueAction)
        {
            this.dialogueAction = dialogueAction;
        }
    }
}