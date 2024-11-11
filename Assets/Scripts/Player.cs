using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class Player : MonoBehaviour
{
    // Vari�veis privadas e serializadas para controle de velocidade do jogador
    [SerializeField] private Transform stackPosition; // Posi��o onde inimigos empilhados ser�o armazenados
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer; // Refer�ncia o renderer do player
    // Componentes adicionais para controle de apar�ncia e anima��o
    [SerializeField] private Material[] material; // Refer�ncia o material que ser� alocado ao renderer do player
    [SerializeField] private Animator playerAnimator; // Refer�ncia o animator do player
    
    private FixedJoystick joystick;  // Refer�ncia ao joystick fixo (para dispositivos m�veis)
    private Rigidbody rb;  // Refer�ncia ao componente Rigidbody do jogador
    private Transform parentPickup;  // Guarda a refer�ncia do objeto pai ao empilhar
    private bool isOnGround;          // Flag para verificar se o jogador est� no ch�o
    private float followSpeed = 20f;  // Velocidade de persegui��o dos inimigos na piha
    
    // Vari�veis para controle de velocidade e suavidade do movimento
    private float currentVelocity;
    private float currentSpeed;
    private float speedVelocity;
    private float MoveSpeed = 5f;

    public Transform cameraTransform;  // Refer�ncia � c�mera principal
    public List<GameObject> Enemies = new List<GameObject>(); // Cria��o de uma lista para armazenar inimigos capturados (stackposition � adicionado como primeiro elemento)
    public bool isPunching; // Flag para verificar se o jogador est� socando

    public bool enableMobileInputs = false;  // Controle para ativar inputs m�veis

    void Start()
    {
        // Inicializa refer�ncias e configura��es iniciais de materiais e componentes
        InitializeComponents();
        // Define o material do jogador
        SetPlayerMaterial(GameController.materialIndex);
    }
    void FixedUpdate()
    {
        // Chama o m�todo de movimenta��o a cada frame fixo
        Movement(cameraTransform);
        // Aumenta a for�a da gravidade
        Gravity();
        // Verifica se h� mais de um inimigo na lista para controlar o espa�amento entre eles
        UpdateEnemyPositions(cameraTransform);
    }

    // M�todo respons�vel por inicializar os componentes
    private void InitializeComponents()
    {
        // Inicializa refer�ncias e configura��es iniciais
        cameraTransform = Camera.main.transform; // Obt�m o transform da Camera Principal
        rb = GetComponent<Rigidbody>(); // Obt�m o Rigidbody
        joystick = GameObject.Find("Fixed Joystick")?.GetComponent<FixedJoystick>(); // Configura o joystick
        playerAnimator = GetComponent<Animator>(); // Configura o animador
    }

    // M�todo respons�vel por definir o material do jogador
    private void SetPlayerMaterial(int materialIndex)
    {
        // Define o material do jogador com base na configura��o do GameController
        if (materialIndex >= 0 && materialIndex < material.Length)
        {
            skinnedMeshRenderer.material = material[materialIndex];
        }
    }

    // M�todo respons�vel pela movimenta��o do jogador
    void Movement(Transform cameraTransform)
    {
        Vector2 input = Vector2.zero;

        // Se os controles m�veis estiverem ativados, usa o joystick para o input
        if (enableMobileInputs)
        {
            input = new Vector2(joystick.Horizontal, joystick.Vertical);
        }

        Vector2 inputDir = input.normalized;

        // Gira o jogador na dire��o do input
        if (inputDir != Vector2.zero)
        {
            float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref currentVelocity, 0.25f);
        }

        // Ajusta a velocidade e movimenta o jogador com suavidade
        float targetSpeed = MoveSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, 0.1f);
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        // Define as anima��es baseadas na magnitude do input
        float inputMagnitude = input.magnitude;

        if (inputMagnitude == 0)
        {
            // Jogador parado
            playerAnimator.SetBool("Walking", false);
            playerAnimator.SetBool("Running", false);
            MoveSpeed = 0f;
        }
        else if (inputMagnitude > 0 && inputMagnitude < 0.5f)
        {
            // Jogador andando
            playerAnimator.SetBool("Walking", true);
            playerAnimator.SetBool("Running", false);
            MoveSpeed = 5f;
        }
        else if (inputMagnitude >= 0.5f)
        {
            // Jogador correndo
            playerAnimator.SetBool("Walking", false);
            playerAnimator.SetBool("Running", true);
            MoveSpeed = 10f;
        }

        // Verifica se o jogador est� no ch�o e n�o est� socando
        if (isOnGround && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {
            isPunching = false;
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Punch") && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            isPunching = true;
        }
        else
        {
            playerAnimator.SetBool("Running", false);
            isPunching = false;
        }
    }

    // M�todo para adicionar gravidade ao jogo
    void Gravity()
    {
        // Aumenta a for�a da gravidade 
        Vector3 customGravity = new Vector3(0, -80f, 0);
        GetComponent<Rigidbody>().AddForce(customGravity, ForceMode.Acceleration);
    }

    // M�todo para ativar a anima��o de soco
    public void Punch()
    {
        playerAnimator.SetTrigger("Punch");
    }

    // M�todo respons�vel pela movimenta��o dos inimigos na pilha
    private void UpdateEnemyPositions(Transform cameraTransform)
    {
        Vector2 input = Vector2.zero;

        // Se os controles m�veis estiverem ativados, usa o joystick para o input
        if (enableMobileInputs)
        {
            input = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        Vector2 inputDir = input.normalized;

        // Verifica se h� mais de um inimigo na lista para controlar o espa�amento entre eles
        if (Enemies.Count > 1)
        {
            for (int i = 1; i < Enemies.Count; i++)
            {
                // Pega o inimigo anterior e o atual da lista
                var lastEnemy = Enemies.ElementAt(i - 1);
                var sectEnemy = Enemies.ElementAt(i);

                    // Gira o inimigo empilhado na dire��o do input
                    if (inputDir != Vector2.zero)
                    {
                        float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                        sectEnemy.transform.eulerAngles = new Vector3(-90, Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref currentVelocity, 0.25f), - 90);
                    }

                sectEnemy.transform.position = new Vector3(
                        Mathf.Lerp(sectEnemy.transform.position.x, lastEnemy.transform.position.x, followSpeed * Time.deltaTime),
                        lastEnemy.transform.position.y + 0.5f,
                        Mathf.Lerp(sectEnemy.transform.position.z, lastEnemy.transform.position.z, followSpeed * Time.deltaTime));
            }
        }
    }

    // Fun��o para adicionar inimigos � lista
    private void AddEnemyToStack(Collider other)
    {
        Transform otherTransform = other.transform;
        Rigidbody otherRB = other.GetComponent<Rigidbody>();
        Collider[] otherColliders = other.GetComponentsInChildren<Collider>();

        otherRB.isKinematic = true;  // Define o Rigidbody como cinem�tico
        other.tag = gameObject.tag;  // Define a tag do inimigo igual � do jogador

        foreach (Collider col in otherColliders)
        {
            col.enabled = false;  // Desativa os colliders para evitar colis�es
        }

        Enemies.Add(other.gameObject);  // Adiciona o inimigo � lista

        if (Enemies.Count == 1)
        {
            parentPickup = otherTransform;
            parentPickup.position = stackPosition.position;
            parentPickup.parent = stackPosition;
        }
        else if (Enemies.Count > 1)
        {
            otherTransform.position = stackPosition.position;
            otherTransform.parent = parentPickup;
        }
    }

    // Verifica se o jogador est� no ch�o ao colidir com objetos que t�m a tag "Ground"
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    // M�todo que adiciona inimigos � lista ao entrar em um trigger com a tag "Enemy"
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && !isPunching && Enemies.Count <= GameController.empilhamentoIndex)
        {
            AddEnemyToStack(other);
        }
    }

}
