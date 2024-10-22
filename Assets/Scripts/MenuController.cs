using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Compression;

public class MenuController : MonoBehaviour
{
    public Text score;
    public Text scoreBackground;
    public Text capacidade;
    public Text capacidadeBackground;

    private void Update()
    {
        if (score != null)
        {
            score.text = "$ " + GameController.money.ToString() + ".00 ";
            scoreBackground.text = "$ " + GameController.money.ToString() + ".00 ";
        }
        if (capacidade != null)
        {
            capacidade.text = "CAPACIDADE: " + GameController.empilhamentoIndex.ToString();
            capacidadeBackground.text = "CAPACIDADE: " + GameController.empilhamentoIndex.ToString();
        }
    }

    private void Awake()
    {
        GameController.SaveData();
    }
    public void StartGame()
    {
            SceneManager.LoadScene("Main");
            GameController.LoadData();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void EditPlayer()
    {
        SceneManager.LoadScene("EditPlayer");
    }

    public void AddEmpilhamento()
    {
        if (GameController.money >= 30 && GameController.empilhamentoIndex <= 10)
        {
            GameController.empilhamentoIndex += 1;
            GameController.money -= 30;
            GameController.SaveData();
        }
    }
}
