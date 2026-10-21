using UnityEngine;

public class MovimientoNPC : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 2f;

    [Header("Sprite de Reposo (Frente)")]
    public Sprite spriteIdleFrente;

    private Transform puntoDestino;
    private Transform puntoSalida;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool haLlegado = false;
    private bool estaSaliendo = false;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Buscar los puntos en la jerarquía
        GameObject objetoDestino = GameObject.Find("PuntoDestinoPuerto");
        GameObject objetoSalida = GameObject.Find("PuntoSalidaDerecha");
        
        if (objetoDestino != null) puntoDestino = objetoDestino.transform;
        if (objetoSalida != null) puntoSalida = objetoSalida.transform;

        if (objetoDestino == null) Debug.LogError("No se encontró 'PuntoDestinoPuerto'.");
        if (objetoSalida == null) Debug.LogError("No se encontró 'PuntoSalidaDerecha'.");

        // Activar animación al iniciar
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Animacion NPC Caminando", 0, 0f);
        }
    }

    void Update()
    {
        // Si fue rechazado y va caminado hacia el punto de salida derecho
        if (estaSaliendo && puntoSalida != null)
        {
            float paso = velocidad * Time.deltaTime;
            Vector3 objetivoSalida = new Vector3(puntoSalida.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, objetivoSalida, paso);

            // Destruir al llegar al punto de salida fuera de pantalla
            if (Mathf.Abs(transform.position.x - puntoSalida.position.x) < 0.05f)
            {
                Destroy(gameObject);
            }
            return;
        }

        if (puntoDestino == null || haLlegado) return;

        // Movimiento normal hacia la puerta
        float pasoNormal = velocidad * Time.deltaTime;
        Vector3 objetivo = new Vector3(puntoDestino.position.x, transform.position.y, transform.position.z);
        
        transform.position = Vector3.MoveTowards(transform.position, objetivo, pasoNormal);

        if (Mathf.Abs(transform.position.x - puntoDestino.position.x) < 0.05f)
        {
            LlegarAPuerta();
        }
    }

    void LlegarAPuerta()
    {
        haLlegado = true;

        if (animator != null) animator.enabled = false;
        if (spriteRenderer != null && spriteIdleFrente != null) spriteRenderer.sprite = spriteIdleFrente;

        FindObjectOfType<ControlInteraccion>().LlegoNPC();
    }
    
    public void AccionAceptar()
    {
        Destroy(gameObject, 1f); 
    }

    public void AccionRechazar()
    {
        // Reactivar la animación y orientar a la derecha
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Animacion NPC Caminando", 0, 0f);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = false; 
        }

        // Indicar que se dirige hacia el punto de salida
        estaSaliendo = true;
    }
}