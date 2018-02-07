using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 喜闻乐见的单例模式游戏系统
/// </summary>
public class GameSystem : MonoBehaviour
{
    //【静态全局变量】
    /// <summary>
    /// 游戏系统单例,系统设置
    /// </summary>
    public static GameSystem setting;
    /// <summary>
    /// 暂停委托变量
    /// </summary>
    public static VoidNVoid pause = null;
    /// <summary>
    /// 继续委托变量
    /// </summary>
    public static VoidNVoid pauseContinue = null;
    /// <summary>
    /// 光标射线信息
    /// </summary>
    public static RaycastHit cursorRaycastHit;
    /// <summary>
    /// 单句字幕
    /// </summary>
    [System.Serializable]
    public struct SingleSubtitle
    {
        public string 内容;
        public float 持续秒数;
        public ColorPreset 颜色;
    }
    /// <summary>
    /// 一段字幕
    /// </summary>
    [System.Serializable]
    public struct Subtitles
    {
        public SingleSubtitle[] subtitles;
    }
    /// <summary>
    /// 单句对话
    /// </summary>
    [System.Serializable]
    public struct SingleSentence
    {
        public string 内容;
        public ColorPreset 颜色;
    }
    /// <summary>
    /// 一段带选项对话
    /// </summary>
    [System.Serializable]
    public struct Sentences
    {
        public string 选项说明文字;
        public SingleSentence[] 正文;
        public Sentences[] 选项;
    }
    /// <summary>
    /// 对话
    /// </summary>
    [System.Serializable]
    public struct Dialog
    {
        public SingleSentence[] 正文;
        public Sentences[] 选项;
    }


    //【状态监控变量】
    /// <summary>
    /// 暂停状态
    /// </summary>
    public static bool paused = false;
    public enum GameStatus
    {
        StartMenu,
        UpMenu,
        DownMenu,
        Playing
    }
    /// <summary>
    /// 当前游戏状态
    /// </summary>
    public static GameStatus gameStatus = GameStatus.StartMenu;






    //【全局系统设置】
    [System.Serializable]
    public struct KeySetting
    {
        public KeyCode 交互;
        public KeyCode 交谈;
    }
    [System.Serializable]
    public struct CursorSpriteSetting
    {
        public Sprite 默认;
        public Sprite 远处调查;
        public Sprite 近处调查;
        public Sprite 菜单;
        public Sprite 菜单2;
    }
    /// <summary>
    /// 颜色预设
    /// </summary>
    public enum ColorPreset
    {
        颜色1,
        颜色2,
        颜色3
    }
    [System.Serializable]
    public struct ColorPresetSetting
    {
        public Color 颜色1;
        public Color 颜色2;
        public Color 颜色3;
    }
    [Header("【系统设置】")]
    public KeySetting the按键设置;
    public CursorSpriteSetting the光标图标设置;
    public float the可视距离 = 100;
    public ColorPresetSetting the颜色预设;



    //【子系统实例】
    [Space(20)]
    [Header("【子系统实例】")]

    [Tooltip("黑幕系统设置")]
    [SerializeField]
    private BlackGroundSystem.Setting blackGroundSystemSetting;
    private static BlackGroundSystem blackGround;
    public static BlackGroundSystem blackGroundSystem
    {
        get
        {
            if (blackGround == null)
            {
                blackGround = setting.gameObject.AddComponent<BlackGroundSystem>();
                blackGround.setting = setting.blackGroundSystemSetting;
            }
            return blackGround;
        }
    }

    [Tooltip("场景加载系统设置")]
    [SerializeField]
    private SceneLoadSystem.Setting sceneLoadSystemSetting;
    private static SceneLoadSystem sceneLoad;
    public static SceneLoadSystem sceneLoadSystem
    {
        get
        {
            if (sceneLoad == null)
            {
                sceneLoad = setting.gameObject.AddComponent<SceneLoadSystem>();
                sceneLoad.setting = setting.sceneLoadSystemSetting;
            }
            return sceneLoad;
        }
    }

    [Tooltip("声音系统设置")]
    [SerializeField]
    private AudioSystem.Setting audioSystemSetting;
    private static new AudioSystem audio;
    public static AudioSystem audioSystem
    {
        get
        {
            if (audio == null)
            {
                audio = setting.gameObject.AddComponent<AudioSystem>();
                audio.setting = setting.audioSystemSetting;
            }
            return audio;
        }
    }



    //【全局实例引用】
    [Space(10)]
    [Header("【实例引用】")]
    [Tooltip("基础物体的材质实例")]
    public Material thingSpriteMat;




    //【全局静态方法】
    /// <summary>
    /// 开始游戏，给按钮调用
    /// </summary>
    public static void GameStart()
    {
        gameStatus = GameStatus.Playing;
        sceneLoadSystem.LoadScene("Scene1");
    }
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
    /// <summary>
    /// 更改光标图标
    /// </summary>
    /// <param name="sprite"></param>
    public static void ChangeCursorSprite(Sprite sprite)
    {
        cursorSpriteRenderer.sprite = sprite;
    }
    /// <summary>
    /// 显示字幕
    /// </summary>
    public static void ShowSubtitles(ref Subtitles subtitles)
    {
        print("TODO:ShowSubtitles");
    }





    //【系统内部实现】

    //player的光标数据
    private static SpriteRenderer cursorSpriteRenderer;
    private static Transform cursorParent;
    private static Camera eyeCamera;
    private static Botton currentActiveBotton = null;

    private void Awake()
    {
        //隐藏光标
        Cursor.visible = false;
        if (setting != null && setting != this)
        {
            //确保单例唯一
            Debug.LogError("游戏系统单例不唯一！");
            Debug.Break();
        }
        setting = this;

        //初始化接收player的光标数据
        cursorSpriteRenderer = PlayerController.cursorParent.GetChild(0).GetComponent<SpriteRenderer>();
        cursorParent = PlayerController.cursorParent;
        eyeCamera = PlayerController.eye.GetComponent<Camera>();
    }
    private void Update()
    {
        //生成光标射线并处理botton 的接收信息
        Physics.Raycast(Camera.main.ScreenPointToRay(eyeCamera.WorldToScreenPoint(cursorParent.position)), out cursorRaycastHit, setting.the可视距离, 1 << LayerMask.NameToLayer("Objects"));
        Ray temp = Camera.main.ScreenPointToRay(eyeCamera.WorldToScreenPoint(cursorParent.position));
        Debug.DrawRay(temp.origin, temp.direction);
        Botton newActiveBotton = cursorRaycastHit.collider == null ? null : cursorRaycastHit.collider.GetComponent<Botton>();
        if (newActiveBotton != currentActiveBotton)
        {
            if (currentActiveBotton != null) currentActiveBotton.OnCursorExit();
            currentActiveBotton = newActiveBotton;
            if (currentActiveBotton != null) currentActiveBotton.OnCursorEnter();
        }
        if (currentActiveBotton != null) currentActiveBotton.OnCursorOver();
    }
    private void Reset()
    {
        tag = "GameSystem";
    }


    private Color ColorPresetToColor(ColorPreset preset)
    {
        switch (preset)
        {
            case ColorPreset.颜色1:
                return the颜色预设.颜色1;
            case ColorPreset.颜色2:
                return the颜色预设.颜色1;
            case ColorPreset.颜色3:
                return the颜色预设.颜色1;
        }
        return Color.black;
    }

    //【流程控制】
    private void Start()
    {
        StartCoroutine(GameProgress());
    }
    /// <summary>
    /// 游戏进度
    /// </summary>
    private IEnumerator GameProgress()
    {
        sceneLoadSystem.LoadScene("StartMenu", true);
        yield return 0;
    }
}

