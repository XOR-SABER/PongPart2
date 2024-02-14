using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1.0f)]
    public float volume = 0.75f;
    [Range(.1f, 3f)]
    public float pitch = 1.0f;
    [HideInInspector]
    public AudioSource source;
    public Sound()
    {
        volume = 0.75f;
        pitch = 1.0f;  
    }
}
