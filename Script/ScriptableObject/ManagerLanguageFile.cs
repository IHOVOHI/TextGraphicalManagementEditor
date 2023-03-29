using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextGraphicalManagementEditor { 
    /// <summary>
    /// �༭�������ļ� �����ṩ���ʹ�õ�String����
    /// </summary>
    [CreateAssetMenu(menuName = "Text Graphical Marager/Manager Language File")]
    public class ManagerLanguageFile : ScriptableObject
    {
        //�༭�������е��ַ���
        public string ManagerSetting = "Manager Setting";
        public string ManagerLanguage = "Manager Language";
        public string FileUrl = "File Url";

        //��ͼ�δ�������ʾ�Ľڵ�����
        public string TextStructrueNode = "TextStructrueNode";
        public string RootNode = "RootNode";
        public string BranchNode = "BranchNode";
        public string AutomaticBranchNode = "AutomaticBranchNode";
        public string SwitchNode = "SwitchNode";
        public string DialogueNode = "DialogueNode";


        //��ͼ�δ����нڵ�ʹ�õ�����
        public string Open = "Open";
        public string Option = "Option:";

        //��ͼ�δ�����ʹ�õ�����
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


        //ͼ�δ����еĹ�����ѡ������
        public string Save = "Save";
        public string SaveAs = "Save As...";
        public string Initialize = "Initialize";
        public string SwitchingMode = "Switching Mode";
        public string Inspector = "Inspector";

        //��ʾ��Ϣ
        public string PleaseDisconnectBeforeDeleting = "Please disconnect before deleting!";
    }

}