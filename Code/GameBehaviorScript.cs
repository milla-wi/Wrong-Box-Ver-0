using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBehaviorScript : MonoBehaviour
{
    // Start is called before the first frame update
   
    public GirlScript Baley;
    public PlayerScript Punk;
    public BackgroundScrollScript BI;
    public ObjControlScript OI;
    public HeartHealthScript HI;

    public enum GAMESTATE {WIN, LOSS, RUNNING, PAUSED, NONE, WATER, LEVEL}
    GAMESTATE a = GAMESTATE.LEVEL;

    public Text gameStateText;

    public GameObject WaterAlert;
    public Text WaterClock;
    public float waterTimeValue = 6;
    public int waterSeconds = 6;

    public bool vendingNear = false;

    public GameObject LossPoint;
    public GameObject GameStateObject;

    public Button reSomething;

    public Button PauseButton;

    public bool waterCountDown = true;

    public float waterWaitValue = 45;
    public int waterWaitSeconds = 45;

    public bool friendCountDown = true;
    public float friendWait = 120;
    public int friendWaitSeconds = 120;

    public bool hissCountDown = true;
    public float hissWait = 35;
    public int hissWaitSeconds = 35;

    public Slider GirlInvisDistance;

    Vector2 invisDistance;
    int differenceInInvis;

    //Vector2
    Vector2 howCloseVec;
    int howCloseDist;

    public Image BaleyGone;
    public HeartHealthScript PunchI;

    public GameObject SpiderAv;
    public GameObject GirlReCharge;

    public PlayerSettings PA;

    public GameObject LevelSelect;

    void Awake()
    {
        PA = FindObjectOfType<PlayerSettings>();
    }

    void Start()
    {
        Punk.animator.enabled = false;
        SpiderAv.SetActive(false);
        HI.heartNum = Punk.lives;
        PunchI.heartNum = Baley.chances;
        invisDistance = LossPoint.transform.position - Baley.disappearTarget.transform.position;
        differenceInInvis = Mathf.RoundToInt(invisDistance.y);

        GirlInvisDistance.minValue = 0;
        GirlInvisDistance.maxValue = differenceInInvis;
        howCloseVec = Baley.GirlReplacement.transform.position - Baley.disappearTarget.transform.position;
        howCloseDist = Mathf.RoundToInt(howCloseVec.y);
        GirlInvisDistance.value = howCloseDist;
        Debug.Log(GirlInvisDistance.value + " Distance");
    }

    // Update is called once per frame
    void Update()
{
        if (PA.levelSelected)
        {
            LevelSelect.SetActive(false);
            PA.levelSelected = false;
            
            a = GAMESTATE.NONE;
            Punk.timerOff = false;
        }

        HI.heartNum = Punk.lives;
        PunchI.heartNum = Baley.chances;
        if (Punk.slow)
        {
            BI.speed = 1.5f;
            PA.spawnNum = PA.slowSpawnNum;
        } else
        {
            BI.speed = BI.startSpeed;
            PA.spawnNum = PA.orgSpawnNum;

        }
        if (!Baley.visible)
        {
            howCloseVec = Baley.GirlReplacement.transform.position - Baley.disappearTarget.transform.position;
            howCloseDist = Mathf.RoundToInt(howCloseVec.y);
            GirlInvisDistance.value = howCloseDist;
            GirlInvisDistance.gameObject.SetActive(true);
        } else
        {
            GirlInvisDistance.gameObject.SetActive(false);
        }
        
        if (Punk.timerOff && a == GAMESTATE.NONE)
        {
            Debug.Log("Game Start");
            BI.scrollOn = true;
            Baley.run = true;
            a = GAMESTATE.RUNNING;
            OI.reset = false;
            OI.paused = false;
            waterCountDown = true;
            Punk.animator.enabled = true;
        }

        if (Punk.timerOff && (a == GAMESTATE.RUNNING || a == GAMESTATE.WATER))
        {
            if (HI.heartNum < Punk.maxLives)
            {
                if (waterCountDown)
                {
                    waterWaitValue -= Time.deltaTime;
                    waterWaitSeconds = Mathf.RoundToInt(waterWaitValue % 45);
                    if (waterWaitSeconds <= 0)
                    {
                        Punk.vendingPossible = true;
                        waterCountDown = false;
                        waterWaitValue = 45;
                        OI.waterBrake = false;
                    }
                }
            }

            if (hissCountDown)
            {
                hissWait -= Time.deltaTime;
                hissWaitSeconds = Mathf.RoundToInt(hissWait % 35);
                if (hissWaitSeconds <= 0)
                {
                    hissWait = 35;
                    hissWaitSeconds = 35;
                    hissCountDown = false;
                    Punk.hissPossible = true;
                    SpiderAv.SetActive(true);
                }
            }

            if (friendCountDown)
            {
                friendWait -= Time.deltaTime;
                friendWaitSeconds = Mathf.RoundToInt(friendWait % 120);
                if (friendWaitSeconds <= 0)
                {
                    friendCountDown = false;
                    friendWait = 45;
                    OI.friendAppear = true;
                }
            }

            Vector2 distance = Baley.Girl.transform.position - Punk.transform.position;

            var differenceInY = Mathf.Abs(distance.y);
            var differenceInX = Mathf.Abs(distance.x);

            if (differenceInY < 20 && !Punk.vendingPossible)
            {
                Punk.vendingPossible = true;
            }
            else if (differenceInY > 20 && Punk.vendingPossible)
            {
                Punk.vendingPossible = false;
            }
            if (differenceInY <= 0.7 && differenceInX <= 0.2) {
                Baley.chances -= 1;

                if (Baley.chances == 0)
                {
                    a = GAMESTATE.WIN;
                    Baley.Girl.transform.position = new Vector2(Baley.Girl.transform.position.x + 0.8f, Baley.Girl.transform.position.y);
                } else {


                    //transform.Translate(Vector3.up * 5.0f * Time.deltaTime, GirlReCharge.transform);
                    Baley.Girl.transform.position = new Vector2(Baley.Girl.transform.position.x, GirlReCharge.transform.position.y);
                }
            } else if (differenceInY <= 0.3 && differenceInX > 0.2) {
                Debug.Log("Close! From Game!");
                Baley.Girl.transform.position = new Vector2(Baley.Girl.transform.position.x, Punk.transform.position.y);
            }

            //if (Punk.slow)
            //{
                //BI.speed -= 1f;
            //}
            
            if ((Punk.lives == 0) || (Baley.GirlReplacement.transform.position.y > LossPoint.transform.position.y))
            {
                a = GAMESTATE.LOSS;
            }
            if (Punk.meetVendingMachine)
            {
                a = GAMESTATE.WATER;
                OI.paused = true;
                BI.scrollOn = false;
                vendingNear = true;
                WaterAlert.SetActive(true);
                WaterClock.text = waterSeconds.ToString();
                Punk.meetVendingMachine = false;
            }
            if (vendingNear)
            {
                waterTimeValue -= Time.deltaTime;
                waterSeconds = Mathf.RoundToInt(waterTimeValue % 60);
                WaterClock.text = waterSeconds.ToString();
                if (waterSeconds <= 0)
                {
                    vendingNear = false;
                    WaterAlert.SetActive(false);
                    OI.paused = false;
                    BI.scrollOn = true;
                    waterTimeValue = 6;
                    waterSeconds = 6;
                    Punk.stopped = false;
                    Punk.vendingCount = false;
                }
            }

            if (Punk.friendMeet && !HI.shield)
            {
                HI.shield = true;
                OI.friendAppear = false;
            } else if (!Punk.friendMeet && HI.shield)
            {
                HI.shield = false;
            }
            if (!Punk.friendMeet && !HI.shield && !friendCountDown)
            {
                friendWait = 120;
                friendWaitSeconds = 120;
                friendCountDown = true;
            }

            if (Punk.hissPossible &&  Punk.hissHappened)
            {
                Punk.hissPossible = false;
                Punk.hissHappened = false;
                SpiderAv.SetActive(false);
            }
        }

        if (a == GAMESTATE.WIN) {
            gameStateText.text = "WIN";
            Punk.animator.enabled = false;
            Punk.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Punk_Win");
            Baley.Girl.GetComponent<Animator>().SetBool("whoops", true);

        } else if (a == GAMESTATE.LOSS) {
            gameStateText.text = "LOSS";
            Punk.animator.SetBool("lost", true);
            Punk.animator.SetBool("pain", false);
            Punk.animator.SetBool("slowed", false);
            BaleyGone.gameObject.SetActive(true);
         } else if (a == GAMESTATE.PAUSED) {
            gameStateText.text = "PAUSED";
        }
        if (a == GAMESTATE.WIN || a == GAMESTATE.LOSS)
        {
            reSomething.GetComponentInChildren<Text>().text = "Restart";
            reSomething.onClick.AddListener(PlayerRestart);
            reSomething.onClick.RemoveListener(PlayerResume);
            waterSeconds = 6;
            waterTimeValue = 6;
            friendCountDown = false;
            waterCountDown = false;
            friendWait = 120;
            friendWaitSeconds = 120;
            hissCountDown = false;
            hissWait = 35;
            hissWaitSeconds = 35;
        }   else if (a == GAMESTATE.PAUSED) {
            reSomething.GetComponentInChildren<Text>().text = "Resume";
            reSomething.onClick.AddListener(PlayerResume);
            reSomething.onClick.RemoveListener(PlayerRestart);
        }
        if (a == GAMESTATE.WIN || a == GAMESTATE.LOSS || a == GAMESTATE.PAUSED)
        {
            OI.paused = true;
            BI.scrollOn = false;
            Baley.run = false;
            GameStateObject.SetActive(true);
            PauseButton.interactable = false;
        }
    }

    public void PlayerRestart()
    {
        Baley.run = false;
        OI.paused = true;
        BI.scrollOn = false;
        vendingNear = false;
        waterSeconds = 6;
        waterTimeValue = 6;
        friendCountDown = true;
        waterCountDown = true;
        waterWaitSeconds = 45;
        waterWaitValue = 45;
        friendWait = 120;
        friendWaitSeconds = 120;
        hissCountDown = false;
        hissWait = 35;
        hissWaitSeconds = 35;
        GameStateObject.SetActive(false);
        BI.GameRestart();
        OI.GameRestart();
        Baley.GameRestart();
        Punk.GameRestart();
        if (a == GAMESTATE.LOSS)
        {
            Punk.animator.SetBool("lost", false);
            BaleyGone.gameObject.SetActive(false);
        }
        if (a == GAMESTATE.WIN)
        {
            Baley.Girl.GetComponent<Animator>().SetBool("whoops", false);
        }
        SpiderAv.SetActive(false);
        Punk.animator.enabled = false;
        Punk.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Punk_Spr_1");
        a = GAMESTATE.LEVEL;
        LevelSelect.SetActive(true);
        Punk.vendingCount = false;
        PauseButton.interactable = true;
    }

    public void PlayerPause()
    {
        a = GAMESTATE.PAUSED;
        
        PauseButton.interactable = false;
        if (waterCountDown)
        {
            waterCountDown = false;
        }
        if (friendCountDown)
        {
            friendCountDown = false;
        }
        if (hissCountDown)
        {
            hissCountDown = false;
        }
    }

    public void PlayerResume()
    {
        a = GAMESTATE.RUNNING;
        OI.paused = false;
        BI.scrollOn = true;
        Baley.run = true;
        friendCountDown = true;
        waterCountDown = true;
        hissCountDown = true;
        GameStateObject.SetActive(false);
        PauseButton.interactable = true;
    }

    public void PlayerWater()
    {
        Punk.stopped = false;
        Punk.lives++;
        Debug.Log("Lives Changed: " + Punk.lives);
        vendingNear = false;
        WaterAlert.SetActive(false);
        OI.paused = false;
        BI.scrollOn = true;
        waterTimeValue = 6;
        waterSeconds = 6;
        OI.waterBrake = true;
        waterCountDown = true;
        a = GAMESTATE.RUNNING;
        Punk.vendingCount = false;
    }

    public void PlayerNoWater()
    {
        vendingNear = false;
        Punk.stopped = false;
        WaterAlert.SetActive(false);
        OI.paused = false;
        BI.scrollOn = true;
        waterTimeValue = 6;
        waterSeconds = 6;
        a = GAMESTATE.RUNNING;
        Punk.vendingCount = false;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
