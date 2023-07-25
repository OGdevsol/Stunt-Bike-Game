using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    public static ScreenResolution instance;
 
    private void Awake()
    {
        
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        Application.targetFrameRate = 60;
        
#if !UNITY_EDITOR && UNITY_ANDROID
        if (SystemInfo.systemMemorySize <=512)
        {
            int x = (int)(Screen.width * 0.2);
            int y = (int)(Screen.height * 0.2);
            Screen.SetResolution(x,y,true);
        }
        if (SystemInfo.systemMemorySize > 512 && SystemInfo.systemMemorySize <= 1024)
        {
            int x = (int)(Screen.width * 0.5);
            int y = (int)(Screen.height * 0.5);
            Screen.SetResolution(x,y,true);
        }
        if (SystemInfo.systemMemorySize > 1024 && SystemInfo.systemMemorySize < 3072)
        {
            int x = (int)(Screen.width *  0.8);
            int y = (int)(Screen.height * 0.8);
            Screen.SetResolution(x,y,true);
        }
#endif       
    }
 
}
