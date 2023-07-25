using System;
using UnityEngine;

// Token: 0x020000DF RID: 223
[Serializable]
 public class MenuAudioPool
 {
     public void play()
     {
         if (Time.time - lastPlay > minInterval)
         {
             lastPlay = Time.time;
             sources[index].Play();
             index = (index + 1) % length;
         }
     }
 
     public void setVolume(float newVolume)
     {
         for (int i = 0; i < length; i++)
         {
             sources[i].volume = newVolume;
         }
     }
     
     public AudioLibrary type;
 
     public string name;
     
     [NonSerialized]
     public AudioSource[] sources;
 
     public AudioClip[] clips;
 
     [NonSerialized]
     public int index;
 
     [NonSerialized]
     public int length;
 
     public float minInterval;
 
     private float lastPlay;
 }



[Serializable]
public class GameAudioPool
{
    public void play()
    {
        if (Time.time - lastPlay > minInterval)
        {
            lastPlay = Time.time;
            sources[index].Play();
            index = (index + 1) % length;
        }
    }

    public void setVolume(float newVolume)
    {
        for (int i = 0; i < length; i++)
        {
            sources[i].volume = newVolume;
        }
    }
    
    public AudioLibrary type;

    public string name;
    
    [NonSerialized]
    public AudioSource[] sources;

    public AudioClip[] clips;

    [NonSerialized]
    public int index;

    [NonSerialized]
    public int length;

    public float minInterval;

    private float lastPlay;
}