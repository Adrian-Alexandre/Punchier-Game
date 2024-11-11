using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrest: MonoBehaviour
{
    // Refer�ncia ao bot�o de pris�o
    public Button arrest;
    // Refer�ncia o jogador
    public Player player;

    // M�todo chamado quando o script � inicializado
    void Start()
    {
        // Desativa o bot�o de pris�o no in�cio do jogo
        arrest.gameObject.SetActive(false);
    }

    // M�todo chamado enquanto o collider do outro objeto permanece dentro do trigger
    private void OnTriggerStay(Collider other)
    {
        // Verifica se o outro objeto tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Ativa o bot�o de venda
            arrest.gameObject.SetActive(true);
        }
    }

    // M�todo chamado quando o collider do outro objeto sai do trigger
    private void OnTriggerExit(Collider other)
    {
        // Verifica se o outro objeto tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Desativa o bot�o de venda
            arrest.gameObject.SetActive(false);
        }
    }

    // M�todo para remover todos os inimigos exceto o primeiro da lista
    public void ArrestEnemy()
    {
        for (int i = 1; i < player.Enemies.Count; i++)
        {
            Destroy(player.Enemies[i]);  // Destroi o inimigo e aumenta o dinheiro
            GameController.money += 10;
        }

        // Mant�m o primeiro elemento da lista e remove os outros
        player.Enemies = new List<GameObject> { player.Enemies[0] };

        GameController.SaveData();
    }
}
