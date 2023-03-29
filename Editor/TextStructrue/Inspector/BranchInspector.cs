using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{
    public class BranchInspector : VisualElement
    {
        BranchNode node;
        VisualElement content;
        List<OptionDataInspector> optionDataInspectors;


        public BranchInspector(BranchNode node)
        {
            this.AddToClassList("BranchInspector");
            optionDataInspectors = new List<OptionDataInspector>();
            this.node = node;
            Button button = new Button();
            button.text = "添加分支";
            button.clicked += AddOptionData;
            content = new VisualElement();
            content.AddToClassList("BranchInspectorContent");
            for (int i = 0; i < node.OptionDatas.Count; i++)
            {
                var inspector = new OptionDataInspector(node.OptionDatas[i], RemoveOutPutPort);
                content.Add(inspector);
                inspector.label.text = node.OptionDatas[i].label.text;
                inspector.OptionDataField.value = node.OptionDatas[i].optionName;
            }
            Add(button);
            Add(content);
        }

        public void AddOptionData() {
            OptionData optionData = node.AddOptionData();
            var inspector = new OptionDataInspector(optionData, RemoveOutPutPort);
            content.Add(inspector);
            optionDataInspectors.Add(inspector);
            SetTitle();
        }

        public void RemoveOutPutPort(OptionDataInspector optionData)
        {
            int id = node.OptionDatas.Count;
            node.RemoveOutPutPort(optionData.optionData);
            if (id != node.OptionDatas.Count)
            {
                content.Remove(optionData);
            }
            SetTitle();
        }

        public void SetTitle()
        {
            for (int i = 0; i < optionDataInspectors.Count; i++)
            {
                optionDataInspectors[i].label.text = optionDataInspectors[i].optionData.label.text;
    }
        }

    }

    /// <summary>
    /// 代表单个选项
    /// </summary>
    public class OptionDataInspector : VisualElement
    {
        public OptionData optionData;
        public Label label;
        public TextField OptionDataField;
        public VisualElement DataValueVisElement;

        public OptionDataInspector(OptionData optionData, Action<OptionDataInspector> RemoveAction)
        {
            this.AddToClassList("OptionDataInspector");

            this.optionData = optionData;

            VisualElement visualElement = new VisualElement();
            visualElement.AddToClassList("row");
            DataValueVisElement = new VisualElement();

            label = new Label("选项");

            OptionDataField = new TextField();
            OptionDataField.RegisterValueChangedCallback(OnOptionDataValueChanged);

            Button button = new Button();
            button.text = "删除";
            button.clicked += () =>
            {
                RemoveAction(this);
            };
            Button AddButton = new Button();
            AddButton.text = "添加数据";
            AddButton.clicked += () =>
            {
                AddOptionDataValueInspector();
            };

            //读取时
            for (int i = 0; i < optionData.optionDataValue.DataName.Count; i++)
            {
                var dataValue = new OptionDataValueInspector(optionData.optionDataValue, i, RemoveDataValue);
                DataValueVisElement.Add(dataValue);
            }

            visualElement.Add(label);
            visualElement.Add(OptionDataField);
            visualElement.Add(button);
            visualElement.Add(AddButton);

            this.Add(visualElement);
            this.Add(DataValueVisElement);
        }

        private void AddOptionDataValueInspector() {
            optionData.optionDataValue.DataName.Add("");
            optionData.optionDataValue.DataValue.Add("");
            var inspector = new OptionDataValueInspector(optionData.optionDataValue, optionData.optionDataValue.DataName.Count-1, RemoveDataValue);
            DataValueVisElement.Add(inspector);
        }

        private void OnOptionDataValueChanged(ChangeEvent<string> evt)
        {
            optionData.optionName = OptionDataField.value;
        }

        public void RemoveDataValue(OptionDataValueInspector optionData)
        {
            int id = optionData.optionDataValue.DataName.LastIndexOf(optionData.DataName);
            optionData.optionDataValue.DataName.RemoveAt(id);
            optionData.optionDataValue.DataValue.RemoveAt(id);
            DataValueVisElement.Remove(optionData);
        }
    }

    public class OptionDataValueInspector : VisualElement
    {
        public OptionDataValue optionDataValue;
        public string DataName;
        public string DataValue;
        public Label label;

        public TextField DataNameField;
        public TextField DataValueField;

        public OptionDataValueInspector(OptionDataValue optionDataValue ,int id, Action<OptionDataValueInspector> RemoveAction)
        {
            this.AddToClassList("OptionDataValueInspector");
            this.optionDataValue = optionDataValue;

            VisualElement nameElement = new VisualElement();
            nameElement.AddToClassList("row");
            Label nameLabel = new Label("名称：");
            DataNameField = new TextField();
            DataNameField.value = DataName = optionDataValue.DataName[id];
            DataNameField.RegisterValueChangedCallback(OnDataNameChanged);
            nameElement.Add(nameLabel);
            nameElement.Add(DataNameField);


            VisualElement valueElement = new VisualElement();
            valueElement.AddToClassList("row");
            Label valueLabel = new Label("数值：");
            DataValueField = new TextField();
            DataValueField.value = optionDataValue.DataValue[id];
            DataValueField.RegisterValueChangedCallback(OnDataVeluaChanged);
            valueElement.Add(valueLabel);
            valueElement.Add(DataValueField);

            Button button = new Button();
            button.text = "删除数据";
            button.clicked += () =>
            {
                RemoveAction(this);
            };
            valueElement.Add(button);

            this.Add(nameElement);
            this.Add(valueElement);
        }

        private void OnDataNameChanged(ChangeEvent<string> evt)
        {
            optionDataValue.DataName[optionDataValue.DataName.LastIndexOf(DataName)] = DataNameField.value;
            DataName = DataNameField.value;
        }

        private void OnDataVeluaChanged(ChangeEvent<string> evt)
        {
            optionDataValue.DataValue[optionDataValue.DataName.LastIndexOf(DataName)] = DataValueField.value;
        }

    }
}