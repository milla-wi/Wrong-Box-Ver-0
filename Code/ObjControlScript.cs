using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjControlScript : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerSettings PA;

    public GameObject objspawnOne;
    public GameObject objspawnTwo;
    public GameObject objspawnThree;

    public int spawnNum = 0;
    public string objName = "";

    public Vector3 spawnChoosen;

    public int objUsedOne = 0;
    public int objUsedTwo = 0;
    public int spawnPoint = 0;

    public GameObject Manhole;
    public GameObject Gum;
    public GameObject WaterPuddle;
    public GameObject Cart;
    public GameObject Pavement;
    public GameObject VendingMachine;

    //People
    public GameObject PersonOne;
    public GameObject PersonTwo;
    public GameObject PersonThree;
    public GameObject PersonFour;

    //Thugs
    public GameObject ThugG;
    public GameObject ThugB;
    public GameObject FriendThug;
    
    public bool paused = true;
    public bool objectRealeased = false;

    public int seconds = 3;
    public float timeValue = 3;

    public bool reset = false;
    public bool waterBrake = false;
    public bool friendAppear = false;

    public int halfChance = 0;
    public bool chanceMade = false;

    public int spotCount = 0;
    public int lastObj = 0;
    public int lastSpot = 0;

    public int lastPerson = 0;
    public int lastThug = 0;
    public int thugCount = 0;

    void Awake()
    {
        PA = FindObjectOfType<PlayerSettings>();
    }

    void Start()
    {
        objName = "";
        timeValue = PA.spawnNum;
        seconds = Mathf.RoundToInt(PA.spawnNum);
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            timeValue -= Time.deltaTime;
            seconds = Mathf.RoundToInt(timeValue % 60);
            if (seconds == Mathf.RoundToInt(PA.spawnNum) && !objectRealeased)
            {
                do {objUsedOne = Random.Range(1, 8);
                } while (lastObj == objUsedOne);
                    lastObj = objUsedOne;
                    ObjectSpawned(objUsedOne);
                    objectRealeased = true;
            } else if (seconds < Mathf.RoundToInt(PA.spawnNum) && seconds > 0) {
                objectRealeased = false;
                if (seconds == 1)
                {
                    halfChance = Random.Range(0, 3);
                    if (halfChance == PA.halfPoint && !chanceMade)
                    {
                        do {objUsedOne = Random.Range(1, 8);} while (lastObj == objUsedOne);
                        lastObj = objUsedOne;
                        ObjectSpawned(objUsedOne);
                        chanceMade = true;
                    }
                }
            } else if (seconds <= 0) {
                timeValue = PA.spawnNum;
                seconds = Mathf.RoundToInt(PA.spawnNum);
                chanceMade = false;
            }
        }
    }

    void ObjectSpawned(int objectSpawned)
    {
            switch (objectSpawned)
            {
                case 1:
                    objName = "Manhole";
                do
                {
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                    Instantiate(Manhole, spawnChoosen, Quaternion.identity);
                    break;
                case 2:
                    objName = "Person";
                    WhichPerson();
                    break;
                case 3:
                    objName = "Cart";
                do
                {
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                Instantiate(Cart, spawnChoosen, Quaternion.identity);
                    break;
                case 4:
                    objName = "Pavement";
                do
                {
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                Instantiate(Pavement, spawnChoosen, Quaternion.identity);
                    break;
                case 5:
                    objName = "Gum";
                do
                {
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                Instantiate(Gum, spawnChoosen, Quaternion.identity);
                    break;
                case 6:
                    if (!waterBrake) {
                        objName = "Vending Machine";
                        Instantiate(VendingMachine, objspawnThree.transform.position, Quaternion.identity);
                    if (lastSpot == 3)
                    {
                        spotCount++;
                    }
                        break;
                    } else {
                        objName = "Water Puddle";
                    do
                    {
                        ObjectPlacement();
                    } while (spotCount > 2 && lastSpot == spawnPoint);
                    Instantiate(WaterPuddle, spawnChoosen, Quaternion.identity);
                        break;
                    }
                case 7:
                    objName = "Thug";
                    WhichThug();
                    break;
            }
        Debug.Log("Object Spawned: " + objName);
    }

    void ObjectPlacement()
    {
        spawnPoint = Random.Range(1, 4);
        switch (spawnPoint)
        {
            case 1:
                // one
                spawnChoosen = objspawnOne.transform.position;
                spawnNum = 1;
                break;
            case 2:
                // two
                spawnChoosen = objspawnTwo.transform.position;
                spawnNum = 2;
                break;
            case 3:
                // three
                spawnChoosen = objspawnThree.transform.position;
                spawnNum = 3;
                break;
        }
        if (spawnPoint == lastSpot)
        {
            spotCount++;
            
        } else
        {
            lastSpot = spawnPoint;
            spotCount = 0;
        }
        //Debug.Log("spawnCount: " + spotCount);
    }

    public void GameRestart()
    {
        reset = true;
        paused = true;
        timeValue = spawnNum;
        seconds = Mathf.RoundToInt(spawnNum);
        spawnNum = 0;
        objName = "";
        spawnPoint = 0;
        objUsedOne = 0;
        objectRealeased = false;
    }

    public void WhichPerson()
    {
        int personNum = 0;
        do
        {
            personNum = Random.Range(1, 5);
        } while (personNum == lastPerson);
        switch (personNum)
        {
            case 1:
                do
                {
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                Instantiate(PersonOne, spawnChoosen, Quaternion.identity);
                break;
            case 2:
                do
                {
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                Instantiate(PersonTwo, spawnChoosen, Quaternion.identity);
                break;
            case 3:
                do
                {
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                Instantiate(PersonThree, spawnChoosen, Quaternion.identity);
                break;
            case 4:
                do
                {
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                Instantiate(PersonFour, spawnChoosen, Quaternion.identity);
                break;

        }
    }

    public void WhichThug()
    {
        int thugNum = 0;
        do
        {
            thugNum = Random.Range(1, 4);
            if (thugNum == lastThug || (thugNum == 3 && !friendAppear))
            {
                thugCount++;
                if (thugNum == 3 && !friendAppear)
                {
                    thugNum = lastThug;
                }
            } else
            {
                thugCount = 0;
            }
        } while (thugNum == lastThug && thugCount > 4);
        switch (thugNum)
        {
            case 1:
                do
                {
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                Instantiate(ThugB, spawnChoosen, Quaternion.identity);
                break;
            case 2:
                do
                {
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                Instantiate(ThugG, spawnChoosen, Quaternion.identity);
                break;
            case 3:
                objName = "Friend Punk";
                do
                {
                    
                    ObjectPlacement();
                } while (spotCount > 2 && lastSpot == spawnPoint);
                Instantiate(FriendThug, spawnChoosen, Quaternion.identity);
                break;
        }
    }
}
