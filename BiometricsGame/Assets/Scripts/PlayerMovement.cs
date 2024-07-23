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

    /*Este espacio de codigo configura los controles*/

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

    /*Fin de configuracion de controles*/

    /*Este espacio del codigo es para mover el personaje*/
    private void Update()
    {
        direccion = controles.Movimiento.Mover.ReadValue<Vector2>();
        ajustarRotacion(direccion.x);
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimesionesCaja, 00f, queEsSuelo);
    }
    private void FixedUpdate()
    {
        Rbd2d.velocity = new Vector2(direccion.x * VelocidadMovimiento, Rbd2d.velocity.y);
    }
    private void ajustarRotacion (float direccionX)
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
    
    /*fin de mover el personaje*/

    /*Configuracion de salto*/

    private void Saltar()
    {
        if (enSuelo == true)
        {
            Rbd2d.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimesionesCaja);
    }
}
