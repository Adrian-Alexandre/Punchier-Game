using UnityEngine;

public class Cam : MonoBehaviour
{
    // Variáveis públicas para controlar a rotação da câmera nos eixos Y (horizontal) e X (vertical)
    public float Yaxis;  // Eixo Y para rotação horizontal
    public float Xaxis;  // Eixo X para rotação vertical

    // Limites de rotação para o eixo X e tempo de suavização para o movimento da câmera
    float RotationMin = -30f;  // Ângulo mínimo para rotação no eixo X
    float RotationMax = 80f;   // Ângulo máximo para rotação no eixo X
    float smoothTime = 0.4f;   // Tempo de suavização para o movimento da câmera

    // Variáveis para controle de inputs móveis
    public bool enableMobileInputs = false;  // Habilita ou desabilita o controle via input móvel
    public FixedTouchField touchField;  // Campo de toque específico para dispositivos móveis

    // Variáveis de rotação e suavização
    Vector3 targetRotation;  // Guarda a rotação alvo da câmera
    Vector3 currentVel;  // Guarda a velocidade atual da rotação usada no SmoothDamp

    // Sensibilidade da rotação
    public float RotationSensitivity = 8f;  // Controla a sensibilidade da rotação da câmera

    // O alvo que a câmera deve seguir (player)
    public Transform target;

    void LateUpdate()
    {
        // Define os eixos de rotação com base no tipo de input (móvel ou mouse)
        if (enableMobileInputs)
        {
            // Se inputs móveis estiverem ativados, ajusta os eixos Y e X da rotação com base no campo de toque do dispositivo móvel
            Yaxis += touchField.TouchDist.x * RotationSensitivity;
            Xaxis -= touchField.TouchDist.y * RotationSensitivity;
        }
        else
        {
            // Se inputs móveis não estiverem ativados, ajusta os eixos Y e X da rotação com base no input do mouse
            Yaxis += Input.GetAxis("Mouse X") * RotationSensitivity;
            Xaxis -= Input.GetAxis("Mouse Y") * RotationSensitivity;
        }

        // Limita a rotação vertical (eixo X) dentro dos valores mínimos e máximos definidos
        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);

        // Suaviza a transição para a rotação alvo usando SmoothDamp para evitar movimentos bruscos
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);

        // Aplica a rotação suavizada calculada à câmera
        transform.eulerAngles = targetRotation;

        // Posiciona a câmera atrás do alvo (player) a uma distância fixa de 7 unidades
        transform.position = target.position - transform.forward * 7f;
    }
}
