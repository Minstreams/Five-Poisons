using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于给thing物体添加功能
/// </summary>
public abstract class Function : MonoBehaviour
{
    private Thing thing;
    /// <summary>
    /// 光标进入时由外部调用
    /// </summary>
    public abstract void OnCursorEnter();
    /// <summary>
    /// 光标指向时由外部调用
    /// </summary>
    public abstract void OnCursorOver();
    /// <summary>
    /// 光标移出时由外部调用
    /// </summary>
    public abstract void OnCursorExit();

    private void Start()
    {
        thing = GetComponent<Thing>();
        thing.onCursorEnter += OnCursorEnter;
        thing.onCursorOver += OnCursorOver;
        thing.onCursorExit += OnCursorExit;
    }
}
