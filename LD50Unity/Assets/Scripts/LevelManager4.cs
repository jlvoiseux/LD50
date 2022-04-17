using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager4 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rk;
    public GameObject rl;
    public GameObject ip1;
    public GameObject ip2;
    public GameObject ip3;

    int step = 0;

    void Start()
    {
        StateManager.Instance.bg.enabled = false;
        rk.SetActive(true);
        rl.SetActive(true);
        ip1.SetActive(false);
        ip2.SetActive(false);
        ip3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (step == 0 && StateManager.Instance.naniInit == true)
        {
            step++;
            StateManager.Instance.startDialogue("castle-novel", "Start");
        }
        else if (step == 1 && StateManager.Instance.bg.enabled)
        {
            step++;
            StateManager.Instance.adv.SetActive(true);
            StateManager.Instance.startDialogue("castle-dialogue", "Pre");
        }
        else if (step == 2 && StateManager.Instance.state == State.AdvMove)
        {
            step++;
            StateManager.Instance.startFight();    
        }
        else if (step == 3 && StateManager.Instance.state == State.AdvMove)
        {
            step++;
            rk.SetActive(false);
            rl.SetActive(false);
            ip1.SetActive(true);
            ip2.SetActive(true);
            ip3.SetActive(true);
        }
    }
}
