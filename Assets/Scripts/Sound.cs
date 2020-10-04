using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource theAudio;

    [SerializeField] public AudioClip explosion;

    void Start()
    {
        theAudio = GetComponent<AudioSource>();
    }
    public void PlayAudio(string name)
    {
        if(name == "explosion")
        {
            theAudio.clip = explosion;
            theAudio.Play();
        }
    }
}
