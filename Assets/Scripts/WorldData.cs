using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    int[] hasItemInGrid = new int[96];
    int[] clearedLayerGrid = new int[96];

    public void setClearedLayerGrid(int position, int data)
    {
        clearedLayerGrid[position] = data;
    }

    //Layers 
    [SerializeField]
    GameObject rockLayer1;

    [SerializeField]
    GameObject rockLayer2;

    [SerializeField]
    GameObject rockLayer3;
//End

    [SerializeField]
    int layerOneRarity;

    [SerializeField]
    int layerTwoRarity;

    [SerializeField]
    int layerThreeRarity;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    GameObject canvasItems;

    [SerializeField]
    GameObject crack;
    
//All items
    [SerializeField]
    GameObject sulpher;

    [SerializeField]
    GameObject diamond;

    [SerializeField]
    GameObject vanadanite;

    [SerializeField]
    GameObject bone;

    [SerializeField]
    GameObject rock;

    [SerializeField]
    GameObject round_rock;

    [SerializeField]
    GameObject small_rock;

    //The position x of the crack and when reaching left, the game is over
    private float crackLength = 9.14F;

    //Changes the crack at the top of the screen
    public void addMined()
    {
        crackLength -= 0.25F;
        if (crackLength < 0.29F)
        {
            crackLength = 0.29F;
        }
        Vector3 positionCrack = crack.transform.position;
        crack.transform.position = new Vector3(crackLength, positionCrack.y, positionCrack.z);

        //Crack "animation"
        Vector3 scaleCrack = crack.transform.localScale;
        crack.transform.localScale = new Vector3(scaleCrack.x, scaleCrack.y, -scaleCrack.z);


        //Position x
        //0.29 -> 9.14 = 8.85s
    }

    // Start is called before the first frame update
    void Start()
    {

        //Generate 11 by 7 grid of Rock Layers with a rarity set by the inspector
        for (int i = 0;i < 12;++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                int generatedLayer = 0;
                if (Random.Range(0, 99) < layerOneRarity)
                {
                    //Generate Layer One
                    GameObject layerOne = GameObject.Instantiate(rockLayer1);
                    layerOne.transform.position = new Vector3(1.25F * i, 1.25F * -j, 0);
                    //Copy Canvas Offset fix
                    layerOne.transform.position -= new Vector3(7.2498F, -7.962F, -0.12F);
                    layerOne.transform.SetParent(canvas.transform);
                    ++generatedLayer;
//Items

                    //Generate a possible Layer Two on top of Layer One
                    if (Random.Range(0, 99) < layerTwoRarity)
                    {
                        GameObject layerTwo = GameObject.Instantiate(rockLayer2);
                        layerTwo.transform.position = new Vector3(1.25F * i, 1.25F * -j, 0);
                        //Copy Canvas Offset fix
                        layerTwo.transform.position -= new Vector3(7.2498F, -7.962F, -0.12F);
                        layerTwo.transform.SetParent(canvas.transform);
                        ++generatedLayer;
 //Items
                        if (Random.Range(0, 99) < layerThreeRarity)
                        {
                            GameObject layerThree = GameObject.Instantiate(rockLayer3);
                            layerThree.transform.position = new Vector3(1.25F * i, 1.25F * -j, 0);
                            //Copy Canvas Offset fix
                            layerThree.transform.position -= new Vector3(7.2498F, -7.962F, -0.12F);
                            layerThree.transform.SetParent(canvas.transform);
                            ++generatedLayer;
//Items
                            spawnItem(0, i, j, generatedLayer);
                            spawnItem(1, i, j, generatedLayer);
                            spawnItem(2, i, j, generatedLayer);
                            spawnItem(3, i, j, generatedLayer);
                            spawnItem(4, i, j, generatedLayer);
                            spawnItem(5, i, j, generatedLayer);
                            spawnItem(6, i, j, generatedLayer);
                        }
                    }
                }
            }
        }
        Destroy(rockLayer1);
        Destroy(rockLayer2);
        Destroy(rockLayer3);
//Clean up the progenitor items
        Destroy(diamond);
        Destroy(bone);
        Destroy(sulpher);
        Destroy(vanadanite);
        Destroy(rock);
        Destroy(round_rock);
        Destroy(small_rock);
    }

    // Update is called once per frame
    void Update(){}

    public void spawnItem(int itemType,int x,int y,int generatedLayer)
    {
        GameObject item = null;
       
        if (hasItemInGrid[(x * y) + x] == 0 && hasItemInGrid[(x * y) + x + 1] == 0)
        {
            //Spawn Diamond
            //Check for 2 by 2
            if (itemType == 0 && generatedLayer == 3 && Random.Range(0, 99) < 15 && x < 11 && y < 7 && hasItemInGrid[(x * y) + x + 1] == 0 && hasItemInGrid[((x + 1) * y) + x] == 0 && hasItemInGrid[(x * (y + 1)) + x + 1] == 0)
            {
                item = GameObject.Instantiate(diamond);
                //2 by 2 setting in Int Array
                hasItemInGrid[((x + 1) * y) + x + 1] = 1;
                int nextRow = y + 1;
                hasItemInGrid[(x * nextRow) + x] = 1;
                hasItemInGrid[((x  + 1) * nextRow) + x + 1] = 1;
            }
            //Dont generate 1 spot outside so we do "x < 11"(0 - 11)s
            else if (itemType == 1 && (generatedLayer == 2 ||  generatedLayer == 3) && Random.Range(0, 99) < 25 && x < 11 && hasItemInGrid[(x * y) + x + 1] == 0)
            {
                item = GameObject.Instantiate(bone);
                //Bone is 2 long
                hasItemInGrid[((x + 1) * y) + x + 1] = 1;
            }
            else if (itemType == 2 && Random.Range(0, 99) < 30)
            {
                item = GameObject.Instantiate(sulpher);
            }
            else if (itemType == 3 && Random.Range(0, 99) < 35)
            {
                item = GameObject.Instantiate(vanadanite);
            }
            else if (itemType == 4 && Random.Range(0, 99) < 35)
            {
                item = GameObject.Instantiate(round_rock);
            }
            else if (itemType == 5 && Random.Range(0, 99) < 35)
            {
                item = GameObject.Instantiate(small_rock);
            }
            else if (itemType == 6 && Random.Range(0, 99) < 35)
            {
                item = GameObject.Instantiate(rock);
            }

            if (item != null)
            {
                hasItemInGrid[(x * y) + x] = 1;
                item.transform.position = new Vector3(1.25F * x, 1.25F * -y, 0);
                //Copy Canvas Offset fix
                item.transform.position -= new Vector3(7.2498F, -7.962F, -0.12F);

                item.transform.SetParent(canvasItems.transform);
            }
        }
    }
}
