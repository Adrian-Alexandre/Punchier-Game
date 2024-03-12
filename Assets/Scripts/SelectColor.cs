using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectColor : MonoBehaviour
{

    public Material[] material;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;


    private void Start()
    {
        if (GameController.materialIndex == 0)
        {
            skinnedMeshRenderer.material = material[0];
        }
        if (GameController.materialIndex == 1)
        {
            skinnedMeshRenderer.material = material[1];
        }
        if (GameController.materialIndex == 2)
        {
            skinnedMeshRenderer.material = material[2];
        }
        if (GameController.materialIndex == 3)
        {
            skinnedMeshRenderer.material = material[3];
        }
    }
    public void Vermelho()
    {
        if (GameController.money >= 10)
        {
            skinnedMeshRenderer.sharedMaterial = material[1];
            GameController.materialIndex = 1;
            GameController.money -= 10;
            GameController.SaveData();
        }

    }
    public void Azul()
    {
        if (GameController.money >= 10)
        {
            skinnedMeshRenderer.sharedMaterial = material[0];
            GameController.materialIndex = 0;
            GameController.money -= 10;
            GameController.SaveData();
        }
    }
    public void Verde()
    {
        if (GameController.money >= 10)
        {
            skinnedMeshRenderer.sharedMaterial = material[3];
            GameController.materialIndex = 3;
            GameController.money -= 10;
            GameController.SaveData();
        }
    }
    public void Amarelo()
    {
        if (GameController.money >= 10) 
        { 
            skinnedMeshRenderer.sharedMaterial = material[2];
            GameController.materialIndex = 2;
            GameController.money -= 10;
            GameController.SaveData();
        }
    }
}
