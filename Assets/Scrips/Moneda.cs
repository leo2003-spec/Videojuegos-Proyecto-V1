using System.Collections;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    public int valor = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddCoin(valor); // Llama a AddCoin en GameManager
            GameStats.CoinsCollected += valor; // Actualiza el contador de monedas en GameStats
            Destroy(gameObject); // Destruye el objeto de la moneda
        }
    }
}
