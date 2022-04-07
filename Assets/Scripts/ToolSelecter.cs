using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSelecter : MonoBehaviour
{
    [SerializeField]
    WorldData worldDataHolder;

    [SerializeField]
    int theToolID = 0;

    [SerializeField]
    GameObject otherButton;

    void Start()
    {
        //Set Hammer off and Pickaxe on
        if(theToolID == 1)
        {
            otherButton.active = false;
        }
    }

    //When bright, the button is selected and the other should be dark
    public void onToolClicked()
    {
        worldDataHolder.setSelectedTool(theToolID);
        otherButton.active = true;
        gameObject.active = false;
    }
}
