using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace TextGraphicalManagementEditor {

    public class AutomaticBranchNode : TextGraphicalManagementEditorNode
    {
        public enum compare {
            Greater = 0,
            Less = 1,
            Equal = 2,
            NotEqual = 3,
            GreaterOrEqual = 4,
            LessOrEqual = 5,
        }

        public compare compareValue;

        /// <summary>
        /// 各选项的数据
        /// </summary>
        public List<AutomaticOptionData> AutomaticOptionData;
        /// <summary>
        /// 内容的显示父级
        /// </summary>
        public VisualElement ContentContainer;
        /// <summary>
        /// 选项的显示父级
        /// </summary>
        public VisualElement OptionDateContainer;

        private TextStructrueGraphView parentVisualElement;

        public AutomaticBranchNode()
        {
            //初始化变量
            AutomaticOptionData = new List<AutomaticOptionData>();

            textObj = new ObjectField();
            textObj.objectType = typeof(DialogueTextFile);
            textObj.RegisterValueChangedCallback(OnTextObjVeluaChanged);

            
            mainContainer.Insert(0, InputContainer);

            this.AddToClassList("AutomaticBranchNode");
            

            //设置端口及标题
            title = ManagerSettingWindow.LanguageObj.AutomaticBranchNode;

            var inputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(Edge));
            InputContainer.Add(inputPort);
            AddOutPutPort();
            this.elementTypeColor = new Color(.6f, 0, 0, .8f);
            this.titleContainer.style.backgroundColor = new Color(.6f, 0, 0, .8f);

            RefreshExpandedState();
        }

        //值改变时调用的事件
        public void OnTextObjVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            title = textObj.value.name;
        }

       

        /// <summary>
        /// 添加输出端口
        /// </summary>
        public AutomaticOptionData AddOutPutPort()
        {
            var outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(Edge));
            outputContainer.Add(outputPort);
            AutomaticOptionData optionData = new AutomaticOptionData(OptionDateContainer, RemoveOutPutPort);
            AutomaticOptionData.Add(optionData);
            return optionData;
        }

        /// <summary>
        /// 删除对应的输出端口 当前端口被连接时不能删除
        /// </summary>
        /// <param name="optionData">需要删除的端口数据</param>
        public void RemoveOutPutPort(AutomaticOptionData optionData)
        {
            int OptionDateID = AutomaticOptionData.LastIndexOf(optionData);
            Port port = (Port)outputContainer[OptionDateID];
            if (!port.connected)
            {
                outputContainer.RemoveAt(OptionDateID);
                AutomaticOptionData.RemoveAt(OptionDateID);
                //OptionDateContainer.RemoveAt(OptionDateID);
            }
            else
            {
                Debug.Log(ManagerSettingWindow.LanguageObj.PleaseDisconnectBeforeDeleting);
            }

        }

        /// <summary>
        /// 节点选中时调用
        /// </summary>
        public override void OnSelected()
        {
            if (parentVisualElement == null)
            {
                parentVisualElement = this.GetFirstOfType<TextStructrueGraphView>();
            }
            parentVisualElement.AddNowCheckedNode(this);
        }

        /// <summary>
        /// 节点取消选中时调用
        /// </summary>
        public override void OnUnselected()
        {
            parentVisualElement.RemoveNowCheckedNode(this);
        }
    }

    /// <summary>
    /// 选项数据控件类
    /// </summary>
    public class AutomaticOptionData
    {

        public string optionName = "";
        public float optionValue = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OptionParent">选项显示的层级</param>
        /// <param name="RemoveAction">删除按钮事件</param>
        public AutomaticOptionData(VisualElement OptionParent, Action<AutomaticOptionData> RemoveAction)
        {

        }
    }
}