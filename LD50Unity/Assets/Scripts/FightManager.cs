using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;

public class FightManager : MonoBehaviour
{
    public GameObject partyContainer;
    public TextMeshProUGUI currMemberNameField;
    public TextMeshProUGUI currMemberLvlField;
    public TextMeshProUGUI logField;
    public Transform currMemberActionsContainer;
    public GameObject actionPrefab;
    public TextMeshProUGUI foeHPField;
    public Foe currFoe;
    public GameObject blackKnight;
    public GameObject abomination;
    public GameObject revLeader;

    public AudioSource select;
    public AudioSource hit;

    PartyMemberBehavior[] party;
    int statePointer = -1;
    int actionNum = 0;
    bool inputState = false;
    Dictionary<string, Action> actionQueue;

    int prevFoeHP;
    int foeHP;
    int turnCount;
    bool guard;
    int fightDone;

    public void Reset()
    {
        StateManager.Instance.state = State.Fight;
        switch (currFoe)
        {
            case Foe.BlackKnightStart:
                foeHP = 1;
                blackKnight.SetActive(true);
                break;
            case Foe.BlackKnightEnd:
                foeHP = 250;
                blackKnight.SetActive(true);
                break;
            case Foe.Abomination:
                foeHP = 50;
                abomination.SetActive(true);
                break;
            case Foe.RevolutionLeader:
                foeHP = 200;
                revLeader.SetActive(true);
                break;
            default:
                blackKnight.SetActive(true);
                break;
        }

        prevFoeHP = foeHP;
        foeHPField.text = "HP: " + foeHP.ToString();
        party = partyContainer.GetComponentsInChildren<PartyMemberBehavior>();
        foreach (PartyMemberBehavior pmb in party)
        {
            pmb.SetPP(0);
            pmb.SetHP(pmb.maxHP);
        }
        actionQueue = new Dictionary<string, Action>();
        logField.text = "";
        turnCount = 1;
        guard = false;
        actionNum = 0;
        statePointer = -1;
        fightDone = 0;

        nextState();

    }

    void OnEnable()
    {
        Reset();
    }

    void Update()
    {
        if (fightDone == 0)
        {
            if (inputState)
            {
                if (Input.GetKeyDown("space"))
                {
                    if (currMemberActionsContainer.GetComponentsInChildren<ActionManager>()[actionNum].correspondingAction.PPCost <= party[statePointer].GetPP())
                    {
                        select.Play();
                        party[statePointer].SetPP(party[statePointer].GetPP() - currMemberActionsContainer.GetComponentsInChildren<ActionManager>()[actionNum].correspondingAction.PPCost);
                        actionQueue[party[statePointer].partyMemberName] = currMemberActionsContainer.GetComponentsInChildren<ActionManager>()[actionNum].correspondingAction;
                        logField.text = "";
                        nextState();
                    }
                    else
                    {
                        logField.text = "Insufficient PP";
                    }
                }
                else if (Input.GetKeyDown("z") || Input.GetKeyDown("w") || Input.GetKeyDown("up"))
                {
                    if (actionNum > 0)
                    {
                        select.Play();
                        currMemberActionsContainer.GetComponentsInChildren<ActionManager>()[actionNum].selector.SetActive(false);
                        actionNum -= 1;
                        currMemberActionsContainer.GetComponentsInChildren<ActionManager>()[actionNum].selector.SetActive(true);
                    }
                }
                else if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
                {
                    if (actionNum < currMemberActionsContainer.GetComponentsInChildren<ActionManager>().Length-1)
                    {
                        select.Play();
                        currMemberActionsContainer.GetComponentsInChildren<ActionManager>()[actionNum].selector.SetActive(false);
                        actionNum += 1;
                        currMemberActionsContainer.GetComponentsInChildren<ActionManager>()[actionNum].selector.SetActive(true);
                    }
                }
            }
        }
        else if (fightDone == 1)
        {
            if (Input.GetKeyDown("space"))
            {
                StateManager.Instance.endFight();
            }
        }
        else if (fightDone == 2)
        {
            if (Input.GetKeyDown("space"))
            {
                StateManager.Instance.restartFight();
            }
        }

    }

    void nextState()
    {
        if(statePointer >= 0 && statePointer < party.Length)
        {
            party[statePointer].selector.SetActive(false);
        }
        
        statePointer += 1;

        if(statePointer == 0)
        {
            foreach(PartyMemberBehavior pmb in party)
            {
                if(pmb.GetHP() <= 0)
                {
                    endFight(false);
                    return;
                }
                pmb.SetPP(pmb.GetPP() + 1);
            }
        }

        if (statePointer >= 0 && statePointer < party.Length)
        {
            inputState = true;
        }

        if (statePointer < party.Length)
        {
            currMemberNameField.text = party[statePointer].partyMemberName;
            currMemberLvlField.text = "Lvl. " + party[statePointer].lvl.ToString();
            foreach(ActionManager am in currMemberActionsContainer.GetComponentsInChildren<ActionManager>())
            {
                Destroy(am.gameObject);
            }
            int count = 0;
            foreach (Action a in party[statePointer].actions)
            {
                GameObject actionContainerGO = Instantiate(actionPrefab, currMemberActionsContainer);
                ActionManager am = actionContainerGO.GetComponent<ActionManager>();
                am.correspondingAction = a;
                am.Populate();
                if(count == 0)
                {
                    am.selector.SetActive(true);
                }
                else
                {
                    am.selector.SetActive(false);
                }
                count++;
            }
            party[statePointer].selector.SetActive(true);
            actionNum = 0;
        }
        else if(statePointer < 2 * party.Length)
        {
            inputState = false;
            currMemberNameField.text = "";
            currMemberLvlField.text = "";
            foreach (ActionManager am in currMemberActionsContainer.GetComponentsInChildren<ActionManager>())
            {
                Destroy(am.gameObject);
            }
            if (actionQueue[party[statePointer - party.Length].partyMemberName].isWorking)
            {
                int dmg = actionQueue[party[statePointer - party.Length].partyMemberName].baseDamage + (int)(UnityEngine.Random.Range(-0.25f, 0.25f) * actionQueue[party[statePointer - party.Length].partyMemberName].baseDamage);
                if (dmg > 0)
                {
                    logField.text = party[statePointer - party.Length].partyMemberName.ToUpper() + " inflicts " + dmg.ToString() + " DMG with " + actionQueue[party[statePointer - party.Length].partyMemberName].actionName + "!";
                    foeHP = Math.Max(0, foeHP - dmg);
                    foeHPField.text = "HP: " + foeHP.ToString();
                    hit.Play();
                    StartCoroutine(WaitForAttack(1));
                }
                else if (dmg < 0)
                {
                    logField.text = party[statePointer - party.Length].partyMemberName.ToUpper() + " enters a defensive stance.";
                    guard = true;
                    StartCoroutine(WaitForAttack(1));
                }
                else
                {
                    logField.text = party[statePointer - party.Length].partyMemberName.ToUpper() + " skipped its turn. A strategic choice?";
                    StartCoroutine(WaitForAttack(1));
                }
            }
            else
            {
                switch (currFoe)
                {
                    case Foe.Abomination:
                        logField.text = "The CRYSTAL MAGE seems to have lost its powers...";
                        StartCoroutine(WaitForAttack(1));
                        break;
                    case Foe.RevolutionLeader:
                        logField.text = "THe RIGHTFUL KING's authority is no more...";
                        StartCoroutine(WaitForAttack(1));
                        break;
                }
            }
        }
        else
        {
            if(foeHP > 0)
            {
                foeAttack();
                StartCoroutine(WaitForAttack(2));
                turnCount += 1;
                statePointer = -1;
                prevFoeHP = foeHP;
            }
            else
            {
                endFight(true);
            }
            
        }
    }

    void foeAttack()
    {
        if(currFoe == Foe.BlackKnightStart)
        {
            logField.text = "BLACK KNIGHT is at its wits'end...";
        }
        else if(currFoe == Foe.Abomination)
        {
            if(turnCount % 2 == 0)
            {
                logField.text = "ABOMINATION inflicts 1000000 DMG with Gates of Babylon!";
                if (!guard)
                {
                    hit.Play();
                    foreach (PartyMemberBehavior pmb in party)
                    {
                        pmb.SetHP(Math.Max(pmb.GetHP() - 1000000, 0));
                    }
                }
                else
                {
                    guard = false;
                    logField.text += " ...but one of the party members parried the attack !";
                }
                
            }
            else
            {
                logField.text = "ABOMINATION is preparing to attack...";
            }
        }
        else if(currFoe == Foe.RevolutionLeader)
        {
            float dice = UnityEngine.Random.Range(0f, 1f);
            if(dice > 0.25f)
            {
                logField.text = "REVOLUTION LEADER inflicts 12 DMG with Gavroche Punch!";
                if (!guard)
                {
                    hit.Play();
                    foreach (PartyMemberBehavior pmb in party)
                    {
                        pmb.SetHP(Math.Max(pmb.GetHP() - 12, 0));
                    }
                }
                else
                {
                    guard = false;
                    logField.text += " ...but one of the party members parried the attack !";
                }
            }
            else
            {
                logField.text = "Your puny attacks cannot stop the revolution! REVOLUTION LEADER recovers " + (prevFoeHP - foeHP).ToString() + " HP!";
                foeHP = prevFoeHP;
                foeHPField.text = "HP: " + foeHP.ToString();
            }
        }
        else if (currFoe == Foe.BlackKnightEnd)
        {
            float dice = UnityEngine.Random.Range(0f, 1f);
            if (dice > 0.25f)
            {
                if (!guard)
                {
                    hit.Play();
                    logField.text = "BLACK KNIGHT inflicts 15 DMG with Rift of Agony!";
                    foreach (PartyMemberBehavior pmb in party)
                    {
                        pmb.SetHP(Math.Max(pmb.GetHP() - 15, 0));
                    }
                }
                else
                {
                    guard = false;
                    logField.text += " ...but one of the party members parried the attack !";
                }
                
            }
            else
            {
                logField.text = "An obsidian fog covers the battlefield. BLACK KNIGHT steals 1 PP!";
                foreach (PartyMemberBehavior pmb in party)
                {
                    pmb.SetPP(Math.Max(pmb.GetPP() - 1, 0));
                }
            }
        }
    }

    void endFight(bool victory)
    {
        if (victory)
        {
            fightDone = 1;
            logField.text = "Victory ! Press SPACE to continue.";
        } 
        else
        {
            fightDone = 2;
            inputState = false;
            currMemberNameField.text = "";
            currMemberLvlField.text = "";
            foreach (ActionManager am in currMemberActionsContainer.GetComponentsInChildren<ActionManager>())
            {
                Destroy(am.gameObject);
            }
            logField.text = "The party was wiped out. Press SPACE to restart the fight.";
        }
    }

    IEnumerator WaitForAttack(int time)
    {
        yield return new WaitForSeconds(time);
        nextState();
    }
}

public enum Foe
{
    BlackKnightStart,
    BlackKnightEnd,
    Abomination,
    RevolutionLeader
}
