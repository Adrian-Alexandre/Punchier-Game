using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sale : MonoBehaviour
{
    public Button sale;

    [System.Obsolete]
    void Start()
    {
        sale.gameObject.active = false;
    }

    [System.Obsolete]
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sale.gameObject.active = true;
        }
        
    }

    [System.Obsolete]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sale.gameObject.active = false;
        }
    }
}
