using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    [SerializeField]
    WorldData worlddata;

    public enum difficultyNames { easy, normal, hard, hardcore };
    int difficultyOnStart = 1;

    public void nextDifficulty()
    {
        string mainText = "Difficulty: ";
        
        if (difficultyOnStart == (int)difficultyNames.easy)
        {
            GetComponentInChildren<Text>().text = mainText + "Easy";
            worlddata.changesDifficulty(0.25F);
        }
        else if (difficultyOnStart == (int)difficultyNames.normal)
        {
            GetComponentInChildren<Text>().text = mainText + "Normal";
            worlddata.changesDifficulty(0.35F);
        }
        else if (difficultyOnStart == (int)difficultyNames.hard)
        {
            GetComponentInChildren<Text>().text = mainText + "Hard";
            worlddata.changesDifficulty(0.5F);
        }
        else if (difficultyOnStart == (int)difficultyNames.hardcore)
        {
            GetComponentInChildren<Text>().text = mainText + "Hardcore";
            worlddata.changesDifficulty(0.75F);
        }

        ++difficultyOnStart;

        if (difficultyOnStart > (int)difficultyNames.hardcore)
        {
            difficultyOnStart = 0;
        }
    }
}
