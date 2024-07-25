using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPickup : MonoBehaviour
{
    public Inventario inventario; // Referencia al inventario del personaje

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Items item = collision.GetComponent<Items>();
            if (item != null)
            {
                inventario.AddItem(item.icon, item.tipo);
                Destroy(collision.gameObject); // Destruir el objeto en la escena una vez recogido
            }
        }
    }
}
