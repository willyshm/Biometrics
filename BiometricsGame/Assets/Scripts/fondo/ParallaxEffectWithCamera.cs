using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffectWithCamera : MonoBehaviour
{
    public Transform[] backgrounds;     // Array de capas del fondo
    public float[] parallaxScales;      // Proporción de movimiento de cada capa
    public float smoothing = 1f;        // Suavizado del movimiento del parallax
    private Transform cameraTransform;  // Referencia a la cámara principal
    private Vector3 previousCameraPosition;  // Posición de la cámara en el frame anterior

    void Start()
    {
        // Obtenemos la referencia de la cámara y guardamos su posición inicial
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

        // Si no se han asignado valores de parallaxScales, los establecemos por defecto
        if (parallaxScales.Length == 0)
        {
            parallaxScales = new float[backgrounds.Length];
            for (int i = 0; i < parallaxScales.Length; i++)
            {
                parallaxScales[i] = 1f;  // Velocidad estándar para cada capa
            }
        }
    }

    void LateUpdate()
    {
        // Recorremos cada capa del fondo
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // Calculamos cuánto se ha movido la cámara desde el último frame
            float parallax = (previousCameraPosition.x - cameraTransform.position.x) * parallaxScales[i];

            // Nueva posición de la capa (solo afectamos el eje X para un fondo horizontal)
            float targetXPosition = backgrounds[i].position.x + parallax;

            // Actualizamos la posición de la capa suavemente
            Vector3 newBackgroundPosition = new Vector3(targetXPosition, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, newBackgroundPosition, smoothing * Time.deltaTime);
        }

        // Actualizamos la posición anterior de la cámara para el próximo frame
        previousCameraPosition = cameraTransform.position;

        // Mantén las capas del fondo siempre frente a la cámara (opcional, si es necesario)
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position = new Vector3(backgrounds[i].position.x, backgrounds[i].position.y, cameraTransform.position.z + 10f);  // Mantén el fondo en frente de la cámara
        }
    }
}
