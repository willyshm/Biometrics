using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffectWithCamera : MonoBehaviour
{
    public Transform[] backgrounds;     // Array de capas del fondo
    public float[] parallaxScales;      // Proporci�n de movimiento de cada capa
    public float smoothing = 1f;        // Suavizado del movimiento del parallax
    private Transform cameraTransform;  // Referencia a la c�mara principal
    private Vector3 previousCameraPosition;  // Posici�n de la c�mara en el frame anterior

    void Start()
    {
        // Obtenemos la referencia de la c�mara y guardamos su posici�n inicial
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

        // Si no se han asignado valores de parallaxScales, los establecemos por defecto
        if (parallaxScales.Length == 0)
        {
            parallaxScales = new float[backgrounds.Length];
            for (int i = 0; i < parallaxScales.Length; i++)
            {
                parallaxScales[i] = 1f;  // Velocidad est�ndar para cada capa
            }
        }
    }

    void LateUpdate()
    {
        // Recorremos cada capa del fondo
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // Calculamos cu�nto se ha movido la c�mara desde el �ltimo frame
            float parallax = (previousCameraPosition.x - cameraTransform.position.x) * parallaxScales[i];

            // Nueva posici�n de la capa (solo afectamos el eje X para un fondo horizontal)
            float targetXPosition = backgrounds[i].position.x + parallax;

            // Actualizamos la posici�n de la capa suavemente
            Vector3 newBackgroundPosition = new Vector3(targetXPosition, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, newBackgroundPosition, smoothing * Time.deltaTime);
        }

        // Actualizamos la posici�n anterior de la c�mara para el pr�ximo frame
        previousCameraPosition = cameraTransform.position;

        // Mant�n las capas del fondo siempre frente a la c�mara (opcional, si es necesario)
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position = new Vector3(backgrounds[i].position.x, backgrounds[i].position.y, cameraTransform.position.z + 10f);  // Mant�n el fondo en frente de la c�mara
        }
    }
}
