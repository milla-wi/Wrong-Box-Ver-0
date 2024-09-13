
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSpawnScript : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject endPointOne;
    public GameObject endPointTwo;
    public GameObject endPointThree;

    public int pointSpawnNum = 0;

    public ObjControlScript OI;

    public float speed = 3.5f;

    public bool baseObj = false;

    public PlayerScript PI;

    public float objSlowSpeed = 1.5f;

    void Start()
    {
        if (OI.objName == "Person" || OI.objName == "Thug") {
            speed = 4.0f;
        } else {
            speed = 3.5f;
        } 
        if (OI.objName == "")
        {
            baseObj = true;
            
        } else {
            
            pointSpawnNum = OI.spawnNum;
            gameObject.name = OI.objName;
            baseObj = false;
        }
        //Debug.Log("Name: " + OI.objName);
    }

    // Update is called once per frame
    void Update()
    {
        if (!OI.paused && !baseObj)
        {
            if (PI.slow)
            {
                if (OI.objName == "Person" || OI.objName == "Thug") {
                    transform.Translate(Vector3.down * (2.2f) * Time.deltaTime);
                } else
                {
                    transform.Translate(Vector3.down * objSlowSpeed * Time.deltaTime);
                }
            }
            else
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
            if ((pointSpawnNum == 1) && (transform.position.y <= endPointOne.transform.position.y))
            {
                Destroy(gameObject);
                
            }
            else if ((pointSpawnNum == 2) && (transform.position.y <= endPointTwo.transform.position.y))
            {
                Destroy(gameObject);
            }
            else if ((pointSpawnNum == 3) && (transform.position.y <= endPointThree.transform.position.y))
            {
                Destroy(gameObject);
            }
        }
        if (OI.reset && !baseObj)
        {
            Destroy(gameObject);
        }
    }
}
