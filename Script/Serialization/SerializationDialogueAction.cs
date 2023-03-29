using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextGraphicalManagementEditor
{
    [Serializable]
    public class SerializationDialogueAction : ISerializationCallbackReceiver
    {
        /// <summary>
        /// �����ⲿ���õ��б�
        /// </summary>
        [SerializeField]
        public List<DialogueAction> list;

        /// <summary>
        /// �������л����б� �����������
        /// </summary>
        [SerializeField, HideInInspector]
        private List<int> dialogueActionsId;
        [SerializeField, HideInInspector]
        private List<BackgroundImage> backgroundImages;
        [SerializeField, HideInInspector]
        private List<BackgroundMusic> backgroundMusics;
        [SerializeField, HideInInspector]
        private List<DialogueFigureImage> dialogueFigureImages;
        [SerializeField, HideInInspector]
        private List<DialogueVoice> dialogueVoice;

        /// <summary>
        /// ��ʼ���������л����б�
        /// </summary>
        public void Initialize(){
            dialogueActionsId = new List<int>();
            backgroundImages = new List<BackgroundImage>();
            backgroundMusics = new List<BackgroundMusic>();
            dialogueFigureImages = new List<DialogueFigureImage>();
            dialogueVoice = new List<DialogueVoice>();
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="dialogueActions"></param>
        public SerializationDialogueAction(List<DialogueAction> dialogueActions)
        {
            this.list = dialogueActions;
            Initialize();
        }

        public SerializationDialogueAction()
        {
            this.list = new List<DialogueAction>();
            Initialize();
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].actionType)
                {
                    case "DialogueFigureImage":
                        list[i] = dialogueFigureImages[dialogueActionsId[i]]; break;
                    case "DialogueVoice":
                        list[i] = dialogueVoice[dialogueActionsId[i]]; break;
                    case "BackgroundMusic":
                        list[i] = backgroundMusics[dialogueActionsId[i]]; break;
                    case "BackgroundImage":
                        list[i] = backgroundImages[dialogueActionsId[i]]; break;
                    default:
                        Debug.Log("defaule");
                        break;
                }
            }
        }


        //�˴����ܴ������� ����ʱ���ԣʱ�о��Ż� ���¼�������ʱ���ظ�����
        public void OnBeforeSerialize()
        {
            Initialize();
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].actionType)
                {
                    case "DialogueFigureImage":
                        dialogueFigureImages.Add(list[i] as DialogueFigureImage);
                        dialogueActionsId.Add(dialogueFigureImages.Count-1); 
                        break;
                    case "DialogueVoice":
                        dialogueVoice.Add(list[i] as DialogueVoice);
                        dialogueActionsId.Add(dialogueVoice.Count - 1); 
                        break;
                    case "BackgroundMusic":
                        backgroundMusics.Add(list[i] as BackgroundMusic);
                        dialogueActionsId.Add(backgroundMusics.Count - 1); 
                        break;
                    case "BackgroundImage":
                        backgroundImages.Add(list[i] as BackgroundImage);
                        dialogueActionsId.Add(backgroundImages.Count - 1); 
                        break;
                    default:
                        break;
                }
            }
        }

    }
}