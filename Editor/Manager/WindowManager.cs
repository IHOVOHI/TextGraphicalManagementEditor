using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace TextGraphicalManagementEditor
{
    public class WindowManager
    {
        private static Dictionary<string, TextGraphicalManagementEditorWindow> windows;
        public static Dictionary<string, TextGraphicalManagementEditorWindow> Windows
        {
            get
            {
                if (windows == null)
                {
                    windows = new Dictionary<string, TextGraphicalManagementEditorWindow>();
                }
                return windows;
            }
        }

        public static TextGraphicalManagementEditorWindow NowWindow;

        /// <summary>
        /// 点击Asset文件打开窗口
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [OnOpenAsset(1)]
        public static bool OpenWindow(int instanceId, int b)
        {
            object obj = EditorUtility.InstanceIDToObject(instanceId);
            if (obj.GetType().ToString() == "TextGraphicalManagementEditor.TextStructrueFile" ||
                obj.GetType().ToString() == "TextGraphicalManagementEditor.PlainTextFile" ||
                obj.GetType().ToString() == "TextGraphicalManagementEditor.DialogueTextFile")
            {
                GetWindow(obj.GetType().ToString(), EditorUtility.InstanceIDToObject(instanceId).name, obj as TextFile);
            }
            return false;
        }

            /// <summary>
            /// 从对象池中获取窗口
            /// </summary>
            /// <typeparam name="T">窗口类型</typeparam>
            /// <param name="typeName">类型名称</param>
            /// <param name="windowName">窗口名称</param>
            /// <returns>窗口实例</returns>
        public static TextGraphicalManagementEditorWindow GetWindow(string typeName, string windowName, TextFile obj)
        {
            TextGraphicalManagementEditorWindow editorWindow = null;
            if (Windows.ContainsKey(windowName))
            {
                editorWindow = Windows[windowName];
                editorWindow.Show();
                editorWindow.Focus();
            }
            else
            {
                editorWindow = EditorWindow.CreateWindow<TextGraphicalManagementEditorWindow>(windowName);
                Windows.Add(windowName, editorWindow);
            }
            SetWindowContentVisualElement(typeName, editorWindow,obj);
            editorWindow.windowName = editorWindow.titleContent.text;
            editorWindow.windowType = typeName;
            return editorWindow;
        }

        public static void SetWindowContentVisualElement (string typeName, TextGraphicalManagementEditorWindow editorWindow, TextFile obj) {
            switch (typeName)
            {
                case "TextGraphicalManagementEditor.TextStructrueFile":
                    editorWindow.ContentVisualElement = new TextStructrueGraphView(editorWindow, obj as TextStructrueFile);
                    break;
                case "TextGraphicalManagementEditor.PlainTextFile":
                    editorWindow.ContentVisualElement = new PlainTextVisualElement(editorWindow, obj as PlainTextFile);
                    break;
                case "TextGraphicalManagementEditor.DialogueTextFile":
                    editorWindow.ContentVisualElement = new DialogueVisualElement(editorWindow, obj as DialogueTextFile);
                    break;
            }
        }

        /// <summary>
        /// 从对象池删除窗口
        /// </summary>
        /// <param name="window">窗口实例</param>
        public static void RemoveWindow(TextGraphicalManagementEditorWindow window )
        {
            if (Windows.ContainsKey(window.windowName))
            {
                Windows.Remove(window.windowName);
            }
        }

        /// <summary>
        /// 打开新的文本结构文档窗口
        /// </summary>
        [MenuItem("Window/Text Graphical Marager/ Text Structrue")]
        public static void OpenMenuTextStructrueWindow()
        {
            GetWindow("TextGraphicalManagementEditor.TextStructrueFile", "NewTextStructrueFile", null);
        }

        /// <summary>
        /// 打开新的普通文本文档窗口
        /// </summary>
        [MenuItem("Window/Text Graphical Marager/Plain Text")]
        public static void OpenMenuPlainTextWindow()
        {
            GetWindow("TextGraphicalManagementEditor.PlainTextFile", "NewPlainTextFile", null);
        }

        /// <summary>
        /// 打开新的对话文档窗口
        /// </summary>
        [MenuItem("Window/Text Graphical Marager/ Dialogue")]
        public static void OpenMenuDialogueWindow()
        {
            GetWindow("TextGraphicalManagementEditor.DialogueTextFile", "NewDialogueTextFile", null);
        }
    }
}
 