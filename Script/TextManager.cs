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
        /// 根据文件路径获取文件后 调用指定类型的指定数据 
        /// </summary>
        /// <param name="flieUrl">文件路径</param>
        /// <param name="TextTitle">文字类型标题</param>
        /// <param name="TextContentId">调用ID</param>
        /// <returns>文字数据</returns>
        public string GetPlainText(string flieUrl,string TextTitle,int TextContentId) {
            PlainTextFile textFile = Resources.Load<PlainTextFile>(flieUrl);
            if (textFile == null) return null;
            return GetPlainText(textFile, TextTitle, TextContentId);
        }



        /// <summary>
        /// 根据文件 调用指定类型的指定数据 
        /// </summary>
        /// <param name="textFile">普通文本文件</param>
        /// <param name="TextTitle">文字类型标题</param>
        /// <param name="TextContentId">调用ID</param>
        /// <returns>文字数据</returns>
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
        /// 根据文件路径获取文件后 调用指定类型的指定数据 
        /// </summary>
        /// <param name="flieUrl">文件路径</param>
        /// <param name="ContentId">调用ID</param>
        /// <returns>文字数据</returns>
        public DialogueDate GetDialogueText(string flieUrl, int ContentId)
        {
            DialogueTextFile textFile = Resources.Load<DialogueTextFile>(flieUrl);
            if (textFile == null) return null;
            return GetDialogueText(textFile,ContentId,out ContentId);
        }

        /// <summary>
        /// 根据文件 调用指定类型的指定数据 
        /// </summary>
        /// <param name="textFile">对话文件</param>
        /// <param name="ContentId">调用ID</param>
        /// <returns>文字数据</returns>
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
        /// 根据文件 调用指定类型的指定数据 
        /// </summary>
        /// <param name="textFile">对话文件</param>
        /// <param name="ContentId">调用ID</param>
        /// <returns>文字数据</returns>
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