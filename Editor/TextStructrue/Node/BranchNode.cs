using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    /// <summary>
    /// 分支节点 TextStructrue图形窗口中使用
    /// </summary>
    public class BranchNode : TextGraphicalManagementEditorNode
    {
        /// <summary>
        /// 各选项的数据
        /// </summary>
        public List<OptionData> OptionDatas;
        /// <summary>
        /// 内容的显示父级
        /// </summary>
        public VisualElement ContentContainer;
        /// <summary>
        /// 选项的显示父级
        /// </summary>
        public VisualElement OptionDateContainer;
        private TextStructrueGraphView parentVisualElement;

        public BranchNode()
        {
            //初始化变量
            OptionDatas = new List<OptionData>();

            //添加.Content层
            ContentContainer = new VisualElement(); ;
            ContentContainer.contentContainer.Add(textObj);
            OptionDateContainer = new VisualElement(); ;
            ContentContainer.contentContainer.Add(OptionDateContainer);

            //调整层级
            mainContainer.Insert(1, ContentContainer);
            mainContainer.Insert(0, InputContainer);
            ContentContainer.AddToClassList("Content");
            OptionDateContainer.AddToClassList("OptionDateContainer");

            ////添加 “增加” 按钮
            //Button AddPortButton = new Button();
            //AddPortButton.clicked += AddOutPutPort;
            //titleButtonContainer.Add(AddPortButton);

            this.AddToClassList("BranchNode");

            //设置端口及标题
            title = ManagerSettingWindow.LanguageObj.BranchNode;

            var inputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(Edge));
            InputContainer.Add(inputPort);
            AddOutPutPort();

            this.elementTypeColor = new Color(.6f, .5f, 0, .8f);
            this.titleContainer.style.backgroundColor = new Color(.6f, .5f, 0, .8f);

            RefreshExpandedState();
        }

        /// <summary>
        /// 添加输出端口
        /// </summary>
        public void AddOutPutPort()
        {
            AddOptionData();
        }

        public OptionData AddOptionData()
        {
            var outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(Edge));
            outputContainer.Add(outputPort);
            OptionData optionData = new OptionData(OptionDateContainer, RemoveOutPutPort);
            OptionDatas.Add(optionData);
            SetTitle();
            return optionData;
        }

        /// <summary>
        /// 删除对应的输出端口 当前端口被连接时不能删除
        /// </summary>
        /// <param name="optionData">需要删除的端口数据</param>
        public void RemoveOutPutPort(OptionData optionData)
        {
            int OptionDateID = OptionDatas.LastIndexOf(optionData);
            Port port = (Port)outputContainer[OptionDateID];
            if (!port.connected)
            {
                outputContainer.RemoveAt(OptionDateID);
                OptionDatas.RemoveAt(OptionDateID);
                OptionDateContainer.RemoveAt(OptionDateID);
            }
            else
            {
                Debug.Log(ManagerSettingWindow.LanguageObj.PleaseDisconnectBeforeDeleting);
            }
            SetTitle();
        }


        public void SetTitle() {
            for (int i = 0; i < OptionDatas.Count; i++)
            {
                OptionDatas[i].SetTitle(i);
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
    public class OptionData
    {
        public string optionName = "";
        /// <summary>
        /// 删除选项按钮
        /// </summary>
        public Button removeButton;
        public Label label;

        public OptionDataValue optionDataValue;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="OptionParent">选项显示的层级</param>
        /// <param name="RemoveAction">删除按钮事件</param>
        public OptionData(VisualElement OptionParent, Action<OptionData> RemoveAction)
        {
            //新建显示层级 并 赋予样式名称
            VisualElement visualElement = new VisualElement();
            visualElement.AddToClassList("OptionDate");
            VisualElement titleContainer = new VisualElement();
            titleContainer.AddToClassList("OptionTitle");

            //初始化各控件
            label = new Label();
            removeButton = new Button();
            SetTitle();

            optionDataValue = new OptionDataValue();
            //removeButton.clicked += () =>
            //{
            //    RemoveAction(this);
            //};

            //将控件添加到显示层级中
            titleContainer.Add(label);
            //titleContainer.Add(removeButton);
            visualElement.Add(titleContainer);
            OptionParent.Add(visualElement);

        }

        public void SetTitle(int id = 0 ) {
            label.text = ManagerSettingWindow.LanguageObj.Option + id ;
        }
    }


}