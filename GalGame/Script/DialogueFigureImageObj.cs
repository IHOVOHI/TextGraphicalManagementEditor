using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueFigureImageObj : MonoBehaviour
{

    Image imageObj;
    IterationFloatObj iterationObj;
    float nowAlpha = 0;
    Color color;

    // Start is called before the first frame update
    void Start()
    {
        imageObj = gameObject.GetComponent<Image>();
        Init();
    }

    public void Init(){
        color = new Color(1, 1, 1, 0);
        imageObj.color = color;
        iterationObj = new IterationFloatObj(0,1,1);
    }


    public void SetImageObjSprite(Sprite sprite ){
        if (sprite == null) {
            iterationObj.End();
            iterationObj = new IterationFloatObj(0.3f, nowAlpha, 0, SetImageAlpha);
            iterationObj.Play();
        }
        else
        {
            imageObj.sprite = sprite;
            iterationObj.End();
            iterationObj = new IterationFloatObj(0.3f, nowAlpha, 1, SetImageAlpha);
            iterationObj.Play();

        }
    }


    /// <summary>
    /// µü´úÊÂ¼þ
    /// </summary>
    /// <param name="value"></param>
    public void SetImageAlpha(float value) {
        nowAlpha = value;
        color.a = nowAlpha;
        imageObj.color = color;
    }
}
