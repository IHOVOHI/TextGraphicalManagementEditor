using System;
using UnityEngine;

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
    }
}