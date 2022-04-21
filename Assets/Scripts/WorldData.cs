using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldData : MonoBehaviour
{
    int[,] hasItemInGrid = new int[12,8];
    int[,] clearedLayerGrid = new int[12,8];

    enum items { Diamond, Bone, Sulpher, Vanadanite, Calcite, rock, roundRock, smallRock };
    
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
    GameObject calcite;

    [SerializeField]
    GameObject bone;

    [SerializeField]
    GameObject rock;

    [SerializeField]
    GameObject round_rock;

    [SerializeField]
    GameObject small_rock;

    [SerializeField]
    GameObject endOfRoundButton;

    //The position x of the crack and when reaching left, the game is over
    private float crackLength = 9.14F;

    //Which tool is used(0 == pickaxe and 1 is Hammer)
    int usingTools = 0;

    //Score of the rounds
    int roundOne = 0;
    int roundTwo = 0;
    int roundThree = 0;

    public int getUsingTools()
    {
        return usingTools;
    }

    //For selecting Tools like Pickaxe(0) and Hammer(1)
    public void setSelectedTool(int toolType)
    {
        if(toolType < 2 && toolType > -1)
        {
            usingTools = toolType;
        }
    }

    //Changes the crack at the top of the screen
    public void addMined(float addToCrack)
    {
        crackLength -= addToCrack;
        if (crackLength < 0.29F)
        {
            crackLength = 0.29F;
            setEndMessage();
        }
        Vector3 positionCrack = crack.transform.position;
        crack.transform.position = new Vector3(crackLength, positionCrack.y, positionCrack.z);

        //Crack "animation"
        Vector3 scaleCrack = crack.transform.localScale;
        crack.transform.localScale = new Vector3(scaleCrack.x, scaleCrack.y, -scaleCrack.z);
        //Position x
        //0.29 -> 9.14 = 8.85s
    }

    //Writes new message for end of round and places it on the screen
    public void setEndMessage()
    {
        endOfRoundButton.transform.position = new Vector3(0, 0, 0);
        //Updating the text or setting the text of the end of rounds
        if (roundOne == 0)
        {
            endOfRoundButton.GetComponentInChildren<Text>().text = "Round 1 score: " + roundOne;
        }
        else if (roundTwo == 0)
        {
            endOfRoundButton.GetComponentInChildren<Text>().text += ", Round 2 score: " + roundTwo;
        }
        else if (roundThree == 0)
        {
            endOfRoundButton.GetComponentInChildren<Text>().text += ", Round 3 score: " + roundThree;
        }

        endOfRoundButton.transform.SetParent(canvas.transform);
    }

    public void resetEndGameObjects()
    {
        crackLength = 9.14F;
        endOfRoundButton.transform.position = new Vector3(50, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        startRound();
    }

    public void startRound()
    {
        //Generate 11 by 7 grid of Rock Layers with a rarity set by the inspector
        for (int i = 11; i >= 0; --i)
        {
            for (int j = 7; j >= 0; --j)
            {
                int generatedLayer = 0;
                if (Random.Range(0, 99) < layerOneRarity)
                {
                    //Generate a possible Layer Two on top of Layer One
                    if (Random.Range(0, 99) < layerTwoRarity)
                    {
                        if (Random.Range(0, 99) < layerThreeRarity)
                        {
                            GameObject layerThree = GameObject.Instantiate(rockLayer3);
                            setPosition(layerThree, i, j);
                            generatedLayer = 3;
                        }
                        else
                        {
                            GameObject layerTwo = GameObject.Instantiate(rockLayer2);
                            setPosition(layerTwo, i, j);
                            generatedLayer = 2;
                        }
                    }
                    else
                    {
                        //Generate Layer One
                        GameObject layerOne = GameObject.Instantiate(rockLayer1);
                        setPosition(layerOne, i, j);
                        generatedLayer = 1;
                    }

                    spawnItem(items.Diamond, i, j, generatedLayer);
                    spawnItem(items.Bone, i, j, generatedLayer);
                    spawnItem(items.Sulpher, i, j, generatedLayer);
                    spawnItem(items.Vanadanite, i, j, generatedLayer);
                    spawnItem(items.Calcite, i, j, generatedLayer);
                    spawnItem(items.rock, i, j, generatedLayer);
                    spawnItem(items.roundRock, i, j, generatedLayer);
                    spawnItem(items.smallRock, i, j, generatedLayer);
                }
                else
                {
                    setClearedLayerGrid(i, j, 1);
                }
            }
        }
        //Move the progenitors outside vieuw field
        //Move layers outside
        rockLayer1.transform.position += new Vector3(50, 0, 0);
        rockLayer2.transform.position += new Vector3(50, 0, 0);
        rockLayer3.transform.position += new Vector3(50, 0, 0);
        //Clean up the progenitor items
        diamond.transform.position += new Vector3(50, 0, 0);
        bone.transform.position += new Vector3(50, 0, 0);
        sulpher.transform.position += new Vector3(50, 0, 0);
        vanadanite.transform.position += new Vector3(50, 0, 0);
        calcite.transform.position += new Vector3(50, 0, 0);
        rock.transform.position += new Vector3(50, 0, 0);
        round_rock.transform.position += new Vector3(50, 0, 0);
        small_rock.transform.position += new Vector3(50, 0, 0);
    }

    //For end of the round, reset position progenitors
    public void resetPositionProgenitors()
    {
        rockLayer1.transform.position -= new Vector3(50, 0, 0);
        rockLayer2.transform.position -= new Vector3(50, 0, 0);
        rockLayer3.transform.position -= new Vector3(50, 0, 0);
        //Clean up the progenitor items
        diamond.transform.position -= new Vector3(50, 0, 0);
        bone.transform.position -= new Vector3(50, 0, 0);
        sulpher.transform.position -= new Vector3(50, 0, 0);
        vanadanite.transform.position -= new Vector3(50, 0, 0);
        calcite.transform.position -= new Vector3(50, 0, 0);
        rock.transform.position -= new Vector3(50, 0, 0);
        round_rock.transform.position -= new Vector3(50, 0, 0);
        small_rock.transform.position -= new Vector3(50, 0, 0);
    }

    //Set the right position in the grid by i and j gird array and set it to the right canvas
    public void setPosition(GameObject newLayer,int x,int y)
    {
        newLayer.transform.position = new Vector3(1.25F * x, 1.25F * -y, 0);
        //Copy Canvas Offset fix
        newLayer.transform.position -= new Vector3(7.2498F, -7.962F, -0.12F);
        newLayer.transform.SetParent(canvas.transform);
    }

    // Update is called once per frame
    void Update(){}
    
    void spawnItem(WorldData.items itemType,int x,int y,int generatedLayer)
    {
        GameObject item = null;
        //int[,]
        //Enum for Items
        //% items more clear
        if (hasItemInGrid[x,y] == 0)
        {
            //Spawn Diamond
            //Check for 2 by 2
            //8 * 2 by 2

            //8 % and 2 by 2
            if (itemType == items.Diamond && generatedLayer == 3 && Random.Range(0, 99) < 8 && x > 0 && y > 0 &&
            hasItemInGrid[x - 1, y] == 0 &&
            hasItemInGrid[x - 1, y - 1] == 0 &&
            hasItemInGrid[x    , y - 1] == 0)
            {
                item = GameObject.Instantiate(diamond);
                hasItemInGrid[x - 1, y] = 1;
                hasItemInGrid[x - 1, y - 1] = 1;
                hasItemInGrid[x    , y - 1] = 1;
            }
            //11 % and 1 by 2
            else if (itemType == items.Bone && generatedLayer == 3 && Random.Range(0, 99) < 11 && x > 1 && y > 1 && (hasItemInGrid[x, y - 1] == 0 || hasItemInGrid[x - 1, y] == 0))
            {
                item = GameObject.Instantiate(bone);
                if (hasItemInGrid[x, y - 1] == 0 && Random.Range(0, 99) < 50)
                {
                    item.transform.rotation = Quaternion.Euler(0, 0, -90F);
                    hasItemInGrid[x, y - 1] = 1;
                }
                else
                {
                    hasItemInGrid[x - 1, y] = 1;
                }
            }
            //7 % and 1 by 1
            else if (itemType == items.Sulpher && Random.Range(0, 99) < 7)
            {
                item = GameObject.Instantiate(sulpher);
            }
            //6 % and 1 by 1
            else if (itemType == items.Vanadanite && Random.Range(0, 99) < 6)
            {
                item = GameObject.Instantiate(vanadanite);
            }
            //6 % and 1 by 1
            else if (itemType == items.Calcite && Random.Range(0, 99) < 7)
            {
                item = GameObject.Instantiate(calcite);
            }
            //8 % and 1 by 1
            else if (itemType == items.roundRock && Random.Range(0, 99) < 8)
            {
                item = GameObject.Instantiate(round_rock);
            }
            //8 % and 1 by 1
            else if (itemType == items.smallRock && Random.Range(0, 99) < 8)
            {
                item = GameObject.Instantiate(small_rock);
            }
            //8 % and 1 by 1
            else if (itemType == items.rock && Random.Range(0, 99) < 8)
            {
                item = GameObject.Instantiate(rock);
            }

            if (item != null)
            {
                hasItemInGrid[x,y] = 1;
                item.transform.position = new Vector3(1.25F * x, 1.25F * -y, 0);
                //Copy Canvas Offset fix
                item.transform.position -= new Vector3(7.2498F, -7.962F, -0.12F);

                item.transform.SetParent(canvasItems.transform);
            }
        }
    }

    public void setClearedLayerGrid(int positionX, int positionY, int data)
    {
        clearedLayerGrid[positionX, positionY] = data;
    }

    //Get the canvas where the UI elements are so it will show up
    public Canvas getCanvas()
    {
        return canvas;
    }
}
