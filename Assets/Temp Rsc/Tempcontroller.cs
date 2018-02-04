using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 临时第一人称角色控制器
/// </summary>
public class Tempcontroller : MonoBehaviour
{
    [SerializeField]
    private float xSensitivity = 1;
    [SerializeField]
    private float ySensitivity = 1;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private bool hideCursor = false;

    private void Start()
    {
        if (hideCursor)
        {
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        transform.rotation =
            Quaternion.Euler(Vector3.up * xSensitivity * Input.GetAxis("Mouse X"))
            * transform.rotation *
            Quaternion.Euler(Vector3.left * ySensitivity * Input.GetAxis("Mouse Y"));

        transform.Translate(speed * Time.deltaTime * (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))));
    }

}
