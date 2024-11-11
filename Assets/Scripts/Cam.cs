using UnityEngine;

public class Cam : MonoBehaviour
{
    // Vari�veis p�blicas para controlar a rota��o da c�mera nos eixos Y (horizontal) e X (vertical)
    public float Yaxis;  // Eixo Y para rota��o horizontal
    public float Xaxis;  // Eixo X para rota��o vertical

    // Limites de rota��o para o eixo X e tempo de suaviza��o para o movimento da c�mera
    float RotationMin = -30f;  // �ngulo m�nimo para rota��o no eixo X
    float RotationMax = 80f;   // �ngulo m�ximo para rota��o no eixo X
    float smoothTime = 0.4f;   // Tempo de suaviza��o para o movimento da c�mera

    // Vari�veis para controle de inputs m�veis
    public bool enableMobileInputs = false;  // Habilita ou desabilita o controle via input m�vel
    public FixedTouchField touchField;  // Campo de toque espec�fico para dispositivos m�veis

    // Vari�veis de rota��o e suaviza��o
    Vector3 targetRotation;  // Guarda a rota��o alvo da c�mera
    Vector3 currentVel;  // Guarda a velocidade atual da rota��o usada no SmoothDamp

    // Sensibilidade da rota��o
    public float RotationSensitivity = 8f;  // Controla a sensibilidade da rota��o da c�mera

    // O alvo que a c�mera deve seguir (player)
    public Transform target;

    void LateUpdate()
    {
        // Define os eixos de rota��o com base no tipo de input (m�vel ou mouse)
        if (enableMobileInputs)
        {
            // Se inputs m�veis estiverem ativados, ajusta os eixos Y e X da rota��o com base no campo de toque do dispositivo m�vel
            Yaxis += touchField.TouchDist.x * RotationSensitivity;
            Xaxis -= touchField.TouchDist.y * RotationSensitivity;
        }
        else
        {
            // Se inputs m�veis n�o estiverem ativados, ajusta os eixos Y e X da rota��o com base no input do mouse
            Yaxis += Input.GetAxis("Mouse X") * RotationSensitivity;
            Xaxis -= Input.GetAxis("Mouse Y") * RotationSensitivity;
        }

        // Limita a rota��o vertical (eixo X) dentro dos valores m�nimos e m�ximos definidos
        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);

        // Suaviza a transi��o para a rota��o alvo usando SmoothDamp para evitar movimentos bruscos
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);

        // Aplica a rota��o suavizada calculada � c�mera
        transform.eulerAngles = targetRotation;

        // Posiciona a c�mera atr�s do alvo (player) a uma dist�ncia fixa de 7 unidades
        transform.position = target.position - transform.forward * 7f;
    }
}
