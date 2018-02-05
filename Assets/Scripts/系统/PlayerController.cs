using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制器，挂到某物体上？
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("【玩家控制器】")]
    [Space(20)]
    [Header("【视角控制设置】")]
    /// <summary>
    /// x轴灵敏度
    /// </summary>
    [SerializeField]
    [Header("x轴灵敏度")]
    [Range(0.1f, 1.3f)]
    private float xSensitivity = 1;
    /// <summary>
    /// y轴灵敏度
    /// </summary>
    [SerializeField]
    [Header("y轴灵敏度")]
    [Range(0.1f, 1.3f)]
    private float ySensitivity = 1;
    /// <summary>
    /// x偏移量的范围
    /// </summary>
    [SerializeField]
    [Header("x偏移量的范围")]
    [Tooltip("相当于光标的横轴灵敏度的倒数，数值越小，光标在横轴上移动越灵敏")]
    [Range(1, 60)]
    private float xRange = 30;
    /// <summary>
    /// y偏移量的范围
    /// </summary>
    [SerializeField]
    [Header("y偏移量的范围")]
    [Tooltip("相当于光标的纵轴灵敏度的倒数，数值越小，光标在纵轴上移动越灵敏")]
    [Range(1, 30)]
    private float yRange = 30;
    /// <summary>
    /// 菜单状态x偏移量的范围
    /// </summary>
    [SerializeField]
    [Header("菜单状态x偏移量的范围")]
    [Tooltip("相当于菜单状态光标的横轴灵敏度的倒数，数值越小，菜单状态光标在横轴上移动越灵敏")]
    [Range(1, 30)]
    private float xMenuRange = 20;
    /// <summary>
    /// 菜单状态y偏移量的范围
    /// </summary>
    [SerializeField]
    [Header("菜单状态y偏移量的范围")]
    [Tooltip("相当于菜单状态光标的纵轴灵敏度的倒数，数值越小，菜单状态光标在纵轴上移动越灵敏")]
    [Range(1, 20)]
    private float yMenuRange = 30;
    /// <summary>
    /// 光标横轴平移量的范围
    /// </summary>
    [SerializeField]
    [Header("光标横轴平移量的范围")]
    [Range(0.01f, 3f)]
    private float cursorXRange = 1;
    /// <summary>
    /// 菜单状态光标横轴平移量的范围
    /// </summary>
    private const float cursorXMenuRange = 3.55f;
    /// <summary>
    /// 光标纵轴平移量的范围
    /// </summary>
    private const float cursorYRange = 2.2f;

    [Space(20)]
    [Header("【移动控制设置】")]
    [SerializeField]
    [Header("前移速度")]
    private float speedForward;
    [SerializeField]
    [Header("后退速度")]
    private float speedBackward;
    [SerializeField]
    [Header("平移速度")]
    private float speedHorizontal;

    [Space(40)]
    /// <summary>
    /// x偏移量
    /// </summary>
    [SerializeField]
    private float xOffset = 0;
    /// <summary>
    /// y偏移量
    /// </summary>
    [SerializeField]
    private float yOffset = 0;
    /// <summary>
    /// 眼睛物体
    /// </summary>
    public static Transform eye;
    /// <summary>
    /// 光标父物体
    /// </summary>
    public static Transform cursorParent;
    /// <summary>
    /// 关联的刚体
    /// </summary>
    private Rigidbody rid;
    /// <summary>
    /// 单例
    /// </summary>
    public static PlayerController player;

    private void Awake()
    {
        eye = transform.Find("Eye");
        cursorParent = eye.Find("CursorParent");
        rid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        StartCoroutine(StartMenu());
    }

    private IEnumerator StartMenu()
    {
        //获取输入量
        float xDelta = Input.GetAxis("Mouse X") * xSensitivity;
        float yDelta = Input.GetAxis("Mouse Y") * ySensitivity;

        //记录偏移量
        xOffset += xDelta;
        if (xOffset < -xMenuRange) xOffset = -xMenuRange;
        if (xOffset > xMenuRange) xOffset = xMenuRange;
        yOffset += yDelta;
        float yMaxRange = yMenuRange / 2;
        if (yOffset < -yMaxRange) yOffset = -yMaxRange;
        if (yOffset > yMaxRange) yOffset = yMaxRange;

        //光标父物体平移计算
        float xxTrans;
        float yyTrans;

        xxTrans = cursorXMenuRange * xOffset / xMenuRange;
        yyTrans = cursorYRange * yOffset / yMaxRange;

        cursorParent.localPosition = new Vector3(xxTrans, yyTrans, 0);

        yield return 0;

        //状态转换
        if(GameSystem.gameStatus == GameSystem.GameStatus.Playing)
        {
            xOffset = 0;
            yOffset = 0;
            yield return Playing();
        }
        else
        {
            yield return StartMenu();
        }
    }

    private IEnumerator Playing()
    {
        //获取输入量
        float xDelta = Input.GetAxis("Mouse X") * xSensitivity;
        float yDelta = Input.GetAxis("Mouse Y") * ySensitivity;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //记录偏移量
        xOffset += xDelta;
        if (xOffset < -xRange) xOffset = -xRange;
        if (xOffset > xRange) xOffset = xRange;
        yOffset += yDelta;

        float yMaxRange = yRange + yMenuRange;
        if (yOffset < -yMaxRange) yOffset = -yMaxRange;
        if (yOffset > yMaxRange) yOffset = yMaxRange;

        //光标父物体平移计算
        float xxTrans;
        float yyTrans;

        xxTrans = cursorXRange * Mathf.Sin(xOffset * Mathf.PI / xRange / 2);
        yyTrans = cursorYRange * Mathf.Sin(yOffset * Mathf.PI / yRange / 2);

        cursorParent.localPosition = new Vector3(xxTrans, yyTrans, 0);

        //主体旋转计算
        float yRot = yOffset;
        transform.Rotate(Vector3.up * xDelta);
        eye.localRotation = Quaternion.Euler(Vector3.left * yRot);

        //移动计算
        rid.velocity = transform.forward * v * (v > 0 ? speedForward : speedBackward) + transform.right * h * speedHorizontal;

        yield return 0;


        //状态转换
        if (yOffset > yRange)
        {
            xOffset = 0;
            rid.velocity = Vector3.zero;
            GameSystem.gameStatus = GameSystem.GameStatus.UpMenu;
            yield return UpMenu();
        }
        else if (yOffset < -yRange)
        {
            xOffset = 0;
            rid.velocity = Vector3.zero;
            GameSystem.gameStatus = GameSystem.GameStatus.DownMenu;
            yield return DownMenu();
        }
        else
        {
            yield return Playing();
        }
    }

    private IEnumerator UpMenu()
    {
        //获取输入量
        float xDelta = Input.GetAxis("Mouse X") * xSensitivity;
        float yDelta = Input.GetAxis("Mouse Y") * ySensitivity;

        //记录偏移量
        xOffset += xDelta;
        if (xOffset < -xMenuRange) xOffset = -xMenuRange;
        if (xOffset > xMenuRange) xOffset = xMenuRange;

        yOffset += yDelta;
        float yMaxRange = yRange + yMenuRange;
        if (yOffset > yMaxRange) yOffset = yMaxRange;

        //光标父物体平移计算
        float xxTrans;
        float yyTrans;

        xxTrans = cursorXMenuRange * xOffset / xMenuRange;
        yyTrans = cursorYRange * ((yOffset - yRange) / yMenuRange * 2 - 1);

        cursorParent.localPosition = new Vector3(xxTrans, yyTrans, 0);

        //主体旋转计算
        float yRot = yOffset;
        eye.localRotation = Quaternion.Euler(Vector3.left * yRot);

        yield return 0;


        //状态转换
        if (yOffset < yRange)
        {
            GameSystem.gameStatus = GameSystem.GameStatus.Playing;
            yield return Playing();
        }
        else
        {
            yield return UpMenu();
        }
    }

    private IEnumerator DownMenu()
    {
        //获取输入量
        float xDelta = Input.GetAxis("Mouse X") * xSensitivity;
        float yDelta = Input.GetAxis("Mouse Y") * ySensitivity;

        //记录偏移量
        xOffset += xDelta;
        if (xOffset < -xMenuRange) xOffset = -xMenuRange;
        if (xOffset > xMenuRange) xOffset = xMenuRange;
        yOffset += yDelta;
        float yMaxRange = yRange + yMenuRange;
        if (yOffset < -yMaxRange) yOffset = -yMaxRange;

        //光标父物体平移计算
        float xxTrans;
        float yyTrans;

        xxTrans = cursorXMenuRange * xOffset / xMenuRange;
        yyTrans = cursorYRange * ((yOffset + yRange + yMenuRange) / yMenuRange * 2 - 1);

        cursorParent.localPosition = new Vector3(xxTrans, yyTrans, 0);

        //主体旋转计算
        float yRot = yOffset;
        eye.localRotation = Quaternion.Euler(Vector3.left * yRot);

        yield return 0;


        //状态转换
        if (yOffset > -yRange)
        {
            GameSystem.gameStatus = GameSystem.GameStatus.Playing;
            yield return Playing();
        }
        else
        {
            yield return DownMenu();
        }

    }
}
