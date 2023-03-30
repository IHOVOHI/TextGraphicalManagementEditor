using System;
using UnityEngine;

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
    }
}