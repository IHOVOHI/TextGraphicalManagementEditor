using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TextGraphicalManagementEditor
{
    
    public class DialogueActionSearchWiodowProvider : ScriptableObject, ISearchWindowProvider
    {
        /// <summary>
        /// Ìí¼Ó²Ëµ¥Ïî
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent(ManagerSettingWindow.LanguageObj.DialogueAction)));
            entries.Add(new SearchTreeEntry(new GUIContent(ManagerSettingWindow.LanguageObj.DialogueFigureImage)) { level = 1, userData = typeof(DialogueFigureImage) });
            entries.Add(new SearchTreeEntry(new GUIContent(ManagerSettingWindow.LanguageObj.DialogueVoice)) { level = 1, userData = typeof(DialogueVoice) });
            entries.Add(new SearchTreeEntry(new GUIContent(ManagerSettingWindow.LanguageObj.BackgroundMusic)) { level = 1, userData = typeof(BackgroundMusic) });
            entries.Add(new SearchTreeEntry(new GUIContent(ManagerSettingWindow.LanguageObj.BackgroundImage)) { level = 1, userData = typeof(BackgroundImage) });

            return entries;
        }

        public delegate bool DialogueMenuWindowProviderDelegate(SearchTreeEntry SearchTreeEntry, SearchWindowContext context);
        public DialogueMenuWindowProviderDelegate OnDialogueMenuWindowProviderDelegate;

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            if (OnDialogueMenuWindowProviderDelegate == null)
            {
                return false;
            }
            return OnDialogueMenuWindowProviderDelegate(SearchTreeEntry, context);
        }

    }
}

