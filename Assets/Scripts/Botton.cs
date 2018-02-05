using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作为父类，接受点击事件
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public abstract class Botton : MonoBehaviour {
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
    ///<summary>
    ///加载组件时被调用
    /// </summary>
    public abstract void Init();

    private void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer("Objects");
        Init();
    }
}
