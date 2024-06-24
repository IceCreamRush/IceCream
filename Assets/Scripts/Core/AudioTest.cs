using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public string bgmName;
    private bool isPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        // TODO fix bug
        GameManager.Instance.SetTimeout<string, bool>(AudioManager.Instance.PlayBGM, bgmName, true, 2000);
        //AudioManager.Instance.PlayBGM(bgmName, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            GameManager.Instance.SetTimeout<string>(AudioManager.Instance.PlayEffect, "Fire_01", 10000);
            isPlaying = true;
        }
    }
}
