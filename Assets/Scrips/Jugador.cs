using System;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float speedCaminar;
    private float movimiento;
    public float fuerzaSalto;
    public LayerMask capaSuelo;
    public float fuerzaGolpe;
    public int danioPorGolpe = 10; // Daño que el jugador inflige a los enemigos

    private Rigidbody2D rb2D; 
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private bool recibeDaño;

    // Sonido de caminar
    public AudioClip sonidoCaminar;
    private AudioSource audioSource;

    // Escaleras
    [SerializeField] private float velocidadEscalar;
    private float gravedadInit;
    private bool escalando;
    private Vector2 input;

    private bool sonidoEnReproduccion = false;  

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        gravedadInit = rb2D.gravityScale;

        if (audioSource == null)
        {
            Debug.LogError("AudioSource no encontrado en el objeto del jugador.");
        }
        else
        {
            audioSource.clip = sonidoCaminar;
        }
    }

    void Update()
    {
        if (!recibeDaño)
            movimiento = Input.GetAxis("Horizontal") * speedCaminar;
        input.y = Input.GetAxis("Vertical");
        rb2D.velocity = new Vector2(movimiento, rb2D.velocity.y); 

        // Reproduce el sonido de caminar solo si el personaje está en movimiento
        if (movimiento != 0)
        {
            animator.SetBool("Caminando", true);
            Vector3 escala = transform.localScale;
            escala.x = movimiento > 0 ? Mathf.Abs(escala.x) : -Mathf.Abs(escala.x);
            transform.localScale = escala;

            if (!sonidoEnReproduccion)
            {
                ReproducirSonido();
            }
        }
        else
        {
            animator.SetBool("Caminando", false);
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                sonidoEnReproduccion = false;
            }
        }

        if (Math.Abs(rb2D.velocity.y) > Mathf.Epsilon)
        {
            animator.SetFloat("VelocidadY", Mathf.Sign(rb2D.velocity.y));
        }
        else
        {
            animator.SetFloat("VelocidadY", 0);
        }

        ProcesarSalto();
        Escalar();
        animator.SetBool("Recibedamage", recibeDaño);
    }

    void ProcesarSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && EstaenSuelo() && !recibeDaño)
        {
            rb2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }

        animator.SetBool("EnSuelo", EstaenSuelo());
    }

    bool EstaenSuelo()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, capaSuelo);
        return raycastHit2D.collider != null;
    }

    public void DañoRecibido(Vector2 direccion)
    {
        if (!recibeDaño)
        {
            recibeDaño = true;
            Vector2 rebore = new Vector2(transform.position.x - direccion.x, 0.5f).normalized;
            rb2D.AddForce(rebore * fuerzaGolpe, ForceMode2D.Impulse);
        }
    }

    public void DesactivaDaño()
    {
        recibeDaño = false;
        rb2D.velocity = Vector2.zero;
    }

    void ReproducirSonido()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            sonidoEnReproduccion = true;
        }
    }

    private void Escalar()
    {
        if ((input.y != 0 || escalando) && (boxCollider.IsTouchingLayers(LayerMask.GetMask("Escaleras"))))
        {
            Vector2 velocidadSubida = new Vector2(rb2D.velocity.x, input.y * velocidadEscalar);
            rb2D.velocity = velocidadSubida;
            rb2D.gravityScale = 0;
            escalando = true;
        }
        else
        {
            rb2D.gravityScale = gravedadInit;
            escalando = false;
        }

        animator.SetBool("Escalando", escalando);
    }

    // Método para infligir daño al enemigo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            EnemigoSlime enemigo = collision.GetComponent<EnemigoSlime>();
            if (enemigo != null)
            {
                enemigo.TomarDaño(danioPorGolpe); // Inflige daño
                GameManager.Instance.AddDamage(danioPorGolpe); // Suma el daño al GameManager
            }
        }
    }
}
