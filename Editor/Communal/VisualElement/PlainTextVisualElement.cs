using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    public class PlainTextVisualElement : GraphView
    {
        public TextGraphicalManagementEditorWindow window;
        /// <summary>
        /// 当前使用的对话文件
        /// </summary>
        private PlainTextFile _textObj;
        public PlainTextFile textObj
        {
            get
            {
                return _textObj;
            }
            set
            {
                _textObj = value;
                if (_textObj != null)
                {
                    window.titleContent.text = _textObj.name;
                }
                State();
            }
        }

        /// <summary>
        /// 文件对应的输入框
        /// </summary>
        public ObjectField objectField;

        /// <summary>
        /// 对话数据输入框组的父级
        /// </summary>
        VisualElement ContentContainer;

        /// <summary>
        /// 对话数据的集合
        /// </summary>
        public List<PlainTextDateField> plainTextDateFields;



        public PlainTextVisualElement(TextGraphicalManagementEditorWindow window, PlainTextFile textObj)
        {
            this.window = window;

            plainTextDateFields = new List<PlainTextDateField>();
            this.StretchToParentSize();
            ContentContainer = new VisualElement();
            ContentContainer.AddToClassList("ContentContainer");

            ///新建并添加当前对话文件的输入框组
            VisualElement objectFieldVisualElement = new VisualElement();
            Label objectLabel = new Label(ManagerSettingWindow.LanguageObj.CurrentPlainTextFile);
            objectField = new ObjectField();
            objectField.objectType = typeof(PlainTextFile);
            objectField.RegisterValueChangedCallback(OnTextObjVeluaChanged);
            objectFieldVisualElement.Add(objectLabel);
            objectFieldVisualElement.Add(objectField);
            objectFieldVisualElement.AddToClassList("PlainTextContainer");

            //新建并添加“添加数据”与“删除数据”按钮和层级
            VisualElement ButtonVisualElement = new VisualElement();
            Button addButton = new Button(AddPlainTextDate);
            addButton.text = ManagerSettingWindow.LanguageObj.AddPlainTextDate;
            Button removeButton = new Button(RemoveDialogueDate);
            removeButton.text = ManagerSettingWindow.LanguageObj.RemovePlainTextDate;
            ButtonVisualElement.Add(addButton);
            ButtonVisualElement.Add(removeButton);
            ButtonVisualElement.AddToClassList("PlainTextContainer");

            contentContainer.Add(objectFieldVisualElement);
            contentContainer.Add(ContentContainer);
            contentContainer.Add(ButtonVisualElement);

            this.window.Save = Save;
            this.window.SaveAs = SaveAs;

            this.textObj = textObj;
        }

        /// <summary>
        /// 添加数据按钮事件 添加对话数据
        /// </summary>
        public void AddPlainTextDate()
        {
            PlainTextDateField plainTextDateField = null;
            plainTextDateField = new PlainTextDateField(() => RemoveDialogueDate(plainTextDateField));
            ContentContainer.Add(plainTextDateField.plainTextDateField);
            plainTextDateFields.Add(plainTextDateField);
        }

        public void AddPlainTextDate(string name, List<string> contents)
        {
            PlainTextDateField plainTextDateField = null;
            plainTextDateField = new PlainTextDateField(() => RemoveDialogueDate(plainTextDateField));
            ContentContainer.Add(plainTextDateField.plainTextDateField);
            plainTextDateFields.Add(plainTextDateField);
            plainTextDateField.TypeStr.value = name;
            for (int i = 0; i < contents.Count; i++)
            {
                plainTextDateField.AddContent(contents[i]);
            }

        }

        /// <summary>
        /// 删除数据按钮事件 删除最后一个数据
        /// </summary>
        public void RemoveDialogueDate()
        {
            if (plainTextDateFields.Count > 0)
            {
                plainTextDateFields.RemoveAt(plainTextDateFields.Count - 1);
                ContentContainer.RemoveAt(plainTextDateFields.Count);
            }
        }

        /// <summary>
        /// 各数据中的删除数据按钮事件 删除对应的数据
        /// </summary>
        public void RemoveDialogueDate(PlainTextDateField plainTextDateField)
        {
            int plainTextDateFieldId = plainTextDateFields.LastIndexOf(plainTextDateField);
            plainTextDateFields.RemoveAt(plainTextDateFieldId);
            ContentContainer.RemoveAt(plainTextDateFieldId);
        }

        public void Save() {
            if (textObj == null)
            {
                SaveAs();
            }
            else
            {
                Save(textObj);
            }
        }

        public void Save(PlainTextFile textObj) {
            textObj.plainTexts = new List<PlainTextDate>();
            for (int i = 0; i < plainTextDateFields.Count; i++)
            {
                PlainTextDate date = new PlainTextDate();
                date.contents = new List<string>();
                date.typeName = plainTextDateFields[i].TypeStr.value;
                for (int j = 0; j < plainTextDateFields[i].contentFields.Count; j++)
                {
                    date.contents.Add(plainTextDateFields[i].contentFields[j].value);
                }
                textObj.plainTexts.Add(date);
            }
            EditorUtility.SetDirty(textObj);
        }

        public void SaveAs() {
            String SaveUrl = EditorUtility.SaveFilePanelInProject("保存到", "new Dialogue Text File", "asset", "请输入要保存的文件名");
            if (SaveUrl == "") return;
            PlainTextFile plainTextFile = ScriptableObject.CreateInstance<PlainTextFile>();
            Save(plainTextFile);
            AssetDatabase.CreateAsset(plainTextFile, SaveUrl);
            AssetDatabase.SaveAssets();
            textObj = plainTextFile;
        }

        public void State() {
            objectField.value = textObj;
            plainTextDateFields = new List<PlainTextDateField>();
            if (textObj != null)
            {
                for (int i = 0; i < textObj.plainTexts.Count; i++)
                {
                    AddPlainTextDate(textObj.plainTexts[i].typeName, textObj.plainTexts[i].contents);
                }
            }
        }

        public void OnTextObjVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            this.textObj = objectField.value as PlainTextFile;
            window.fileObj = this.textObj;
        }
    }

    public class PlainTextDateField {

        /// <summary>
        /// 当前的输入框组父类
        /// </summary>
        public VisualElement plainTextDateField;

        public TextField TypeStr;
        public VisualElement ContentContainer;
        public List<TextField> contentFields;

        public PlainTextDateField(Action Remove){
            plainTextDateField = new VisualElement();
            contentFields = new List<TextField>();

            VisualElement cutOffRule = new VisualElement();
            cutOffRule.AddToClassList("CutOffRule");

            VisualElement TypeContainer = new VisualElement();
            Label TypeLabel = new Label(ManagerSettingWindow.LanguageObj.Type);
            TypeStr = new TextField();
            Button RemoveButton = new Button(Remove);
            TypeContainer.Add(TypeLabel);
            TypeContainer.Add(TypeStr);
            TypeContainer.Add(RemoveButton);
            TypeContainer.AddToClassList("PlainTextContent");
            TypeContainer.AddToClassList("TypeContainer");

            ContentContainer = new VisualElement();
            Label ContentLabel = new Label(ManagerSettingWindow.LanguageObj.Content);
            Button AddButton = new Button(AddContent);
            AddButton.text = ManagerSettingWindow.LanguageObj.AddContent;
            VisualElement ContentTitleContainer = new VisualElement();
            ContentTitleContainer.Add(ContentLabel);
            ContentTitleContainer.Add(AddButton);
            ContentContainer.Add(ContentTitleContainer);
            ContentTitleContainer.AddToClassList("ContentTitleContainer");

            ContentContainer.AddToClassList("PlainTextContentContainer");
            plainTextDateField.AddToClassList("PlainTextDataContent");

            plainTextDateField.contentContainer.Add(cutOffRule);
            plainTextDateField.contentContainer.Add(TypeContainer);
            plainTextDateField.contentContainer.Add(ContentContainer);
        }

        public void AddContent() {
            VisualElement visualElement = new VisualElement();
            TextField textField = new TextField();
            Button button = new Button(()=> { RemoveContent(textField); });
            visualElement.Add(textField);
            visualElement.Add(button);
            visualElement.AddToClassList("PlainTextContent");
            contentFields.Add(textField);
            ContentContainer.Add(visualElement);
        }

        public void AddContent(string content)
        {
            VisualElement visualElement = new VisualElement();
            TextField textField = new TextField();
            textField.value = content;
            Button button = new Button(() => { RemoveContent(textField); });
            visualElement.Add(textField);
            visualElement.Add(button);
            visualElement.AddToClassList("PlainTextContent");
            contentFields.Add(textField);
            ContentContainer.Add(visualElement);
        }

        public void RemoveContent(TextField textField) {
            int contentId = contentFields.IndexOf(textField);
            contentFields.RemoveAt(contentId);
            ContentContainer.RemoveAt(contentId+1);
        }
    }
}