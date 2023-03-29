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
        //ȫ�ֱ��� ��ǰ���õĲ�������ļ�
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

        //ȫ�ֱ��� ��ǰ���õĲ�������ļ�
        private static ManagerLanguageFile languageObj;
        public static ManagerLanguageFile LanguageObj
        {
            get
            {
                if (languageObj == null)
                {
                    //���Ի�ȡ�����е������ļ�
                    languageObj = SettingFileObj.ManagerLanguage;
                    //û�л�ȡ���Ļ���������һ��
                    if (languageObj == null)
                    {
                        languageObj = CreateInstance<ManagerLanguageFile>();
                        AssetDatabase.CreateAsset(languageObj, "Assets/TextGraphicalManagementEditor/Editor/Resources/Setting/" + "State Manager Language.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        //���浽�����ļ���
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
            //�����ļ������
            languageObjField = new ObjectField();
            languageObjField.objectType = typeof(ManagerLanguageFile);
            Label languageObjLabel = new Label(LanguageObj.ManagerLanguage);
            languageObjField.RegisterValueChangedCallback(OnTextObjVeluaChanged);
            //�ļ���·�������
            VisualElement fileUrlFieldVisualElement = new VisualElement();
            fileUrlField = new TextField();
            Label fileUrlLabel = new Label("�ļ�����·����");
            Button fileUrlButton = new Button(()=> {
                string url = EditorUtility.OpenFolderPanel("����·��ѡ��", Application.dataPath, "");
                if (url != null && url != "")
                {
                    fileUrlField.value = url;
                }
            });
            fileUrlButton.text = "ѡ��·��";
            fileUrlFieldVisualElement.Add(fileUrlLabel);
            fileUrlFieldVisualElement.Add(fileUrlField);
            fileUrlFieldVisualElement.Add(fileUrlButton);
            fileUrlFieldVisualElement.AddToClassList("SettingView");

            //�༭�õ�������
            VisualElement fileMainFieldVisualElement = new VisualElement();
            Label fileMainLabel = new Label("�༭�������ԣ�");
            fileMainFieldVisualElement.Add(fileMainLabel);
            fileMainFieldVisualElement.Add(fileMainField);
            fileMainFieldVisualElement.AddToClassList("SettingView");

            //�����б�
            VisualElement LanguageVisualElement = new VisualElement();
            VisualElement LanguageListVisualElement = new VisualElement();
            VisualElement LanguageButtonVisualElement = new VisualElement();
            Button AddLanguageName = new Button(() => {
                LanguageField languageField = new LanguageField();
                languageFields.Add(languageField);
                LanguageListVisualElement.Add(languageField); 
            });
            AddLanguageName.text = "�������";
            Button updateFile = new Button();
            updateFile.text = "ͬ�������ļ�";
            Label LanguageTitle = new Label("�����б�");
            LanguageButtonVisualElement.Add(AddLanguageName);
            LanguageButtonVisualElement.Add(updateFile);
            LanguageButtonVisualElement.AddToClassList("SettingView");
            LanguageButtonVisualElement.AddToClassList("LanguageButtonView");
            LanguageVisualElement.Add(LanguageTitle);
            LanguageVisualElement.Add(LanguageListVisualElement);
            LanguageVisualElement.Add(LanguageButtonVisualElement);
            LanguageVisualElement.AddToClassList("LanguageList");

            //������Ԫ����ӵ�����
            MainElement.Add(languageObjField);
            MainElement.Add(fileUrlFieldVisualElement);
            MainElement.Add(fileMainFieldVisualElement);
            MainElement.Add(LanguageVisualElement);

            rootVisualElement.Add(MainElement);
            rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("Uss/ManagerSetting"));

            //��ʼ������
            //��ǰʹ�õı༭�������ļ�
            languageObjField.value = LanguageObj;
            //�ı��ļ���ŵĸ���·��
            fileUrlField.value = SettingFileObj.FileUrl;
            //��ȡ��ǰ����·���µĲ�ͬ���Ե��ı��ļ�·�� ��������ʾ�������
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

        //�����ڹر�ʱ 
        public void OnDestroy()
        {
            //��������
            Save();
        }

        //�����ô���
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
        /// ��������
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
        /// ����������ѡ�������˵�
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
        /// �������е������ı��ļ�
        /// </summary>
        public void UpdateFile()
        {
            //�������б��ļ���¼ ��ȥ��· ���ظ�·���ᵼ���һ�δ������·�� ���������� ������ɲ���Ҫ�Ŀ��ٵȣ�
            fileNames = new List<string>();
            for (int i = 0; i < languageFields.Count; i++)
            {
                if (languageFields[i].LanguageName != null && languageFields[i].LanguageName != "" )
                {
                    fileNames.Add(languageFields[i].LanguageName);
                }
            }
            //��ȡ�����ļ��� ��¼�������б���
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
            //���������б� �������ļ������� ���Ʊ༭�������ļ����ƶ���·��
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
        /// �����ı��ļ� ���������ʱʹ�� (�ݹ�ʵ��)
        /// </summary>
        /// <param name="Language">���Ƶ���Ŀ������</param>
        /// <param name="Url">��Ҫ���Ƶ�·�� ����������·���������ļ���</param>
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
        /// �����ı��ļ� �������е��ı��ļ�ʱʹ�� (�ݹ�ʵ��)
        /// </summary>
        /// <param name="Language">���µ�Ŀ������</param>
        /// <param name="Url">��Ҫ���Ƶ�·�� ����������·���������ļ���</param>
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

            //����������ɾ�����ļ��д�����������ɾ��
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
            //�����������������ļ����Ƶ�����������
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
            //����������ɾ�����ļ�������������ɾ��
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
    /// �������������
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
