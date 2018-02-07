using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBGMTester : MonoBehaviour
{
    [Header("BGM测试组件")]
    [SerializeField]
    private AudioClip audioClip;

    [ContextMenu("测试！")]
    private void test()
    {
        GameSystem.audioSystem.PlayBGM(audioClip);
    }
}
