using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiogroup : MonoBehaviour
{
    public List<AudioClip> audioClips;
    
    void Start()
    {
        globalq.audioClips = audioClips;
        //Debug.Log(audioClips);
        //Debug.Log(globalq.audioClips);
    }
    // Start is called before the first frame update
}
