using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor
{ 
    public class AutomaticBranchInspector : VisualElement
    {
        AutomaticBranchNode node;
        VisualElement content;
        List<AutomaticOptionDataInspector> optionDataInspectors;

        EnumField dropdownField;


        public AutomaticBranchInspector(AutomaticBranchNode node)
        {
            this.AddToClassList("AutomaticBranchInspector");
            optionDataInspectors = new List<AutomaticOptionDataInspector>();
            this.node = node;

            dropdownField = new EnumField();
            dropdownField.label = "比较方式:";
            dropdownField.Init(node.compareValue);
            dropdownField.value = node.compareValue;
            dropdownField.RegisterValueChangedCallback(OnOptionNameVeluaChanged);

            Button button = new Button();
            button.text = "添加分支";
            button.clicked += AddOptionData;
            content = new VisualElement();
            content.AddToClassList("AutomaticBranchInspectorContent");
            for (int i = 0; i < node.AutomaticOptionData.Count; i++)
            {
                var inspector = new AutomaticOptionDataInspector(node.AutomaticOptionData[i], RemoveOutPutPort);
                content.Add(inspector);
                optionDataInspectors.Add(inspector);
                inspector.optionDataField.value = node.AutomaticOptionData[i].optionName;
                inspector.optionValueField.value = node.AutomaticOptionData[i].optionValue.ToString();
            }
            Add(dropdownField);
            Add(button);
            Add(content);
            SetTitle();
        }

        public void AddOptionData()
        {
            AutomaticOptionData optionData = node.AddOutPutPort();
            var inspector = new AutomaticOptionDataInspector(optionData, RemoveOutPutPort);
            content.Add(inspector);
            optionDataInspectors.Add(inspector);
            SetTitle();
        }

        public void RemoveOutPutPort(AutomaticOptionDataInspector optionData)
        {
            int id = node.AutomaticOptionData.Count;
            node.RemoveOutPutPort(optionData.optionData);
            if (id != node.AutomaticOptionData.Count)
            {
                content.Remove(optionData);
            }
            SetTitle();
        }

        public void SetTitle()
        {
            for (int i = 0; i < optionDataInspectors.Count; i++)
            {
                optionDataInspectors[i].label.text =  ManagerSettingWindow.LanguageObj.Option + i;
            }
        }

        private void OnOptionNameVeluaChanged(ChangeEvent<Enum> evt)
        {
            node.compareValue = (AutomaticBranchNode.compare)dropdownField.value;
        }
    }


    public class AutomaticOptionDataInspector : VisualElement
    {
        public AutomaticOptionData optionData;
        public Label label;
        public TextField optionDataField;
        public TextField optionValueField;

        public AutomaticOptionDataInspector(AutomaticOptionData optionData, Action<AutomaticOptionDataInspector> RemoveAction)
        {
            this.AddToClassList("AutomaticOptionDataInspector");
            this.optionData = optionData;
            VisualElement title = new VisualElement();
            label = new Label("选项");

            Button button = new Button();
            button.text = "删除";
            button.clicked += () =>
            {
                RemoveAction(this);
            };
            title.Add(label);
            title.Add(button);
            title.AddToClassList("row");

            VisualElement optionDataVisual = new VisualElement();
            Label optionDataLabel = new Label("参数名称:");
            optionDataField = new TextField();
            optionDataField.RegisterValueChangedCallback(OnOptionNameVeluaChanged);
            optionDataVisual.Add(optionDataLabel);
            optionDataVisual.Add(optionDataField);
            optionDataVisual.AddToClassList("row");


            VisualElement optionValueVisual = new VisualElement();
            Label optionValueLabel = new Label("数值:");
            optionValueField = new TextField();
            optionValueField.RegisterValueChangedCallback(OnOptionVeluaChanged);
            optionValueVisual.Add(optionValueLabel);
            optionValueVisual.Add(optionValueField);
            optionValueVisual.AddToClassList("row");


            this.Add(title);
            this.Add(optionDataVisual);
            this.Add(optionValueVisual);
        }

        private void OnOptionNameVeluaChanged(ChangeEvent<string> evt)
        {
            optionData.optionName = optionDataField.value;
        }

        private void OnOptionVeluaChanged(ChangeEvent<string> evt)
        {
            optionData.optionValue = float.Parse(optionValueField.value);
        }

    }
}