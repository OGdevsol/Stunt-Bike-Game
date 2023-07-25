using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobileSpec : MonoBehaviour
{
 
    public void Awake()
    {
        
        if (SystemInfo.systemMemorySize > 512 && SystemInfo.systemMemorySize <= 1024)
        {
            RamThousandTwentyFour();
        }
      
      
    }
    
    public void RamThousandTwentyFour()
    {
        PlayerPrefs.SetString("AdmobAdsOnly", "True");
    }
    
}
