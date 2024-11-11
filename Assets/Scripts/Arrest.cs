using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrest: MonoBehaviour
{
    // Referência ao botão de prisão
    public Button arrest;
    // Referência o jogador
    public Player player;

    // Método chamado quando o script é inicializado
    void Start()
    {
        // Desativa o botão de prisão no início do jogo
        arrest.gameObject.SetActive(false);
    }

    // Método chamado enquanto o collider do outro objeto permanece dentro do trigger
    private void OnTriggerStay(Collider other)
    {
        // Verifica se o outro objeto tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Ativa o botão de venda
            arrest.gameObject.SetActive(true);
        }
    }

    // Método chamado quando o collider do outro objeto sai do trigger
    private void OnTriggerExit(Collider other)
    {
        // Verifica se o outro objeto tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Desativa o botão de venda
            arrest.gameObject.SetActive(false);
        }
    }

    // Método para remover todos os inimigos exceto o primeiro da lista
    public void ArrestEnemy()
    {
        for (int i = 1; i < player.Enemies.Count; i++)
        {
            Destroy(player.Enemies[i]);  // Destroi o inimigo e aumenta o dinheiro
            GameController.money += 10;
        }

        // Mantém o primeiro elemento da lista e remove os outros
        player.Enemies = new List<GameObject> { player.Enemies[0] };

        GameController.SaveData();
    }
}
