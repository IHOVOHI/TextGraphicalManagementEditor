// C# Example
// 演示未保存更改API的脚本

using UnityEngine;
using UnityEditor;

public class UnsavedChangesExampleWindow : EditorWindow
{
    [MenuItem("Example/Editor Window With Unsaved Changes")]
    static void Init()
    {
        UnsavedChangesExampleWindow window = (UnsavedChangesExampleWindow)EditorWindow.GetWindowWithRect(typeof(UnsavedChangesExampleWindow), new Rect(100, 100, 400, 400));

        window.saveChangesMessage = "This window has unsaved changes. Would you like to save?";
        window.Show();
    }

    void OnGUI()
    {
        saveChangesMessage = EditorGUILayout.TextField(saveChangesMessage);

        EditorGUILayout.LabelField(hasUnsavedChanges ? "I have changes!" : "No changes.", EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("Try to close the window.");

        using (new EditorGUI.DisabledScope(hasUnsavedChanges))
        {
            if (GUILayout.Button("创建未保存的更改"))
                hasUnsavedChanges = true;
        }

        using (new EditorGUI.DisabledScope(!hasUnsavedChanges))
        {
            if (GUILayout.Button("Save"))
                SaveChanges();

            //if (GUILayout.Button("Discard")) ;
            //    //DiscardChanges();
        }
    }

    public override void SaveChanges()
    {
        // Your custom save procedures here

        Debug.Log($"{this} saved successfully!!!");
        base.SaveChanges();
    }


    //public override void DiscardChanges()
    //{
    //    // Your custom procedures to discard changes

    //    Debug.Log($"{this} discarded changes!!!");
    //    base.DiscardChanges();
    //}
}