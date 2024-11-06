using UnityEngine;

public class Cam : MonoBehaviour
{
    // Eixos de rota��o para controlar a posi��o da c�mera
    public float Yaxis;  // Eixo Y para rota��o horizontal
    public float Xaxis;  // Eixo X para rota��o vertical

    // Limites de rota��o e suaviza��o para a c�mera
    float RotationMin = -30f;  // �ngulo m�nimo para rota��o no eixo X
    float RotationMax = 80f;   // �ngulo m�ximo para rota��o no eixo X
    float smoothTime = 0.4f;   // Tempo de suaviza��o para o movimento da c�mera

    // Controle de inputs m�veis
    public bool enableMobileInputs = false;     // Habilita ou desabilita o controle via input m�vel
    public FixedTouchField touchField;          // Campo de toque para dispositivos m�veis

    // Vari�veis de rota��o e suaviza��o
    Vector3 targetRotation;  // Rota��o alvo da c�mera
    Vector3 currentVel;      // Velocidade atual da rota��o usada no SmoothDamp

    // Sensibilidade da rota��o
    public float RotationSensitivity = 8f;

    // Alvo que a c�mera deve seguir
    public Transform target;

    void LateUpdate()
    {
        // Define os eixos de rota��o com base no tipo de input (m�vel ou mouse)
        if (enableMobileInputs)
        {
            // Ajusta o eixo Y e o eixo X da rota��o com base no campo de toque em dispositivos m�veis
            Yaxis += touchField.TouchDist.x * RotationSensitivity;
            Xaxis -= touchField.TouchDist.y * RotationSensitivity;
        }
        else
        {
            // Ajusta o eixo Y e o eixo X da rota��o com base no input do mouse
            Yaxis += Input.GetAxis("Mouse X") * RotationSensitivity;
            Xaxis -= Input.GetAxis("Mouse Y") * RotationSensitivity;
        }

        // Limita a rota��o vertical (eixo X) dentro dos valores m�nimos e m�ximos
        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);

        // Suaviza a transi��o para a rota��o alvo usando SmoothDamp
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);

        // Aplica a rota��o calculada � c�mera
        transform.eulerAngles = targetRotation;

        // Posiciona a c�mera atr�s do alvo a uma dist�ncia fixa de 10 unidades
        transform.position = target.position - transform.forward * 10f;
    }
}
