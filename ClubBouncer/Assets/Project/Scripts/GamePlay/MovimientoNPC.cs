using System.Collections.Generic;
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

    private ClienteData clienteActual;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        float pasoNormal = velocidad * Time.deltaTime;
        Vector3 objetivo = new Vector3(puntoDestino.position.x, transform.position.y, transform.position.z);
        
        transform.position = Vector3.MoveTowards(transform.position, objetivo, pasoNormal);

        if (Mathf.Abs(transform.position.x - puntoDestino.position.x) < 0.05f)
        {
            LlegarAPuerta();
        }
    }

    public void EstablecerCliente(ClienteData datos)
    {
        clienteActual = datos;
        haLlegado = false;
        estaSaliendo = false;
        transform.position = posicionInicial;

        // Asignamos el controlador exclusivo que viene del ScriptableObject
        if (animator != null && clienteActual != null && clienteActual.controladorAnimator != null)
        {
            animator.runtimeAnimatorController = clienteActual.controladorAnimator;
        }

        IniciarCaminata();
    }

    void LlegarAPuerta()
    {
        haLlegado = true;

        if (animator != null) 
        {
            animator.enabled = false;
        }

        if (spriteRenderer != null && clienteActual != null && clienteActual.spriteLateralParado != null)
        {
            spriteRenderer.sprite = clienteActual.spriteLateralParado;
        }

        FindObjectOfType<ControlInteraccion>()?.LlegoNPC();
    }

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
            // El controlador propio ya sabe qué animación reproducir al encenderse
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
        }

        if (spriteRenderer != null && clienteActual != null && clienteActual.spriteLateralCaminando != null)
        {
            spriteRenderer.sprite = clienteActual.spriteLateralCaminando;
        }
    }
}