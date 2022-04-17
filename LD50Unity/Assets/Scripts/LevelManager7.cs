using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager7 : MonoBehaviour
{
    // Start is called before the first frame update
    public InterestPointBehavior ip;
    public AudioSource chaconne;

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
            StateManager.Instance.startDialogue("final-battle-end-novel", "Start");
            StateManager.Instance.adv.SetActive(true);
        }
        else if (step == 1 && ip.triggered && StateManager.Instance.state == State.AdvMove)
        {
            step++;
            chaconne.Play();
            StateManager.Instance.startFight();
            
        }
        else if (step == 2 && StateManager.Instance.state == State.AdvMove)
        {
            step++;
            SceneManager.LoadSceneAsync(0);

        }
    }
}
