using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class win_back : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        globalq.level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene(0);
    }
}
