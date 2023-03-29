using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextGraphicalManagementEditor { 
    /// <summary>
    /// 编辑器语言文件 用于提供插件使用的String数据
    /// </summary>
    [CreateAssetMenu(menuName = "Text Graphical Marager/Manager Language File")]
    public class ManagerLanguageFile : ScriptableObject
    {
        //编辑器设置中的字符串
        public string ManagerSetting = "Manager Setting";
        public string ManagerLanguage = "Manager Language";
        public string FileUrl = "File Url";

        //在图形窗口中显示的节点名称
        public string TextStructrueNode = "TextStructrueNode";
        public string RootNode = "RootNode";
        public string BranchNode = "BranchNode";
        public string AutomaticBranchNode = "AutomaticBranchNode";
        public string SwitchNode = "SwitchNode";
        public string DialogueNode = "DialogueNode";


        //在图形窗口中节点使用的文字
        public string Open = "Open";
        public string Option = "Option:";

        //在图形窗口中使用的文字
        public string CurrentDialogueFile = "Current Dialogue File:";
        public string CurrentDialogueTime = "Current Dialogue Time:";
        public string CurrentDialogueTitle = "Current Dialogue Title:";
        public string CurrentDialogueScenes = "Current Dialogue Scenes:";
        public string AddDialogueDate = "Add Dialogue Date";
        public string RemoveDialogueDate = "Remove Dialogue Date";
        public string Name = "Name:";
        public string Time = "Time:";
        public string Scene = "Scenes:";
        public string Content = "Content:";

        public string CurrentPlainTextFile = "Current Plain Text File:";
        public string AddPlainTextDate = "Add Plain Text Date";
        public string RemovePlainTextDate = "Remove Plain Text Date";
        public string AddContent = "Add Content";
        public string Type = "Type:";

        public string LodeText = "LodeText";
        public string SaveText = "SaveText";
        public string LineAlignment = "LineAlignment";
        public string Distributed = "Distributed";
        public string NameColorLabel = "NameColor";
        public string Unfold = "Unfold";
        public string Refresh = "Refresh";

        public string DialogueAction = "DialogueAction";
        public string DialogueFigureImage = "DialogueFigureImage";
        public string DialogueVoice = "DialogueVoice";
        public string BackgroundImage = "BackgroundImage";
        public string BackgroundMusic = "BackgroundMusic";


        //图形窗口中的工具栏选项名称
        public string Save = "Save";
        public string SaveAs = "Save As...";
        public string Initialize = "Initialize";
        public string SwitchingMode = "Switching Mode";
        public string Inspector = "Inspector";

        //提示信息
        public string PleaseDisconnectBeforeDeleting = "Please disconnect before deleting!";
    }

}