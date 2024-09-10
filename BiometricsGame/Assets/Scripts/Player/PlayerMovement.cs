using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este script controla el movimiento del personaje//
public class PlayerMovement : MonoBehaviour
{
    /*Variables*/
    public Controles controles;
    public Vector2 direccion;
    public Rigidbody2D Rbd2d;
    public float VelocidadMovimiento;
    public bool mirandoDerecha = true;
    public float fuerzaSalto;
    public LayerMask queEsSuelo;
    public Transform controladorSuelo;
    public Vector3 dimesionesCaja;
    public bool enSuelo;

    // Añadimos referencia al Animator
    public Animator animator;

    private void Awake()
    {
        controles = new();
    }

    private void OnEnable()
    {
        controles.Enable();
        controles.Movimiento.Saltar.started += _ => Saltar();
    }

    private void OnDisable()
    {
        controles.Disable();
        controles.Movimiento.Saltar.started -= _ => Saltar();
    }

    private void Update()
    {
        direccion = controles.Movimiento.Mover.ReadValue<Vector2>();
        ajustarRotacion(direccion.x);

        // Detectar si está en el suelo
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimesionesCaja, 0f, queEsSuelo);

        // Actualizar animaciones
        animator.SetFloat("Velocidad", Mathf.Abs(direccion.x)); // Velocidad absoluta (para ambas direcciones)
        animator.SetBool("enSuelo", enSuelo);

        // Solo marcar que está saltando si no está en el suelo
        if (!enSuelo)
        {
            animator.SetBool("Saltar", true);
        }
    }

    private void FixedUpdate()
    {
        Rbd2d.velocity = new Vector2(direccion.x * VelocidadMovimiento, Rbd2d.velocity.y);
    }

    private void ajustarRotacion(float direccionX)
    {
        if (direccionX > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (direccionX < 0 && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void Saltar()
    {
        if (enSuelo)
        {
            Rbd2d.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
            animator.SetBool("Saltar", true);  // Iniciar animación de salto
        }
    }

    // Detectar cuándo el personaje ha aterrizado
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            animator.SetBool("Saltar", false);  // Terminar animación de salto
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimesionesCaja);
    }
}

