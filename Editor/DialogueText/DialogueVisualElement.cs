using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ͼ�λ��ı�����༭��
/// </summary>
namespace TextGraphicalManagementEditor
{
    /// <summary>
    /// �Ի��ļ���VisuaElement�༭����
    /// </summary>
    public class DialogueVisualElement : TextGraphicalManagementVisualElement
    {
        /// <summary>
        /// ��ǰʹ�õĶԻ��ļ�
        /// </summary>
        private DialogueTextFile _textObj;
        public DialogueTextFile textObj {
            get {
                return _textObj;
            }
        }

        /// <summary>
        /// �Ի��ļ���Ӧ�������
        /// </summary>
        public ObjectField objectField;
        /// <summary>
        /// 
        /// </summary>
        public SerializationDictionary<string, Color> DialogueNameColor;
        VisualElement nameColorContentVisualElement;
        /// <summary>
        /// ��ǰ�Ի��ı��� ��ʱ�� ���ص� �����
        /// </summary>
        public TextField dialogueTitleField;
        public TextField dialogueTimeField;
        public TextField dialogueSceneField;
        /// <summary>
        /// �Ի����ݵļ���
        /// </summary>
        public List<DialogueDate> dialogueDateFields;
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
        /// ��ǰ�����еĽڵ�
        /// </summary>
        List<Node> thisNodes;
        /// <summary>
        /// ��ʼ���ڵ�
        /// </summary>
        DialogueStartNode rootNode;
        /// <summary>
        /// �ڵ�������
        /// </summary>
        float xInterval = 200;
        float yInterval = 175;

        VisualElement nowInspector;

        /// <summary>
        /// �Ի������������ĸ���
        /// </summary>
        VisualElement ContentContainer;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="window">��ǰ�Ĵ���</param>
        /// <param name="textStructureFile">��ǰ����ʹ�õ��ı��ṹ�ļ�</param>
        public DialogueVisualElement(TextGraphicalManagementEditorWindow window, DialogueTextFile textObj)
        {
            //���浱ǰ�Ĵ���
            this.window = window;
            //���浱ǰ����ʹ�õ��ı��ṹ�ļ�
            this._textObj = textObj;
            //����ǰGraphView�������ǰ����
            this.StretchToParentSize();

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());

            this.AddManipulator(new SelectionDragger());

            this.AddManipulator(new RectangleSelector());


            //����Ҽ��˵�
            var menuWindow = ScriptableObject.CreateInstance<DialogueSearchMenuWindowProvider>();
            menuWindow.OnDialogueMenuWindowProviderDelegate = OnSearchTreeEntry;

            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindow);
            };

            window.Save = Save;
            window.SaveAs = SaveAs;
            window.Initialize = Initialze;

            //�����ӿ�λ��
            this.viewTransform.position = window.position.size * 0.5f;

            rootNode = new DialogueStartNode();
            AddElement(rootNode);

            //if (this.textObj != null)
            //{
            //    StartFile();
            //}


            thisNodes = new List<Node>();
            //д�벼�ֿؼ�
            SetLayout();

            dialogueDateFields = new List<DialogueDate>();
            nowCheckedNode = new List<TextGraphicalManagementEditorNode>();

            Lode();

            styleSheets.Add(Resources.Load<StyleSheet>("Uss/DialogueVisualElement"));
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

            foreach (var port in ports.ToList())
            {
                if (startPort.node == port.node || startPort.direction == port.direction || startPort.portType != port.portType)
                {
                    continue;
                }
                returnPort.Add(port);
            }
            return returnPort;

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

        public void Lode() {
            if (textObj != null)
            {
                if (textObj.TextFile != null)
                {
                    objectField.value = textObj.TextFile;
                }
                if (textObj.dialogueTitle != null)
                {
                    dialogueTitleField.value = textObj.dialogueTitle;
                }
                if (textObj.dialogueTime != null)
                {
                    dialogueTimeField.value = textObj.dialogueTime;
                }
                if (textObj.dialogueScene != null)
                {
                    dialogueSceneField.value = textObj.dialogueScene;
                }
                if (textObj.nameColors != null)
                {
                    DialogueNameColor = textObj.nameColors;
                }
                else {
                    DialogueNameColor = new SerializationDictionary<string, Color>();
                }
                if (textObj.dialogueDates != null)
                {
                    SetNode(textObj.dialogueDates, rootNode.GetPosition(), true);
                }
            }
        }

        public void Save() {
            if (textObj == null)
            {
                SaveAs();
            }
            else
            {
                textObj.TextFile = objectField.value as TextAsset;
                textObj.dialogueTitle = dialogueTitleField.value;
                textObj.dialogueTime = dialogueTimeField.value;
                textObj.dialogueScene = dialogueSceneField.value;
                if (DialogueNameColor != null)
                {
                    textObj.nameColors = DialogueNameColor;
                }

                List<DialogueTextNode> saveNode = GetNowLinkNode();

                List<DialogueDate> nodeDate = new List<DialogueDate>();
                for (int i = 0; i < saveNode.Count; i++)
                {
                    DialogueDate date = new DialogueDate();
                    date.name = saveNode[i].dialogueName;
                    date.time = saveNode[i].time;
                    date.scene = saveNode[i].scene;
                    date.content = saveNode[i].content;
                    date.actions = saveNode[i].dialogueActions;
                    date.savePosition = saveNode[i].GetPosition();
                    nodeDate.Add(date);
                }

                textObj.dialogueDates = nodeDate;

                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(textObj);
            }
        }

        public void SaveAs() {
            String SaveUrl = EditorUtility.SaveFilePanelInProject("���浽", "new Dialogue Text File", "asset", "�����뱣����ļ���");
            if (SaveUrl == "") return;
            DialogueTextFile dialogueTextFile = ScriptableObject.CreateInstance<DialogueTextFile>();
            AssetDatabase.CreateAsset(dialogueTextFile, SaveUrl);
            AssetDatabase.SaveAssets();
            _textObj = dialogueTextFile;
            Save();
        }
        
        public void Initialze()
        {
            Debug.Log("��ʽ��");
        }

        /// <summary>
        /// ���ɶ���ڵ㲢����
        /// </summary>
        /// <param name="dialogueDate">�����õ�����</param>
        /// <param name="startPosition">��ʼλ��</param>
        void SetNode(List<DialogueDate> dialogueDate, Rect startPosition, bool isLinkRoodNode = false) {
            DialogueTextNode LastNode = null;
            for (int i = 0; i < dialogueDate.Count; i++)
            {
                DialogueTextNode textNode = SetNode(dialogueDate[i]);
                if (dialogueDate[i].savePosition != null)
                {
                    textNode.SetPosition(dialogueDate[i].savePosition);
                }
                else {
                    startPosition.y += yInterval;
                    textNode.SetPosition(startPosition);
                }

                if (i != 0)
                {
                    Port port = LastNode.outputContainer[0] as Port;
                    Port inputPort = textNode.InputContainer[0] as Port;
                    AddEdge(port, inputPort);
                }
                LastNode = textNode;
                if (i == 0 && isLinkRoodNode)
                {
                    AddEdge(rootNode.outputPort,textNode.inputPort);
                }
                thisNodes.Add(textNode);
            }
        }

        /// <summary>
        /// ���ɵ����ڵ�
        /// </summary>
        /// <param name="dialogueDate"></param>
        /// <returns></returns>
        DialogueTextNode SetNode(DialogueDate dialogueDate) {
            DialogueTextNode textNode = new DialogueTextNode();
            textNode.dialogueName = dialogueDate.name; 
            textNode.content = dialogueDate.content;
            textNode.time = dialogueDate.time;
            textNode.scene = dialogueDate.scene;
            textNode.DialogueNameField.value = dialogueDate.name;
            textNode.DialogueContentField.value = dialogueDate.content;
            textNode.SetTitleColor(GetNameColor(textNode.dialogueName));
            if (dialogueDate.actions != null)
            {
                textNode.dialogueActions = dialogueDate.actions;
            }
            AddElement(textNode);
            return textNode;
        }
        
        /// <summary>
        /// �������˿�֮���������
        /// </summary>
        /// <param name="outputPort"></param>
        /// <param name="inputPort"></param>
        void AddEdge(Port outputPort,Port inputPort) {
            Edge edge = outputPort.ConnectTo(inputPort);
            AddElement(edge);
        }

        /// <summary>
        /// ɾ����ǰͼ�е�����������
        /// </summary>
        void RemoveAllEdge() {
            var edge = edges;
            edge.ForEach((Edge edge1)=> {
                edge1.input.Disconnect(edge1);
                edge1.output.Disconnect(edge1);
                RemoveElement(edge1);  
            });
        }

        /// <summary>
        /// ��ȡ��ǰ�Ӹ��ڵ�������ӵ����нڵ�
        /// </summary>
        /// <returns></returns>
        List<DialogueTextNode> GetNowLinkNode() {
            List<DialogueTextNode> saveNode = new List<DialogueTextNode>();
            Node node = rootNode;
            Edge edge;
            while (node.outputContainer.childCount > 0)
            {
                IEnumerator<Edge> ie = (node.outputContainer[0] as Port).connections.GetEnumerator();
                edge = null;
                while (ie.MoveNext())
                {
                    edge = ie.Current;
                }
                ie.Reset();
                if (edge != null && edge.input.node != null)
                {
                    node = edge.input.node;
                    saveNode.Add(node as DialogueTextNode);
                }
                else
                {
                    break;
                }
            }
            return saveNode;
        }

        /// <summary>
        /// д��ڵ��������
        /// </summary>
         public override void SetNodeInspector() {
            if (nowInspector != null)
            {
                inspectorContainerVisualElemnt.Remove(nowInspector);
                nowInspector = null;
            }
            if (nowCheckedNode.Count == 1)
            {
                switch (nowCheckedNode[0].GetType().ToString()) {
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
        /// ��ȡ���ֶ�Ӧ����ɫ ��������Ϊ�����������ɫ�����
        /// </summary>
        /// <param name="dialogueName"></param>
        /// <returns></returns>
        public Color GetNameColor(string dialogueName) {
            if (dialogueName == null)
                return Color.white;
            if ( !DialogueNameColor.ToDictionary().ContainsKey(dialogueName) )
            {
                float h = UnityEngine.Random.Range(0f, 1f);
                float s = UnityEngine.Random.Range(0f, 1f);
                Color color = Color.HSVToRGB(h, s, 0.5f);
                DialogueNameColor.ToDictionary().Add(dialogueName, color);
            }
            return DialogueNameColor.ToDictionary()[dialogueName];
        }

        /// <summary>
        /// д��������岼�ֿؼ�
        /// </summary>
        void SetLayout() {
            miniMap = new MiniMap();
            Rect miniMapRect = new Rect(1100,20, 200, 200);
            miniMap.SetPosition(miniMapRect);
            this.contentContainer.Add(miniMap);

            inspector = new Blackboard();
            Rect rect = new Rect(0,20,300,600);
            inspector.title = ManagerSettingWindow.LanguageObj.Inspector;
            inspector.subTitle = "";
            inspector.SetPosition(rect);
            this.contentContainer.Add(inspector);
            inspectorContainerVisualElemnt = new ScrollView();
            inspector.contentContainer.Add(inspectorContainerVisualElemnt);

            //�½��Ի��ļ��ı�����Ϣ�㼶
            VisualElement titleVisualElemnt = new VisualElement();
            titleVisualElemnt.AddToClassList("titleContainer");
            inspectorContainerVisualElemnt.Add(titleVisualElemnt);

            ///�½�����ӵ�ǰ�Ի��ļ����������
            VisualElement objectFieldVisualElement = new VisualElement();
            Label objectLabel = new Label(ManagerSettingWindow.LanguageObj.CurrentDialogueFile);
            objectField = new ObjectField();
            objectField.objectType = typeof(TextAsset);
            objectField.RegisterValueChangedCallback(OnTextObjVeluaChanged);
            objectFieldVisualElement.Add(objectLabel);
            objectFieldVisualElement.Add(objectField);
            objectFieldVisualElement.AddToClassList("DialogueContainer");

            //�½�����ӵ�ǰ�Ի����⡢ʱ�䡢�������������
            VisualElement dialogueTitleVisualElement = new VisualElement();
            Label dialogueTitleLabel = new Label(ManagerSettingWindow.LanguageObj.CurrentDialogueTitle);
            dialogueTitleField = new TextField();
            dialogueTitleVisualElement.Add(dialogueTitleLabel);
            dialogueTitleVisualElement.Add(dialogueTitleField);
            dialogueTitleVisualElement.AddToClassList("DialogueContainer");

            VisualElement dialogueTimeVisualElement = new VisualElement();
            Label dialogueTimeLabel = new Label(ManagerSettingWindow.LanguageObj.CurrentDialogueTime);
            dialogueTimeField = new TextField();
            dialogueTimeVisualElement.Add(dialogueTimeLabel);
            dialogueTimeVisualElement.Add(dialogueTimeField);
            dialogueTimeVisualElement.AddToClassList("DialogueContainer");

            VisualElement dialogueSceneVisualElement = new VisualElement();
            Label dialogueSceneLabel = new Label(ManagerSettingWindow.LanguageObj.CurrentDialogueScenes);
            dialogueSceneField = new TextField();
            dialogueSceneVisualElement.Add(dialogueSceneLabel);
            dialogueSceneVisualElement.Add(dialogueSceneField);
            dialogueSceneVisualElement.AddToClassList("DialogueContainer");

            VisualElement saveButtonVisualElement = new VisualElement();
            Button readingTextButton = new Button();
            readingTextButton.text = ManagerSettingWindow.LanguageObj.LodeText;
            readingTextButton.clicked += OnReadingTextButtonClick;
            Button saveingTextButton = new Button();
            saveingTextButton.text = ManagerSettingWindow.LanguageObj.SaveText;
            saveingTextButton.clicked += OnReadingTextButtonClick;
            saveButtonVisualElement.Add(readingTextButton);
            saveButtonVisualElement.Add(saveingTextButton);
            saveButtonVisualElement.AddToClassList("DialogueContainer");

            VisualElement layoutButtonVisualElement = new VisualElement();
            Button lineAlignmentButton = new Button();
            lineAlignmentButton.text = ManagerSettingWindow.LanguageObj.LineAlignment;
            lineAlignmentButton.clicked += OnLineAlignmentButtonClick;
            Button distributedButton = new Button();
            distributedButton.text = ManagerSettingWindow.LanguageObj.Distributed;
            distributedButton.clicked += OnDistributedButtonClick;
            layoutButtonVisualElement.Add(lineAlignmentButton);
            layoutButtonVisualElement.Add(distributedButton);
            layoutButtonVisualElement.AddToClassList("DialogueContainer");

            VisualElement nameColorVisualElement = new VisualElement();
            Label nameColorLabel = new Label(ManagerSettingWindow.LanguageObj.NameColorLabel);
            Button nameColorButton = new Button();
            nameColorButton.text = ManagerSettingWindow.LanguageObj.Unfold;
            nameColorButton.clicked += OnNameColorButtonClick;
            Button nameColorResetButton = new Button();
            nameColorResetButton.text = ManagerSettingWindow.LanguageObj.Refresh;
            nameColorResetButton.clicked += OnNameColorResetButtonClick;
            nameColorVisualElement.Add(nameColorLabel);
            nameColorVisualElement.Add(nameColorButton);
            nameColorVisualElement.Add(nameColorResetButton);
            nameColorVisualElement.AddToClassList("DialogueContainer");

            nameColorContentVisualElement = new VisualElement();
            nameColorContentVisualElement .AddToClassList("NameColorContent");

            titleVisualElemnt.Add(objectFieldVisualElement);
            titleVisualElemnt.Add(dialogueTitleVisualElement);
            titleVisualElemnt.Add(dialogueTimeVisualElement);
            titleVisualElemnt.Add(dialogueSceneVisualElement);
            titleVisualElemnt.Add(saveButtonVisualElement);
            titleVisualElemnt.Add(layoutButtonVisualElement);
            titleVisualElemnt.Add(nameColorVisualElement);
            titleVisualElemnt.Add(nameColorContentVisualElement);
        }
        /// <summary>
        /// ���ֶ�Ӧ��ɫ��岼�ֿؼ�
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        VisualElement NameColorVisualElement(string name) {
            VisualElement visualElement = new VisualElement();
            Label nameColorLabel = new Label(name);
            nameColorLabel.AddToClassList("NameColorLabel");
            var colorField = new ColorField();
            colorField.value = DialogueNameColor.ToDictionary()[name];
            colorField.RegisterValueChangedCallback((ChangeEvent<Color> changeEvent) => {
                OnNameColorFieldValue(name, colorField.value);
            });
            visualElement. Add(nameColorLabel);
            visualElement.Add(colorField);
            visualElement.AddToClassList("DialogueContainer");

            return visualElement;
        }

        /// <summary>
        /// ֱ�߶��밴ť�¼�
        /// </summary>
        void OnLineAlignmentButtonClick()
        {
            List<DialogueTextNode> saveNode = GetNowLinkNode();
            for (int i = 0; i < saveNode.Count; i++)
            {
                Rect rect = new Rect() ;
                rect.x = rootNode.GetPosition().x + (rect.width / 2) - (saveNode[i].GetPosition().width / 2);
                rect.y += yInterval *(i+1);
                saveNode[i].SetPosition(rect);
            }
        }

        /// <summary>
        /// ��ɢ���밴ť�¼�
        /// </summary>
        void OnDistributedButtonClick()
        {

            List<DialogueTextNode> saveNode = GetNowLinkNode();
            int id = 0;
            Dictionary<string, int> keys = new Dictionary<string, int>();


            for (int i = 0; i < saveNode.Count; i++)
            {
                if (saveNode[i].dialogueName != null && !keys.ContainsKey(saveNode[i].dialogueName))
                {
                    keys.Add(saveNode[i].dialogueName, id);
                    id++;
                }

                int xValue;
                if (saveNode[i].dialogueName != null)
                {
                    xValue = (keys[saveNode[i].dialogueName] % 2) != 0 ? (keys[saveNode[i].dialogueName] / 2) + 1 : (keys[saveNode[i].dialogueName] / 2) * -1;

                }
                else
                {
                    xValue = 0;
                }
                Rect rect = new Rect();
                rect.x = rootNode.GetPosition().x + (rect.width / 2) - (saveNode[i].GetPosition().width / 2) + (xInterval * xValue);
                rect.y += yInterval * (i + 1);
                saveNode[i].SetPosition(rect);
            }
        }

        /// <summary>
        /// ��ȡ�ı���ť�¼�
        /// </summary>
        void OnReadingTextButtonClick() {

            //��ȡ�ļ����ݲ�����
            if (textObj.TextFile != null)
            {
                string textFileText = textObj.TextFile.text.Replace('��', ':');
                string[] textFileDate = textFileText.Split('\n');

                List<DialogueTextNode> textNodes = GetNowLinkNode();
                List<DialogueDate> dialogueDates = new List<DialogueDate>();
                //�����ı����� ������һ�ݸ�ʽ���������
                for (int i = 0; i < textFileDate.Length; i++)
                {

                    DialogueDate dialogueDate = new DialogueDate();
                    textFileDate[i].Trim();
                    string[] date = textFileDate[i].Split(':');
                    while (date[0] == "Time" || date[0] == "Scene")
                    {
                        if (date.Length > 1)
                        {
                            if (date[0] == "Time")
                            {
                                dialogueDate.time = date[1];
                                i++;
                            }
                            else
                            {
                                dialogueDate.scene = date[1];
                                i++;
                            }
                            date = textFileDate[i].Split(':');
                        }
                    }
                    if (date.Length > 1)
                    {
                        dialogueDate.name = date[0];
                        dialogueDate.content = date[1].Trim() ;
                        dialogueDates.Add(dialogueDate);
                        GetNameColor(dialogueDate.name);
                    }
                    else {
                        dialogueDates[dialogueDates.Count - 1].content += date[0].Trim();
                    }
                }

                List<int> AddTextDate = new List<int>();
                Dictionary<int,int> changeTextDate = new Dictionary<int, int>();
                List<int> RamoveTextDate = new List<int>();

                int textNodeId = 0;
                int pointer = 0;
                int textNodeIdPointer = 0;
                bool isStop = false;
                //��ⲻͬ��λ��

                //�ȶ���Ҫ�Աȵ������б�ѭ��
                for (int i = 0; i < dialogueDates.Count; i++)
                {
                    isStop = false;
                    //�ж�������Ҫ�Աȵ������Ƿ���ͬ�͵�ǰ���Աȵ��������Ƿ���
                    if (textNodeId < textNodes.Count && !(dialogueDates[i].name == textNodes[textNodeId].dialogueName && dialogueDates[i].content == textNodes[textNodeId].content))
                    {
                        //��һ�αȶԲ�ͬ ��¼��ǰ�Ĳ���  ���н���ȶ�
                        for (int j = 1; j <= pointer; j++)
                        {
                            if (dialogueDates[i].name == textNodes[textNodeId - j].dialogueName && dialogueDates[i].content == textNodes[textNodeId - j].content)
                            {
                                for (int k = pointer; k >= j + 1; k--)
                                {
                                    //Debug.Log(1);

                                    changeTextDate.Add(i - k, textNodeId - k);
                                }
                                for (int k = j; k >=1 ; k--)
                                {
                                    AddTextDate.Add(i - k);
                                }
                                isStop = true;
                                textNodeId -= j;
                                pointer = -1;
                                break;
                            }
                        }

                        //��һ��ȶ�δ�ҵ� ������һ��ȶ�
                        if (!isStop)
                        {
                            for (int j = 1; j <= pointer; j++)
                            {
                                if (dialogueDates[i - j].name == textNodes[textNodeId].dialogueName && dialogueDates[i - j].content == textNodes[textNodeId].content)
                                {
                                    for (int k = pointer; k >= j + 1 ; k--)
                                    {
                                        //Debug.Log(1);

                                        changeTextDate.Add(i - k, textNodeId - k);
                                    }
                                    for (int k = j; k >=1 ; k--)
                                    {
                                        RamoveTextDate.Add(textNodeId - k);
                                    }
                                    i -= j;
                                    pointer = -1;
                                    break;
                                }
                            }
                        }
                        pointer++;
                        textNodeIdPointer = pointer;
                    }
                    //����һ�����ݲ�ͬ����������ͬ ������������ݱ��޸� 
                    else if (textNodeId < textNodes.Count && pointer > 0)
                    {
                        for (int j = pointer; j > 0; j--)
                        {
                            //Debug.Log(1);
                            changeTextDate.Add(i - j, textNodeId - j);
                        }
                        pointer = 0;
                    }
                    //�����Ա��������� ��ʹ�����һ�����ݽ��бȶ�
                    else if (textNodeId >= textNodes.Count)
                    {
                        if (pointer == 0)
                        {
                           AddTextDate.Add(i);
                        }
                        if (pointer != 0)
                        {
                            if (dialogueDates[i].name == textNodes[textNodes.Count - 1 ].dialogueName && dialogueDates[i].content == textNodes[textNodes.Count - 1].content)
                            {
                                for (int k = pointer - 1; k >= 1; k--)
                                {
                                    //Debug.Log(pointer);
                                    changeTextDate.Add(i - textNodeIdPointer + pointer - k - 1, textNodes.Count -1 - k);
                                }
                                for (int k = textNodeIdPointer - pointer + 1; k >= 1; k--)
                                {
                                    AddTextDate.Add(i - k);
                                }
                                for (int k = i + 1; k < dialogueDates.Count; k++)
                                {
                                    AddTextDate.Add(k);
                                }
                                pointer = 0;
                                break;
                            }
                            else if (i == dialogueDates.Count - 1)
                            {
                                for (int k = pointer; k >= 1; k--)
                                {
                                    //Debug.Log(pointer);
                                    changeTextDate.Add( i - textNodeIdPointer + pointer - k , textNodes.Count - k);
                                }
                                for (int k = textNodeIdPointer - pointer; k >= 0; k--)
                                {
                                    AddTextDate.Add(i - k);
                                }
                            }
                            textNodeIdPointer++ ;
                        }
                    }
                    textNodeId++;

                    //���Ա���������ʱ ��ʣ��ı��Ա����ݽ��д���
                    if (i == dialogueDates.Count - 1 && textNodeId < textNodes.Count)
                    {
                        if (pointer == 0)
                        {
                            for (int j = textNodes.Count - 1; j > textNodeId - 1; j--)
                            {
                                RamoveTextDate.Add(j);
                            }
                        }
                        else
                        {
                            for (int j = textNodeId; j < textNodes.Count; j++)
                            {
                                if (dialogueDates[i].name == textNodes[j].dialogueName && dialogueDates[i].content == textNodes[j].content)
                                {
                                    for (int k = pointer - 1; k >= 1; k--)
                                    {
                                        changeTextDate.Add(i - k, textNodeId +1 + pointer - k);
                                    }
                                    for (int k = j - textNodeId + pointer; k >= 1; k--)
                                    {
                                        RamoveTextDate.Add(j - k);
                                    }
                                    for (int k = j + 1; k < textNodes.Count; k++)
                                    {
                                        RamoveTextDate.Add(k);
                                    }
                                    pointer = 0;
                                    break;
                                }
                                else if (j == textNodes.Count - 1)
                                {
                                    for (int k = pointer - 1 ; k >= 1; k--)
                                    {
                                        changeTextDate.Add(i - k, textNodeId - 2 + pointer - k);
                                    }
                                    for (int k = j - textNodeId - 1 + pointer; k >= 0; k--)
                                    {
                                        RamoveTextDate.Add(j - k);
                                    }
                                    pointer = 0;
                                    break;
                                }
                            }
                        }
                    }
                }


                //���Ա���ɵ����ݽ���Ӧ��
                //Ӧ���޸ĵ�����
                var changeDate = changeTextDate.GetEnumerator();
                while (changeDate.MoveNext())
                {
                    Debug.Log("�޸�ǰ:" + textNodes[changeDate.Current.Value].dialogueName + ":" + textNodes[changeDate.Current.Value].content);
                    Debug.Log("�޸ĺ�:" + dialogueDates[changeDate.Current.Key].name + ":" + dialogueDates[changeDate.Current.Key].content);
                    textNodes[changeDate.Current.Value].dialogueName = dialogueDates[changeDate.Current.Key].name;
                    textNodes[changeDate.Current.Value].DialogueNameField.value = dialogueDates[changeDate.Current.Key].name;
                    textNodes[changeDate.Current.Value].content = dialogueDates[changeDate.Current.Key].content;
                    textNodes[changeDate.Current.Value].DialogueContentField.value = dialogueDates[changeDate.Current.Key].content;
                    textNodes[changeDate.Current.Value].time = dialogueDates[changeDate.Current.Key].time;
                    textNodes[changeDate.Current.Value].scene = dialogueDates[changeDate.Current.Key].scene;
                }
                changeDate.Dispose();

                //����Ҫɾ�������ݽ�������
                RamoveTextDate.Sort((x, y) => x.CompareTo(y));
                Console.WriteLine(RamoveTextDate);
                //Ӧ����Ҫɾ��������
                for (int i = 0; i < RamoveTextDate.Count; i++)
                {
                    Rect rect1 = textNodes[RamoveTextDate[i] - i].GetPosition();
                    rect1.x = rootNode.GetPosition().x -  (2* xInterval) - (rootNode.GetPosition().width / 2) - (rect1.width / 2);
                    textNodes[RamoveTextDate[i] - i].SetPosition(rect1);
                    Debug.Log("ɾ��:" + textNodes[RamoveTextDate[i] - i].dialogueName + ":" + textNodes[RamoveTextDate[i] - i].content);
                    textNodes.RemoveAt(RamoveTextDate[i] - i);
                }

                //����Ҫ��ӵ����ݽ�������
                AddTextDate.Sort((x, y) => x.CompareTo(y));
                Console.WriteLine(AddTextDate);
                //Ӧ����Ҫ��ӵ�����
                for (int i = 0; i < AddTextDate.Count; i++)
                {
                    textNodes.Insert(AddTextDate[i], SetNode(dialogueDates[AddTextDate[i]]));
                    Debug.Log("���:" + dialogueDates[AddTextDate[i]].name + ":" + dialogueDates[AddTextDate[i]].content);
                }

                //���ı��������������� ������
                RemoveAllEdge();
                for (int i = 0; i < textNodes.Count; i++)
                {
                    if (i==0)
                    {
                        Port inputPort = textNodes[i].InputContainer[0] as Port;
                        AddEdge(rootNode.outputPort, inputPort);
                    }
                    else
                    {
                        Port port = textNodes[i-1].outputContainer[0] as Port;
                        Port inputPort = textNodes[i].InputContainer[0] as Port;
                        AddEdge(port, inputPort);
                    }

                    Rect nodeRect = rootNode.GetPosition();
                    nodeRect.x += (nodeRect.width / 2) - (textNodes[i].GetPosition().width / 2);
                    nodeRect.y += yInterval *(i+1);
                    textNodes[i].SetPosition(nodeRect);
                }

                for (int i = 0; i < AddTextDate.Count; i++)
                {
                    Rect rect1 = textNodes[AddTextDate[i]].GetPosition();
                    rect1.x += xInterval;
                    rect1.y += yInterval * (AddTextDate[i] + 1) + rootNode.GetPosition().y;
                    textNodes[AddTextDate[i]].SetPosition(rect1);
                }

                changeDate = changeTextDate.GetEnumerator();
                while (changeDate.MoveNext())
                {
                    Rect rect1 = textNodes[changeDate.Current.Key].GetPosition();
                    rect1.x -=  xInterval ;
                    rect1.y = yInterval * (changeDate.Current.Key + 1) + rootNode.GetPosition().y;
                    textNodes[changeDate.Current.Key].SetPosition(rect1);
                }
                changeDate.Dispose();
            }
        }

        /// <summary>
        /// ���ֶ�Ӧ��ɫ����չ����ť�¼�
        /// </summary>
        void OnNameColorButtonClick()
        {
            if (nameColorContentVisualElement.childCount>0)
            {
                nameColorContentVisualElement.Clear();
            }
            else
            {
                if (DialogueNameColor !=null)
                {
                    var Enum = DialogueNameColor.ToDictionary().GetEnumerator();
                    while (Enum.MoveNext())
                    {
                        nameColorContentVisualElement.Add(NameColorVisualElement(Enum.Current.Key));
                    }
                    Enum.Dispose();
                }
            }
        }

        /// <summary>
        /// ���ֶ�Ӧ��ɫ����ˢ�°�ť�¼�
        /// </summary>
        void OnNameColorResetButtonClick()
        {
            List<string> names = new List<string>();
            var Enum = DialogueNameColor.ToDictionary().GetEnumerator();
            while (Enum.MoveNext())
            {
                names.Add(Enum.Current.Key);
            }
            Enum.Dispose();

            List<DialogueTextNode> node = GetNowLinkNode();
            for (int i = 0; i < node.Count; i++)
            {
                node[i].SetTitleColor(GetNameColor(node[i].dialogueName));
                if (names.Contains(node[i].dialogueName))
                {
                    names.Remove(node[i].dialogueName);
                }
            }

            for (int i = 0; i < names.Count; i++)
            {
                DialogueNameColor.ToDictionary().Remove(names[i]);
            }

            OnNameColorButtonClick();


        }

        /// <summary>
        /// ���ֶ�Ӧ��ɫ�����ֵˢ���¼�
        /// </summary>
        void OnNameColorFieldValue(string name,Color color)
        {
            DialogueNameColor.ToDictionary()[name] = color;
        }
        void OnTextObjVeluaChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            textObj.TextFile = objectField.value as TextAsset;
        }
    }
}
        
