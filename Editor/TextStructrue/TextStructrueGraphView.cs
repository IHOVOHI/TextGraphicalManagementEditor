using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;
using UnityEditor;

namespace TextGraphicalManagementEditor { 
    public class TextStructrueGraphView : TextGraphicalManagementVisualElement
    {
        
        /// <summary>
        /// ��ǰ����ʹ�õ��ı��ṹ�ļ�
        /// </summary>
        TextStructrueFile thisTextStructureFile;
        /// <summary>
        /// ��ǰ�����еĽڵ�
        /// </summary>
        List<Node> thisNodes;

        RootNode rootNode;

        VisualElement nowInspector;


        /// <summary>
        /// �������
        /// </summary>
        private Blackboard inspector;
        public ScrollView inspectorContainerVisualElemnt;
        /// <summary>
        /// С��ͼ���
        /// </summary>
        private MiniMap miniMap;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="window">��ǰ�Ĵ���</param>
        /// <param name="textStructureFile">��ǰ����ʹ�õ��ı��ṹ�ļ�</param>
        public TextStructrueGraphView(TextGraphicalManagementEditorWindow window , TextStructrueFile textStructureFile) {
            //���浱ǰ�Ĵ���
            this.window = window;
            //���浱ǰ����ʹ�õ��ı��ṹ�ļ�
            this.thisTextStructureFile = textStructureFile;
            //����ǰGraphView�������ǰ����
            this.StretchToParentSize();

            SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());

            this.AddManipulator(new SelectionDragger());

            this.AddManipulator(new RectangleSelector());


            //����Ҽ��˵�
            var menuWindow = ScriptableObject.CreateInstance<TextStructrueSearchMenuWindowProvider>();
            menuWindow.OnTextStructrueMenuWindowProviderDelegate = OnSearchTreeEntry;

            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindow);
            };

            window.Save = Save;
            window.SaveAs = SaveAs;
            window.Initialize = Initialze;


            //�����ӿ�λ��
            this.viewTransform.position = window.position.size * 0.5f;
        
            if (textStructureFile != null)
            {
                StateFile();
            }
            else
            {
                rootNode = new RootNode();
                AddElement(rootNode);
            }

            nowCheckedNode = new List<TextGraphicalManagementEditorNode>();

            miniMap = new MiniMap();
            Rect miniMapRect = new Rect(1100, 20, 200, 200);
            miniMap.SetPosition(miniMapRect);
            this.contentContainer.Add(miniMap);

            inspector = new Blackboard();
            inspector.title = ManagerSettingWindow.LanguageObj.Inspector;
            inspector.subTitle = "";
            Rect rect = new Rect(0, 20, 300, 600);
            inspector.SetPosition(rect);
            this.contentContainer.Add(inspector);
            inspectorContainerVisualElemnt = new ScrollView();
            inspector.contentContainer.Add(inspectorContainerVisualElemnt);

            styleSheets.Add(Resources.Load<StyleSheet>("Uss/TextStructrueGraphView"));
        }

        /// <summary>
        /// ʹ���Ҽ��˵����ɽڵ�
        /// </summary>
        /// <param name="SearchTreeEntry"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool OnSearchTreeEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            window.SetHasUnsavedChanges(true);
            var type = SearchTreeEntry.userData as Type;
            Node node = Activator.CreateInstance(type) as Node;
            //���ýڵ�λ�� �������Ļ�ϵĵ��λ�� - ����λ�� - �ӿ�λ�� / ���ű���
            node.SetPosition(new Rect((context.screenMousePosition - (Vector2)this.viewTransform.position - window.position.position) / this.scale, new Vector2()));
            AddElement(node);
            return true;
        }

        /// <summary>
        /// ��д�ڵ������߼�
        /// </summary>
        /// <param name="startPort"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> returnPort = new List<Port>();

            foreach (var port in ports.ToList()) {
                if (startPort.node == port.node|| startPort.direction == port.direction || startPort .portType != port.portType)
                {
                    continue;
                }
                returnPort.Add(port);
            }
            return returnPort;

        }



        /// <summary>
        /// д��ڵ��������
        /// </summary>
        public override void SetNodeInspector()
        {
            if (nowInspector != null)
            {
                inspectorContainerVisualElemnt.Remove(nowInspector);
                nowInspector = null;
            }
            if (nowCheckedNode.Count == 1)
            {
                switch (nowCheckedNode[0].GetType().ToString())
                {
                    case "TextGraphicalManagementEditor.DialogueNode":
                        DialogueInspector DialogueInspector = new DialogueInspector(nowCheckedNode[0] as DialogueNode);
                        inspectorContainerVisualElemnt.Add(DialogueInspector);
                        nowInspector = DialogueInspector;
                        nowInspector.SetEnabled(true);
                        break;

                    case "TextGraphicalManagementEditor.BranchNode":
                        BranchInspector BranchInspector = new BranchInspector(nowCheckedNode[0] as BranchNode);
                        inspectorContainerVisualElemnt.Add(BranchInspector);
                        nowInspector = BranchInspector;
                        nowInspector.SetEnabled(true);
                        break;

                    case "TextGraphicalManagementEditor.AutomaticBranchNode":
                        AutomaticBranchInspector AutomaticBranchInspector = new AutomaticBranchInspector(nowCheckedNode[0] as AutomaticBranchNode);
                        inspectorContainerVisualElemnt.Add(AutomaticBranchInspector);
                        nowInspector = AutomaticBranchInspector;
                        nowInspector.SetEnabled(true);
                        break;
                    case "TextGraphicalManagementEditor.DialogueTextNode":
                        DialogueTextInspector visualElement = new DialogueTextInspector(nowCheckedNode[0] as DialogueTextNode);
                        inspectorContainerVisualElemnt.Add(visualElement);
                        nowInspector = visualElement;
                        nowInspector.SetEnabled(true);
                        break;
                }
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        public void Save() {
            if (thisTextStructureFile == null)
            {
                SaveAs();
            }
            else {
                thisTextStructureFile.Position = viewTransform.position;
                thisTextStructureFile.nodeDatas = new SerialiizationTextStructrueNode();
                thisNodes = new List<Node>();
                this.nodes.ForEach(NodeSave);
                this.edges.ForEach(EdgeSave);
                EditorUtility.SetDirty(thisTextStructureFile);
            }
        }

        /// <summary>
        /// ���浽
        /// </summary>
        public void SaveAs() {
            String SaveUrl = EditorUtility.SaveFilePanelInProject("���浽", "new Text Structrue File", "asset", "������Ҫ������ļ���");
            TextStructrueFile textStructrueFile = ScriptableObject.CreateInstance<TextStructrueFile>();
            if (SaveUrl == ""|| SaveUrl == null)
            {
                return;
            }
            AssetDatabase.CreateAsset(textStructrueFile, SaveUrl);
            AssetDatabase.SaveAssets();
            thisTextStructureFile = textStructrueFile;
            Save();
        }

        public void Initialze() {
            Debug.Log("��ʽ��");
        }

        /// <summary>
        /// ��ʼ���ļ�
        /// </summary>
        public void StateFile() {
            //��ǰ�ĵ��ṹ����
            if (thisTextStructureFile != null)
            {
                //��սڵ㻺��
                thisNodes = new List<Node>();

                if (thisTextStructureFile.Position != Vector3.zero)
                {
                    this.viewTransform.position = thisTextStructureFile.Position;
                }

                if (thisTextStructureFile.nodeDatas.list.Count<1)
                {
                    RootNode root = new RootNode();
                    root.textObj.value = thisTextStructureFile;
                    AddElement(root);
                }
                //���ڵ���ӵ�GraphView��
                for (int i = 0; i < thisTextStructureFile.nodeDatas.list.Count; i++)
                {
                    var type = GetNodeType(thisTextStructureFile.nodeDatas.list[i].NodeType);
                    Node node = Activator.CreateInstance(type) as Node;
                    if (type == typeof(RootNode))
                    {
                        rootNode = node as RootNode;
                        rootNode.textObj.value = thisTextStructureFile;
                    }
                    node.SetPosition(thisTextStructureFile.nodeDatas.list[i].NodePosition);
                    TextGraphicalManagementEditorNode editorNode = (TextGraphicalManagementEditorNode)node;
                    if (thisTextStructureFile.nodeDatas.list[i].NodeFile != null)
                    {
                        editorNode.textObj.value = thisTextStructureFile.nodeDatas.list[i].NodeFile;
                    }
                    node = StateNodeFile(thisTextStructureFile.nodeDatas.list[i], node);
                    AddElement(node);
                    thisNodes.Add(node);
                }
                //���ڵ���������
                for (int i = 0; i < thisTextStructureFile.nodeDatas.list.Count; i++)
                {
                    for (int j = 0; j < thisTextStructureFile.nodeDatas.list[i].TrailingNodes.Count; j++)
                    {
                        if (thisTextStructureFile.nodeDatas.list[i].TrailingNodes[j] >= 0)
                        {
                            Port port = thisNodes[i].outputContainer[j] as Port;
                            TextGraphicalManagementEditorNode textNode = (TextGraphicalManagementEditorNode)thisNodes[thisTextStructureFile.nodeDatas.list[i].TrailingNodes[j]];
                            if (textNode.InputContainer.childCount > 0)
                            {
                                Port inputPort = textNode.InputContainer[0] as Port;
                                AddElement(port.ConnectTo(inputPort));
                            }
                        }
                    }
                }
            }
        }

        //���߱���
        public void EdgeSave(Edge edge) {
            int inputNodeId = thisNodes.IndexOf(edge.output.node);
            int inputNodePortId = thisNodes[inputNodeId].outputContainer.IndexOf(edge.output);
            thisTextStructureFile.nodeDatas.list[inputNodeId].TrailingNodes[inputNodePortId] = thisNodes.IndexOf(edge.input.node);
        }

        //�ڵ㱣��
        public void NodeSave(Node node) {
            NodeData nodeData = new NodeData();
            nodeData.NodeId = thisTextStructureFile.nodeDatas.list.Count;
            nodeData.NodeType = node.GetType().ToString();
            nodeData.NodePosition = node.GetPosition();
            nodeData.TrailingNodes = new List<int>();
            for (int i = 0; i < node.outputContainer.childCount; i++)
            {
                nodeData.TrailingNodes.Add(-1);
            }
            TextGraphicalManagementEditorNode editorNode = (TextGraphicalManagementEditorNode)node;
            if (editorNode.textObj != null)
            {
                nodeData.NodeFile = (TextFile)editorNode.textObj.value;
            }
            SaveNodeFile(nodeData,node);
            thisNodes.Add(node);
            //GetNowLinkNode();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        ///// <summary>
        ///// ��ȡ��ǰ�Ӹ��ڵ�������ӵ����нڵ�
        ///// </summary>
        ///// <returns></returns>
        //List<Node> GetNowLinkNode()
        //{
        //    List<Node> saveNode = new List<Node>();
        //    Node node = rootNode;
        //    Edge edge;
        //    saveNode.Add(node);
        //    for (int i = 0; i < saveNode.Count; i++)
        //    {
        //        for (int j = 0; j < node.outputContainer.childCount; j++)
        //        {
        //            IEnumerator<Edge> ie = (node.outputContainer[j] as Port).connections.GetEnumerator();
        //            Debug.Log(ie);
        //            edge = null;
        //            while (ie.MoveNext())
        //            {
        //                edge = ie.Current;
        //                if (edge != null && edge.input.node != null)
        //                {
        //                    saveNode.Add(edge.input.node);
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //            ie.Reset();
        //        }
        //    }
            
        //    Debug.Log(saveNode.Count);
        //    return saveNode;
        //}

        //֮��ɳ����Ż� ��ȡ�ڵ�����
        public Type GetNodeType( string TypeName) { 
            switch (TypeName)
            {
                case "TextGraphicalManagementEditor.RootNode": return typeof(RootNode); 
                case "TextGraphicalManagementEditor.DialogueNode": return typeof(DialogueNode);
                case "TextGraphicalManagementEditor.AutomaticBranchNode": return typeof(AutomaticBranchNode);
                case "TextGraphicalManagementEditor.BranchNode": return typeof(BranchNode);
                case "TextGraphicalManagementEditor.SwitchNode": return typeof(SwitchNode);
                case "TextGraphicalManagementEditor.DialogueTextNode": return typeof(DialogueTextNode);
            }
            Debug.Log("�������ƴ����û�и����ͣ��������ƣ�" + TypeName);
            return null;
        }

        /// <summary>
        /// ����ڵ��ļ�����
        /// </summary>
        /// <param name="nodeData">�ڵ�����</param>
        /// <param name="node">��ǰ�ڵ�</param>
        public void SaveNodeFile(NodeData nodeData, Node node) {
            switch (nodeData.NodeType)
            {
                case "TextGraphicalManagementEditor.RootNode":
                    thisTextStructureFile.roodId = thisTextStructureFile.nodeDatas.list.Count;
                    break;
                case "TextGraphicalManagementEditor.BranchNode":
                    BranchNode branchNode = (BranchNode)node;
                    BranchNodeData branchNodeData = new BranchNodeData(nodeData);
                    branchNodeData.Initialize();
                    for (int i = 0; i < branchNode.OptionDatas.Count; i++)
                    {
                        branchNodeData.branchName.Add(branchNode.OptionDatas[i].optionName);
                        branchNodeData.optionDatas.Add(branchNode.OptionDatas[i].optionDataValue);
                    }
                    nodeData = branchNodeData;
                    break;
                case "TextGraphicalManagementEditor.AutomaticBranchNode":
                    AutomaticBranchNode automaticBranchNode = (AutomaticBranchNode)node;
                    AutomaticBranchNodeData automaticBranchNodeDate = new AutomaticBranchNodeData(nodeData)  ;
                    automaticBranchNodeDate.compare = ((int)automaticBranchNode.compareValue);
                    automaticBranchNodeDate.Initialize();
                    for (int i = 0; i < automaticBranchNode.AutomaticOptionData.Count; i++)
                    {
                        automaticBranchNodeDate.branchName.Add(automaticBranchNode.AutomaticOptionData[i].optionName);
                        automaticBranchNodeDate.branchValue.Add(automaticBranchNode.AutomaticOptionData[i].optionValue);
                    }
                    nodeData = automaticBranchNodeDate;
                    break;

                case "TextGraphicalManagementEditor.DialogueTextNode":
                    DialogueTextNode dialogueTextNode = (DialogueTextNode)node;
                    DialogueDate date = new DialogueDate(nodeData);
                    date.name = dialogueTextNode.dialogueName;
                    date.time = dialogueTextNode.time;
                    date.scene = dialogueTextNode.scene;
                    date.content = dialogueTextNode.content;
                    date.actions = dialogueTextNode.dialogueActions;
                    date.savePosition = dialogueTextNode.GetPosition();
                    nodeData = date;
                    break;
                   
            }
            thisTextStructureFile.nodeDatas.list.Add(nodeData);
        }

        /// <summary>
        /// ��ȡ�ڵ��ļ����� ��ͬ�ڵ�Ĳ����ȡ������
        /// </summary>
        /// <param name="nodeData">�ڵ�����</param>
        /// <param name="node">��ǰ�ڵ�</param>
        /// <returns>���ĺ�Ľڵ�</returns>
        public Node StateNodeFile(NodeData nodeData, Node node) {
            switch (nodeData.NodeType) {
                case "TextGraphicalManagementEditor.BranchNode":
                    BranchNode branchNode = (BranchNode)node;
                    BranchNodeData branchNodeData = (BranchNodeData)nodeData;
                    for (int i = 0; i < nodeData.TrailingNodes.Count; i++)
                    {
                        if (i>0)
                        {
                            branchNode.AddOutPutPort();
                        }
                        branchNode.OptionDatas[i].optionName = branchNodeData.branchName[i];
                        branchNode.OptionDatas[i].optionDataValue = branchNodeData.optionDatas[i];
                    }
                    break;

                case "TextGraphicalManagementEditor.AutomaticBranchNode":
                    AutomaticBranchNode automaticBranchNode = (AutomaticBranchNode)node;
                    AutomaticBranchNodeData automaticBranchNodeData = (AutomaticBranchNodeData)nodeData;
                    automaticBranchNode.compareValue = (AutomaticBranchNode.compare)automaticBranchNodeData.compare;
                    for (int i = 0; i < nodeData.TrailingNodes.Count; i++)
                    {
                        if (i > 0)
                        {
                            automaticBranchNode.AddOutPutPort();
                        }
                        automaticBranchNode.AutomaticOptionData[i].optionName = automaticBranchNodeData.branchName[i];
                        automaticBranchNode.AutomaticOptionData[i].optionValue = automaticBranchNodeData.branchValue[i];
                    }
                    break;

                case "TextGraphicalManagementEditor.DialogueNode":
                    DialogueNode dialogueNode = (DialogueNode)node;
                    if (dialogueNode.textObj.value == null) break;
                    DialogueTextFile file = dialogueNode.textObj.value as DialogueTextFile;
                    dialogueNode.title = file.dialogueTitle;
                    break;

                case "TextGraphicalManagementEditor.DialogueTextNode":
                    DialogueTextNode textNode = (DialogueTextNode)node;
                    DialogueDate dialogueDate = (DialogueDate)nodeData;
                    textNode.dialogueName = dialogueDate.name;
                    textNode.content = dialogueDate.content;
                    textNode.time = dialogueDate.time;
                    textNode.scene = dialogueDate.scene;
                    textNode.DialogueNameField.value = dialogueDate.name;
                    textNode.DialogueContentField.value = dialogueDate.content;
                    if (dialogueDate.actions != null)
                    {
                        textNode.dialogueActions = dialogueDate.actions;
                    }
                    break;
            }
            return node;
        }

    }
}
