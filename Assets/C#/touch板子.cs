using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class touch板子 : MonoBehaviour
{
    // Start is called before the first frame update
    
    private float pian = globalq.pian;
    private float times = globalq.times;
    Rigidbody2D rigidbody_ball;
    Rigidbody2D rigidbody_wall;
    SpriteRenderer sprite_ball;
    SpriteRenderer sprite_wall;
    BoxCollider2D box_wall;
    void Start()
    {
        rigidbody_wall = GetComponent<Rigidbody2D>();
        sprite_wall = GetComponent<SpriteRenderer>();
        box_wall = GetComponent<BoxCollider2D>();
        box_wall.size = sprite_wall.size;
    }

    // Update is called once per frame
    void Update()
    {
        if(globalq.count > 0){
            if (CompareTag("rock") || CompareTag("save") || CompareTag("stop")) {
                if(globalq.spawnpoint != rigidbody_wall.position) {
                    sprite_wall.maskInteraction = SpriteMaskInteraction.None; //初始化刷新
                    rigidbody_wall.bodyType = RigidbodyType2D.Dynamic;
                }
            }
            globalq.reset = false;
        }
    }

    float min(float a,float b){
        
        if(a < 0) {
            b = -globalq.maxx * times;
            if(a > b) return a;
            else return b;
        }
        else{
            b = globalq.maxx * times;
            if(a < b) return a;
            else return b;
        }
    }
    float max(float a){
        if (a < 0){
            if(a > times * -0.4) return 0;
            else return a;
        }else{
            if(a < times * 0.4) return 0;
            else return a;
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        OnCollisionEnter2D(collision);
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    //OnCollisionEnter2D(collision);
    //}
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag != "ball") return; //
        rigidbody_ball = collision.gameObject.GetComponent<Rigidbody2D>();
        sprite_ball = collision.gameObject.GetComponent<SpriteRenderer>();
        if (CompareTag("save")){  //存 flag 
            if (globalq.fly[0] != '\0') globalq.set_clip(4);
            globalq.fly[0] = '\0';
            globalq.speed = new Vector2(0.0f, 0.25f * times);
            rigidbody_ball.position = rigidbody_wall.position;
            if(!globalq.isnewlevel) globalq.spawnpoint = rigidbody_wall.position;
            sprite_wall.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            rigidbody_wall.bodyType = RigidbodyType2D.Kinematic;
            return;
        }
        else if (CompareTag("stop")){  //存 flag 
            if (globalq.fly[0] != '\0') globalq.set_clip(4);
            globalq.fly[0] = '\0';
            globalq.fly_jump[0] = false;
            globalq.speed = new Vector2(0.0f, 0.25f * times);
            rigidbody_ball.position = rigidbody_wall.position + new Vector2(0.0f,0.05f);
            sprite_wall.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            rigidbody_wall.bodyType = RigidbodyType2D.Kinematic;
            return;
        }
        else if (CompareTag("destination")){ // 终点
            if (globalq.player_number < 0) return;
            globalq.fly[0] = '\0';
            globalq.fly_jump[0] = false;
            globalq.player_number -= 10;
            sprite_ball.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            globalq.speed = Vector2.zero;
            globalq.last_level = globalq.level;
            if(globalq.level == globalq.max_level) SceneManager.LoadScene(3); // 通关！
            globalq.all_level[++globalq.level].SetActive(true);
            globalq.spawnpoint = GameObject.Find($"save{globalq.level}").GetComponent<Rigidbody2D>().position;
            globalq.isnewlevel = true;
            globalq.set_clip(3);
            return;
        }
        //float ball_y = collision.gameObject.transform.localScale.y / 2;
        //float ball_x = collision.gameObject.transform.localScale.x / 2;
        float wall_y = transform.localScale.y * sprite_wall.size.y / 2;
        float pos_wall_y = rigidbody_wall.position.y;
        float pos_ball_y = rigidbody_ball.position.y;
        float wall_x = transform.localScale.x * sprite_wall.size.x / 2;
        float pos_wall_x = rigidbody_wall.position.x;
        float pos_ball_x = rigidbody_ball.position.x;

        if (pos_ball_y < pos_wall_y - wall_y + pian / 3 && globalq.speed.y > 0){ //下面
            globalq.speed = new Vector2(globalq.speed.x,-globalq.down * times);
            globalq.set_clip(10);
            for (var i = 0; i < 100; i++) {
                pos_ball_y = rigidbody_ball.position.y;
                if (pos_ball_y < pos_wall_y - wall_y + pian) break;
                rigidbody_ball.position = new Vector2 (rigidbody_ball.position.x, rigidbody_ball.position.y - 0.01f); //避免在墙里面
            }
        }
        else if(pos_ball_y > pos_wall_y + wall_y - pian / 3 && globalq.speed.y < 0){ //上面
            //Debug.Log(1);
            if(CompareTag("wall")) {
                globalq.speed = new Vector2(globalq.speed.x, globalq.wall * times); //方块类型判断 墙
                globalq.set_clip(1);
            }
            else if(CompareTag("jump")) {
                globalq.speed = new Vector2(globalq.speed.x, globalq.jump * times);
                rigidbody_ball.position = new Vector2(rigidbody_ball.position.x, rigidbody_ball.position.y + globalq.jump_shift);
                globalq.set_clip(6);
            } // 跳
            else if (CompareTag("left")){ //左箭头
                globalq.fly[0] = '<';
                if(Input.GetKey(KeyCode.LeftArrow) && globalq.speed.x > 0){
                    globalq.fly_jump[0] = true;
                    rigidbody_ball.position = new Vector2(pos_wall_x - globalq.fly_shift, pos_wall_y + wall_y / 1.5f);
                }
                else rigidbody_ball.position = new Vector2(pos_wall_x - globalq.fly_shift, pos_wall_y);
                globalq.speed = Vector2.zero;
                globalq.set_clip(8);
                return;

            }
            else if (CompareTag("right")){ //右箭头
                globalq.fly[0] = '>';
                if(Input.GetKey(KeyCode.RightArrow) && globalq.speed.x < 0){
                    globalq.fly_jump[0] = true;
                    rigidbody_ball.position = new Vector2(pos_wall_x + globalq.fly_shift, pos_wall_y + wall_y / 1.5f);
                }
                else rigidbody_ball.position = new Vector2(pos_wall_x + globalq.fly_shift, pos_wall_y);
                globalq.speed = Vector2.zero;
                globalq.set_clip(8);
                return;
            }
            else if (CompareTag("bomb")) { //炸弹
                globalq.player_number -= 1;
                sprite_ball.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                rigidbody_ball.constraints = RigidbodyConstraints2D.FreezePosition;
                globalq.speed = Vector2.zero;
                globalq.set_clip(0);
            }
            else if (CompareTag("rock")){ //石头
                sprite_wall.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                rigidbody_wall.bodyType = RigidbodyType2D.Kinematic;
                globalq.speed = new Vector2(globalq.speed.x, globalq.wall * times);
                globalq.set_clip(2);
            }
            else if(CompareTag("glass")) {//玻璃
                globalq.speed = new Vector2(globalq.speed.x, globalq.glass * times);
                globalq.set_clip(7);
            } 


            //for (var i = 0; i < 100; i++){
            //    pos_ball_y = rigidbody_ball.position.y;
            //    if (pos_ball_y > pos_wall_y + wall_y - pian) break;
            //    rigidbody_ball.position = new Vector2 (rigidbody_ball.position.x, rigidbody_ball.position.y + 0.01f); //避免在墙里面
            //}
        }

        pos_ball_y = rigidbody_ball.position.y;
        //if (globalq.fly[0] != '\0'){  //撞到停
        //    if (globalq.fly[0] == '>') globalq.speed = new Vector2(-globalq.fly_end_x * times, globalq.fly_end_y * times);
        //    if (globalq.fly[0] == '<') globalq.speed = new Vector2(globalq.fly_end_x * times, globalq.fly_end_y * times);
        //    globalq.set_clip(4);
        //    globalq.fly[0] = '\0';
        //    globalq.fly_jump[0] = false;
        //}

        if (pos_ball_y > pos_wall_y - wall_y + pian / 4 && pos_ball_y < pos_wall_y + wall_y - pian / 4){

            if(pos_ball_x < pos_wall_x){
                if (globalq.fly[0] == '>'){
                    globalq.speed = new Vector2(-globalq.fly_end_x * times, globalq.fly_end_y * times);
                    //globalq.set_clip(4);
                    globalq.fly[0] = '\0';
                    globalq.fly_jump[0] = false;
                }

            }

            if (pos_ball_x > pos_wall_x){
                if (globalq.fly[0] == '<')
                {
                    globalq.speed = new Vector2(globalq.fly_end_x * times, globalq.fly_end_y * times);
                    //globalq.set_clip(4);
                    globalq.fly[0] = '\0';
                    globalq.fly_jump[0] = false;
                }

            }

            //if(pos_ball_x < pos_wall_x - wall_x + pian && globalq.speed.x > 0){ //左边 or
            //}else if(pos_ball_x > pos_wall_x + wall_x - pian && globalq.speed.x < 0){ //右边
            if (pos_ball_x < pos_wall_x && globalq.speed.x > 0){ //左边 or 
            
                if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)){ // 反墙
                    globalq.speed = new Vector2 (-globalq.rebound_x*times, globalq.rebound_y * times);
                    rigidbody_ball.position = new Vector2 (rigidbody_ball.position.x - globalq.rebound_shift_x, rigidbody_ball.position.y + globalq.rebound_shift_y);
                    globalq.set_clip(11);
                } 
                else{  //撞墙
                    globalq.speed = new Vector2 (min(max((globalq.speed.x-globalq.touch_x*times)*-1),globalq.maxx*times),globalq.speed.y);
                    rigidbody_ball.position = new Vector2 (rigidbody_ball.position.x - globalq.touch_shift_x, rigidbody_ball.position.y);
                    globalq.set_clip(10);
                }
                //for (var i = 0; i < 100; i++) {
                //    pos_ball_x = rigidbody_ball.position.x;
                //    if (pos_ball_x < pos_wall_x - wall_x + pian) break;
                //    rigidbody_ball.position = new Vector2 (rigidbody_ball.position.x - 0.01f, rigidbody_ball.position.y); //避免在墙里面
                //}
            }else if (pos_ball_x > pos_wall_x && globalq.speed.x < 0){ //右边
                if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)){ // 反墙
                    globalq.speed = new Vector2(globalq.rebound_x * times, globalq.rebound_y * times);
                    rigidbody_ball.position = new Vector2(rigidbody_ball.position.x + globalq.rebound_shift_x, rigidbody_ball.position.y + globalq.rebound_shift_y);
                    globalq.set_clip(11);
                }
                else{
                    globalq.speed = new Vector2(min(max((globalq.speed.x+globalq.touch_x*times) * -1),globalq.maxx*times), globalq.speed.y);
                    rigidbody_ball.position = new Vector2(rigidbody_ball.position.x + globalq.touch_shift_x, rigidbody_ball.position.y);
                    globalq.set_clip(10);
                }
                //for (var i = 0; i < 100; i++){
                //    pos_ball_x = rigidbody_ball.position.x;
                //    if (pos_ball_x > pos_wall_x - wall_x - pian) break;
                //    rigidbody_ball.position = new Vector2(rigidbody_ball.position.x + 0.01f, rigidbody_ball.position.y); //避免在墙里面
                //}
            }
        }
    }



}
