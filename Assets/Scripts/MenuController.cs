using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Refer�ncias aos elementos UI para exibir o score e capacidade
    public Text score;
    public Text scoreBackground;
    public Text capacidade;
    public Text capacidadeBackground;

    // M�todo chamado a cada frame
    private void Update()
    {
        // Atualiza o texto do score e seu fundo
        if (score != null)
        {
            score.text = "$ " + GameController.money.ToString() + ".00 ";
            scoreBackground.text = "$ " + GameController.money.ToString() + ".00 ";
        }

        // Atualiza o texto da capacidade e seu fundo
        if (capacidade != null)
        {
            capacidade.text = "CAPACIDADE: " + GameController.empilhamentoIndex.ToString();
            capacidadeBackground.text = "CAPACIDADE: " + GameController.empilhamentoIndex.ToString();
        }
    }

    // M�todo para iniciar o jogo
    public void StartGame()
    {
        SceneManager.LoadScene("Main"); // Carrega a cena principal do jogo
        GameController.LoadData(); // Carrega os dados do jogo
        GameController.SaveData(); // Salva os dados do jogo
    }

    // M�todo para sair do jogo
    public void ExitGame()
    { 
        Application.Quit(); // Encerra o aplicativo
    }

    // M�todo para voltar ao menu principal
    public void BackToMainMenu()
    {
        GameController.SaveData(); // Salva os dados do jogo
        SceneManager.LoadScene("Menu"); // Carrega a cena do menu    
    }

    // M�todo para editar o jogador
    public void EditPlayer()
    {
        SceneManager.LoadScene("EditPlayer"); // Carrega a cena de edi��o do jogador
    }

    // M�todo para adicionar empilhamento
    public void AddEmpilhamento()
    {
        if (GameController.money >= 30)
        {
            GameController.empilhamentoIndex += 1; // Incrementa o �ndice de empilhamento
            GameController.money -= 30; // Deduz o custo do dinheiro
            GameController.SaveData(); // Salva os dados do jogo
        }
    }
}
