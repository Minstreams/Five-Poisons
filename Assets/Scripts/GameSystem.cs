using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 喜闻乐见的单例模式游戏系统
/// </summary>
public class GameSystem : MonoBehaviour
{
    /****************
     * 静态全局变量 *
     ****************/
    /// <summary>
    /// 游戏系统单例,系统设置
    /// </summary>
    public static GameSystem setting;
    /// <summary>
    /// 返回值和参数都为void的delegate类型
    /// </summary>
    public delegate void VoidNVoid();
    /// <summary>
    /// 暂停委托变量
    /// </summary>
    public static VoidNVoid pause = null;
    /// <summary>
    /// 继续委托变量
    /// </summary>
    public static VoidNVoid pauseContinue = null;

    /****************
     * 状态监控变量 *
     ****************/
    /// <summary>
    /// 暂停状态
    /// </summary>
    public static bool paused = false;
    public enum GameStatus
    {

    }
    /// <summary>
    /// 当前游戏状态
    /// </summary>
    public static GameStatus gameStatus;

    /****************
     * 全局系统设置 *
     ****************/
    [System.Serializable]
    public struct KeySetting
    {
        public KeyCode 交互;
        public KeyCode 交谈;
    }
    [Header("【系统设置】")]
    public KeySetting the按键设置;

    /****************
     * 全局实例引用 *
     ****************/
    [Space(10)]
    [Header("【实例引用】")]
    [Tooltip("基础物体的材质实例")]
    public Material thingSpriteMat;

    /****************
     * 全局静态方法 *
     ****************/
    /// <summary>
    /// 暂停
    /// </summary>
    public static void Pause()
    {
        if (pause != null) pause();
        paused = true;
    }
    /// <summary>
    /// 继续
    /// </summary>
    public static void PauseContinue()
    {
        if (pauseContinue != null) pauseContinue();
        paused = false;
    }

    /****************
     * 系统内部实现 *
     ****************/
    private void Awake()
    {
        Cursor.visible = false;
        if (setting != null && setting != this)
        {
            //确保单例唯一
            Debug.LogError("游戏系统单例不唯一！");
            Debug.Break();
        }
        setting = this;
    }

    private void Reset()
    {
        tag = "GameSystem";
    }
}

