using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartRound : MonoBehaviour
{
    [SerializeField] WorldData worlddata;

    // Update is called once per frame
    public void onNextRoundClick()
    {
        //Looks for all the children GameObjects in the Canvas
        for (int i = 0; i < worlddata.getCanvas().transform.GetChildCount(); i++)
        {
            GameObject item = worlddata.getCanvas().transform.GetChild(i).gameObject;
            string nameClone = item.name;
            //Looks for only (Cloned) Objects in the canvas and remove those, the progenitors are needed for next round
            if(nameClone.EndsWith(")"))
            {
                Destroy(item);
            }
        }
        worlddata.resetEndGameObjects();
        //Sets the progenitors(Not clones) back at there original positions
        worlddata.resetPositionProgenitors();
        //Start next round
        worlddata.startRound();
    }
}
