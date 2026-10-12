using UnityEngine;

public class ControladorMovimientoAnimacion : MonoBehaviour
{
    public Transform puntoDestino; // El punto exacto frente a la puerta donde se detiene
    public float velocidad = 3f;   // Velocidad a la que camina
    private Animator animator;
    private bool haLlegado = false;

    void Start()
    {
        // Obtenemos el componente Animator del personaje
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!haLlegado && puntoDestino != null)
        {
            // Mover al personaje progresivamente hacia el destino
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino.position, velocidad * Time.deltaTime);
            
            // Activar la animación de caminar
            if (animator != null)
            {
                animator.SetBool("estaCaminando", true);
            }

            // Verificar si ya llegó al destino
            if (Vector3.Distance(transform.position, puntoDestino.position) < 0.1f)
            {
                haLlegado = true;
                
                // Detener la animación de caminar (pasa a Idle/Quieto)
                if (animator != null)
                {
                    animator.SetBool("estaCaminando", false);
                }
            }
        }
    }
}