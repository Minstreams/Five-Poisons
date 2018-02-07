using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景加载系统，封装场景加载方法
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(GameSystem))]
public class SceneLoadSystem : MonoBehaviour
{
    //【Setting】
    [System.Serializable]
    public struct Setting
    {
        [Tooltip("快速加载时间")]
        [Range(0.1f, 1)]
        public float rapidSeconds;
        [Tooltip("默认快速加载")]
        public bool defaultRapid;
        [Tooltip("进度跟踪平滑值")]
        [Range(0.05f, 1)]
        public float smoothRate;
    }
    [Header("【已经整合进GameSystem，作为组件时仅供测试功能，测试完请删除此组件】")]
    [HideInInspector]
    public Setting setting;

    //【Interface】
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="toLoad">需要加载的场景的名字</param>
    /// <param name="rapid">是否需要快速加载</param>
    public void LoadScene(string toLoad, bool rapid)
    {
        StopAllCoroutines();
        StartCoroutine(loadScene(toLoad, rapid));
    }
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="toLoad">需要加载的场景的名字</param>
    public void LoadScene(string toLoad)
    {
        LoadScene(toLoad, setting.defaultRapid);
    }

    //【Core】
    /// <summary>
    /// 当前加载的场景
    /// </summary>
    private string currentScene;
    /// <summary>
    /// 实际记录进度
    /// </summary>
    private AsyncOperation progress;
    /// <summary>
    /// 模拟记录进度
    /// </summary>
    private float progressValue;
    /// <summary>
    /// 加载进度演出的委托方法
    /// </summary>
    public static VoidNFloat progressPerform;
    /// <summary>
    /// 加载场景完毕的初始化方法
    /// </summary>
    public static VoidNVoid overInit;

    /// <summary>
    /// 开始载入
    /// </summary>
    private IEnumerator loadScene(string toLoad, bool rapid)
    {
        //初始化
        progressValue = 0;

        //淡出
        yield return new WaitForSeconds(GameSystem.blackGroundSystem.FadeIn());

        //卸载场景
        if (SceneManager.GetSceneByName(currentScene).IsValid())
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }

        //加载场景
        currentScene = toLoad;
        progress = SceneManager.LoadSceneAsync(toLoad, LoadSceneMode.Additive);

        //快速加载
        if (rapid) yield return new WaitForSeconds(setting.rapidSeconds);
        if (rapid && progress.isDone)
        {
            //若加载成功则结束
            print("Loaded rapidly : " + toLoad);
            yield return over();
        }
        else
        {
            //否则播放加载动画
            yield return loadPerform();
        }
    }
    /// <summary>
    /// 载入演出
    /// </summary>
    private IEnumerator loadPerform()
    {
        //加载Loading场景
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
        GameSystem.blackGroundSystem.FadeOut();

        //循环检测
        while (progress.progress < 1)
        {
            progressValue += (progress.progress - progressValue) * setting.smoothRate;
            if (progressPerform != null) progressPerform(progressValue);
            yield return 0;
        }
        if (progressPerform != null) progressPerform(1);

        //结束
        yield return new WaitForSeconds(GameSystem.blackGroundSystem.FadeIn());
        SceneManager.UnloadSceneAsync("Loading");
        yield return over();

    }
    /// <summary>
    /// 结束载入
    /// </summary>
    private IEnumerator over()
    {
        if (overInit != null) overInit();
        yield return new WaitForSeconds(GameSystem.blackGroundSystem.FadeOut());
        yield return 0;
    }
}
