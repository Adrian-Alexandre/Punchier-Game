using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectColor : MonoBehaviour
{
    // Array para armazenar os materiais
    public Material[] material;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    // M�todo chamado ao iniciar o script
    private void Start()
    {
        // Define o material inicial baseado no �ndice do GameController
        UpdateMaterial(GameController.materialIndex);
    }

    // M�todo para atualizar o material da mesh
    private void UpdateMaterial(int index)
    {
        if (index >= 0 && index < material.Length)
        {
            skinnedMeshRenderer.material = material[index];
        }
        else
        {
            Debug.LogWarning("�ndice de material fora do alcance!");
        }
    }

    // M�todo para trocar o material para Vermelho
    public void Vermelho()
    {
        TrocaMaterial(1);
    }

    // M�todo para trocar o material para Azul
    public void Azul()
    {
        TrocaMaterial(0);
    }

    // M�todo para trocar o material para Verde
    public void Verde()
    {
        TrocaMaterial(3);
    }

    // M�todo para trocar o material para Amarelo
    public void Amarelo()
    {
        TrocaMaterial(2);
    }

    // M�todo gen�rico para trocar o material e atualizar o GameController
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
