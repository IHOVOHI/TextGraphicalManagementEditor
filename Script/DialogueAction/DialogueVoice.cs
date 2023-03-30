using System;
using UnityEngine;

namespace TextGraphicalManagementEditor
{
    [Serializable]
    public class DialogueVoice : DialogueAction
    {
        public AudioClip dialogueVoice;

        public DialogueVoice()
        {
            actionType = "DialogueVoice";
        }
    }
}