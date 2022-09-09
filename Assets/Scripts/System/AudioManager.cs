using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class AudioConfiguration
    {
        public string Tag;
        public AudioClip Clip;
        public float Volume = 1.0f;
        public int Type;
    }

    [SerializeField] private float musicMultiplier;
    [SerializeField] private float gameMultiplier;
    [SerializeField] private float AIMultiplier;

    [SerializeField] AudioConfiguration[] Configuration;

    public static AudioManager _instance;
    private Dictionary<string, AudioConfiguration> audioClips = new Dictionary<string, AudioConfiguration>();
    private AudioSource backgoundAudioSource;
    private static AudioSource cameraAudioSource;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        foreach (AudioConfiguration ac in Configuration)
        {
            audioClips.Add(CleanTag(ac.Tag), ac);
        }
        //DontDestroyOnLoad(_instance);
    }

    void Start()
    {

        // Setup Background Audio

        musicMultiplier = AudioSettings.Instance.musicVol;

        AudioConfiguration backgroundConfig = FetchClip("Background");
        var backgroundClip = FetchClip("Background").Clip;
        backgoundAudioSource = CreateBackgroundAudioSource();
        backgoundAudioSource.volume = backgroundConfig.Volume * musicMultiplier;
        backgoundAudioSource.clip = backgroundConfig.Clip;
        if (backgroundClip != null) backgoundAudioSource.Play();

        // Setup Camera Audio
        cameraAudioSource = CreateAudioSource();

    }

    private void FixedUpdate()
    {
        var playerSettings = AudioSettings.Instance;
        musicMultiplier = playerSettings.musicVol;
        gameMultiplier = playerSettings.gameVol;
        AIMultiplier = playerSettings.AIVol;
    }

    private string CleanTag(string tag)
    {
        return tag.Trim().ToLower();
    }

    private AudioSource CreateBackgroundAudioSource()
    {
        var source = CreateAudioSource();
        source.loop = true;
        source.playOnAwake = true;
        return source;
    }

    private AudioSource CreateAudioSource()
    {
        var cameraGameObject = Camera.main.gameObject;
        var source = cameraGameObject.AddComponent<AudioSource>();
        return source;
    }

    private AudioConfiguration FetchClip(string tag)
    {
        tag = CleanTag(tag);
        if (audioClips.ContainsKey(tag))
        {
            return audioClips[tag];
        }
        else
        {
            Debug.Log("No audio clip found for tag: " + tag);
            return null;
        }
    }

    public AudioSource GetBackgroundMusic() {
        return backgoundAudioSource;
    }

    public static void PlayClipNow(string tag, float volumeMultiplier = 1.0f)
    {
        if (_instance == null)
        {
            Debug.LogWarning("No AudioManager instance exists in the current scene.");
            return;
        }

        var config = _instance.FetchClip(tag);
        if (config == null) return;

        if (config.Type == 1)
        {
            volumeMultiplier = _instance.musicMultiplier;
        }
        if (config.Type == 0)
        {
            volumeMultiplier = _instance.gameMultiplier;
        }
        if (config.Type == 2)
        {
            volumeMultiplier = _instance.AIMultiplier;
        }

        cameraAudioSource.PlayOneShot(config.Clip, config.Volume * volumeMultiplier);
    }

    public static void PlayClipAtPoint(Vector3 position, string tag, float volumeMultiplier = 1.0f)
    {
        if (_instance == null)
        {
            Debug.LogWarning("No AudioManager instance exists in the current scene.");
            return;
        }

        var config = _instance.FetchClip(tag);
        if (config == null) return;

        if (config.Type == 1)
        {
            volumeMultiplier = _instance.musicMultiplier;
        }
        if (config.Type == 0)
        {
            volumeMultiplier = _instance.gameMultiplier;
        }
        if (config.Type == 2)
        {
            volumeMultiplier = _instance.AIMultiplier;
        }

        AudioSource.PlayClipAtPoint(config.Clip, position, config.Volume * volumeMultiplier);
    }

    // TODO: more audio functionality
}
