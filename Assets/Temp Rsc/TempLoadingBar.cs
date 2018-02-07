using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLoadingBar : MonoBehaviour
{

    private void Start()
    {
        SceneLoadSystem.progressPerform += perform;
    }

    private void perform(float progress)
    {
        print("Loading " + progress * 100 + "% Done!");
    }

    private void OnDestroy()
    {
        SceneLoadSystem.progressPerform -= perform;
    }
}
