using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GiveInitialVelocity : MonoBehaviour
{
    private Vector2 tem;
    private readonly float times = globalq.times; // constant
    Rigidbody2D rigidbody;
    public static SpriteRenderer sprite;
    public static SpriteRenderer transition_animation;
    public static GameObject camera1;
    public static GameObject camera2;


    void Start()
    {
        globalq.reset = false;
        globalq.isnewlevel = false;
        globalq.count = 0;
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        transition_animation = GameObject.Find("transition animation").GetComponent<SpriteRenderer>();
        camera1 = GameObject.Find("Main Camera");
        camera2 = GameObject.Find("Main Camera Large");
        camera2.SetActive(false);
    }

    void Awake()
    {
        globalq.all_level.Clear();
        if (SceneManager.GetActiveScene().name == "SampleScene") return;
        globalq.all_level.Add(GameObject.Find($"{1}"));
        for (int i = 1; i <= globalq.max_level; i++) {  //设置其他的不看到
            globalq.all_level.Add(GameObject.Find($"{i}"));
            globalq.all_level[i].SetActive(false);
        }
        globalq.all_level[globalq.level].SetActive(true);
    }

    void Update() {
        //Debug.Log(globalq.fly[0]);
        if (globalq.camera_count != 0) globalq.camera_count = (globalq.camera_count + 1) % 11; // 等待
        if (Input.GetKey(KeyCode.Q) && globalq.camera_count == 0) { //切换相机
            if (globalq.view){
                globalq.view = false;
                camera1.SetActive(false);
                camera2.SetActive(true);
            }
            else {
                globalq.view = true;
                camera1.SetActive(true);
                camera2.SetActive(false);
            }
            globalq.camera_count = 1;
        }
        if(Input.GetKey(KeyCode.E) && globalq.camera_count == 0 && globalq.view && camera1.GetComponent<Camera>().orthographicSize < 8) {
            camera1.GetComponent<Camera>().orthographicSize += 1;
            globalq.camera_count = 1;
        }
        if (Input.GetKey(KeyCode.W) && globalq.camera_count == 0 && globalq.view && camera1.GetComponent<Camera>().orthographicSize > 1) {
            camera1.GetComponent<Camera>().orthographicSize -= 1;
            globalq.camera_count = 1;
        }

        if (Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene(0);
        if (globalq.player_number <= 0) {  //等1s
            if (globalq.isnewlevel) transition_animation.color = new Color(0.1370594f, 0.4931208f, 0.8301887f, transition_animation.color.a + 0.02f);
            else transition_animation.color = new Color(0, 0, 0, transition_animation.color.a + 0.02f);
            globalq.count++;
            if(globalq.count == 60) {
                globalq.set_clip(9);
                rigidbody.position = globalq.spawnpoint;  //出生点
            }
            if (globalq.count == 90) {
                globalq.reset = true;
                transition_animation.color = Color.clear;
            }
            else return;
        }
        if (globalq.reset){  //重生
            globalq.speed = Vector2.zero;
            globalq.player_number = 1;

            if (SceneManager.GetActiveScene().name != "SampleScene")
            {
                transition_animation.transform.position = GameObject.Find($"{globalq.level}").transform.position + new Vector3(12f, -7.5f, 0.0f); //过度动画
                camera2.transform.position = GameObject.Find($"{globalq.level}").transform.position + new Vector3(12f, -7.5f, -10.0f);
            }
            if (globalq.isnewlevel && globalq.last_level != globalq.level) globalq.all_level[globalq.last_level].SetActive(false); //节约资源.jpg
            sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask; //小球显示
            rigidbody.constraints = RigidbodyConstraints2D.None;
            globalq.count = 0;
            globalq.isnewlevel = false;
            globalq.reset = false;
        }
        if(globalq.fly[0] == '>') //fly right
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                globalq.fly[0] = '\0';
                globalq.set_clip(4);
                if (globalq.fly_jump[0]) { //空跳
                    globalq.fly_jump[0] = false;
                    globalq.speed = new Vector2(globalq.fly_end_jump_x * times, globalq.fly_end_jump_y * times);
                }
                else globalq.speed = new Vector2(globalq.fly_end_x*times, globalq.fly_end_y*times);
                //Debug.Log("1");
            }
            else rigidbody.position = new Vector2(rigidbody.position.x+globalq.fly_per, rigidbody.position.y);
            return;
        }
        else if (globalq.fly[0] == '<') // fly left
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                globalq.fly[0] = '\0';
                globalq.set_clip(4);
                if (globalq.fly_jump[0]){ //空跳
                    globalq.fly_jump[0] = false;
                    globalq.speed = new Vector2(-globalq.fly_end_jump_x * times, globalq.fly_end_jump_y * times);
                }
                else globalq.speed = new Vector2(-globalq.fly_end_x*times, globalq.fly_end_y*times);
                //Debug.Log("3");
            }
            else rigidbody.position = new Vector2(rigidbody.position.x-globalq.fly_per, rigidbody.position.y);
            return;
        }
        else
        {
            tem = new Vector2(0.0f, -globalq.gra * times); //other

            //if (Input.GetKey(KeyCode.UpArrow)) tem = new Vector2(0.0f, 0.05f * times); //test
            //if (Input.GetKey(KeyCode.DownArrow)) tem = new Vector2(0.0f, -0.01f * times);

            if (Input.GetKey(KeyCode.LeftArrow) && globalq.speed.x > -globalq.maxx * times && !(Input.GetKey(KeyCode.RightArrow))) tem = new Vector2(-globalq.move_x * times, tem.y);
            if (Input.GetKey(KeyCode.RightArrow) && globalq.speed.x < globalq.maxx * times && !(Input.GetKey(KeyCode.LeftArrow))) tem = new Vector2(globalq.move_x * times, tem.y);
            globalq.speed += tem;

            //最大最小速度
            if (globalq.speed.y > globalq.maxup * times) globalq.speed = new Vector2(globalq.speed.x, globalq.maxup * times);
            if (globalq.speed.y < -globalq.maxgra * times) globalq.speed = new Vector2(globalq.speed.x, -globalq.maxgra * times);
            if (globalq.speed.x < -globalq.maxx * times) globalq.speed = new Vector2(globalq.speed.x + 0.05f * times, globalq.speed.y);
            if (globalq.speed.x > globalq.maxx * times) globalq.speed = new Vector2(globalq.speed.x - 0.05f * times, globalq.speed.y);
        }
        rigidbody.position += globalq.speed;

    }
}
