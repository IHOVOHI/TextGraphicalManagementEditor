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
    /// ��֧�ڵ� TextStructrueͼ�δ�����ʹ��
    /// </summary>
    public class BranchNode : TextGraphicalManagementEditorNode
    {
        /// <summary>
        /// ��ѡ�������
        /// </summary>
        public List<OptionData> OptionDatas;
        /// <summary>
        /// ���ݵ���ʾ����
        /// </summary>
        public VisualElement ContentContainer;
        /// <summary>
        /// ѡ�����ʾ����
        /// </summary>
        public VisualElement OptionDateContainer;
        private TextStructrueGraphView parentVisualElement;

        public BranchNode()
        {
            //��ʼ������
            OptionDatas = new List<OptionData>();

            //���.Content��
            ContentContainer = new VisualElement(); ;
            ContentContainer.contentContainer.Add(textObj);
            OptionDateContainer = new VisualElement(); ;
            ContentContainer.contentContainer.Add(OptionDateContainer);

            //�����㼶
            mainContainer.Insert(1, ContentContainer);
            mainContainer.Insert(0, InputContainer);
            ContentContainer.AddToClassList("Content");
            OptionDateContainer.AddToClassList("OptionDateContainer");

            ////��� �����ӡ� ��ť
            //Button AddPortButton = new Button();
            //AddPortButton.clicked += AddOutPutPort;
            //titleButtonContainer.Add(AddPortButton);

            this.AddToClassList("BranchNode");

            //���ö˿ڼ�����
            title = ManagerSettingWindow.LanguageObj.BranchNode;

            var inputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(Edge));
            InputContainer.Add(inputPort);
            AddOutPutPort();

            this.elementTypeColor = new Color(.6f, .5f, 0, .8f);
            this.titleContainer.style.backgroundColor = new Color(.6f, .5f, 0, .8f);

            RefreshExpandedState();
        }

        /// <summary>
        /// �������˿�
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
        /// ɾ����Ӧ������˿� ��ǰ�˿ڱ�����ʱ����ɾ��
        /// </summary>
        /// <param name="optionData">��Ҫɾ���Ķ˿�����</param>
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
        /// �ڵ�ѡ��ʱ����
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
        /// �ڵ�ȡ��ѡ��ʱ����
        /// </summary>
        public override void OnUnselected()
        {
            parentVisualElement.RemoveNowCheckedNode(this);
        }
    }

    /// <summary>
    /// ѡ�����ݿؼ���
    /// </summary>
    public class OptionData
    {
        public string optionName = "";
        /// <summary>
        /// ɾ��ѡ�ť
        /// </summary>
        public Button removeButton;
        public Label label;

        public OptionDataValue optionDataValue;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="OptionParent">ѡ����ʾ�Ĳ㼶</param>
        /// <param name="RemoveAction">ɾ����ť�¼�</param>
        public OptionData(VisualElement OptionParent, Action<OptionData> RemoveAction)
        {
            //�½���ʾ�㼶 �� ������ʽ����
            VisualElement visualElement = new VisualElement();
            visualElement.AddToClassList("OptionDate");
            VisualElement titleContainer = new VisualElement();
            titleContainer.AddToClassList("OptionTitle");

            //��ʼ�����ؼ�
            label = new Label();
            removeButton = new Button();
            SetTitle();

            optionDataValue = new OptionDataValue();
            //removeButton.clicked += () =>
            //{
            //    RemoveAction(this);
            //};

            //���ؼ���ӵ���ʾ�㼶��
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