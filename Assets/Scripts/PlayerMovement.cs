using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.TextCore.Text;
using System.Threading;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] Rigidbody rb;
    [SerializeField] private Vector3 dir;
    [SerializeField] Transform stackPosition;
    [SerializeField] public List<GameObject> Enemies = new List<GameObject>();
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Transform parentPickup;
    [SerializeField] private Material[] material;
    [SerializeField] private Animator playerAnimator;
    public bool isPunching;
    [SerializeField] private bool isOnGround;
    
    private FixedJoystick joystick;

    private float moveH;
    private float moveV;
    
    void Start()
    {
        if(GameController.materialIndex == 0)
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

        rb = GetComponent<Rigidbody>();
        joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        MovementMobile();
        if (isOnGround == true && moveH != 0 || moveV != 0)
        {
            playerAnimator.SetBool("Walking", true);
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Punch") && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            isPunching = true;
        }
        else {
            playerAnimator.SetBool("Walking", false);
            isPunching = false;
        }
        
        }

    void MovementMobile()
    {
        moveH = joystick.Horizontal;
        moveV = joystick.Vertical;

        Vector3 dir = new Vector3 (moveH, 0, moveV);
        rb.velocity = new Vector3(moveH * playerSpeed, rb.velocity.y, moveV * playerSpeed);

        if(dir != Vector3.zero)
        {
            transform.LookAt(transform.position + dir);
        }
    }

    public void SaleEnemy()
    {
        for (int i = 0; i < Enemies.Count; i++){
            Destroy(Enemies[i].gameObject);
            GameController.money += 10;
        }
        Enemies.Clear();
        GameController.SaveData();
    }

    public void Punch()
    {
        playerAnimator.SetTrigger("Punch"); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.tag == "Enemy" && isPunching == false && Enemies.Count < GameController.empilhamentoIndex)
            {
                Transform otherTransform = other.transform;

                Rigidbody otherRB = otherTransform.GetComponent<Rigidbody>();
                otherRB.isKinematic = true;
                other.enabled = false;
                if (parentPickup == null)
                {
                    parentPickup = otherTransform;
                    parentPickup.position = stackPosition.position;
                    parentPickup.parent = stackPosition;
                    Enemies.Add(otherTransform.gameObject);
                }
                else
                {
                    parentPickup.position += Vector3.up * (otherTransform.localScale.y);
                    otherTransform.position = stackPosition.position;
                    otherTransform.parent = parentPickup;
                    Enemies.Add(otherTransform.gameObject);
                }
            }
    }
    }
