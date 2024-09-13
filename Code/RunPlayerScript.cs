using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunPlayerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public bool stopped = true;
    public bool timerOff = false;

    public int seconds = 5;
    public float timeValue = 5;
    
    public Text timerText;
    public GameObject COUNTDOWN;

    public int lives = 4;

    public GameObject PointOne;
    public GameObject PointTwo;
    public GameObject PointThree;

    public bool vendingPossible = false;
    public bool meetVendingMachine = false;
    public bool vendingCount = false;

    public bool slow = false;

    public float slowValue = 10;
    public int slowSeconds = 10;

    public int maxLives = 5;

    void Start()
    {
        transform.position = PointTwo.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerOff)
        {
            timeValue -= Time.deltaTime;
            seconds = Mathf.RoundToInt(timeValue % 60);
            timerText.text = seconds.ToString();

            if (seconds <= 0)
            {
                Debug.Log("Timer Done!");
                timeValue = 5;
                seconds = 5;
                stopped = false;
                timerText.text = "";
                COUNTDOWN.SetActive(false);
                timerOff = true;
            }
        } 

        if (!stopped)
        {
            if ((Input.GetKeyDown(KeyCode.A)) && (transform.position == PointTwo.transform.position))
            {
                transform.position = PointOne.transform.position;
            }
            else if ((Input.GetKeyDown(KeyCode.A)) && (transform.position == PointThree.transform.position))
            {
                transform.position = PointTwo.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.D) && transform.position == PointTwo.transform.position)
            {
                transform.position = PointThree.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.D) && transform.position == PointOne.transform.position)
            {
                transform.position = PointTwo.transform.position;
            }
        }

        if (slow)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            slowValue -= Time.deltaTime;
            slowSeconds = Mathf.RoundToInt(slowValue % 60);
            if (slowSeconds <= 0)
            {
                slow = false;
                slowValue = 10;
                slowSeconds = 10;
            }
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void FixedUpdate()
    {

    }

    void OnTriggerEnter2D(Collider2D OB)
    {
        Debug.Log("Something Entered!");
        Debug.Log(OB.gameObject.name);
        if (OB.gameObject.name == "Manhole" || OB.gameObject.name == "Cart" || OB.gameObject.name == "Water Puddle")
        {
            lives--;
            slow = true;
            Debug.Log("Lives Changed: " + lives);
            Debug.Log("Slow Active!");
        }
        else if (OB.gameObject.name == "Person" || OB.gameObject.name == "Gum")
        {
            slow = true;
            Debug.Log("Slow Active!");
        }
        else if (OB.gameObject.name == "Thug" || OB.gameObject.name == "Pavement")
        {
            lives--;
            Debug.Log("Lives Changed: " + lives);
        }
        else if (OB.gameObject.name == "Water")
        {
            if (vendingPossible && (lives < maxLives))
            {
                meetVendingMachine = true;
                vendingCount = true;
                stopped = true;
                Debug.Log("Hello? Water?");
            }
        }
    }

    public void GameRestart()
    {
        transform.position = PointTwo.transform.position;
        meetVendingMachine = false;
        vendingPossible = true;
        slow = false;
        stopped = true;
        lives = 4;
        COUNTDOWN.SetActive(true);
        timeValue = 5;
        seconds = 5;
        timerOff = false;
    }
}
