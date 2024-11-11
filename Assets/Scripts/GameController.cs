using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Vari�veis est�ticas para armazenar dinheiro, �ndice do material e �ndice do empilhamento
    public static float money;
    public static int materialIndex;
    public static int empilhamentoIndex = 1;

    // Refer�ncias aos elementos UI para exibir o score
    public Text score;
    public Text scoreBackground;
    public PauseMenu pauseMenu;

    // M�todo chamado quando o script � inicializado
    void Awake()
    {
        GameController.LoadData(); // Carrega os dados salvos do jogo
    }

    // M�todo est�tico para carregar os dados do PlayerPrefs
    public static void LoadData()
    {
        GameController.empilhamentoIndex = PlayerPrefs.GetInt("EmpilhamentoIndex");
        GameController.materialIndex = PlayerPrefs.GetInt("MaterialIndex");
        GameController.money = PlayerPrefs.GetFloat("Money");
    }

    // M�todo est�tico para salvar os dados no PlayerPrefs
    public static void SaveData()
    {
        PlayerPrefs.SetInt("EmpilhamentoIndex", GameController.empilhamentoIndex);
        PlayerPrefs.SetInt("MaterialIndex", GameController.materialIndex);
        PlayerPrefs.SetFloat("Money", GameController.money);
        PlayerPrefs.Save(); // Salva as altera��es feitas
    }

    // M�todo chamado a cada frame
    void Update()
    {
        // Atualiza o texto do score e seu fundo
        if (score != null)
        {
            score.text = "$ " + GameController.money.ToString() + ".00 ";
            scoreBackground.text = "$ " + GameController.money.ToString() + ".00 ";
        }

        // Verifica se a tecla ESC foi pressionada para pausar o jogo
        if (Input.GetKey(KeyCode.Escape))
        {
            pauseMenu.Pause(); // Chama o m�todo Pause do PauseMenu
            SaveData(); // Salva os dados do jogo
        }
    }
}
