﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAudioTester : MonoBehaviour
{
    [Header("音效测试组件")]
    [SerializeField]
    private AudioClip audioClip;

    [ContextMenu("测试！")]
    private void test()
    {
        GameSystem.audioSystem.Play(audioClip);
    }
}
