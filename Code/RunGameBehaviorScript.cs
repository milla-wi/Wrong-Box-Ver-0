using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class RunGameBehaviorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public RunPlayerScript Baley;
    public BackgroundScrollScript BI;
    public ObjControlScript OI;
    public HeartHealthScript HI;

    public enum GAMESTATE {OVER, RUNNING, PAUSED, NONE, WATER}
    GAMESTATE a = GAMESTATE.NONE;

    public Text gameStateText;

    public GameObject WaterAlert;
    public Text WaterClock;
    public float waterTimeValue = 6;
    public int waterSeconds = 6;

    public bool vendingNear = false;

    public GameObject GameStateObject;

    public Button reSomething;

    public Button PauseButton;

    public bool waterCountDown = true;

    public int StepTimer = 0;
    public float waterWaitValue = 45;
    public int waterWaitSeconds = 45;
    public int slowSteps = 0;
    public float slowTimeTotal = 0;

    public int HighScore = 0;
    public Text ScoreText;

    float timeValue = 0;

    public Slider HighScoreDistance;

    void Start()
    {
        GrabHighScore();
        OI.waterBrake = true;
        HI.heartNum = Baley.lives;

        if (HighScore > 0)
        {
            HighScoreDistance.maxValue = HighScore;
        } else
        {
            HighScoreDistance.maxValue = 0;
        }
        HighScoreDistance.minValue = 0;

    }

    // Update is called once per frame
    void Update()
    {
        HI.heartNum = Baley.lives;
        if (Baley.slow)
        {
            BI.speed = 1.2f;
        }
        else
        {
            BI.speed = BI.startSpeed;
        }
        if (Baley.timerOff && a == GAMESTATE.NONE)
        {
            Debug.Log("Game Start");
            BI.scrollOn = true;
            a = GAMESTATE.RUNNING;
            OI.reset = false;
            OI.paused = false;
        }

        if (Baley.timerOff && (a == GAMESTATE.RUNNING || a == GAMESTATE.WATER))
        {
            timeValue += Time.deltaTime;
            
            if (Baley.slow)
            {
                slowTimeTotal += Time.deltaTime;
                slowSteps = Mathf.RoundToInt(slowTimeTotal % 2);
            }
            StepTimer = Mathf.RoundToInt(timeValue) - slowSteps;
            HighScoreDistance.value = StepTimer;
            if (StepTimer > HighScore)
            {
                HighScoreDistance.maxValue = StepTimer;
            }
            
            if (waterCountDown)
            {
                waterWaitValue -= Time.deltaTime;
                waterWaitSeconds = Mathf.RoundToInt(waterWaitValue % 45);
                if (waterWaitSeconds <= 0)
                {
                    Baley.vendingPossible = true;
                    waterCountDown = false;
                    waterWaitValue = 45;
                    OI.waterBrake = false;
                }
            }

            if (Baley.lives == 0)
            {
                a = GAMESTATE.OVER;
            }
            if (Baley.meetVendingMachine)
            {
                a = GAMESTATE.WATER;
                OI.paused = true;
                BI.scrollOn = false;
                vendingNear = true;
                WaterAlert.SetActive(true);
                WaterClock.text = waterSeconds.ToString();
                Baley.meetVendingMachine = false;
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
                    Baley.stopped = false;
                    Baley.vendingCount = false;
                    waterCountDown = true;
                    OI.waterBrake = true;
                }
            }
        }

        if (a == GAMESTATE.OVER)
        {
            gameStateText.text = "GAME OVER";
            if (StepTimer <= HighScore)
            {
                ScoreText.text = HighScore + " ft";
            }
            else
            {
                ScoreText.text = StepTimer + " ft";
                WriteNewHighScore();
                HighScore = StepTimer;
            }
            reSomething.GetComponentInChildren<Text>().text = "Restart";
            reSomething.onClick.AddListener(PlayerRestart);
            reSomething.onClick.RemoveListener(PlayerResume);
            waterSeconds = 6;
            waterTimeValue = 6;
            timeValue = 0;
            StepTimer = 0;
            slowSteps = 0;
            slowTimeTotal = 0;
            waterCountDown = false;
            waterWaitValue = 45;
            waterWaitSeconds = 45;
        }
        else if (a == GAMESTATE.PAUSED)
        {
            gameStateText.text = "PAUSED";
            reSomething.GetComponentInChildren<Text>().text = "Resume";
            reSomething.onClick.AddListener(PlayerResume);
            reSomething.onClick.RemoveListener(PlayerRestart);
            if (waterCountDown)
            {
                waterCountDown = false;
            }
        }

        if (a == GAMESTATE.OVER || a == GAMESTATE.PAUSED)
        {
            OI.paused = true;
            BI.scrollOn = false;
            GameStateObject.SetActive(true);
            PauseButton.interactable = false;
        }
    }

    public void PlayerRestart()
    {
        OI.paused = true;
        BI.scrollOn = false;
        vendingNear = false;
        waterSeconds = 6;
        waterTimeValue = 6;
        GameStateObject.SetActive(false);

        timeValue = 0;
        StepTimer = 0;
        waterCountDown = true;
        Baley.vendingCount = false;
        
        waterWaitValue = 45;
        waterWaitSeconds = 45;
        BI.GameRestart();
        OI.GameRestart();
        Baley.GameRestart();
        

        a = GAMESTATE.NONE;
        PauseButton.interactable = true;
        OI.waterBrake = true;
        OI.friendAppear = false;
    }

    public void PlayerPause()
    {
        a = GAMESTATE.PAUSED;
        PauseButton.interactable = false;
    }

    public void PlayerResume()
    {
        a = GAMESTATE.RUNNING;
        OI.paused = false;
        BI.scrollOn = true;
        GameStateObject.SetActive(false);
        if (waterWaitSeconds > 0)
        {
            waterCountDown = true;
        }
        PauseButton.interactable = true;
    }

    public void PlayerWater()
    {
        Baley.lives++;
        Baley.stopped = false;
        Debug.Log("Lives Changed: " + Baley.lives);
        vendingNear = false;
        Baley.meetVendingMachine = false;
        WaterAlert.SetActive(false);
        OI.paused = false;
        BI.scrollOn = true;
        waterSeconds = 6;
        waterTimeValue = 6;
        a = GAMESTATE.RUNNING;
        OI.waterBrake = true;
        Baley.vendingCount = false;
        waterCountDown = true;
        waterWaitValue = 45;
        waterWaitSeconds = 45;
    }

    public void PlayerNoWater()
    {
        vendingNear = false;
        Baley.stopped = false;
        WaterAlert.SetActive(false);
        OI.paused = false;
        BI.scrollOn = true;
        waterSeconds = 6;
        waterTimeValue = 6;
        OI.waterBrake = true;
        a = GAMESTATE.RUNNING;
        Baley.vendingCount = false;
        waterCountDown = true;
        waterWaitValue = 45;
        waterWaitSeconds = 45;
    }

    public void QuitGame()
    {
        a = GAMESTATE.NONE;
        SceneManager.LoadScene("Main Menu");
    }

    public void WriteNewHighScore()
    {
        StreamWriter ScoreWriter = new StreamWriter("Assets/HighScore.txt");
        ScoreWriter.WriteLine(StepTimer);
        ScoreWriter.Close();
    }

    public void GrabHighScore()
    {
        StreamReader ScoreReader = new StreamReader("Assets/HighScore.txt");
        HighScore = int.Parse(ScoreReader.ReadLine());
        ScoreReader.Close();
    }
}
