using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer_Generator : MonoBehaviour
{
    [SerializeField]
    GameObject rockLayer1;

    [SerializeField]
    GameObject rockLayer2;

    [SerializeField]
    GameObject rockLayer3;

    [SerializeField]
    int layerOneRarity;

    [SerializeField]
    int layerTwoRarity;

    [SerializeField]
    int layerThreeRarity;


    // Start is called before the first frame update
    void Start()
    {
        //Generate 11 by 7 grid of Rock Layers with a rarity set by the inspector
        for (int i = 0;i < 12;++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if (Random.Range(0, 99) < layerOneRarity)
                {
                    //Generate Layer One
                    GameObject layerOne = GameObject.Instantiate(rockLayer1);
                    layerOne.transform.position += new Vector3(1.25F * i, 1.25F * -j, 0);
                    //Generate a possible Layer Two on top of Layer One
                    if (Random.Range(0, 99) < layerTwoRarity)
                    {
                        GameObject layerTwo = GameObject.Instantiate(rockLayer2);
                        layerTwo.transform.position += new Vector3(1.25F * i, 1.25F * -j, -0.2F);

                        if (Random.Range(0, 99) < layerThreeRarity)
                        {
                            GameObject layerThree = GameObject.Instantiate(rockLayer3);
                            layerThree.transform.position += new Vector3(1.25F * i, 1.25F * -j, -0.4F);
                        }
                    }
                }
            }
        }
        //Rarity or else it will be destroyed(not "Generated" but it is the origin point)
        if (Random.Range(0, layerOneRarity) > 0)
        {
            Destroy(rockLayer1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
