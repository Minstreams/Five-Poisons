using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑幕系统，负责提供让画面之间平滑过渡的方法
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(GameSystem))]
public class BlackGroundSystem : MonoBehaviour
{
    //【Setting】
    [System.Serializable]
    public struct Setting
    {
        [Tooltip("黑幕实例")]
        public Renderer blackGroundInstance;
        [Tooltip("默认淡入淡出时间")]
        public float defaultTime;
    }
    [Header("【已经整合进GameSystem，作为组件时仅供测试功能，测试完请删除此组件】")]
    [HideInInspector]
    public Setting setting;


    //【Interface】
    [ContextMenu("淡入测试")]
    /// <summary>
    /// 让黑幕淡入
    /// </summary>
    /// <returns>返回经历时长</returns>
    public float FadeIn()
    {
        return FadeIn(setting.defaultTime);
    }
    /// <summary>
    /// 让黑幕淡入
    /// </summary>
    /// <param name="seconds">经历时间</param>
    /// <returns>返回经历时长</returns>
    public float FadeIn(float seconds)
    {
        StopAllCoroutines();
        StartCoroutine(fadeIn(seconds));
        return seconds;
    }
    [ContextMenu("淡出测试")]
    /// <summary>
    /// 让黑幕淡出
    /// </summary>
    /// <returns>返回经历时长</returns>
    public float FadeOut()
    {
        return FadeOut(setting.defaultTime);
    }
    /// <summary>
    /// 让黑幕淡出
    /// </summary>
    /// <param name="seconds">经历时长</param>
    /// <returns>返回经历时长</returns>
    public float FadeOut(float seconds)
    {
        StopAllCoroutines();
        StartCoroutine(fadeOut(seconds));
        return seconds;
    }


    //【Core】
    private IEnumerator fadeIn(float seconds)
    {
        setting.blackGroundInstance.gameObject.SetActive(true);
        Color color = Color.black;
        color.a = 0;
        while (color.a < 1)
        {
            setting.blackGroundInstance.material.SetColor("_Color", color);
            yield return 0;
            color.a += Time.deltaTime / seconds;
        }
        color.a = 1;
        setting.blackGroundInstance.material.SetColor("_Color", color);
        yield return 0;
    }
    private IEnumerator fadeOut(float seconds)
    {
        Color color = Color.black;
        while (color.a > 0)
        {
            setting.blackGroundInstance.material.SetColor("_Color", color);
            yield return 0;
            color.a -= Time.deltaTime / seconds;
        }
        color.a = 0;
        setting.blackGroundInstance.material.SetColor("_Color", color);
        setting.blackGroundInstance.gameObject.SetActive(false);
        yield return 0;
    }
}
