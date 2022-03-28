using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLayer : MonoBehaviour
{
    [SerializeField]
    int rocklayerType = 0;

    [SerializeField]
    WorldData worldDataHolder;

    public void onMineLayer()
    {
        if(rocklayerType == 1)
        {
            Vector3 position = transform.position + new Vector3(7.2498F, -7.962F, 0F);
            //Look for the position in the grid by looking at the position of the GameObject
            int xPosition = (int)(position.x / 1.25F);
            int yPosition = (int)((-position.y + 0.1F) / 1.25F);
            //Debug.Log(xPosition + ":" + yPosition);
            worldDataHolder.setClearedLayerGrid(xPosition + yPosition, 1);
        }
        worldDataHolder.addMined();
        Destroy(gameObject);
    }
}
