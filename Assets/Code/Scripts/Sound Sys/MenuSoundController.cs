using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundController : MonoBehaviour
{
    
    #region Instance
 
    private static MenuSoundController _instance;

    public static MenuSoundController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MenuSoundController>();
            }

            return _instance;
        }
    }
 
    #endregion
     
    private void OnEnable()
    {
        
        DataController.sfxVolumeChanged += SfxVolumeChanged;
        DataController.musicVolumeChanged += MusicVolumeChanged;
    }

    private void OnDisable()
    {
        DataController.sfxVolumeChanged -= SfxVolumeChanged;
        DataController.musicVolumeChanged -= MusicVolumeChanged;
    }
    
    private void Awake()
    {
        _instance = this;
  
    }

    private void Start()
    {
        audioMusic.clip = musicSounds[Random.Range(0, musicSounds.Length)];
        audioMusic.Play();

        CreatePool();
        
        MusicVolumeChanged(DataController.instance.musicVolume);
        SfxVolumeChanged(DataController.instance.sfxVolume);
    }

    private void SfxVolumeChanged(float newVolume)
    {
        foreach (KeyValuePair<AudioLibrary, MenuAudioPool> keyValuePair in audioList)
        {
            keyValuePair.Value.setVolume(newVolume);
        }
    }

    private void MusicVolumeChanged(float newVolume)
    {
        if (menuMusicSource != null)
            menuMusicSource.volume = newVolume;
    }
    
    public void CreatePool()
    {
        GameObject gameObject = new GameObject();
        gameObject.name = "AudioPool";
        audioList = new Dictionary<AudioLibrary, MenuAudioPool>();
        foreach (MenuAudioPool audioPool in pool)
        {
            audioPool.length = audioPool.clips.Length;
            audioPool.sources = new AudioSource[audioPool.length];
            for (int j = 0; j < audioPool.length; j++)
            {
                AudioClip audioClip = audioPool.clips[j];
                GameObject gameObject2 = new GameObject();
                gameObject2.name = audioClip.name;
                gameObject2.transform.parent = gameObject.transform;
                gameObject2.AddComponent<AudioSource>();
                gameObject2.GetComponent<AudioSource>().clip = audioClip;
                audioPool.sources[j] = gameObject2.GetComponent<AudioSource>();
            }
            audioList[audioPool.type] = audioPool;
        }
    }
    
    public void playFromPool(AudioLibrary audioType)
    {
        audioList[audioType].play();
    }
    
    private float sfxVolume;
    
    private float musicVolume;
    
    public AudioSource audioMusic;

    public AudioSource menuMusicSource;
    
    public AudioClip[] musicSounds;

    public MenuAudioPool[] pool;

    private Dictionary<AudioLibrary, MenuAudioPool> audioList;
    
}
