using UnityEngine;

public class Bandit : MonoBehaviour
{
    // Componentes de Ragdoll do bandido: Rigidbodies e Colliders
    [SerializeField] private Rigidbody[] _ragRb;      // Array de Rigidbodies para o efeito Ragdoll
    [SerializeField] private Collider[] _ragColliders; // Array de Colliders para o efeito Ragdoll
    [SerializeField] private AudioSource audioSource;  // Fonte de �udio para o som do soco
    [SerializeField] private ParticleSystem blood; // Particula para efeito de soco

    // Refer�ncia o jogador
    public Player player;

    // Inicializa o Ragdoll e configura��es de �udio
    void Awake()
    {
        // Coleta todos os Rigidbodies e Colliders filhos para o Ragdoll
        _ragRb = GetComponentsInChildren<Rigidbody>();
        _ragColliders = GetComponentsInChildren<Collider>();

        // Busca o objeto de �udio e obt�m o componente de �udio
        GameObject audioObject = GameObject.Find("AudioPunch");
        audioSource = audioObject.GetComponent<AudioSource>();

        // Desativa o Ragdoll inicialmente
        DisableRagdoll();
    }

    // Detecta colis�es para ativar o Ragdoll quando o jogador soca o bandido
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se o jogador est� socando e a colis�o ocorre com a m�o
        if (player.isPunching == true && collision.collider.tag == "Hand")
        {
            EnableRagdoll();  // Ativa o Ragdoll
            audioSource.Play();  // Reproduz o som do soco

            // Itera por todos os rigidbodies do ragdoll
            foreach (Rigidbody rigidbody in _ragRb)
            {
                // Calcula a dire��o, for�a do impulso e adiciona o efeito de particula no contato
                Vector3 direction = rigidbody.transform.position - collision.contacts[0].point;
                Vector3 collisionpoint = collision.contacts[0].point;
                
                ParticleSystem effect = Instantiate(blood, collisionpoint, Quaternion.identity);

                effect.Play();

                rigidbody.AddForce(direction.normalized * 90, ForceMode.Impulse);    
            }
        }        
    }

    // Mant�m o Ragdoll ativado enquanto o jogador est� socando e a colis�o � com a m�o
    private void OnCollisionStay(Collision collision)
    {
        if (player.isPunching == true && collision.collider.tag == "Hand")
        {
            EnableRagdoll();
            audioSource.Play();
            
            // Itera por todos os rigidbodies do ragdoll
            foreach (Rigidbody rigidbody in _ragRb)
            {
                // Calcula a dire��o e for�a do impulso
                Vector3 direction = rigidbody.transform.position - collision.contacts[0].point;
                Vector3 collisionpoint = collision.contacts[0].point;

                ParticleSystem effect = Instantiate(blood, collisionpoint, Quaternion.identity);

                effect.Play();

                rigidbody.AddForce(direction.normalized * 90, ForceMode.Impulse);
            }
        }
    }

    // Desativa o Ragdoll e ativa o controle de anima��o normal
    public void DisableRagdoll()
    {
        // Desativa todos os colliders do Ragdoll
        foreach (Collider col in _ragColliders)
        {
            col.enabled = false;
        }

        // Define os Rigidbodies do Ragdoll como cinem�ticos (sem resposta a f�sica)
        foreach (Rigidbody rigid in _ragRb)
        {
            rigid.isKinematic = true;
        }

        // Ativa o componente de anima��o e o collider principal
        GetComponent<Animator>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }

    // Ativa o Ragdoll, desativando a anima��o e o collider principal
    public void EnableRagdoll()
    {
        // Ativa todos os colliders do Ragdoll para intera��o f�sica
        foreach (Collider col in _ragColliders)
        {
            col.enabled = true;
        }

        // Define os Rigidbodies do Ragdoll para reagirem a f�sica
        foreach (Rigidbody rigid in _ragRb)
        {
            rigid.isKinematic = false;
        }

        // Desativa o componente de anima��o e remove o collider principal do objeto
        GetComponent<Animator>().enabled = false;
        Destroy(GetComponent<Collider>());
    }
}
