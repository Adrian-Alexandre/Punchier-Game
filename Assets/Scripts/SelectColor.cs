using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectColor : MonoBehaviour
{
    // Array para armazenar os materiais
    public Material[] material;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    // Método chamado ao iniciar o script
    private void Start()
    {
        // Define o material inicial baseado no índice do GameController
        UpdateMaterial(GameController.materialIndex);
    }

    // Método para atualizar o material da mesh
    private void UpdateMaterial(int index)
    {
        if (index >= 0 && index < material.Length)
        {
            skinnedMeshRenderer.material = material[index];
        }
        else
        {
            Debug.LogWarning("Índice de material fora do alcance!");
        }
    }

    // Método para trocar o material para Vermelho
    public void Vermelho()
    {
        TrocaMaterial(1);
    }

    // Método para trocar o material para Azul
    public void Azul()
    {
        TrocaMaterial(0);
    }

    // Método para trocar o material para Verde
    public void Verde()
    {
        TrocaMaterial(3);
    }

    // Método para trocar o material para Amarelo
    public void Amarelo()
    {
        TrocaMaterial(2);
    }

    // Método genérico para trocar o material e atualizar o GameController
    private void TrocaMaterial(int index)
    {
        if (GameController.money >= 10)
        {
            skinnedMeshRenderer.sharedMaterial = material[index];
            GameController.materialIndex = index;
            GameController.money -= 10;
            GameController.SaveData();
        }
        else
        {
            Debug.LogWarning("Dinheiro insuficiente!");
        }
    }
}
