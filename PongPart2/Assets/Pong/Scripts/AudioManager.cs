using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour{   
    public Sound[] sounds;
    public Hashtable LookUpTable;
    // Start is called before the first frame update
    void Awake() {
        LookUpTable = new Hashtable();
        int index = 0;
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            LookUpTable.Add(s.name, index);
            index++;
        }
    }

    public void Play(string soundName) {
        if (LookUpTable.ContainsKey(soundName)) {
            int index = (int)LookUpTable[soundName];
            sounds[index].source.Play();
        }
        else Debug.Log("Audio: " + soundName + " is not avaliable");
    }

    public void playAtPitch(string soundName, float tempPitch) {
        if (LookUpTable.ContainsKey(soundName)) {
            int index = (int)LookUpTable[soundName];
            float defaultPitch = sounds[index].pitch;
            sounds[index].source.pitch = tempPitch;
            sounds[index].source.Play();
            // Reset it..
            StartCoroutine(ResetPitchAfterSoundFinish(sounds[index].source, defaultPitch));
        }
        else Debug.Log("Audio: " + soundName + " is not avaliable");
    }

    private IEnumerator ResetPitchAfterSoundFinish(AudioSource source, float defaultPitch)
    {
        // Wait until the sound finishes playing
        while (source.isPlaying) yield return null;
        // Reset the pitch after the sound finishes playing
        source.pitch = defaultPitch;
    }
}
