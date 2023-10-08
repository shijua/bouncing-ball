using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalq : MonoBehaviour
{
    public static char[] fly = new char[2];
    public static bool[] die = new bool[2];
    public static bool[] fly_jump = new bool[2];
    public static List<GameObject> all_level = new List<GameObject>();
    public static List<AudioClip> audioClips = null;
    public static AudioSource audio;
    //public static float times = 3.5f;

    public static int camera_count = 10; //相机切换完延迟(暂时q建不可用)
    public static bool view = true;  //相机切换
    public static bool isnewlevel = false; //判断是不是换关了
    public static int player_number = 1;   // 用不到qwq
    public static float times = 0.06f; //参数总体调节
    public static bool reset = false; // 重置
    public static int count = 0;    // 换关或者重新来的时候 等待时间
    public static int last_level = 0;  //之前的关 用于设置inactive
    public static int level = 1;  //当前关卡
    public static int max_level = 40; //最大关卡
    public static float pian = 0.4f; //避免进去
    public static float frame = 5.0f / 6.0f; //framerate的奇怪问题

    public static Vector2 spawnpoint = new Vector2(1.7f, -4.7f);
    public static Vector2 speed = Vector2.zero;




    //game constant
    public const float maxx = 1.02f;
    public const float maxgra = 1.65f;
    public const float maxup = 3.0f;
    public static float gra = 0.08f * frame;
    public static float move_x = 0.065f * frame;

    public const float rebound_x = 0.90f;
    public const float rebound_shift_x = 0.85f;
    public const float rebound_y = 1.6f;
    public const float rebound_shift_y = 0.1f;
    public const float touch_x = 0.3f;
    public const float touch_shift_x = 0.5f;

    public const float fly_end_x = 0.96f;
    public const float fly_end_y = 0.2f;
    public const float fly_end_jump_x = 1.02f;
    public const float fly_end_jump_y = 1.55f;
    public const float fly_shift = 1.1f;
    public const float fly_per = 0.15f;
    public const float wall = 1.75f;
    public const float jump = 2.9f;
    public const float jump_shift = 0.0f;
    public const float down = 1.5f;
    public const float glass = 0.8f;

    void Start()
    {
        Application.targetFrameRate = 60; //限制fps
        //GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
        audio = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    public static void set_clip(int a)
    {
        audio.clip = audioClips[a];
        audio.Play();

    }
}


