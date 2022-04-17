using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionManager : MonoBehaviour
{
    public Action correspondingAction;
    public GameObject selector;
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI PPCostField;

    public void Populate()
    {
        nameField.text = correspondingAction.actionName;
        PPCostField.text = "PP Cost: " + correspondingAction.PPCost.ToString();
    }
}
