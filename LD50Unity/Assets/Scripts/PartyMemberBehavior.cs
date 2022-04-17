using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PartyMemberBehavior : MonoBehaviour
{
    public TextMeshProUGUI HPField;
    public TextMeshProUGUI PPField;
    public GameObject selector;
    public string partyMemberName;
    public int maxHP;
    public int lvl;
    public List<Action> actions = new List<Action>();

    void Awake()
    {
        SetHP(maxHP);
    }

    public int GetHP()
    {
        return int.Parse(HPField.text.Split(':')[1]);
    }

    public int GetPP()
    {
        return int.Parse(PPField.text.Split(':')[1]);
    }

    public void SetHP(int val)
    {
        HPField.text = "HP: " + val.ToString();
    }

    public void SetPP(int val)
    {
        PPField.text = "PP: " + val.ToString();
    }
}

[System.Serializable]
public class Action
{
    public string actionName;
    public int baseDamage;
    public int PPCost;
    public bool isWorking;
}
