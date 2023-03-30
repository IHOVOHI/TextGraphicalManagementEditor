using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace TextGraphicalManagementEditor {
    [Serializable]
    public class BackgroundImage : DialogueAction
    {
        public Sprite backgroundImage;

        public BackgroundImage()
        {
            actionType = "BackgroundImage";
        }
    }
}