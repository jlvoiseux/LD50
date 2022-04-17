using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager6 : MonoBehaviour
{
    // Start is called before the first frame update
    int step = 0;

    void Start()
    {
        StateManager.Instance.bg.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (step == 0 && StateManager.Instance.naniInit == true)
        {
            step++;
            StateManager.Instance.startDialogue("home-novel", "Start");
        }
        else if (step == 1 && StateManager.Instance.bg.enabled)
        {
            step++;
            StateManager.Instance.adv.SetActive(true);
        }
    }
}
