using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BasicInformation : Function
{
    [Header("【基本调查文本，无距离要求】")]
    public GameSystem.Subtitles information;


    public override void OnCursorEnter()
    {
        GameSystem.ChangeCursorSprite(GameSystem.setting.the光标图标设置.远处调查);
    }

    public override void OnCursorOver()
    {
        if (Input.GetKeyDown(GameSystem.setting.the按键设置.交互))
        {
            print("【交互】" + gameObject);
            GameSystem.ShowSubtitles(ref information);
        }
    }

    public override void OnCursorExit()
    {

    }
}
