using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public float musicVol = 1;
    public float AIVol = 1;
    public float gameVol = 1;

    public static AudioSettings Instance;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void setMusic(float volume)
    {
        musicVol = volume;
    }
    public void setAI(float volume)
    {
        AIVol = volume;
    }
    public void setGame(float volume)
    {
        gameVol = volume;
    }
}
