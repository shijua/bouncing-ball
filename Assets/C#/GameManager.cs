using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    InputField fieldcomponent;
    int val;
    Slider slider;
    Slider slider_music;
    AudioSource music;


    void Start()
    {
        fieldcomponent = GameObject.Find("InputField").GetComponent<InputField>();
        slider = GameObject.Find("Slider_volume").GetComponent<Slider>();
        slider_music = GameObject.Find("Slider_music").GetComponent<Slider>();
        music = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }

    public static bool IsNumber(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return false;
        const string pattern = "^[0-9]*$";
        Regex rx = new Regex(pattern);
        return rx.IsMatch(s);
    }

    public void Go()
    {
        if (globalq.count != 0) return;
        if (!IsNumber(fieldcomponent.text)) return;
        val = Convert.ToInt32(fieldcomponent.text);
        if (val <= globalq.max_level && val > 0)
        {
            globalq.isnewlevel = true;
            GiveInitialVelocity.sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            globalq.last_level = globalq.level;
            globalq.level = val;
            globalq.all_level[globalq.level].SetActive(true);
            globalq.spawnpoint = GameObject.Find($"save{globalq.level}").GetComponent<Rigidbody2D>().position;
            globalq.speed = Vector2.zero;
            globalq.player_number -= 10;
        }
    }

    public void Restart()
    {
        if (globalq.count != 0) return;
        GiveInitialVelocity.sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        globalq.speed = Vector2.zero;
        globalq.player_number -= 10;
    }

    public void Sound_change()
    {
        globalq.audio.volume = slider.value;
    }

    public void Music_change()
    {
        music.volume =  slider_music.value;
    }
}
