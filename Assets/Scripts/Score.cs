using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    //Points per item
    enum itemPoints { Diamond = 100, Bone = 45, Sulpher = 25, Vanadanite = 15, Calcite = 10, rock = -3, roundRock = -5, smallRock = -3 };

    enum rounds { roundOne, roundTwo, roundThree };

    //Score of the rounds
    int roundOne = 0;
    int roundTwo = 0;
    int roundThree = 0;

    public void clearScore()
    {
        roundOne = 0;
        roundTwo = 0;
        roundThree = 0;
    }

    public int getRoundOneScore()
    {
        return roundOne;
    }

    public int getRoundTwoScore()
    {
        return roundTwo;
    }

    public int getRoundThreeScore()
    {
        return roundThree;
    }

    void addCurrentRoundScore(int score,int round)
    {
        if(round == (int)rounds.roundOne)
        {
            roundOne += score;
        }
        else if (round == (int)rounds.roundTwo)
        {
            roundTwo += score;
        }
        else if (round == (int)rounds.roundThree)
        {
            roundThree += score;
        }
    }

    WorldData worldDataHolder;

    public Score(WorldData theWorldDataHolder)
    {
        worldDataHolder = theWorldDataHolder;
    }

    //Adds score for the revealed Item that was burried under the Rock Layer
    public void addRoundScore(int xPosition, int yPosition)
    {
        int scoreToAdd = 0;
        if (worldDataHolder.getClearedLayerGrid()[xPosition, yPosition] == 1)
        {
            if (worldDataHolder.getHasItemInGrid()[xPosition, yPosition] == (int)WorldData.items.Diamond)
            {
                scoreToAdd = (int)itemPoints.Diamond;
            }
            else if (worldDataHolder.getHasItemInGrid()[xPosition, yPosition] == (int)WorldData.items.Bone)
            {
                scoreToAdd = (int)itemPoints.Bone;
            }
            else if (worldDataHolder.getHasItemInGrid()[xPosition, yPosition] == (int)WorldData.items.Sulpher)
            {
                scoreToAdd = (int)itemPoints.Sulpher;
            }
            else if (worldDataHolder.getHasItemInGrid()[xPosition, yPosition] == (int)WorldData.items.Vanadanite)
            {
                scoreToAdd = (int)itemPoints.Vanadanite;
            }
            else if (worldDataHolder.getHasItemInGrid()[xPosition, yPosition] == (int)WorldData.items.Calcite)
            {
                scoreToAdd = (int)itemPoints.Calcite;
            }
            else if (worldDataHolder.getHasItemInGrid()[xPosition, yPosition] == (int)WorldData.items.rock)
            {
                scoreToAdd = (int)itemPoints.rock;
            }
            else if (worldDataHolder.getHasItemInGrid()[xPosition, yPosition] == (int)WorldData.items.roundRock)
            {
                scoreToAdd = (int)itemPoints.roundRock;
            }
            else if (worldDataHolder.getHasItemInGrid()[xPosition, yPosition] == (int)WorldData.items.smallRock)
            {
                scoreToAdd = (int)itemPoints.smallRock;
            }

            addCurrentRoundScore(scoreToAdd, worldDataHolder.getCurrentRound());
        }
    }
}
