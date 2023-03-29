using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace TextGraphicalManagementEditor
{
    /// <summary>
    /// TextStructrue文件图形窗口的右键菜单
    /// </summary>
    public class TextStructrueSearchMenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        /// <summary>
        /// 添加菜单项
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent(ManagerSettingWindow.LanguageObj.TextStructrueNode)));
            entries.Add(new SearchTreeEntry(new GUIContent(ManagerSettingWindow.LanguageObj.DialogueNode)) { level = 1, userData = typeof(DialogueNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(ManagerSettingWindow.LanguageObj.BranchNode)) { level = 1, userData = typeof(BranchNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(ManagerSettingWindow.LanguageObj.SwitchNode)) { level = 1, userData = typeof(SwitchNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(ManagerSettingWindow.LanguageObj.AutomaticBranchNode)) { level = 1, userData = typeof(AutomaticBranchNode) });
            entries.Add(new SearchTreeEntry(new GUIContent(ManagerSettingWindow.LanguageObj.DialogueNode+"(单)")) { level = 1, userData = typeof(DialogueTextNode) });


            return entries;
        }

        public delegate bool TextStructrueMenuWindowProviderDelegate(SearchTreeEntry SearchTreeEntry, SearchWindowContext context);
        public TextStructrueMenuWindowProviderDelegate OnTextStructrueMenuWindowProviderDelegate;

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            if (OnTextStructrueMenuWindowProviderDelegate == null)
            {
                return false;
            }
            return OnTextStructrueMenuWindowProviderDelegate(SearchTreeEntry, context);
        }

    }
}
