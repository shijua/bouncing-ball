using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_setting : MonoBehaviour
{
    GameObject first;
    GameObject intro;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        first = GameObject.Find("first");
        intro = GameObject.Find("intro");
        back_to_homepage();
    }

    public void test_environment()
    {
        SceneManager.LoadScene(1);
    }

    public void real_environment()
    {
        SceneManager.LoadScene(2);
    }

    public void back_to_homepage()
    {
        first.SetActive(true);
        intro.SetActive(false);
    }

    public void goto_intro()
    {
        intro.SetActive(true);
        first.SetActive(false);
    }    
}
