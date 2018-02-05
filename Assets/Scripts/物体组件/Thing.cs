using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// 游戏物体类
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[AddComponentMenu("MyAsset/Thing")]
[RequireComponent(typeof(BoxCollider))]
public class Thing : Botton
{
    /****************
     *   主体方法   *
     ****************/
    [Header("【物体组件】")]
    [Tooltip("是否始终对向摄像机")]
    public bool yLookAtCamera;

    private void Update()
    {
        if (yLookAtCamera)
        {
            LookAtCamera();
        }
    }

    public override void OnCursorEnter()
    {
        if (onCursorEnter != null) onCursorEnter();
    }
    public override void OnCursorOver()
    {
        if (onCursorOver != null) onCursorOver();
    }
    public override void OnCursorExit()
    {
        GameSystem.ChangeCursorSprite(GameSystem.setting.the光标图标设置.默认);
        if (onCursorExit != null) onCursorExit();
    }


    /****************
     *   委托功能   *
     ****************/
    public GameSystem.VoidNVoid onCursorEnter;
    public GameSystem.VoidNVoid onCursorOver;
    public GameSystem.VoidNVoid onCursorExit;


    /****************
     *   测试方法   *
     ****************/
    [ContextMenu("面对相机")]
    private void LookAtCamera()
    {
        Vector3 delta = transform.position - Camera.main.transform.position;
        delta.y = 0;
        transform.rotation = Quaternion.LookRotation(delta);
    }
    [ContextMenu("设置基础调查文本")]
    private void AddBasicInformation()
    {
        gameObject.AddComponent<BasicInformation>();
    }
    [ContextMenu("设置对话文本")]
    private void AddDialogInformation()
    {
        gameObject.AddComponent<DialogInformation>();
    }

    public override void Init()
    {
        //添加组件时替换材质，修改标签
        GetComponent<SpriteRenderer>().sharedMaterial = GameObject.FindGameObjectWithTag("GameSystem").GetComponent<GameSystem>().thingSpriteMat;
    }
#if UNITY_EDITOR
    [Space(10)]
    [Header("标签名称(给自己看着方便的)")]
    public string text = "";
    [Range(0.1f, 2)]
    public float size = 1f;
    private static GUIStyle style;
    private static GUIStyle Style
    {
        get
        {
            if (style == null)
            {
                style = new GUIStyle(EditorStyles.largeLabel);
                style.alignment = TextAnchor.MiddleCenter;
                style.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
                style.fontSize = 32;
            }
            return style;
        }

    }
    void OnDrawGizmos()
    {
        if (text == "") return;
        Vector3 point = transform.position + Vector3.up * GetComponent<SpriteRenderer>().sprite.bounds.extents.y * transform.localScale.y;
        float dist = (Camera.current.transform.position - point).magnitude;

        float fontSize = Mathf.Lerp(64, 12, dist / 10f) * size;

        Style.fontSize = (int)fontSize;

        Vector3 wPos = point + Camera.current.transform.up * dist * 0.07f;

        Vector3 scPos = Camera.current.WorldToScreenPoint(wPos);
        if (scPos.z <= 0)
        {
            return;
        }

        float alpha = Mathf.Clamp(-Camera.current.transform.forward.y, 0f, 1f);
        alpha = 1f - ((1f - alpha) * (1f - alpha));

        alpha = Mathf.Lerp(-0.2f, 1f, alpha);

        Handles.BeginGUI();

        scPos.y = Screen.height - scPos.y; // Flip Y


        Vector2 strSize = Style.CalcSize(new GUIContent(text));

        Rect rect = new Rect(0f, 0f, strSize.x + 6, strSize.y + 4);
        rect.center = scPos - Vector3.up * rect.height * 0.5f;
        GUI.color = new Color(0f, 0f, 0f, 0.8f * alpha);
        GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
        GUI.color = Color.white;
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.Label(rect, text, Style);
        GUI.color = Color.white;

        Handles.EndGUI();
    }
#endif
}

