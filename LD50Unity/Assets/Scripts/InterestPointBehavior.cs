using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Naninovel;

public class InterestPointBehavior : MonoBehaviour
{
    public GameObject ip;
    public InteractionType type;
    public string scriptName;
    public string label;
    public int nextScene;
    public bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        ip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(StateManager.Instance.state == State.AdvMove && !triggered)
        {
            if (Input.GetKeyDown("space") && ip.activeSelf)
            {
                triggered = true;
                if (type == InteractionType.Dialogue)
                {
                    StateManager.Instance.startDialogue(scriptName, label);
                }
                else if(type == InteractionType.NextScene)
                {
                    SceneManager.LoadSceneAsync(nextScene);
                }
            }
        }

        if (triggered)
        {
            ip.SetActive(false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ip.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ip.SetActive(false);
    }
}

public enum InteractionType
{
    Dialogue,
    NextScene
}
