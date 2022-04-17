using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager1 : MonoBehaviour
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
            StateManager.Instance.startDialogue("final-battle-start-novel", "Start");
        }
        else if (step == 1 && StateManager.Instance.bg.enabled)
        {
            step++;
            StateManager.Instance.startFight();
        }
        else if (step == 2 && StateManager.Instance.state == State.AdvMove)
        {
            step++;
            StateManager.Instance.startDialogue("final-battle-start-dialogue", "Forgiveness");
        }
        else if (step == 3 && StateManager.Instance.state == State.AdvMove)
        {
            step++;
            SceneManager.LoadSceneAsync(2);
        }
    }
}
