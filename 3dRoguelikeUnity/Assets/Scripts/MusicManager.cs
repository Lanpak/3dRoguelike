using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;

    private AudioSource source;

    void Awake()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.clip = tracks[0];
    }

    void Start()
    {
        StartTrack(1, 0);
    }

    public void ChangeTrack(int tracknum)
    {
        source.clip = tracks[tracknum];
    }
    
    public void StartTrack(int tracknum, int delay)
    {
        source.PlayDelayed(delay);
    }

}
