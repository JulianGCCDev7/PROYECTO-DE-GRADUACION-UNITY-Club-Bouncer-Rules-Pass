using UnityEngine;

public class MovimientoNPC : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 2f;

    [Header("Sprite de Reposo (Frente)")]
    [Tooltip("Arrastra aquí desde la carpeta Assets el Sprite del personaje mirando de frente / estático.")]
    public Sprite spriteIdleFrente;

    private Transform puntoDestino;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool haLlegado = false;
    

    void Start()
    {
        // Obtener componentes
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Buscar el objeto de destino en la jerarquía
        GameObject objetoDestino = GameObject.Find("PuntoDestinoPuerto");
        
        if (objetoDestino != null)
        {
            puntoDestino = objetoDestino.transform;
        }
        else
        {
            Debug.LogError("No se encontró el objeto 'PuntoDestinoPuerto' en la Jerarquía.");
        }

        // Asegurarse de que el Animator esté reproduciendo la caminata al inicio
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Animacion NPC Caminando", 0, 0f);
        }
    }

    void Update()
    {
        if (puntoDestino == null || haLlegado) return;

        // Desplazar al personaje hacia el punto de destino
        float paso = velocidad * Time.deltaTime;
        Vector3 objetivo = new Vector3(puntoDestino.position.x, transform.position.y, transform.position.z);
        
        transform.position = Vector3.MoveTowards(transform.position, objetivo, paso);

        // Al alcanzar la posición de la puerta
        if (Mathf.Abs(transform.position.x - puntoDestino.position.x) < 0.05f)
        {
            LlegarAPuerta();
        }
    }

    void LlegarAPuerta()
    {
        haLlegado = true;

        // 1. Desactivar el Animator para detener la secuencia de caminata
        if (animator != null)
        {
            animator.enabled = false;
        }

        // 2. Cambiar la imagen directamente al Sprite estático/de frente
        if (spriteRenderer != null && spriteIdleFrente != null)
        {
            spriteRenderer.sprite = spriteIdleFrente;
        }

        FindObjectOfType<ControlInteraccion>().LlegoNPC();
    }
    // Método llamado al presionar ACEPTAR
    public void AccionAceptar()
    {
        // Aquí puedes poner la lógica cuando el NPC entra
        Destroy(gameObject, 1f); 
    }

    // Método llamado al presionar RECHAZAR
    public void AccionRechazar()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = true; // Voltea el sprite para que regrese
        }
        Destroy(gameObject, 1.5f); 
    }
}