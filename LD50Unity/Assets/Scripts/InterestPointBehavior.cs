using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;

public class InterestPointBehavior : MonoBehaviour
{
    public GameObject ip;
    public InteractionType type;
    public string scriptName;
    public string label;

    // Start is called before the first frame update
    void Start()
    {
        ip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(StateManager.Instance.state == State.AdvMove)
        {
            if (Input.GetKeyDown("space") && ip.activeSelf)
            {
                if (type == InteractionType.Dialogue)
                {
                    StateManager.Instance.state = State.AdvDialogue;

                    var inputManager = Engine.GetService<IInputManager>();
                    inputManager.ProcessInput = true;

                    var scriptPlayer = Engine.GetService<IScriptPlayer>();
                    scriptPlayer.PreloadAndPlayAsync(scriptName, label: label).Forget();
                }
            }
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
    Dialogue
}
