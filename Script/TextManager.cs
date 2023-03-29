using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TextGraphicalManagementEditor
{
    public class TextManager : MonoBehaviour
    {
        private SettingsFile _settings;
        public SettingsFile Settings {
            get {
                if (_settings == null)
                {

                }
                return _settings;
            }
        }

        private static TextManager instance;
        public static TextManager Instance {
            get {
                if (instance == null)
                {
                    GameObject gameObject = new GameObject("TextManager");
                    instance = gameObject.AddComponent<TextManager>();
                }
                return instance;
            }
        }

        public string TextFileURL;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            
        }


        /// <summary>
        /// �����ļ�·����ȡ�ļ��� ����ָ�����͵�ָ������ 
        /// </summary>
        /// <param name="flieUrl">�ļ�·��</param>
        /// <param name="TextTitle">�������ͱ���</param>
        /// <param name="TextContentId">����ID</param>
        /// <returns>��������</returns>
        public string GetPlainText(string flieUrl,string TextTitle,int TextContentId) {
            PlainTextFile textFile = Resources.Load<PlainTextFile>(flieUrl);
            if (textFile == null) return null;
            return GetPlainText(textFile, TextTitle, TextContentId);
        }



        /// <summary>
        /// �����ļ� ����ָ�����͵�ָ������ 
        /// </summary>
        /// <param name="textFile">��ͨ�ı��ļ�</param>
        /// <param name="TextTitle">�������ͱ���</param>
        /// <param name="TextContentId">����ID</param>
        /// <returns>��������</returns>
        public string GetPlainText(PlainTextFile textFile, string TextTitle, int TextContentId)
        {
            string returnDate = "";
            if (textFile == null) return returnDate;

            for (int i = 0; i < textFile.plainTexts.Count; i++)
            {
                if (textFile.plainTexts[i].typeName == TextTitle)
                {
                    returnDate = textFile.plainTexts[i].contents[TextContentId % textFile.plainTexts[i].contents.Count];
                    return returnDate;
                }
            }
            return returnDate;
        }

        /// <summary>
        /// �����ļ�·����ȡ�ļ��� ����ָ�����͵�ָ������ 
        /// </summary>
        /// <param name="flieUrl">�ļ�·��</param>
        /// <param name="ContentId">����ID</param>
        /// <returns>��������</returns>
        public DialogueDate GetDialogueText(string flieUrl, int ContentId)
        {
            DialogueTextFile textFile = Resources.Load<DialogueTextFile>(flieUrl);
            if (textFile == null) return null;
            return GetDialogueText(textFile,ContentId,out ContentId);
        }

        /// <summary>
        /// �����ļ� ����ָ�����͵�ָ������ 
        /// </summary>
        /// <param name="textFile">�Ի��ļ�</param>
        /// <param name="ContentId">����ID</param>
        /// <returns>��������</returns>
        public static DialogueDate GetDialogueText(DialogueTextFile textFile,int contentId, out int returnId)
        {
            returnId = -1;
            DialogueDate returnDate = null;
            if (textFile == null) {
                return null; 
            }
            if (textFile.dialogueDates.Count >contentId)
            {
                returnDate = textFile.dialogueDates[contentId];
                returnId = contentId + 1;
                return returnDate;
            }
            return null;
        }

        /// <summary>
        /// �����ļ� ����ָ�����͵�ָ������ 
        /// </summary>
        /// <param name="textFile">�Ի��ļ�</param>
        /// <param name="ContentId">����ID</param>
        /// <returns>��������</returns>
        public static NodeData GetTextStructrue(TextStructrueFile textFile, int contentId, out int returnId ,Dictionary<string , float> AutomaticOptionValue)
        {
            returnId = -1;
            NodeData returnDate = null;
            if (textFile == null)
            {
                return null;
            }
            if (contentId != -1)
            {
                returnDate = textFile.nodeDatas.list[contentId];
                switch (returnDate.NodeType)
                {
                    case "TextGraphicalManagementEditor.AutomaticBranchNode":
                        if (textFile.nodeDatas.list[contentId].TrailingNodes.Count > 0)
                        {
                            AutomaticBranchNodeData automatic = returnDate as AutomaticBranchNodeData;
                            for (int i = 0; i < automatic.branchName.Count; i++)
                            {
                                if (AutomaticOptionValue.ContainsKey(automatic.branchName[i]))
                                {
                                    switch (automatic.compare)
                                    {
                                        case 0:
                                            if (AutomaticOptionValue[automatic.branchName[i]] > automatic.branchValue[i])
                                            {
                                                returnId = textFile.nodeDatas.list[contentId].TrailingNodes[i];
                                                return returnDate;
                                            }
                                            break;
                                        case 1:
                                            if (AutomaticOptionValue[automatic.branchName[i]] < automatic.branchValue[i])
                                            {
                                                returnId = textFile.nodeDatas.list[contentId].TrailingNodes[i];
                                                return returnDate;
                                            }
                                            break;
                                        case 2:
                                            if (AutomaticOptionValue[automatic.branchName[i]] == automatic.branchValue[i])
                                            {
                                                returnId = textFile.nodeDatas.list[contentId].TrailingNodes[i];
                                                return returnDate;
                                            }
                                            break;
                                        case 3:
                                            if (AutomaticOptionValue[automatic.branchName[i]] != automatic.branchValue[i])
                                            {
                                                returnId = textFile.nodeDatas.list[contentId].TrailingNodes[i];
                                                return returnDate;
                                            }
                                            break;
                                        case 4:
                                            if (AutomaticOptionValue[automatic.branchName[i]] >= automatic.branchValue[i])
                                            {
                                                returnId = textFile.nodeDatas.list[contentId].TrailingNodes[i];
                                                return returnDate;
                                            }
                                            break;
                                        case 5:
                                            if (AutomaticOptionValue[automatic.branchName[i]] <= automatic.branchValue[i])
                                            {
                                                returnId = textFile.nodeDatas.list[contentId].TrailingNodes[i];
                                                return returnDate;
                                            }
                                            break;
                                    }
                                }
                            }
                            returnId = textFile.nodeDatas.list[contentId].TrailingNodes[0];
                        }
                        break;
                    case "TextGraphicalManagementEditor.BranchNode":
                        if (textFile.nodeDatas.list[contentId].TrailingNodes.Count > 0)
                        {
                            returnId = contentId;
                        }
                        break;

                    default:
                        if (textFile.nodeDatas.list[contentId].TrailingNodes.Count > 0)
                        {
                            returnId = textFile.nodeDatas.list[contentId].TrailingNodes[0];
                        }
                        break;
                }
                return returnDate;
            }
            return returnDate;
        }

    }
}