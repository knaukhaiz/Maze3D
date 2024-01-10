using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GamePopup : MonoBehaviour
{
    public TextMeshProUGUI gameStatus;
    public GameObject popupObject;
    public void GameEnd(int num = 0)
    {
        Time.timeScale = 0;
        if (num == 1)
        {
            gameStatus.text = "YOU WON!";
        }
        else
        {
            gameStatus.text = "YOU LOST!";
        }
        popupObject.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
