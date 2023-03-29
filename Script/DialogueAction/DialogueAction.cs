using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    [Serializable]
    public class DialogueAction
    {
        public string actionType;
        public DialogueActionInspctor inspector;

        public virtual DialogueActionInspctor SetInspctor() {
            return null;
        }
    }

    public class DialogueActionInspctor : VisualElement
    {

        public Button RemoveButton;

    }

}