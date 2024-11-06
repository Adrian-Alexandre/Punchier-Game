using UnityEngine;
using UnityEngine.UI;

public class Sale : MonoBehaviour
{
    // Refer�ncia ao bot�o de venda
    public Button sale;

    // M�todo chamado quando o script � inicializado
    void Start()
    {
        // Desativa o bot�o de venda no in�cio do jogo
        sale.gameObject.SetActive(false);
    }

    // M�todo chamado enquanto o collider do outro objeto permanece dentro do trigger
    private void OnTriggerStay(Collider other)
    {
        // Verifica se o outro objeto tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Ativa o bot�o de venda
            sale.gameObject.SetActive(true);
        }
    }

    // M�todo chamado quando o collider do outro objeto sai do trigger
    private void OnTriggerExit(Collider other)
    {
        // Verifica se o outro objeto tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Desativa o bot�o de venda
            sale.gameObject.SetActive(false);
        }
    }
}
