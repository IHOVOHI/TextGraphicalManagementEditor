using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextGraphicalManagementEditor;

public class GalGameDemoMain : MonoBehaviour
{
    [HideInInspector]
    public int textStructrueID = 0;
    public TextStructrueFile textStructrue;

    [HideInInspector]
    public int dialogueTextID = -1;
    public DialogueTextFile dialogueText;

    public Text nameText;
    public Text content;
    public GameObject optionPanel;
    public GameObject optionPrefab;
    public GameObject mainButton;
    public GameObject nameObj;

    public Image backgroundImage;
    public GameObject dialogueFigureImagePanel;
    public Dictionary<int,DialogueFigureImageObj> dialogueFigureImages;
    public List<DialogueFigureImageObj> listDialogueFigureImages;

    public IterationStringObj iterationObj;

    public Dictionary<string,float> valuePairs;
   
    void Start()
    {
        Init();
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void Init() {
        valuePairs = new Dictionary<string, float>();
        iterationObj = new IterationStringObj();
        dialogueFigureImages = new Dictionary<int, DialogueFigureImageObj>();
        listDialogueFigureImages = new List<DialogueFigureImageObj>();
        DialogueFigureImageObj[] objs = dialogueFigureImagePanel.GetComponentsInChildren<DialogueFigureImageObj>();
        for (int i = 0; i < objs.Length; i++)
        {
            dialogueFigureImages.Add(int.Parse(objs[i].gameObject.name),objs[i]);
            listDialogueFigureImages.Add(objs[i]);
            Debug.Log(objs[i].gameObject.name);
        }
    }

    /// <summary>
    /// ���ݻ�ȡ��������������
    /// </summary>
    public void getDialogueTextData() {
        DialogueDate dialogueDate = null;
        //�жϵ�ǰ�Ƿ�������������ʾ
        if (iterationObj.isIneration)
        {
            iterationObj.End();
            content.text = iterationObj.stateValue;
            return;
        }
        //�жϵ�ǰ�ĶԻ��Ƿ���� ����� ������һ�ڵ�
        if (dialogueTextID != -1)
        {
            dialogueDate = TextManager.GetDialogueText(dialogueText, dialogueTextID, out dialogueTextID);
        }        
        //���ݶԻ���������
        if (dialogueDate != null)
        {
            LoadDialogueData(dialogueDate);
        }
    }

    /// <summary>
    /// ���ݻ�ȡ�Ľڵ���������
    /// </summary>
    public void getTextData()
    {
        NodeData data;
        //�жϵ�ǰ�Ի��Ƿ����
        if (dialogueTextID == -1)
        {
            data = TextManager.GetTextStructrue(textStructrue, textStructrueID, out textStructrueID, valuePairs);
            //�ж��Ƿ����
            if (textStructrueID == -1)
            {
                Debug.Log("end");
            }

            if (data != null)
            {
                switch (data.NodeType)
                {
                    case "TextGraphicalManagementEditor.DialogueNode":
                        dialogueText = data.NodeFile as DialogueTextFile;
                        dialogueTextID = 0;
                        getDialogueTextData();
                        break;
                    case "TextGraphicalManagementEditor.BranchNode":
                        mainButton.SetActive(false);
                        optionPanel.SetActive(true);

                        var transforms = optionPanel.GetComponentsInChildren<Transform>();
                        for (int i = 1; i < transforms.Length; i++)
                        {
                            Destroy(transforms[i].gameObject);
                        }

                        BranchNodeData branchNodeData = data as BranchNodeData;
                        for (int i = 0; i < branchNodeData.branchName.Count; i++)
                        {
                            GameObject optionObj = Instantiate(optionPrefab);
                            optionObj.transform.SetParent(optionPanel.transform);
                            optionObj.GetComponentInChildren<Text>().text = branchNodeData.branchName[i];
                            int id = i;
                            optionObj.GetComponentInChildren<Button>().onClick.AddListener(()=> {
                                OnButtonClick(branchNodeData.TrailingNodes[id]);
                                //��¼ѡ��ӵ�����
                                for (int j = 0; j< branchNodeData.optionDatas[id].DataName.Count; j++)
                                {
                                    if (valuePairs.ContainsKey(branchNodeData.optionDatas[id].DataName[j]))
                                    {
                                        valuePairs[branchNodeData.optionDatas[id].DataName[j]] += float.Parse(branchNodeData.optionDatas[id].DataValue[j]);
                                    }
                                    else
                                    {
                                        valuePairs.Add(branchNodeData.optionDatas[id].DataName[j], float.Parse(branchNodeData.optionDatas[id].DataValue[j]));
                                    }
                                }
                            });
                        }
                        break;
                    case "TextGraphicalManagementEditor.SwitchNode":
                        textStructrue = data.NodeFile as TextStructrueFile;
                        textStructrueID = textStructrue.roodId;
                        break;
                    case "TextGraphicalManagementEditor.AutomaticBranchNode":
                        getTextData();
                        break;
                    case "TextGraphicalManagementEditor.DialogueTextNode":
                        LoadDialogueData(data as DialogueDate);
                        break;
                }
            }
        }
        else
        {
            getDialogueTextData();
            if (dialogueTextID == -1)
            {
                getTextData();
            }
        }
    }

    public void LoadDialogueData(DialogueDate dialogueDate) {
        //Debug.Log("���֣�" + dialogueDate.name);
        nameText.text = "��" + dialogueDate.name + "��";
        nameObj.SetActive(true);
        if (dialogueDate.name == "")
        {
            nameObj.SetActive(false);
        }
        //Debug.Log("���ݣ�" + dialogueDate.content);
        iterationObj = new IterationStringObj(10, dialogueDate.content, CsText, () => { content.text = dialogueDate.content; });
        iterationObj.Play();

        for (int i = 0; i < listDialogueFigureImages.Count; i++)
        {
            listDialogueFigureImages[i].SetImageObjSprite(null);
        }

        for (int i = 0; i < dialogueDate.actions.list.Count; i++)
        {
            //Debug.Log(dialogueDate.actions.list[i].actionType);

            switch (dialogueDate.actions.list[i].actionType)
            {
                case "BackgroundMusic":
                    break;
                case "BackgroundImage":
                    BackgroundImage backgroundImage = dialogueDate.actions.list[i] as BackgroundImage;
                    if (backgroundImage.backgroundImage != null)
                    {
                        this.backgroundImage.sprite = backgroundImage.backgroundImage;
                    }
                    break;
                case "DialogueFigureImage":
                    DialogueFigureImage bialogueFigureImage = dialogueDate.actions.list[i] as DialogueFigureImage;
                    dialogueFigureImages[bialogueFigureImage.location].SetImageObjSprite(bialogueFigureImage.dialogueFigureImage);
                    break;
                case "DialogueVoice":
                    break;

            }
        }
    }

    /// <summary>
    /// ��ť���ص��¼�
    /// </summary>
    /// <param name="id"></param>
    public void OnButtonClick(int id){
        textStructrueID = id;
        optionPanel.SetActive(false);
        getTextData();
        mainButton.SetActive(true);
    }

    public void CsText(string value) {
        content.text = value;
    }
}
