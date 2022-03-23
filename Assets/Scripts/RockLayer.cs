using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLayer : MonoBehaviour
{
    [SerializeField]
    WorldData worldDataHolder;

    public void onMineLayer()
    {
        worldDataHolder.addMined();
        Destroy(gameObject);
    }
}
