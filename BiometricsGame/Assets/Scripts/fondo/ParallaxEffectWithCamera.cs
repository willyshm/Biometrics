using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffectWithCamera : MonoBehaviour
{
    public Transform cameraTransform;  // La cámara principal
    public float parallaxEffect;       // Velocidad de parallax

    private float lengthX;             // Longitud del sprite en el eje X
    private float startPosX;           // Posición inicial del sprite en X

    void Start()
    {
        // Obtenemos la posición inicial y el tamaño del sprite en X
        startPosX = transform.position.x;
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x;

        // Si no se asigna la cámara, tomamos la principal
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Calculamos el movimiento del fondo basado en el movimiento de la cámara
        float temp = (cameraTransform.position.x * (1 - parallaxEffect));
        float dist = (cameraTransform.position.x * parallaxEffect);

        // Actualizamos la posición del fondo con parallax
        transform.position = new Vector3(startPosX + dist, transform.position.y, transform.position.z);

        // Repetimos el fondo cuando la cámara pasa el borde del sprite
        if (temp > startPosX + lengthX)
        {
            startPosX += lengthX;
        }
        else if (temp < startPosX - lengthX)
        {
            startPosX -= lengthX;
        }
    }
}
