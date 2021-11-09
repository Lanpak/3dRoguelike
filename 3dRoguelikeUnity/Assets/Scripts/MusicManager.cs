using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;

    private AudioSource source;
    private LevelManager manager;

    void Awake()
    {
        manager = GameObject.Find("Manager").GetComponent<LevelManager>();

        source = gameObject.GetComponent<AudioSource>();
        source.clip = tracks[0];
    }

    void Start()
    {
        StartTrack(manager.floor - 1,0);
    }

    public void ChangeTrack(int tracknum)
    {
        source.clip = tracks[tracknum];
    }
    
    public void StartTrack(int tracknum, int delay)
    {
        ChangeTrack(tracknum);
        source.PlayDelayed(delay);
    }

}
