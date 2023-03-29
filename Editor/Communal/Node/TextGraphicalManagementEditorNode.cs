using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TextGraphicalManagementEditor { 
    /// <summary>
    /// 重新封装的节点父级
    /// </summary>
    public class TextGraphicalManagementEditorNode : Node
    {
        /// <summary>
        /// 当前使用的文件物体
        /// </summary>
        public ObjectField textObj;

        /// <summary>
        /// 当前节点的输入接口容器
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
        /// 断开所有端口的连接
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
