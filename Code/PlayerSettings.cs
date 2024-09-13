using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    public string level;

    public float orgSpawnNum = 3;
    public float slowSpawnNum = 6;
    public float spawnNum = 3;
    public int halfPoint = 1;

    public bool levelSelected = false;
   

    public void LevelSelect(Text playerLevel)
    {
        level = playerLevel.text;
        switch (playerLevel.text)
        {
            case "Easy":
                orgSpawnNum = 4;
                break;
            case "Normal":
                orgSpawnNum = 3;
                //doubleSpawn = true;
                break;
            case "Hard":
                orgSpawnNum = 2.5f;
                //doubleSpawn = true;
                break;
        }
        spawnNum = orgSpawnNum;
        halfPoint = (Mathf.RoundToInt(spawnNum) / 2);
        slowSpawnNum = orgSpawnNum * 2;
        levelSelected = true;
    }
}
