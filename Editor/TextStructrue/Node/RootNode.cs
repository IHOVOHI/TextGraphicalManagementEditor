using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    /// <summary>
    /// 根节点 保存当前使用的文本结构文件
    /// </summary>
    public class RootNode : TextGraphicalManagementEditorNode
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RootNode()
        {

            //节点名称
            title = ManagerSettingWindow.LanguageObj.RootNode;

            //添加输出端口
            var outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(Edge));
            outputContainer.Add(outputPort);

            //当前使用的文件
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
