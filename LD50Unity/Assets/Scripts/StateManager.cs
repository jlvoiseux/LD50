using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;

public class StateManager : MonoBehaviour
{
    public SpriteRenderer fader;
    public SpriteRenderer bg;
    public State state;

    public bool fadeToBlackFlag = false;
    public bool fadeToTransparentFlag = false;
    float fadeTarget;
    float fadeCounter;

    private static StateManager _instance;
    public static StateManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    async void Start()
    {
        await RuntimeInitializer.InitializeAsync();
        var switchCommand = new SwitchToAdventureMode { ResetState = false };
        await switchCommand.ExecuteAsync();
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