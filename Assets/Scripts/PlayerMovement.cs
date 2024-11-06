using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    // Variáveis privadas e serializadas para controle de velocidade do jogador e distância mínima para manter entre inimigos
    [SerializeField] private float playerSpeed, distance;
    [SerializeField] private Transform stackPosition; // Posição onde inimigos empilhados serão armazenados
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    // Componentes adicionais para controle de aparência e animação
    [SerializeField] private Material[] material;
    [SerializeField] private Animator playerAnimator;
    
    private FixedJoystick joystick;  // Referência ao joystick fixo (para dispositivos móveis)
    private Rigidbody rb;  // Referência ao componente Rigidbody do jogador
    private Transform parentPickup;  // Guarda a referência do objeto pai ao empilhar
    private bool isOnGround;          // Flag para verificar se o jogador está no chão
    private float followSpeed = 20f;  // Velocidade de perseguição dos inimigos
    // Variáveis para controle de velocidade e suavidade do movimento
    private float currentVelocity;
    private float currentSpeed;
    private float speedVelocity;
    private float MoveSpeed = 5f;

    public Transform cameraTransform;  // Referência à câmera principal
    public List<GameObject> Enemies = new List<GameObject>(); // Criação de uma lista para armazenar inimigos capturados (o jogador é adicionado como primeiro elemento)
    public bool isPunching;          // Flag para verificar se o jogador está socando

    public bool enableMobileInputs = false;  // Controle para ativar inputs móveis

    void Start()
    {
        // Inicializa referências e configurações iniciais de materiais e componentes
        InitializeComponents();
        // Define o material do jogador
        SetPlayerMaterial(GameController.materialIndex);
    }

    void FixedUpdate()
    {
        // Chama o método de movimentação a cada frame fixo
        Movement(cameraTransform);  
        // Verifica se há mais de um inimigo na lista para controlar o espaçamento entre eles
        UpdateEnemyPositions();
    }

    // Método responsável por inicializar os componentes
    private void InitializeComponents()
    {
        // Inicializa referências e configurações iniciais
        cameraTransform = Camera.main.transform; // Obtém o transform da Camera Principal
        rb = GetComponent<Rigidbody>(); // Obtém o Rigidbody
        joystick = GameObject.Find("Fixed Joystick")?.GetComponent<FixedJoystick>(); // Configura o joystick
        playerAnimator = GetComponent<Animator>(); // Configura o animador
    }

    // Método responsável por definir o material do jogador
    private void SetPlayerMaterial(int materialIndex)
    {
        // Define o material do jogador com base na configuração do GameController
        if (materialIndex >= 0 && materialIndex < material.Length)
        {
            skinnedMeshRenderer.material = material[materialIndex];
        }
    }

    // Método responsável pela movimentação do jogador
    void Movement(Transform cameraTransform)
    {
        Vector2 input = Vector2.zero;

        // Se os controles móveis estiverem ativados, usa o joystick para o input
        if (enableMobileInputs)
        {
            input = new Vector2(joystick.Horizontal, joystick.Vertical);
        }

        Vector2 inputDir = input.normalized;

        // Gira o jogador na direção do input
        if (inputDir != Vector2.zero)
        {
            float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref currentVelocity, 0.25f);
        }

        // Ajusta a velocidade e movimenta o jogador com suavidade
        float targetSpeed = MoveSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, 0.1f);
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        // Configura a animação com base no estado de movimento e de soco
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

    // Método responsável pela movimentação dos inimigos na pilha
    private void UpdateEnemyPositions()
    {
        // Verifica se há mais de um inimigo na lista para controlar o espaçamento entre eles
        if (Enemies.Count > 1)
        {
            for (int i = 1; i < Enemies.Count; i++)
            {
                // Pega o inimigo anterior e o atual da lista
                var lastEnemy = Enemies.ElementAt(i - 1);
                var sectEnemy = Enemies.ElementAt(i);

                // Calcula a distância desejada entre o inimigo atual e o anterior
                var DesireDistance = Vector3.Distance(sectEnemy.transform.position, lastEnemy.transform.position);

                // Ajusta a posição do inimigo atual se a distância for menor que o valor de `Distance`
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

    // Método para remover todos os inimigos exceto o primeiro da lista
    public void SaleEnemy()
    {
        for (int i = 1; i < Enemies.Count; i++)
        {
            Destroy(Enemies[i]);  // Destroi o inimigo e aumenta o dinheiro
            GameController.money += 10;
        }

        // Mantém o primeiro elemento da lista e remove os outros
        Enemies = new List<GameObject> { Enemies[0] };
        GameController.SaveData();
    }

    // Método para ativar a animação de soco
    public void Punch()
    {
        playerAnimator.SetTrigger("Punch");
    }

    // Verifica se o jogador está no chão ao colidir com objetos que têm a tag "Ground"
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    // Método que adiciona inimigos à lista ao entrar em um trigger com a tag "Enemy"
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && !isPunching && Enemies.Count <= GameController.empilhamentoIndex)
        {
            AddEnemyToStack(other);
        }
    }

    // Função para adicionar inimigos à lista
    private void AddEnemyToStack(Collider other)
    {
        Transform otherTransform = other.transform;
        Rigidbody otherRB = other.GetComponent<Rigidbody>();
        Collider[] otherColliders = other.GetComponentsInChildren<Collider>();

        otherRB.isKinematic = true;  // Define o Rigidbody como cinemático
        other.tag = gameObject.tag;  // Define a tag do inimigo igual à do jogador

        foreach (Collider col in otherColliders)
        {
            col.enabled = false;  // Desativa os colliders para evitar colisões
        }

        Enemies.Add(other.gameObject);  // Adiciona o inimigo à lista

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
