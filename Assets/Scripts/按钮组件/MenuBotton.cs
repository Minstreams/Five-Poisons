using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[AddComponentMenu("MyAssets/Menu Botton")]
[RequireComponent(typeof(BoxCollider))]
public abstract class MenuBotton : Botton
{
    [Header("【按钮】")]
    [Tooltip("按钮在一般状态下的图片")]
    public Sprite normal;
    [Tooltip("按钮在鼠标移上时的图片")]
    public Sprite over;
    [Tooltip("按钮在鼠标按下时的图片")]
    public Sprite down;

    private bool isActive = false;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = normal;
    }
    public override void Init() { }

    public override void OnCursorEnter()
    {
        GameSystem.ChangeCursorSprite(GameSystem.setting.the光标图标设置.菜单2);
        GetComponent<SpriteRenderer>().sprite = over;
    }

    public override void OnCursorExit()
    {
        GameSystem.ChangeCursorSprite(GameSystem.setting.the光标图标设置.菜单);
        isActive = false;
        GetComponent<SpriteRenderer>().sprite = normal;
    }

    public override void OnCursorOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isActive = true;
            GetComponent<SpriteRenderer>().sprite = down;
        }
        if (Input.GetMouseButtonUp(0) && isActive)
        {
            GetComponent<SpriteRenderer>().sprite = over;
            ClickDown();
        }
    }
    /// <summary>
    /// 按下按钮时调用
    /// </summary>
    public abstract void ClickDown();

    [ContextMenu("面对相机")]
    private void LookAtCamera()
    {
        Vector3 delta = transform.position - Camera.main.transform.position;
        //delta.y = 0;
        transform.rotation = Quaternion.LookRotation(delta);
    }
    [ContextMenu("y面对相机")]
    private void yLookAtCamera()
    {
        Vector3 delta = transform.position - Camera.main.transform.position;
        delta.y = 0;
        transform.rotation = Quaternion.LookRotation(delta);
    }
}
