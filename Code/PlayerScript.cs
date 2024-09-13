using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public bool stopped = true;
    public bool timerOff = true;

    public int seconds = 5;
    public float timeValue = 5;

    public Text timerText;
    public GameObject COUNTDOWN;

    public int lives = 4;

    public GameObject PointOne;
    public GameObject PointTwo;
    public GameObject PointThree;

    public bool vendingPossible = true;
    public bool meetVendingMachine = false;
    public bool vendingCount = false;

    public bool slow = false;
    public bool hiss = false;

    public float slowValue = 10;
    public int slowSeconds = 10;

    public int maxLives = 5;

    public bool friendMeet = false;
    public bool hissPossible = false;

    public bool hissHappened = false;

    public Animator animator;

    public bool inPain = false;

    public float painTimeValue = 3;
    public int painTimeSec = 3;

    public Animator spiderAnimator;

    public bool toggle;

    //public PunkInput RunPls;
    //public InputAction move;
    //public InputAction hissPls;

   /* private void Awake()
    {
        RunPls = new PunkInput();
    }

    private void OnEnable()
    {
        move = RunPls.Player.Move;
        move.Enable();
        hissPls = RunPls.Player.Hiss;
        hissPls.Enable();

        move.performed += Move;
        hissPls.performed += Hiss;
    }

    private void OnDisable()
    {
        move.Disable();
        hissPls.Disable();
    }*/

    void Start()
    {
        transform.position = PointTwo.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerOff)
        {
            COUNTDOWN.SetActive(true);
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

        if (hiss)
        {
            timeValue -= Time.deltaTime;
            seconds = Mathf.RoundToInt(timeValue % 60);
            spiderAnimator.SetBool("hissing", true);
            if (seconds <= 0)
            {
                hiss = false;
                timeValue = 5;
                seconds = 5;
                hissHappened = true;
                spiderAnimator.SetBool("hissing", false);
            }
        }

        if (inPain)
        {
            animator.SetBool("pain", true);
            painTimeValue -= Time.deltaTime;
            painTimeSec = Mathf.RoundToInt(painTimeValue);
            if (painTimeSec <= 0)
            {
                inPain = false;
                painTimeValue = 3;
                painTimeSec = 3;
                animator.SetBool("pain", false);
            }
        }
        if (!stopped) {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
            if (touchPosition.y < -0.5)
                {
                    if (touchPosition.x < -0.8 && transform.position == PointTwo.transform.position)
                    {
                        transform.position = PointOne.transform.position;
                    }
                    else if (touchPosition.x < 0.8 && transform.position == PointThree.transform.position)
                    {
                        transform.position = PointTwo.transform.position;
                    }
                    else if (touchPosition.x > 0.8 && transform.position == PointTwo.transform.position)
                    {
                        transform.position = PointThree.transform.position;
                    }
                    else if (touchPosition.x > 0.8 && transform.position == PointOne.transform.position)
                    {
                        transform.position = PointTwo.transform.position;
                    }
                }
                if (touchPosition.y > 3.6 && touchPosition.x > 1.57)
                {

                    hiss = true;
                    Debug.Log("HISS!");
                }
        }
            if (Input.GetKeyDown(KeyCode.A) && transform.position == PointTwo.transform.position)
            {
                transform.position = PointOne.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.A) && transform.position == PointThree.transform.position)
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
            } if (Input.GetKeyDown(KeyCode.Space) && !hiss && hissPossible) {
                hiss = true;
                Debug.Log("HISS!");
            }
        }

        if (slow)
        {
            animator.SetBool("slowed", true);
            slowValue -= Time.deltaTime;
            slowSeconds = Mathf.RoundToInt(slowValue % 60);
            if (slowSeconds <= 0)
            {
                slow = false;
                slowValue = 10;
                slowSeconds = 10;
            }
        } else
        {
            animator.SetBool("slowed", false);
        }

        /*if (toggle && meetVendingMachine)
        {
            //yello???
            if (Input.GetKeyDown(KeyCode.A))
            {
                
            } else if (Input.GetKeyDown(KeyCode.D))
            {

            }
        }*/
    }
    //collision != trigger && collider != collision

    void OnTriggerEnter2D(Collider2D OB)
    {
        Debug.Log("Something Entered!");
        Debug.Log(OB.gameObject.name);
        if (OB.gameObject.name == "Manhole" || OB.gameObject.name == "Cart" || OB.gameObject.name == "Water Puddle")
        {
            if (!friendMeet)
            {
                lives--;
                animator.SetBool("pain", true);
                inPain = true;
            } else
            {
                friendMeet = false;
            }
            slow = true;
            Debug.Log("Lives Changed: " + lives);
            Debug.Log("Slow Active!");
        } else if (OB.gameObject.name == "Person" || OB.gameObject.name == "Gum") {
            slow = true;
            Debug.Log("Slow Active!");
        }
        else if (OB.gameObject.name == "Thug" || OB.gameObject.name == "Pavement")
        {
            if (!friendMeet)
            {
                lives--;
                animator.SetBool("pain", true);
                inPain = true;
            }
            else
            {
                friendMeet = false;
            }
            Debug.Log("Lives Changed: " + lives);
        }
        else if (OB.gameObject.name == "Vending Machine")
        {
            if (vendingPossible && (lives < maxLives))
            {
                meetVendingMachine = true;
                vendingCount = true;
                stopped = true;
                Debug.Log("Hello? Water?");
            }
        } else if (OB.gameObject.name == "Friend Punk")
        {
            friendMeet = true;
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
        hiss = false;
        //COUNTDOWN.SetActive(true);
        timeValue = 5;
        seconds = 5;
       // timerOff = false;
    }

   /* private void Move(InputAction.CallbackContext context)
    {
        if (Keyboard.current.aKey.wasPressedThisFrame && transform.position == PointTwo.transform.position)
        {
            transform.position = PointOne.transform.position;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame && transform.position == PointThree.transform.position)
        {
            transform.position = PointTwo.transform.position;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame && transform.position == PointTwo.transform.position)
        {
            transform.position = PointThree.transform.position;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame && transform.position == PointOne.transform.position)
        {
            transform.position = PointTwo.transform.position;
        }
    }

    private void Hiss(InputAction.CallbackContext context)
    {
        hiss = true;
    }*/
}
