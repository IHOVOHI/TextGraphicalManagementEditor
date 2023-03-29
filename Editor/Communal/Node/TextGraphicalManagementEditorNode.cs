using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor { 
    /// <summary>
    /// ���·�װ�Ľڵ㸸��
    /// </summary>
    public class TextGraphicalManagementEditorNode : Node
    {
        /// <summary>
        /// ��ǰʹ�õ��ļ�����
        /// </summary>
        public ObjectField textObj;

        /// <summary>
        /// ��ǰ�ڵ������ӿ�����
        /// </summary>
        public VisualElement InputContainer {
            get {
                if (_inputContainer == null)
                {
                    _inputContainer = new VisualElement();
                    _inputContainer.AddToClassList("PortContainer");
                }
                return _inputContainer;
            }
        }
        private VisualElement _inputContainer;

        public TextGraphicalManagementEditorNode()
        {
            outputContainer.AddToClassList("PortContainer");
        }

        /// <summary>
        /// �Ͽ����ж˿ڵ�����
        /// </summary>
        public void DisconnectAll(){
            for (int i = 0; i < outputContainer.childCount; i++)
            {
                Port port = this.outputContainer[i] as Port;
                port.DisconnectAll();
            }
            for (int i = 0; i < InputContainer.childCount; i++)
            {
                Port inputPort = this.InputContainer[i] as Port;
                inputPort.DisconnectAll();
            }
        }
    }
}
