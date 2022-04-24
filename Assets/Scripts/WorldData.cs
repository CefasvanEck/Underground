using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldData : MonoBehaviour
{
    int[,] hasItemInGrid = new int[12, 8];
    int[,] clearedLayerGrid = new int[12, 8];

    //Which round the player is in
    int currentRound;

    //Has all the round score
    Score itemScores;

    public enum items { empty, Diamond, Bone, Sulpher, Vanadanite, Calcite, rock, roundRock, smallRock };

    enum itemsRare { Diamond = 8, Bone = 11, Sulpher = 7, Vanadanite = 6, Calcite = 7, rock = 8, roundRock = 8, smallRock = 8 };

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

    [SerializeField]
    Text roundText;

    [SerializeField]
    Text finalScoreText;

    [SerializeField]
    Text buttonClickMessage;

    [SerializeField]
    RestartRound restartRound;

    private float difficultyCrackSpeed;

    //Get the Float of how fast the crack will travel after one click
    public float getCrackSpeed()
    {
        return difficultyCrackSpeed;
    }

    //The position x of the crack and when reaching left, the game is over
    private float crackLength;

    //Which tool is used(0 == pickaxe and 1 is Hammer)
    int usingTools;

    //List with cloned Rock layers
    //(Used for when the round is over, all Rock Layers will be deleted and show where all the items were hidden)
    List<GameObject> rockLayers;

    //Hammer or pickaxe that is in use
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

    //Which round the Player is in
    public int getCurrentRound()
    {
        return currentRound;
    }

    //Changes the crack at the top of the screen
    public bool addMined(float addToCrack)
    {
        if (crackLength < 0.29F)
        {
            crackLength = 0.29F;
            return true;
        }
        else
        {
            crackLength -= addToCrack;
            return false;
        }
    }

    //Updates the position of the crack on the screen
    public void updateCrackLength()
    {
        //Update position of crack
        Vector3 positionCrack = crack.transform.position;
        crack.transform.position = new Vector3(crackLength, positionCrack.y, positionCrack.z);
    }

    public void reverseCrack()
    {
        //Crack "animation"(Reverse crack image)
        Vector3 scaleCrack = crack.transform.localScale;
        crack.transform.localScale = new Vector3(scaleCrack.x, scaleCrack.y, -scaleCrack.z);
    }

    //The list of Cloned Rock Layers(Clone)
    public List<GameObject> getRockLayers()
    {
        return rockLayers;
    }

    //Resets crack length, clears arrays with layer and Item data and move the end of round button outside the vieuw
    public void resetEndGameObjects()
    {
        hasItemInGrid = new int[12, 8];
        clearedLayerGrid = new int[12, 8];
        crackLength = 9.14F;
        updateCrackLength();
        endOfRoundButton.transform.position = new Vector3(50, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        difficultyCrackSpeed = 0.25F;
        crackLength = 9.14F;
        usingTools = 0;
        itemScores = new Score(this);
        currentRound = 0;
        rockLayers = new List<GameObject>();
    }

    //Changes the crack peed when mining so you have less clicks when the number is high
    public void changesDifficulty(float speed)
    {
        difficultyCrackSpeed = speed;
    }

    //Starts the first round and hides menu buttons
    public void startGame()
    {
        startRound();
        //Hide the buttons
        this.restartRound.showMenuButtons(false);
    }

    void Update()
    {
        //Make sure that all the layers have been mined and that when
        //crackLength == 0.29F, round is over and should reveal all exsisting Items
        //We do this in the Update() because if we do it after the last Mined Layer, the GameObject.Instantiate does not
        //fire directly(?) and this will cause Rock Layers to appear on the screen after everything should be gone
        if (crackLength == 0.29F)
        {
            setEndMessage();
            crackLength = 0.3F;
        }
    }

    //Writes new message for end of round and places it on the screen
    void setEndMessage()
    {
        ++currentRound;
        endOfRoundButton.transform.position = new Vector3(0, 0, 0);

        //Updating the text or setting the text of the end of rounds
        if (currentRound == 1)
        {
            roundText.text = "Round 1 score: " + itemScores.getRoundOneScore();
            finalScoreText.text = "";
        }
        else if (currentRound == 2)
        {
            roundText.text += ", Round 2 score: " + itemScores.getRoundTwoScore();
        }
        else if (currentRound == 3)
        {
            roundText.text += ", Round 3 score: " + itemScores.getRoundThreeScore();
            //All score from the 3 rounds
            finalScoreText.text = "Final Score: " + (itemScores.getRoundOneScore() + itemScores.getRoundTwoScore() + itemScores.getRoundThreeScore());
            //New message
            buttonClickMessage.text = "Click to return to the Main Menu";
        }

        endOfRoundButton.transform.SetParent(canvas.transform);
        restartRound.revealAllItems();
    }

    //When last round, clear all score because you will be send back to the main menu
    public void resetScore()
    {
        currentRound = 0;
        itemScores.clearScore();
    }

    //Generates all items and layers and moves the progenitors out of vieuw
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
                            rockLayers.Add(layerThree);
                        }
                        else
                        {
                            GameObject layerTwo = GameObject.Instantiate(rockLayer2);
                            setPosition(layerTwo, i, j);
                            generatedLayer = 2;
                            rockLayers.Add(layerTwo);
                        }
                    }
                    else
                    {
                        //Generate Layer One
                        GameObject layerOne = GameObject.Instantiate(rockLayer1);
                        setPosition(layerOne, i, j);
                        generatedLayer = 1;
                        rockLayers.Add(layerOne);
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
        //Move layers outside
        rockLayer1.transform.position += new Vector3(50, 0, 0);
        rockLayer2.transform.position += new Vector3(50, 0, 0);
        rockLayer3.transform.position += new Vector3(50, 0, 0);
        //Move the progenitors outside vieuw field
        diamond.transform.position += new Vector3(50, 0, 0);
        bone.transform.position += new Vector3(50, 0, 0);
        sulpher.transform.position += new Vector3(50, 0, 0);
        vanadanite.transform.position += new Vector3(50, 0, 0);
        calcite.transform.position += new Vector3(50, 0, 0);
        rock.transform.position += new Vector3(50, 0, 0);
        round_rock.transform.position += new Vector3(50, 0, 0);
        small_rock.transform.position += new Vector3(50, 0, 0);
    }

    //For end of the round, reset position progenitors(Which where the clones come from)
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

    //Spawn the item under the rocklayer
    void spawnItem(WorldData.items itemType,int x,int y,int generatedLayer)
    {
        GameObject item = null;
        if (hasItemInGrid[x,y] == 0)
        {
            //Spawn Diamond
            //Check for 2 by 2
            if (itemType == items.Diamond && generatedLayer == 3 && Random.Range(0, 99) < (int)itemsRare.Diamond && x > 0 && y > 0 &&
            hasItemInGrid[x - 1, y] == 0 &&
            hasItemInGrid[x - 1, y - 1] == 0 &&
            hasItemInGrid[x    , y - 1] == 0)
            {
                item = GameObject.Instantiate(diamond);
                hasItemInGrid[x    , y] = (int)items.Diamond;
                hasItemInGrid[x - 1, y] = (int)items.Diamond;
                hasItemInGrid[x - 1, y - 1] = (int)items.Diamond;
                hasItemInGrid[x    , y - 1] = (int)items.Diamond;
            }
            //Bone
            //11 % and 1 by 2
            else if (itemType == items.Bone && generatedLayer == 3 && Random.Range(0, 99) < (int)itemsRare.Bone && x > 1 && y > 1 && (hasItemInGrid[x, y - 1] == 0 || hasItemInGrid[x - 1, y] == 0))
            {
                item = GameObject.Instantiate(bone);
                hasItemInGrid[x, y] = (int)items.Bone;
                if (hasItemInGrid[x, y - 1] == 0 && Random.Range(0, 99) < 50)
                {
                    item.transform.rotation = Quaternion.Euler(0, 0, -90F);
                    hasItemInGrid[x, y - 1] = (int)items.Bone;
                }
                else
                {
                    hasItemInGrid[x - 1, y] = (int)items.Bone;
                }
            }
            //Sulpher
            //7 % and 1 by 1
            else if (itemType == items.Sulpher && Random.Range(0, 99) < (int)itemsRare.Sulpher)
            {
                item = GameObject.Instantiate(sulpher);
                hasItemInGrid[x, y] = (int)items.Sulpher;
            }
            //Vanadanite
            //6 % and 1 by 1
            else if (itemType == items.Vanadanite && Random.Range(0, 99) < (int)itemsRare.Vanadanite)
            {
                item = GameObject.Instantiate(vanadanite);
                hasItemInGrid[x, y] = (int)items.Vanadanite;
            }
            //Calcite
            //6 % and 1 by 1
            else if (itemType == items.Calcite && Random.Range(0, 99) < (int)itemsRare.Calcite)
            {
                item = GameObject.Instantiate(calcite);
                hasItemInGrid[x, y] = (int)items.Calcite;
            }
            //Rock
            //8 % and 1 by 1
            else if (itemType == items.roundRock && Random.Range(0, 99) < (int)itemsRare.roundRock)
            {
                item = GameObject.Instantiate(round_rock);
                hasItemInGrid[x, y] = (int)items.roundRock;
            }
            //8 % and 1 by 1
            else if (itemType == items.smallRock && Random.Range(0, 99) < (int)itemsRare.smallRock)
            {
                item = GameObject.Instantiate(small_rock);
                hasItemInGrid[x, y] = (int)items.smallRock;
            }
            //8 % and 1 by 1
            else if (itemType == items.rock && Random.Range(0, 99) < (int)itemsRare.rock)
            {
                item = GameObject.Instantiate(rock);
                hasItemInGrid[x, y] = (int)items.rock;
            }

            if (item != null)
            {
                item.transform.position = new Vector3(1.25F * x, 1.25F * -y, 0);
                //Copy Canvas Offset fix
                item.transform.position -= new Vector3(7.2498F, -7.962F, -0.12F);
                item.transform.SetParent(canvasItems.transform);
            }
        }
    }

    //Set the right position in the grid by i and j gird array and set it to the right canvas
    public void setPosition(GameObject newLayer, int x, int y)
    {
        newLayer.transform.position = new Vector3(1.25F * x, 1.25F * -y, 0);
        //Copy Canvas Offset fix
        newLayer.transform.position -= new Vector3(7.2498F, -7.962F, -0.12F);
        newLayer.transform.SetParent(canvas.transform);
    }

    //Set the data in the grid of layers
    public void setClearedLayerGrid(int positionX, int positionY, int data)
    {
        clearedLayerGrid[positionX, positionY] = data;
    }

    //Get the canvas where the UI elements are so it will show up
    public Canvas getCanvas()
    {
        return canvas;
    }

    public int[,] getHasItemInGrid()
    {
        return hasItemInGrid;
    }

    public int[,] getClearedLayerGrid()
    {
        return clearedLayerGrid;
    }

    public Score getScore()
    {
        return itemScores;
    }
}
