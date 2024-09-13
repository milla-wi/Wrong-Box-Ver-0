using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlScript : MonoBehaviour
{
    public GameObject Girl;
    public GameObject GirlReplacement;

    public int chances = 2;
    public GameObject disappearTarget;

    public GameObject StartPoint;

    public bool visible = false;
    public bool run = false;

    public PlayerScript Punk;

    public int girlTimer = 0;
    public int stepCounter = 0;
    float timeValue = 0;

    bool stepTaken = false;

    // Start is called before the first frame update
    void Start()
    {
        GirlReplacement.transform.position = StartPoint.transform.position;
        Girl.SetActive(false);
        Girl.transform.position = disappearTarget.transform.position;
        visible = false;
        Girl.GetComponent<Animator>().enabled = false;
    }

    void Update()
    {
        //add jump and evade animation for obstacles and people
        if (run) {

            timeValue += Time.deltaTime;
            girlTimer = Mathf.RoundToInt(timeValue % 60);
            stepCounter = girlTimer % 2;

            if ((Girl.transform.position.y > disappearTarget.transform.position.y) && (visible))
            {
                Girl.SetActive(false);
                Girl.transform.position = disappearTarget.transform.position;
                visible = false;
                Debug.Log("Visible on!");
            }
            if (visible)
            {
                Girl.SetActive(true);
                Girl.GetComponent<Animator>().enabled = true;
            }
            if (stepCounter == 1)
            {
                stepTaken = false;
            } else if (stepCounter == 0 && girlTimer != 0 && !stepTaken){
                if (visible)
                {
                    Vector2 distance = Punk.transform.position - Girl.transform.position;

                    var differenceInY = Mathf.Abs(distance.y);
                    var differenceInX = Mathf.Abs(distance.x);

                    if (differenceInY <= 0.7 && differenceInX > 0.2)
                    {
                        Girl.transform.position = new Vector2(Girl.transform.position.x, Punk.transform.position.y);
                        Debug.Log("Close! From Girl Script");
                    } else if ((Punk.slow && !Punk.hiss) || (Punk.hiss && Punk.slow && differenceInY <= 5) || (Punk.vendingCount)) {
                        Girl.transform.position = new Vector2(Girl.transform.position.x, Girl.transform.position.y + 0.5f);
                        if (Punk.hiss)
                        {
                            Girl.GetComponent<Animator>().SetBool("curious", true);
                        }
                    }
                    else if (Punk.hiss && !Punk.slow && differenceInY <= 5)
                    {
                        Girl.transform.position = new Vector2(Girl.transform.position.x, Girl.transform.position.y - 1);
                        Girl.GetComponent<Animator>().SetBool("curious", true);
                    }
                    else if (Punk.hiss && !Punk.slow && differenceInY > 5)
                    {
                        Girl.transform.position = new Vector2(Girl.transform.position.x, Girl.transform.position.y + 1);
                        Girl.GetComponent<Animator>().SetBool("worried", true);
                    }
                    else if (Punk.hiss && Punk.slow && differenceInY > 5) {
                        Girl.transform.position = new Vector2(Girl.transform.position.x, Girl.transform.position.y + 1.5f);
                        Girl.GetComponent<Animator>().SetBool("worried", true);
                    } else {
                        Girl.transform.position = new Vector2(Girl.transform.position.x, Girl.transform.position.y - 0.5f);
                        Girl.GetComponent<Animator>().SetBool("curious", false);
                        Girl.GetComponent<Animator>().SetBool("worried", false);
                    }
                    stepTaken = true;
                }
                if (!visible)
                {

                    if (GirlReplacement.transform.position.y < disappearTarget.transform.position.y)
                    {
                        GirlReplacement.transform.position = disappearTarget.transform.position;
                        visible = true;
                    }
                    else
                    {
                        if (Punk.slow == true)
                        {
                            GirlReplacement.transform.position = new Vector2(GirlReplacement.transform.position.x, GirlReplacement.transform.position.y + 0.5f);
                        }
                        else
                        {
                            GirlReplacement.transform.position = new Vector2(GirlReplacement.transform.position.x, GirlReplacement.transform.position.y - 0.5f);
                        }
                    }
                    stepTaken = true;
                }
                
            }
        }
    }
    public void GameRestart()
    {
        run = false;
        Girl.SetActive(true);
        Girl.transform.position = disappearTarget.transform.position;
        GirlReplacement.transform.position = StartPoint.transform.position;
        visible = true;
        girlTimer = 0;
        stepCounter = 0;
        chances = 2;
        stepTaken = false;
        timeValue = 0;
    }
}
