using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平滑追踪transform
/// </summary>
[AddComponentMenu("MyAssets/Tracker")]
[DisallowMultipleComponent]
public class Tracker : MonoBehaviour {
    public Transform target;
    [Header("平滑度")]
    [Range(0.05f,0.95f)]
    public float smoothness = 0.5f;
    [Header("是否追踪角度")]
    public bool ifRotate = false;

    private void FixedUpdate()
    {
        transform.position+=((1 - smoothness) * (target.position - transform.position));
        if (ifRotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, 1 - smoothness);
        }
    }
}
