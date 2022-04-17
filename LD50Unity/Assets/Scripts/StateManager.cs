using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;

public class StateManager : MonoBehaviour
{
    public SpriteRenderer fader;
    public SpriteRenderer bg;
    public State state;

    public GameObject adv;
    public GameObject fight;

    public bool fadeToBlackFlag = false;
    public bool fadeToTransparentFlag = false;
    public bool naniInit = false;

    float fadeTarget;
    float fadeCounter;

    private static StateManager _instance;
    public static StateManager Instance { get { return _instance; } }

    async void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        await RuntimeInitializer.InitializeAsync();
        Instance.naniInit = true;
    }

    void Update()
    {
        if (fadeToBlackFlag)
        {
            fadeCounter += Time.deltaTime;
            fader.color = Color.Lerp(Color.clear, Color.white, fadeCounter / fadeTarget);
            if(fader.color == Color.white)
            {
                fadeToBlackFlag = false;
            }
        }
        else if (fadeToTransparentFlag)
        {
            fadeCounter += Time.deltaTime;
            fader.color = Color.Lerp(Color.white, Color.clear, fadeCounter / fadeTarget);
            if (fader.color == Color.clear)
            {
                fadeToTransparentFlag = false;
            }
        }
    }

    public void startDialogue(string scriptName, string label)
    {
        Instance.state = State.AdvDialogue;

        var inputManager = Engine.GetService<IInputManager>();
        inputManager.ProcessInput = true;

        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        scriptPlayer.PreloadAndPlayAsync(scriptName, label: label).Forget();
    }

    public void startMove()
    {
        Instance.state = State.AdvMove;
        fadeToTransparent(1f);
        Instance.adv.SetActive(true);
    }

    public void startFight()
    {
        fadeToTransparent(1f);
        Instance.state = State.Fight;
        Instance.adv.SetActive(false);
        Instance.fight.SetActive(true);
    }

    public void endFight()
    {
        fadeToTransparent(1f);
        Instance.state = State.AdvMove;
        Instance.fight.SetActive(false);
        Instance.adv.SetActive(true);
    }

    public void restartFight()
    {
        fadeToTransparent(1f);
        Instance.fight.GetComponent<FightManager>().Reset();
    }

    public void fadeToBlack(float time)
    {
        if(!fadeToBlackFlag && !fadeToTransparentFlag)
        {
            fadeToBlackFlag = true;
            fadeTarget = time;
            fadeCounter = 0;
        }
    }

    public void fadeToTransparent(float time)
    {
        if (!fadeToBlackFlag && !fadeToTransparentFlag)
        {
            fadeToTransparentFlag = true;
            fadeTarget = time;
            fadeCounter = 0;
        }
    }
}

public enum State
{
    AdvMove,
    AdvDialogue,
    Fight,
    Novel
}