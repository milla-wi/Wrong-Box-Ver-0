using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScrollScript : MonoBehaviour
{
    // Start is called before the first frame update
    // y = -8.9, 9.0, 9.1

    public float speed = 3.5f;
    Vector3 startPosition;

    public float endY;

    public bool scrollOn = false;

    public float startSpeed = 3.5f;

    void Start()
    {
        startPosition = transform.position;
        speed = 3.5f;
        startSpeed = 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollOn)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.position.y <= endY)
            {
                transform.position = startPosition;
                Debug.Log("Repeat Happened");
            }
        }
    }

    public void GameRestart()
    {
        scrollOn = false;
        transform.position = startPosition;
    }
}
