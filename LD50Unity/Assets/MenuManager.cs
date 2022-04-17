using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Sprite credits;
    public Sprite menu;
    
    bool isMenu = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("backspace"))
        {
            if (isMenu)
            {
                isMenu = false;
                GetComponent<Image>().sprite = credits;
            }
            else
            {
                isMenu = true;
                GetComponent<Image>().sprite = menu;
            }
        }
        else if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
