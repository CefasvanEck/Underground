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
        if(rocklayerType == 3)
        {

        }
        worldDataHolder.addMined();
        Destroy(gameObject);
    }
}
