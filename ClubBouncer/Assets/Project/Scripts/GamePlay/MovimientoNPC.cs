using UnityEngine;

public class MovimientoNPC : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 2f;

    private Transform puntoDestino;
    private Transform puntoSalida;
    private Transform puntoInicio;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool haLlegado = false;
    private bool estaSaliendo = false;
    private Vector3 posicionInicial;

    // Almacenamos el cliente actual para usar sus 3 sprites
    private ClienteData clienteActual;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Buscar puntos de referencia
        GameObject objetoDestino = GameObject.Find("PuntoDestinoPuerto");
        GameObject objetoSalida = GameObject.Find("PuntoSalidaDerecha");
        GameObject objetoInicio = GameObject.Find("PuntoInicioIzquierda");
        
        if (objetoDestino != null) puntoDestino = objetoDestino.transform;
        if (objetoSalida != null) puntoSalida = objetoSalida.transform;
        if (objetoInicio != null) puntoInicio = objetoInicio.transform;

        if (objetoInicio != null)
        {
            posicionInicial = objetoInicio.transform.position;
        }
        else
        {
            posicionInicial = transform.position;
        }
    }

    void Update()
    {
        // 1. Si fue RECHAZADO: camina hacia la derecha (salida)
        if (estaSaliendo && puntoSalida != null)
        {
            float paso = velocidad * Time.deltaTime;
            Vector3 objetivoSalida = new Vector3(puntoSalida.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, objetivoSalida, paso);

            if (Mathf.Abs(transform.position.x - puntoSalida.position.x) < 0.05f)
            {
                ReiniciarAlPuntoInicial();
            }
            return;
        }

        if (puntoDestino == null || haLlegado) return;

        // 2. Movimiento NORMAL hacia la puerta del club
        float pasoNormal = velocidad * Time.deltaTime;
        Vector3 objetivo = new Vector3(puntoDestino.position.x, transform.position.y, transform.position.z);
        
        transform.position = Vector3.MoveTowards(transform.position, objetivo, pasoNormal);

        if (Mathf.Abs(transform.position.x - puntoDestino.position.x) < 0.05f)
        {
            LlegarAPuerta();
        }
    }

    // Este método lo llama el GeneradorClientes al iniciar o pasar de cliente
    public void EstablecerCliente(ClienteData datos)
    {
        clienteActual = datos;
        haLlegado = false;
        estaSaliendo = false;
        transform.position = posicionInicial;
        IniciarCaminata();
    }

    void LlegarAPuerta()
    {
        haLlegado = true;

        // Desactivamos el animator para que deje mostrar el sprite estático de perfil
        if (animator != null) animator.enabled = false;

        if (spriteRenderer != null && clienteActual != null && clienteActual.spriteLateralParado != null)
        {
            spriteRenderer.sprite = clienteActual.spriteLateralParado;
        }

        FindObjectOfType<ControlInteraccion>()?.LlegoNPC();
    }

    // Llama a este método cuando quieras ponerlo de frente (al abrir la tarjeta/UI)
    public void CambiarAFrontal()
    {
        if (animator != null) animator.enabled = false;

        if (spriteRenderer != null && clienteActual != null && clienteActual.spriteFrontal != null)
        {
            spriteRenderer.sprite = clienteActual.spriteFrontal;
        }
    }
    
    public void AccionAceptar()
    {
        CambiarAFrontal();
        Invoke(nameof(ReiniciarAlPuntoInicial), 1.5f);
    }

    public void AccionRechazar()
    {
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Animacion NPC Caminando", 0, 0f);
        }

        if (spriteRenderer != null && clienteActual != null && clienteActual.spriteLateralCaminando != null)
        {
            spriteRenderer.sprite = clienteActual.spriteLateralCaminando;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = false; 
        }

        estaSaliendo = true;
    }

    public void ReiniciarAlPuntoInicial()
    {
        estaSaliendo = false;
        haLlegado = false;
        transform.position = posicionInicial;

        ControladorUI controlUI = FindObjectOfType<ControladorUI>();
        if (controlUI != null && controlUI.textoAvisoResultado != null)
        {
            controlUI.textoAvisoResultado.text = "";
        }

        GeneradorClientes generador = FindObjectOfType<GeneradorClientes>();
        if (generador != null)
        {
            generador.SiguienteCliente();
        }
    }

    private void IniciarCaminata()
    {
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Animacion NPC Caminando", 0, 0f);
        }

        if (spriteRenderer != null && clienteActual != null && clienteActual.spriteLateralCaminando != null)
        {
            spriteRenderer.sprite = clienteActual.spriteLateralCaminando;
        }
    }
}