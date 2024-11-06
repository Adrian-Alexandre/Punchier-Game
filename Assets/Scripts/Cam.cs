using UnityEngine;

public class Cam : MonoBehaviour
{
    // Eixos de rotação para controlar a posição da câmera
    public float Yaxis;  // Eixo Y para rotação horizontal
    public float Xaxis;  // Eixo X para rotação vertical

    // Limites de rotação e suavização para a câmera
    float RotationMin = -30f;  // Ângulo mínimo para rotação no eixo X
    float RotationMax = 80f;   // Ângulo máximo para rotação no eixo X
    float smoothTime = 0.4f;   // Tempo de suavização para o movimento da câmera

    // Controle de inputs móveis
    public bool enableMobileInputs = false;     // Habilita ou desabilita o controle via input móvel
    public FixedTouchField touchField;          // Campo de toque para dispositivos móveis

    // Variáveis de rotação e suavização
    Vector3 targetRotation;  // Rotação alvo da câmera
    Vector3 currentVel;      // Velocidade atual da rotação usada no SmoothDamp

    // Sensibilidade da rotação
    public float RotationSensitivity = 8f;

    // Alvo que a câmera deve seguir
    public Transform target;

    void LateUpdate()
    {
        // Define os eixos de rotação com base no tipo de input (móvel ou mouse)
        if (enableMobileInputs)
        {
            // Ajusta o eixo Y e o eixo X da rotação com base no campo de toque em dispositivos móveis
            Yaxis += touchField.TouchDist.x * RotationSensitivity;
            Xaxis -= touchField.TouchDist.y * RotationSensitivity;
        }
        else
        {
            // Ajusta o eixo Y e o eixo X da rotação com base no input do mouse
            Yaxis += Input.GetAxis("Mouse X") * RotationSensitivity;
            Xaxis -= Input.GetAxis("Mouse Y") * RotationSensitivity;
        }

        // Limita a rotação vertical (eixo X) dentro dos valores mínimos e máximos
        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);

        // Suaviza a transição para a rotação alvo usando SmoothDamp
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);

        // Aplica a rotação calculada à câmera
        transform.eulerAngles = targetRotation;

        // Posiciona a câmera atrás do alvo a uma distância fixa de 10 unidades
        transform.position = target.position - transform.forward * 10f;
    }
}
