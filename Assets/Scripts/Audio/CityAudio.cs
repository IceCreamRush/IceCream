using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBGM("City", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
