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
        /// ��ѡ�������
        /// </summary>
        public List<AutomaticOptionData> AutomaticOptionData;
        /// <summary>
        /// ���ݵ���ʾ����
        /// </summary>
        public VisualElement ContentContainer;
        /// <summary>
        /// ѡ�����ʾ����
        /// </summary>
        public VisualElement OptionDateContainer;

        private TextStructrueGraphView parentVisualElement;

        public AutomaticBranchNode()
        {
            //��ʼ������
            AutomaticOptionData = new List<AutomaticOptionData>();

            textObj = new ObjectField();
            textObj.objectType = typeof(DialogueTextFile);
            textObj.RegisterValueChangedCallback(OnTextObjVeluaChanged);

            
            mainContainer.Insert(0, InputContainer);

            this.AddToClassList("AutomaticBranchNode");
            

            //���ö˿ڼ�����
            title = ManagerSettingWindow.LanguageObj.AutomaticBranchNode;

            var inputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(Edge));
            InputContainer.Add(inputPort);
            AddOutPutPort();
            this.elementTypeColor = new Color(.6f, 0, 0, .8f);
            this.titleContainer.style.backgroundColor = new Color(.6f, 0, 0, .8f);

            RefreshExpandedState();
        }

        //ֵ�ı�ʱ���õ��¼�
        public void OnTextObjVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            title = textObj.value.name;
        }

       

        /// <summary>
        /// �������˿�
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
        /// ɾ����Ӧ������˿� ��ǰ�˿ڱ�����ʱ����ɾ��
        /// </summary>
        /// <param name="optionData">��Ҫɾ���Ķ˿�����</param>
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
    public class AutomaticOptionData
    {

        public string optionName = "";
        public float optionValue = 0;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="OptionParent">ѡ����ʾ�Ĳ㼶</param>
        /// <param name="RemoveAction">ɾ����ť�¼�</param>
        public AutomaticOptionData(VisualElement OptionParent, Action<AutomaticOptionData> RemoveAction)
        {

        }
    }
}