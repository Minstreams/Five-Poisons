using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 开始按钮
/// </summary>
[AddComponentMenu("Bottons/Start")]
public class StartMenuBotton : MenuBotton
{
    public override void ClickDown()
    {
        GameSystem.GameStart();
    }
}
