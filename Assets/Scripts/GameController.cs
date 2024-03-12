using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static float money;
    public static float materialIndex;
    public static int empilhamentoIndex = 1;
    public Text score;
    public Text scoreBackground;
    public PauseMenu pauseMenu;

    void Start()
    {
        GameController.LoadData();
    }

    public static void LoadData()
    {
        GameController.money = PlayerPrefs.GetFloat("Money");
    }

    public static void SaveData()
    {
            PlayerPrefs.SetInt("EmpilhamentoIndex", GameController.empilhamentoIndex);
            PlayerPrefs.SetFloat("MaterialIndex", GameController.materialIndex);
            PlayerPrefs.SetFloat("Money", GameController.money);
            PlayerPrefs.Save();
    }

    void Update()
    {
        if(score != null)
        {
            score.text = "$ " + GameController.money.ToString() + ".00 " ;
            scoreBackground.text = "$ " + GameController.money.ToString() + ".00 ";
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            pauseMenu.Pause();
        }
    }
    
}
