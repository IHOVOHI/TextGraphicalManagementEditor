using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor {

    public class DialogueStartNode : TextGraphicalManagementEditorNode
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DialogueStartNode()
        {

            //节点名称
            title = ManagerSettingWindow.LanguageObj.RootNode;

            //添加输出端口
            outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(Edge));
            outputContainer.Add(outputPort);

            //当前使用的文件
            textObj = new ObjectField();
            textObj.value = null;
            this.capabilities -= Capabilities.Deletable;
            RefreshExpandedState();
            this.AddToClassList("RootNode");
        }

        public Port outputPort;


        /// <summary>
        /// 节点选中时调用
        /// </summary>
        public override void OnSelected()
        {
            this.GetFirstOfType<DialogueVisualElement>().AddNowCheckedNode(this);
        }

        /// <summary>
        /// 节点取消选中时调用
        /// </summary>
        public override void OnUnselected()
        {
            this.GetFirstOfType<DialogueVisualElement>().RemoveNowCheckedNode(this);
        }
    }

}