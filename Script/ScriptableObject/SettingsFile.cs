using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextGraphicalManagementEditor { 
    /// <summary>
    /// 设置文件 用于保存插件设置
    /// </summary>
    [CreateAssetMenu(menuName = "Text Graphical Marager/Settings File")]
    public class SettingsFile : ScriptableObject
    {
        public ManagerLanguageFile ManagerLanguage;
        public string FileUrl = "";
        public string MainLanguage;
        public List<string> LanguageName;
    }

}