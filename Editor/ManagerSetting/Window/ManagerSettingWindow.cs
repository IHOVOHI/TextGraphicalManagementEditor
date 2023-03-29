using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor {
    public class ManagerSettingWindow : EditorWindow
    {
        //全局变量 当前引用的插件设置文件
        private static SettingsFile settingFile;
        public static SettingsFile SettingFileObj
        {
            get
            {
                if (!settingFile)
                {
                    settingFile = AssetDatabase.LoadAssetAtPath<SettingsFile>("Assets/TextGraphicalManagementEditor/Editor/Resources/Setting/Setting File.asset");
                    if (!settingFile)
                    {
                        Directory.CreateDirectory(Application.dataPath + "TextGraphicalManagementEditor/Editor/Resources/Setting");
                        settingFile = ScriptableObject.CreateInstance<SettingsFile>();
                        AssetDatabase.CreateAsset(settingFile, "TextGraphicalManagementEditor/Editor/Resources/Setting/" + "Setting File.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
                return settingFile;
            }
        }

        //全局变量 当前引用的插件语言文件
        private static ManagerLanguageFile languageObj;
        public static ManagerLanguageFile LanguageObj
        {
            get
            {
                if (languageObj == null)
                {
                    //尝试获取设置中的语言文件
                    languageObj = SettingFileObj.ManagerLanguage;
                    //没有获取到的话重新生成一个
                    if (languageObj == null)
                    {
                        languageObj = CreateInstance<ManagerLanguageFile>();
                        AssetDatabase.CreateAsset(languageObj, "Assets/TextGraphicalManagementEditor/Editor/Resources/Setting/" + "State Manager Language.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        //保存到设置文件中
                        SettingFileObj.ManagerLanguage = LanguageObj;
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
                return languageObj; 
            }
            set
            {
                languageObj = value;
            }
        }

        ObjectField languageObjField;
        TextField fileUrlField;
        PopupField<string> fileMainField;
        List<string> fileNames;
        List<LanguageField> languageFields;

        public void OnEnable()
        {
            fileNames = new List<string>();
            languageFields = new List<LanguageField>();

            ScrollView MainElement = new ScrollView();
            //语言文件输入框
            languageObjField = new ObjectField();
            languageObjField.objectType = typeof(ManagerLanguageFile);
            Label languageObjLabel = new Label(LanguageObj.ManagerLanguage);
            languageObjField.RegisterValueChangedCallback(OnTextObjVeluaChanged);
            //文件主路径输入框
            VisualElement fileUrlFieldVisualElement = new VisualElement();
            fileUrlField = new TextField();
            Label fileUrlLabel = new Label("文件父级路径：");
            Button fileUrlButton = new Button(()=> {
                string url = EditorUtility.OpenFolderPanel("父级路径选择", Application.dataPath, "");
                if (url != null && url != "")
                {
                    fileUrlField.value = url;
                }
            });
            fileUrlButton.text = "选择路径";
            fileUrlFieldVisualElement.Add(fileUrlLabel);
            fileUrlFieldVisualElement.Add(fileUrlField);
            fileUrlFieldVisualElement.Add(fileUrlButton);
            fileUrlFieldVisualElement.AddToClassList("SettingView");

            //编辑用的主语言
            VisualElement fileMainFieldVisualElement = new VisualElement();
            Label fileMainLabel = new Label("编辑用主语言：");
            fileMainFieldVisualElement.Add(fileMainLabel);
            fileMainFieldVisualElement.Add(fileMainField);
            fileMainFieldVisualElement.AddToClassList("SettingView");

            //语言列表
            VisualElement LanguageVisualElement = new VisualElement();
            VisualElement LanguageListVisualElement = new VisualElement();
            VisualElement LanguageButtonVisualElement = new VisualElement();
            Button AddLanguageName = new Button(() => {
                LanguageField languageField = new LanguageField();
                languageFields.Add(languageField);
                LanguageListVisualElement.Add(languageField); 
            });
            AddLanguageName.text = "添加语言";
            Button updateFile = new Button();
            updateFile.text = "同步语言文件";
            Label LanguageTitle = new Label("语言列表：");
            LanguageButtonVisualElement.Add(AddLanguageName);
            LanguageButtonVisualElement.Add(updateFile);
            LanguageButtonVisualElement.AddToClassList("SettingView");
            LanguageButtonVisualElement.AddToClassList("LanguageButtonView");
            LanguageVisualElement.Add(LanguageTitle);
            LanguageVisualElement.Add(LanguageListVisualElement);
            LanguageVisualElement.Add(LanguageButtonVisualElement);
            LanguageVisualElement.AddToClassList("LanguageList");

            //将所有元素添加到窗口
            MainElement.Add(languageObjField);
            MainElement.Add(fileUrlFieldVisualElement);
            MainElement.Add(fileMainFieldVisualElement);
            MainElement.Add(LanguageVisualElement);

            rootVisualElement.Add(MainElement);
            rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("Uss/ManagerSetting"));

            //初始化数据
            //当前使用的编辑器语言文件
            languageObjField.value = LanguageObj;
            //文本文件存放的父级路径
            fileUrlField.value = SettingFileObj.FileUrl;
            //获取当前父级路径下的不同语言的文本文件路径 并将其显示到面板中
            DirectoryInfo directoryInfo = new DirectoryInfo(SettingFileObj.FileUrl);
            DirectoryInfo[] files = directoryInfo.GetDirectories();
            foreach (var file in files)
            {
                LanguageField objectField = new LanguageField(file.Name);
                fileNames.Add(file.Name);
                languageFields.Add(objectField);
                LanguageListVisualElement.Add(objectField);
            }

            UpdatePopupField();
            fileMainFieldVisualElement.Add(fileMainField);

            updateFile.clicked += ()=> { 
                UpdateFile();
                fileMainFieldVisualElement.RemoveAt(1);
                UpdatePopupField();
                fileMainFieldVisualElement.Add(fileMainField);
            };
        }

        //当窗口关闭时 
        public void OnDestroy()
        {
            //保存设置
            Save();
        }

        //打开设置窗口
        [MenuItem("Window/Text Graphical Marager/Manager Setting")]
        public static void OpenWindow()
        {
            GetWindow<ManagerSettingWindow>(LanguageObj.ManagerSetting);
        }

        public void OnTextObjVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            languageObj = languageObjField.value as ManagerLanguageFile;
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        public void Save() {
            SettingFileObj.ManagerLanguage = LanguageObj;
            SettingFileObj.FileUrl = fileUrlField.value;
            SettingFileObj.MainLanguage = fileMainField.value;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(SettingFileObj);
        }
        
        /// <summary>
        /// 更新主语言选择下拉菜单
        /// </summary>
        public void UpdatePopupField()
        {
            if (fileNames.Contains(SettingFileObj.MainLanguage))
            {
                fileMainField = new PopupField<string>(fileNames, fileNames.IndexOf(SettingFileObj.MainLanguage));
            }
            else if (fileNames.Count > 0)
            {
                fileMainField = new PopupField<string>(fileNames, 0);
            }
            else
            {
                fileMainField = new PopupField<string>();
            }
        }

        /// <summary>
        /// 更新所有的语言文本文件
        /// </summary>
        public void UpdateFile()
        {
            //将语言列表文件记录 除去空路 （重复路径会导向第一次处理过的路径 但尽量避免 以免造成不必要的卡顿等）
            fileNames = new List<string>();
            for (int i = 0; i < languageFields.Count; i++)
            {
                if (languageFields[i].LanguageName != null && languageFields[i].LanguageName != "" )
                {
                    fileNames.Add(languageFields[i].LanguageName);
                }
            }
            //获取现有文件夹 记录到语言列表中
            List<string> languageNameList = new List<string>(); 
            DirectoryInfo directoryInfo = new DirectoryInfo(SettingFileObj.FileUrl);
            DirectoryInfo[] files = directoryInfo.GetDirectories();
            foreach (var file in files)
            {
                if (fileNames.Contains(file.Name))
                {
                    languageNameList.Add(file.Name);
                }
            }
            //遍历语言列表 若语言文件不存在 复制编辑主语言文件到制定的路径
            for (int i = 0; i < fileNames.Count; i++)
            {
                if (!languageNameList.Contains(fileNames[i]))
                {
                    CopyAllTextFile(fileNames[i]);
                }
                else
                {
                    UpdateTextFile(fileNames[i]);
                }
            }


        }


        /// <summary>
        /// 复制文本文件 添加新语言时使用 (递归实现)
        /// </summary>
        /// <param name="Language">复制到的目标语音</param>
        /// <param name="Url">需要复制的路径 不包括父级路径与语言文件夹</param>
        public void CopyAllTextFile(string Language ,string Url = "") {
            Directory.CreateDirectory(SettingFileObj.FileUrl + "/" + Language + Url);
            DirectoryInfo directoryInfo = new DirectoryInfo(SettingFileObj.FileUrl + "/" + SettingFileObj.MainLanguage + Url);
            DirectoryInfo[] mainFiles = directoryInfo.GetDirectories();
            foreach (var file in mainFiles)
            {
                CopyAllTextFile(Language,Url + "/" + file.Name);
            }
            string[] CsfileNames = Directory.GetFiles(SettingFileObj.FileUrl + "/" + SettingFileObj.MainLanguage + Url);
            foreach (var item in CsfileNames)
            {
                if (!item.EndsWith(".meta"))
                {
                    File.Copy(item, SettingFileObj.FileUrl + "/" + Language + Url + "/" + item.Remove(0, item.IndexOf(@"\") + 1));
                }
            }
        }

        /// <summary>
        /// 更新文本文件 更新已有的文本文件时使用 (递归实现)
        /// </summary>
        /// <param name="Language">更新的目标语音</param>
        /// <param name="Url">需要复制的路径 不包括父级路径与语言文件夹</param>
        public void UpdateTextFile(string Language, string Url = "")
        {
            string MainUrl = SettingFileObj.FileUrl + "/" + SettingFileObj.MainLanguage + Url;
            string LanguageUrl = SettingFileObj.FileUrl + "/" + Language + Url;
            Directory.CreateDirectory(LanguageUrl);
            DirectoryInfo MainDirectoryInfo = new DirectoryInfo(MainUrl);
            DirectoryInfo[] mainDirectorys = MainDirectoryInfo.GetDirectories();
            foreach (var file in mainDirectorys)
            {
                UpdateTextFile(Language, Url + "/" + file.Name);
            }

            //将主语言中删除的文件夹从其他语言中删除
            DirectoryInfo languageDirectoryInfo = new DirectoryInfo(LanguageUrl);
            DirectoryInfo[] languageDirectory = languageDirectoryInfo.GetDirectories();
            foreach (var file in languageDirectory)
            {
                //Debug.Log(MainUrl + "/" + file.FullName);
                if (!Directory.Exists(MainUrl +"/"+ file.Name))
                {
                    Directory.Delete(LanguageUrl + "/" + file.Name,true);
                    File.Delete(LanguageUrl + "/" + file.Name + ".meta");
                }
            }
            //将主语言中新增的文件复制到其他语言中
            string[] mainFileNames = Directory.GetFiles(MainUrl);
            foreach (var item in mainFileNames)
            {
                if (!item.EndsWith(".meta"))
                {
                    if (!File.Exists(LanguageUrl + "/" + item.Remove(0, item.IndexOf(@"\") + 1)))
                    {
                        File.Copy(item, LanguageUrl + "/" + item.Remove(0, item.IndexOf(@"\") + 1));
                    }
                }
            }
            //将主语言中删除的文件从其他语言中删除
            string[] languageFileNames = Directory.GetFiles(LanguageUrl);
            foreach (var item in languageFileNames)
            {
                if (!item.EndsWith(".meta"))
                {
                    if (!File.Exists(MainUrl + "/" + item.Remove(0, item.IndexOf(@"\") + 1)))
                    {
                        File.Delete(LanguageUrl + "/" + item.Remove(0, item.IndexOf(@"\") + 1));
                        File.Delete(LanguageUrl + "/" + item.Remove(0, item.IndexOf(@"\") + 1)+".meta");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 语言栏输入框组
    /// </summary>
    public class LanguageField:VisualElement{

        public string LanguageName;
        private TextField LanguageNameField;

        public LanguageField(string value = "") {

            LanguageNameField = new TextField();
            LanguageNameField.RegisterValueChangedCallback(OnLanguageNameVeluaChanged);
            LanguageName = value;
            LanguageNameField.value = LanguageName;
            Button button = new Button();
            contentContainer.Add(LanguageNameField);
            contentContainer.Add(button);
            contentContainer.AddToClassList("SettingView");
            contentContainer.AddToClassList("LanguageField");
        }

        public void OnLanguageNameVeluaChanged(ChangeEvent<string> changeEvent)
        {
            LanguageName = LanguageNameField.value;
        }

    }
}
