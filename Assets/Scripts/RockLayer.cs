using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLayer : MonoBehaviour
{
    [SerializeField]
    int rocklayerType = 0;

    [SerializeField]
    WorldData worldDataHolder;

    [SerializeField]
    GameObject nextLayer;

    public void onMineLayer()
    {
        //Enum for Rock Layer
        if (rocklayerType == 1)
        {
            //In var called offsetFix
            Vector3 position = transform.position + new Vector3(7.2498F, -7.962F, 0F);
            //Look for the position in the grid by looking at the position of the GameObject
            int xPosition = (int)(position.x / 1.25F);
            int yPosition = (int)((-position.y + 0.1F) / 1.25F);
            //Debug.Log(xPosition + ":" + yPosition);
            worldDataHolder.setClearedLayerGrid(xPosition + yPosition, 1);
        }
        else if (rocklayerType == 2)
        {
            GameObject next = GameObject.Instantiate(nextLayer);
            next.transform.position = gameObject.transform.position;
            next.transform.SetParent(worldDataHolder.getCanvas().transform);
        }
        else if (rocklayerType == 3)
        {
            GameObject next = GameObject.Instantiate(nextLayer);
            next.transform.position = gameObject.transform.position;
            next.transform.SetParent(worldDataHolder.getCanvas().transform);
        }
        worldDataHolder.addMined();
        Destroy(gameObject);
        
    }

    //GameObject gameobject gameObject.transform.parent.gameObject.GetComponent(typeof(HingeJoint));
}


//Unity gitignor 3de jaars
