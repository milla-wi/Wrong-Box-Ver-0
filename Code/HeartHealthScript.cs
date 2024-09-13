using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartHealthScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Image[] Hearts;

    public int heartNum;

    public bool shield;

    public bool punches;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < heartNum)
            {
                Hearts[i].enabled = true;
            } else {
                Hearts[i].enabled = false;
            }
        }
        if (shield)
        {
            Hearts[heartNum - 1].sprite = Resources.Load<Sprite>("Life_Punk_Guard_1");
        } else if (heartNum > 0 && !punches)
        {
            Hearts[heartNum - 1].sprite = Resources.Load<Sprite>("Life Punk");
        }
    }
}
