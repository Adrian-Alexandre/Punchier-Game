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
    [SerializeField] private float playerSpeed, Distance;
    [SerializeField] Rigidbody rb;
    [SerializeField] private Vector3 dir;
    [SerializeField] Transform stackPosition;

    //Cria a lista para empilhamento dos inimigos, iremos adicionar o player como primeiro elemento desta lista
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

    public float gravityModifier = 10f;

    float currentVelocity;
    float currentSpeed;
    float speedVelocity;
    float MoveSpeed = 5f;

    public bool enableMobileInputs = false;

    public Transform cameraTranform;

    public Transform playerTransform;

    void Start()
    {
        cameraTranform = Camera.main.transform;

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

        rb = GetComponent<Rigidbody>();
        joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        playerAnimator = gameObject.GetComponent<Animator>();
        Physics.gravity *= gravityModifier;

    }

    void Update()
    {
        if (Enemies.Count > 1)
        {
            for (int i = 1; i < Enemies.Count; i++)
            {
                var firstEnemy = Enemies.ElementAt(i - 1);
                var sectEnemy = Enemies.ElementAt(i);

                var DesireDistance = Vector3.Distance(sectEnemy.transform.position, firstEnemy.transform.position);

                if (DesireDistance <= Distance)
                {
                    sectEnemy.transform.position = new Vector3(Mathf.Lerp(sectEnemy.transform.position.x, firstEnemy.transform.position.x, playerSpeed * Time.deltaTime), Mathf.Lerp(sectEnemy.transform.position.y, firstEnemy.transform.position.y + 0.7f, playerSpeed * Time.deltaTime), sectEnemy.transform.position.z);
                }
            }
        }


        
    }

    void FixedUpdate()
    {
        // MovementMobile();
        Movement(cameraTranform);
        /*if (isOnGround == true && moveH != 0 || moveV != 0)
        {
            playerAnimator.SetBool("Walking", true);
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Punch") && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            isPunching = true;
        }
        else
        {
            playerAnimator.SetBool("Walking", false);
            isPunching = false;
        }*/

    }

    /* void MovementMobile()
     {
         moveH = joystick.Horizontal;
         moveV = joystick.Vertical;

         Vector3 dir = new Vector3(moveH, 0, moveV);
         rb.velocity = new Vector3(moveH * playerSpeed, rb.velocity.y, moveV * playerSpeed);

         if (dir != Vector3.zero)
         {
             transform.LookAt(transform.position + dir);
         }

     }*/

    void Movement(Transform cameraTransform)
    {
        Vector2 input = Vector2.zero;

        if (enableMobileInputs)
        {
            input = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        else
        {

        }
         
        Vector2 inputDir = input.normalized;


        if (inputDir != Vector2.zero)
        {
            float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref currentVelocity, 0.25f);
        }

        float targetSpeed = MoveSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, 0.1f);
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        if (isOnGround == true && joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            playerAnimator.SetBool("Walking", true);
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Punch") && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            isPunching = true;
        }
        else
        {
            playerAnimator.SetBool("Walking", false);
            isPunching = false;
        }
    }

    public void SaleEnemy()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            Destroy(Enemies[i].gameObject);
            GameController.money += 10;
        }
        parentPickup = null;
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
            Rigidbody otherRB = other.GetComponent<Rigidbody>();
            Collider[] otherColliders = other.GetComponentsInChildren<Collider>();

            otherRB.isKinematic = true;
            otherTransform.rotation = Quaternion.Euler(stackPosition.rotation.x - 90, stackPosition.rotation.y, stackPosition.rotation.z);
            other.tag = gameObject.tag;

            foreach (Collider col in otherColliders)
            {
                col.enabled = false;
            }

            if (parentPickup == null)
             {
                 parentPickup = otherTransform;
                 parentPickup.position = stackPosition.position;
                 parentPickup.parent = stackPosition;
                 Enemies.Add(otherTransform.gameObject);
             }
             else
             {
                
                otherTransform.position = stackPosition.position;
                otherTransform.parent = parentPickup;
                Enemies.Add(otherTransform.gameObject);
             }
            
        }
    }
}
