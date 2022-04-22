using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLayer : MonoBehaviour
{
    [SerializeField]
    int rocklayerType = 0;

    //The named rock layers, 3 of them exsist 
    enum rockLayers { layerLow, layerMid, layerTop };

    [SerializeField]
    WorldData worldDataHolder;

    [SerializeField]
    GameObject nextLayer;

    int useHammer = 0;

    public void setDestroyedByHammer()
    {
        useHammer = 1;
    }

    public void onMineLayer()
    {
        //Lowest Layer when mined, should not spawn new rock layers and should add Points for revealing items
        if (rocklayerType == (int)rockLayers.layerLow)
        {
            //In var called offsetFix
            Vector3 position = transform.position + new Vector3(7.2498F, -7.962F, 0F);
            //Look for the position in the grid by looking at the position of the GameObject
            int xPosition = (int)(position.x / 1.25F);
            int yPosition = (int)((-position.y + 0.1F) / 1.25F);
            //Debug.Log(xPosition + ":" + yPosition);
            worldDataHolder.setClearedLayerGrid(xPosition, yPosition, 1);


            worldDataHolder.itemScores.addRoundScore(xPosition, yPosition);


        }
        //Should jut spawn the next Rock Layer when mined
        else if (rocklayerType == (int)rockLayers.layerMid || rocklayerType == (int)rockLayers.layerTop)
        {
            GameObject next = GameObject.Instantiate(nextLayer);
            next.transform.position = gameObject.transform.position;
            next.transform.SetParent(worldDataHolder.getCanvas().transform);
            worldDataHolder.getRockLayers().Add(next);
        }
        //Hammer Function
        areaDamage();
        //Destroy this mined Rock Layer
        Destroy(gameObject);
    }

    //The hammer does area damage in a + but will do less damage then what the pickaxe will do when mining all the + parts
    public void areaDamage()
    {
        //For using the hammer to mine in a + and not a single rock layer
        if (useHammer == 0 && worldDataHolder.getUsingTools() == 1)
        {
            //Origin position of this Rock Layer
            Vector3 thisPosition = transform.position;
            //List of all the Rock Layers in the Canvas
            RockLayer[] layers = gameObject.transform.parent.GetComponentsInChildren<RockLayer>();
            //Left, right, up and down layers should be mined too because of the hammer
            foreach (RockLayer rockLayer in layers)
            {
                Vector3 layerPosition = rockLayer.gameObject.transform.position;
                //Mine right side
                if (layerPosition.x == thisPosition.x + 1.25 && layerPosition.y == thisPosition.y)
                {
                    rockLayer.setDestroyedByHammer();
                    rockLayer.onMineLayer();
                    worldDataHolder.addMined(0.25F / 4F);
                }
                //Mine left side
                if (layerPosition.x == thisPosition.x - 1.25 && layerPosition.y == thisPosition.y)
                {
                    rockLayer.setDestroyedByHammer();
                    rockLayer.onMineLayer();
                    worldDataHolder.addMined(0.25F / 4F);
                }

                //Mine up
                if (layerPosition.x == thisPosition.x && layerPosition.y == thisPosition.y + 1.25)
                {
                    rockLayer.setDestroyedByHammer();
                    rockLayer.onMineLayer();
                    worldDataHolder.addMined(0.25F / 4F);
                }

                //Mine Down
                if (layerPosition.x == thisPosition.x && layerPosition.y == thisPosition.y - 1.25)
                {
                    rockLayer.setDestroyedByHammer();
                    rockLayer.onMineLayer();
                    worldDataHolder.addMined(0.25F / 4F);
                }
            }
        }
        else
        {
            //Add the crack
            worldDataHolder.addMined(0.25F);
        }
    }
}
