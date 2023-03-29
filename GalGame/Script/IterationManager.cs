using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ���������� ������
/// </summary>
public class IterationManager : MonoBehaviour
{
    /// <summary>
    /// ����������
    /// </summary>
    private static IterationManager instance;
    public static IterationManager Instance {
        get{
            if (!instance)
            {
                instance = new GameObject("InstanceManager").AddComponent<IterationManager>();
            }

            return instance;
        }
    }

    /// <summary>
    /// �����б�
    /// </summary>
    private List<IterationObj> iterations;
    public List<IterationObj> Iterations {
        get {
            if (iterations == null)
            {
                iterations = new List<IterationObj>();
            }
            return iterations;
        }
    }


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Iterations.Count>0)
        {
            for (int i = 0; i < Iterations.Count; i++)
            {
                Iterations[i].nowTime += Time.deltaTime;
                if (Iterations[i].nowTime >= Iterations[i].endTime)
                {
                    Iterations[i].isIneration = false;
                    if (Iterations[i].endAction != null)
                    {
                        Iterations[i].endAction();
                    }
                    Iterations.Remove(Iterations[i]);
                    break;
                }
                if (Iterations[i].updateAction != null)
                {
                    Iterations[i].updateAction();
                }
            }
        }
    }

    public static void AddIneration(IterationObj iterationObj)
    {
        if (Instance.Iterations.Contains(iterationObj))
        {
            return;
        }
        iterationObj.isIneration = true;
        Instance.Iterations.Add(iterationObj);
    }

    public static void RemoveIneration(IterationObj iterationObj)
    {
        if (!Instance.Iterations.Contains(iterationObj))
        {
            return;
        }
        iterationObj.isIneration = false;
        Instance.Iterations.Remove(iterationObj);
    }
}

/// <summary>
/// ����
/// </summary>
public class IterationObj
{
    /// <summary>
    /// ����ʱ��
    /// </summary>
    public float endTime;
    /// <summary>
    /// ��ǰʱ��
    /// </summary>
    public float nowTime;
    /// <summary>
    /// �����¼�
    /// </summary>
    public Action endAction;
    /// <summary>
    /// update�¼�
    /// </summary>
    public Action updateAction;
    /// <summary>
    /// �Ƿ��ڵ���
    /// </summary>
    public bool isIneration = false;

    /// <summary>
    /// 0->1 ��������
    /// </summary>
    public float schedule {
        get {
            return (nowTime / endTime) >1? 1 : nowTime / endTime;
        }
    }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public void Reset()
    {
        this.nowTime = 0;
    }

    /// <summary>
    /// ��ʼ
    /// </summary>
    public void Play()
    {
        if (IterationManager.Instance.Iterations.Contains(this))
        {
            return;
        }
        Reset();
        IterationManager.AddIneration(this);
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Continue()
    {
        IterationManager.AddIneration(this);
    }

    /// <summary>
    /// ����
    /// </summary>
    public void End()
    {
        IterationManager.RemoveIneration(this);
    }
}

/// <summary>
/// Float����
/// </summary>
public class IterationFloatObj : IterationObj
{
    public float stateValue;
    public float endValue;

    /// <summary>
    /// ����ֵ
    /// </summary>
    public float returnValue {
        get {
            return (endValue - stateValue) * schedule + stateValue;
        }
    }

    /// <summary>
    /// ����ʱ�䲹��
    /// </summary>
    /// <param name="inerationTime">��ʱ��</param>
    /// <param name="stateValue">��ʼֵ</param>
    /// <param name="endValue">����ֵ</param>
    /// <param name="update">ÿ֡�¼�</param>
    /// <param name="endAction">�����¼�</param>
    public IterationFloatObj(float inerationTime, float stateValue, float endValue, Action<float> update = null , Action endAction = null)
    {
        this.endTime = inerationTime;
        this.nowTime = 0;
        this.stateValue = stateValue;
        this.endValue = endValue;
        this.endAction = endAction;
        if (update!=null)
        {
            this.updateAction = () => { update(returnValue); };
        }
    }


}

/// <summary>
/// String����
/// </summary>
public class IterationStringObj : IterationObj
{

    public string stateValue;
    public string endValue;


    public int endPlace;

    /// <summary>
    /// ����ֵ
    /// </summary>
    public string returnValue
    {
        get
        {
            if (endPlace != (int)(stateValue.Length * schedule))
            {
                endPlace =(int)(stateValue.Length * schedule);
                endValue = stateValue.Remove(endPlace);
            }
            return endValue;
        }
    }

    /// <summary>
    /// ����ÿ����������
    /// </summary>
    /// <param name="size">ÿ����ʾ������</param>
    /// <param name="stateValue">Ŀ������</param>
    /// <param name="update">ÿ֡�¼�</param>
    /// <param name="endAction">�����¼�</param>
    public IterationStringObj(int size, string stateValue, Action<string> update = null, Action endAction = null)
    {
        endTime = stateValue.Length / size;
        endValue = "";
        this.nowTime = 0;
        this.stateValue = stateValue;
        this.endAction = endAction;
        if (update != null)
        {
            this.updateAction = () => { update(returnValue); };
        }
    }


    /// <summary>
    /// ����ʱ�䲹��
    /// </summary>
    /// <param name="inerationTime">��ʱ��</param>
    /// <param name="stateValue">Ŀ������</param>
    /// <param name="update">ÿ֡�¼�</param>
    /// <param name="endAction">�����¼�</param>
    public IterationStringObj( float inerationTime, string stateValue, Action<string> update = null, Action endAction = null)
    {
        this.endTime = inerationTime;
        endValue = "";
        this.nowTime = 0;
        this.stateValue = stateValue;
        this.endAction = endAction;
        if (update != null)
        {
            this.updateAction = () => { update(returnValue); };
        }
    }

    public IterationStringObj()
    {
    }
}

