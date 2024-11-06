using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    // Vari�veis privadas e serializadas para controle de velocidade do jogador e dist�ncia m�nima para manter entre inimigos
    [SerializeField] private float playerSpeed, distance;
    [SerializeField] private Transform stackPosition; // Posi��o onde inimigos empilhados ser�o armazenados
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    // Componentes adicionais para controle de apar�ncia e anima��o
    [SerializeField] private Material[] material;
    [SerializeField] private Animator playerAnimator;
    
    private FixedJoystick joystick;  // Refer�ncia ao joystick fixo (para dispositivos m�veis)
    private Rigidbody rb;  // Refer�ncia ao componente Rigidbody do jogador
    private Transform parentPickup;  // Guarda a refer�ncia do objeto pai ao empilhar
    private bool isOnGround;          // Flag para verificar se o jogador est� no ch�o
    private float followSpeed = 20f;  // Velocidade de persegui��o dos inimigos
    // Vari�veis para controle de velocidade e suavidade do movimento
    private float currentVelocity;
    private float currentSpeed;
    private float speedVelocity;
    private float MoveSpeed = 5f;

    public Transform cameraTransform;  // Refer�ncia � c�mera principal
    public List<GameObject> Enemies = new List<GameObject>(); // Cria��o de uma lista para armazenar inimigos capturados (o jogador � adicionado como primeiro elemento)
    public bool isPunching;          // Flag para verificar se o jogador est� socando

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
        // Verifica se h� mais de um inimigo na lista para controlar o espa�amento entre eles
        UpdateEnemyPositions();
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

        // Configura a anima��o com base no estado de movimento e de soco
        if (isOnGround == true && joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            playerAnimator.SetBool("Walking", true);
            isPunching = false;
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

    // M�todo respons�vel pela movimenta��o dos inimigos na pilha
    private void UpdateEnemyPositions()
    {
        // Verifica se h� mais de um inimigo na lista para controlar o espa�amento entre eles
        if (Enemies.Count > 1)
        {
            for (int i = 1; i < Enemies.Count; i++)
            {
                // Pega o inimigo anterior e o atual da lista
                var lastEnemy = Enemies.ElementAt(i - 1);
                var sectEnemy = Enemies.ElementAt(i);

                // Calcula a dist�ncia desejada entre o inimigo atual e o anterior
                var DesireDistance = Vector3.Distance(sectEnemy.transform.position, lastEnemy.transform.position);

                // Ajusta a posi��o do inimigo atual se a dist�ncia for menor que o valor de `Distance`
                if (DesireDistance <= distance)
                {
                    sectEnemy.transform.rotation = Quaternion.Euler(sectEnemy.transform.rotation.x - 90, sectEnemy.transform.rotation.y, sectEnemy.transform.rotation.z);
                    sectEnemy.transform.position = new Vector3(
                        Mathf.Lerp(sectEnemy.transform.position.x, lastEnemy.transform.position.x, followSpeed * Time.deltaTime),
                        lastEnemy.transform.position.y + 0.6f,
                        Mathf.Lerp(sectEnemy.transform.position.z, lastEnemy.transform.position.z, followSpeed * Time.deltaTime)
                    );
                }
            }
        }
    }

    // M�todo para remover todos os inimigos exceto o primeiro da lista
    public void SaleEnemy()
    {
        for (int i = 1; i < Enemies.Count; i++)
        {
            Destroy(Enemies[i]);  // Destroi o inimigo e aumenta o dinheiro
            GameController.money += 10;
        }

        // Mant�m o primeiro elemento da lista e remove os outros
        Enemies = new List<GameObject> { Enemies[0] };
        GameController.SaveData();
    }

    // M�todo para ativar a anima��o de soco
    public void Punch()
    {
        playerAnimator.SetTrigger("Punch");
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
}
