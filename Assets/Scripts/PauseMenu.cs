using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Painel de pausa a ser exibido ou ocultado quando o jogo � pausado
    public GameObject PausePanel;

    // M�todo para pausar o jogo
    public void Pause()
    {
        PausePanel.SetActive(true);  // Exibe o painel de pausa
        Time.timeScale = 0;          // Congela o tempo do jogo, pausando todas as a��es
    }

    // M�todo para continuar o jogo ap�s a pausa
    public void Continue()
    {
        PausePanel.SetActive(false); // Oculta o painel de pausa
        Time.timeScale = 1;          // Retoma o tempo normal do jogo
    }

    // M�todo para voltar ao menu principal
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu"); // Carrega a cena do menu principal
        Time.timeScale = 1;             // Garante que o tempo do jogo esteja normal
    }

    // M�todo para ir para a tela de edi��o do jogador
    public void EditPlayer()
    {
        SceneManager.LoadScene("EditPlayer"); // Carrega a cena de edi��o do jogador
        Time.timeScale = 1;                   // Garante que o tempo do jogo esteja normal
    }
}
