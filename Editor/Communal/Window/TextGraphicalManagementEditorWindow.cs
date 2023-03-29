using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    public class TextGraphicalManagementEditorWindow : EditorWindow
    {
        public string windowName;
        public string windowType;

        private TextFile _fileObj;
        public TextFile fileObj
        {
            get { return _fileObj; }
            set
            {
                _fileObj = value;
                WindowManager.SetWindowContentVisualElement(windowType, this, _fileObj);
            }
        }

        private VisualElement contentVisualElement;
        public VisualElement ContentVisualElement
        {
            get { return contentVisualElement; }
            set
            {
                contentVisualElement = value;
                if (rootVisualElement.childCount > 1)
                {
                    rootVisualElement.RemoveAt(0);
                }
                if (contentVisualElement != null)
                {
                    rootVisualElement.Insert(0, contentVisualElement);
                }
            }
        }

        public Action Save;
        public Action SaveAs;
        public Action Initialize;
        public Action Inspector;

        public static TextGraphicalManagementEditorWindow OpenWindow(string windowType,string windowName, TextFile obj)
        {
            return WindowManager.GetWindow(windowType, windowName,obj);
        }

        private void OnEnable()
        {

            using (new EditorGUI.DisabledScope(!hasUnsavedChanges))
            {
                if (GUILayout.Button("Save"))
                    SaveChanges();
            }

            var toolbar = new IMGUIContainer(() =>
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
                if (GUILayout.Button(ManagerSettingWindow.LanguageObj.Save, EditorStyles.toolbarButton))
                {
                    this.hasUnsavedChanges = false;
                    Save?.Invoke();
                }
                GUILayout.Space(5);
                if (GUILayout.Button(ManagerSettingWindow.LanguageObj.SaveAs, EditorStyles.toolbarButton))
                {
                    SaveAs?.Invoke();
                }
                //GUILayout.Space(5);
                //if (GUILayout.Button(ManagerSettingWindow.LanguageObj.Initialize, EditorStyles.toolbarButton))
                //{
                //    Initialize?.Invoke();
                //}

                GUILayout.FlexibleSpace();

                //if (GUILayout.Button(ManagerSettingWindow.LanguageObj.Inspector, EditorStyles.toolbarButton))
                //{
                //    Inspector?.Invoke();
                //}
                GUILayout.EndHorizontal();
            });

            if (contentVisualElement != null)
            {
                rootVisualElement.Insert(0, contentVisualElement);
            }

            rootVisualElement.Add(toolbar);
            rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("Uss/PlainTextVisualElement"));
        }

        public override void SaveChanges()
        {
            Save?.Invoke();
            this.hasUnsavedChanges = false;
        }

        public void OnDisable()
        {
            //Save?.Invoke();
            WindowManager.RemoveWindow(this);
        }

        public void OnFocus()
        {
            WindowManager.NowWindow = this;
        }


        public void SetHasUnsavedChanges(bool value)
        {
            this.hasUnsavedChanges = value;
        }
    }
}
