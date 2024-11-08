using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoSlime : MonoBehaviour
{
    private Rigidbody2D rigidbody2;
    
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float distancia;
    [SerializeField] private LayerMask Ensuelo;
    [SerializeField] private float vida;

    private Animator animator;

    private void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        rigidbody2.velocity = new Vector2(velocidadMovimiento * transform.right.x, rigidbody2.velocity.y);
        RaycastHit2D infSuelo = Physics2D.Raycast(transform.position, transform.right, distancia, Ensuelo);
        
        if (infSuelo) {
            Girar();
        }
    }

    public void TomarDaño(float daño)
{
    vida -= daño;

    // Convierte el daño a int antes de pasarlo
    GameManager.Instance.AddDamage(Mathf.RoundToInt(daño));

    if (vida <= 0)
    {
        Muerte();
    }
}

    private void Muerte()
    {
        animator.SetTrigger("Muerte");
        float tiempoMuerte = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("DestruirEnemigo", tiempoMuerte);
    }

    private void DestruirEnemigo()
    {
        Destroy(gameObject);
    }

    private void Girar()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * distancia);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccion = new Vector2(transform.position.x, 0);
            collision.gameObject.GetComponent<Jugador>().DañoRecibido(direccion);
            GameManager.Instance.PerderVida();
        }
    }
}
