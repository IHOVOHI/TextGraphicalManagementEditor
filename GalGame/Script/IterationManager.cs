using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 迭代管理器 工具类
/// </summary>
public class IterationManager : MonoBehaviour
{
    /// <summary>
    /// 管理器单例
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
    /// 迭代列表
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
/// 基类
/// </summary>
public class IterationObj
{
    /// <summary>
    /// 结束时间
    /// </summary>
    public float endTime;
    /// <summary>
    /// 当前时间
    /// </summary>
    public float nowTime;
    /// <summary>
    /// 结束事件
    /// </summary>
    public Action endAction;
    /// <summary>
    /// update事件
    /// </summary>
    public Action updateAction;
    /// <summary>
    /// 是否在迭代
    /// </summary>
    public bool isIneration = false;

    /// <summary>
    /// 0->1 迭代过程
    /// </summary>
    public float schedule {
        get {
            return (nowTime / endTime) >1? 1 : nowTime / endTime;
        }
    }

    /// <summary>
    /// 重置时间
    /// </summary>
    public void Reset()
    {
        this.nowTime = 0;
    }

    /// <summary>
    /// 开始
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
    /// 继续
    /// </summary>
    public void Continue()
    {
        IterationManager.AddIneration(this);
    }

    /// <summary>
    /// 结束
    /// </summary>
    public void End()
    {
        IterationManager.RemoveIneration(this);
    }
}

/// <summary>
/// Float迭达
/// </summary>
public class IterationFloatObj : IterationObj
{
    public float stateValue;
    public float endValue;

    /// <summary>
    /// 返回值
    /// </summary>
    public float returnValue {
        get {
            return (endValue - stateValue) * schedule + stateValue;
        }
    }

    /// <summary>
    /// 根据时间补间
    /// </summary>
    /// <param name="inerationTime">总时间</param>
    /// <param name="stateValue">开始值</param>
    /// <param name="endValue">结束值</param>
    /// <param name="update">每帧事件</param>
    /// <param name="endAction">结束事件</param>
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
/// String迭代
/// </summary>
public class IterationStringObj : IterationObj
{

    public string stateValue;
    public string endValue;


    public int endPlace;

    /// <summary>
    /// 返回值
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
    /// 根据每秒字数补间
    /// </summary>
    /// <param name="size">每秒显示的字数</param>
    /// <param name="stateValue">目标文字</param>
    /// <param name="update">每帧事件</param>
    /// <param name="endAction">结束事件</param>
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
    /// 根据时间补间
    /// </summary>
    /// <param name="inerationTime">总时间</param>
    /// <param name="stateValue">目标文字</param>
    /// <param name="update">每帧事件</param>
    /// <param name="endAction">结束事件</param>
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

