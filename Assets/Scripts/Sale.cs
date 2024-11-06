using UnityEngine;
using UnityEngine.UI;

public class Sale : MonoBehaviour
{
    // Referência ao botão de venda
    public Button sale;

    // Método chamado quando o script é inicializado
    void Start()
    {
        // Desativa o botão de venda no início do jogo
        sale.gameObject.SetActive(false);
    }

    // Método chamado enquanto o collider do outro objeto permanece dentro do trigger
    private void OnTriggerStay(Collider other)
    {
        // Verifica se o outro objeto tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Ativa o botão de venda
            sale.gameObject.SetActive(true);
        }
    }

    // Método chamado quando o collider do outro objeto sai do trigger
    private void OnTriggerExit(Collider other)
    {
        // Verifica se o outro objeto tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Desativa o botão de venda
            sale.gameObject.SetActive(false);
        }
    }
}
