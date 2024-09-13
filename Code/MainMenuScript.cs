using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject[] ControlPages;
    public GameObject ControlBook;
    public int currentPage;
    public GameObject MainMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToChaseMode()
    {
        SceneManager.LoadScene("Chase Mode");
    }

    public void GoToEndlessMode()
    {
        SceneManager.LoadScene("Endless Run Mode");
    }

    public void ShowControls()
    {
        ControlBook.SetActive(true);
        currentPage = 0; 
        ControlPages[currentPage].SetActive(true);
        MainMenu.SetActive(false);
    }

    public void BackPage()
    {
        if (currentPage > 0)
        {
            ControlPages[currentPage].SetActive(false);
            currentPage--;
            ControlPages[currentPage].SetActive(true);
        } else
        {
            Debug.Log("This is the beginning");
        }
    }

    public void FlipForward()
    {
        if (currentPage < ControlPages.Length - 1)
        {
            ControlPages[currentPage].SetActive(false);
            currentPage++;
            ControlPages[currentPage].SetActive(true);
        } else
        {
            Debug.Log("This is the end.");
        }
    }

    public void ExitControls()
    {
        ControlBook.SetActive(false);
        ControlPages[currentPage].SetActive(false);
        currentPage = 0;
        MainMenu.SetActive(true);
    }
}
