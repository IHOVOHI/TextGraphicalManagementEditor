using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    /// <summary>
    /// ���ڵ� ���浱ǰʹ�õ��ı��ṹ�ļ�
    /// </summary>
    public class RootNode : TextGraphicalManagementEditorNode
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public RootNode()
        {

            //�ڵ�����
            title = ManagerSettingWindow.LanguageObj.RootNode;

            //�������˿�
            var outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(Edge));
            outputContainer.Add(outputPort);

            //��ǰʹ�õ��ļ�
            textObj = new ObjectField();
            textObj.objectType = typeof(TextStructrueFile);
            textObj.allowSceneObjects = false;
            textObj.RegisterValueChangedCallback(OnTextObjVeluaChanged);
            textObj.value = null;
            //VisualElement visualElement = new VisualElement();
            //visualElement.contentContainer.Add(textObj);
            //mainContainer.Insert(1, visualElement);
            //visualElement.AddToClassList("Content");
            this.capabilities -= Capabilities.Deletable;
            RefreshExpandedState();
            this.AddToClassList("RootNode");
        }

        public void OnTextObjVeluaChanged(ChangeEvent<Object> changeEvent)
        {
            //title = _textObj.value.name;
        }
    }
}
