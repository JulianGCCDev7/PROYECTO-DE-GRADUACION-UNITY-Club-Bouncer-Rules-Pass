using UnityEngine;
using TMPro;

public class ControlInteraccion : MonoBehaviour
{
    [Header("Referencias de Interfaz (UI)")]
    public GameObject botonInspeccionar;
    public GameObject tarjetaID;

    [Header("Texto de Aviso en Pantalla")]
    public TextMeshProUGUI textoAvisoResultado;

    [Header("Referencias de Gameplay")]
    public EvaluadorPortero evaluador;
    public GeneradorClientes generador;

    [Header("Estados de Control")]
    public bool npcEnLaPuerta = false;
    private bool tarjetaAbierta = false;

    private void Start()
    {
        if (botonInspeccionar != null) botonInspeccionar.SetActive(false);
        if (tarjetaID != null) tarjetaID.SetActive(false);
        if (textoAvisoResultado != null) textoAvisoResultado.text = "";
    }

    private void Update()
    {
        if (npcEnLaPuerta && !tarjetaAbierta)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                AbrirTarjeta();
            }
        }
    }

    public void LlegoNPC()
    {
        npcEnLaPuerta = true;
        tarjetaAbierta = false; // Permitir abrir la tarjeta de nuevo para este nuevo NPC

        // Limpiar mensaje de resultado anterior al llegar un nuevo cliente
        if (textoAvisoResultado != null) 
        {
            textoAvisoResultado.text = "";
        }

        if (botonInspeccionar != null)
        {
            botonInspeccionar.SetActive(true);
        }
    }

    public void AbrirTarjeta()
    {
        tarjetaAbierta = true;

        if (botonInspeccionar != null)
        {
            botonInspeccionar.SetActive(false);
        }

        if (tarjetaID != null)
        {
            tarjetaID.SetActive(true);
        }

        // AQUÍ ESTÁ LA MAGIA: Ordenamos al MovimientoNPC que cambie al sprite de frente
        MovimientoNPC npcActual = FindObjectOfType<MovimientoNPC>();
        if (npcActual != null)
        {
            npcActual.CambiarAFrontal();
        }
    }

    public void BotonAceptar()
    {
        ProcesarDecision(true);
    }

    public void BotonRechazar()
    {
        ProcesarDecision(false);
    }

    private void ProcesarDecision(bool permitioPaso)
    {
        MovimientoNPC npcActual = FindObjectOfType<MovimientoNPC>();
        
        if (evaluador == null)
        {
            evaluador = FindObjectOfType<EvaluadorPortero>();
        }

        if (generador == null)
        {
            generador = FindObjectOfType<GeneradorClientes>();
        }

        if (evaluador != null && generador != null)
        {
            ClienteData clienteActual = generador.ObtenerClienteActual();

            if (clienteActual != null)
            {
                // Capturamos el mensaje detallado del EvaluadorPortero
                string mensajeResultado = evaluador.DecidirEntrada(clienteActual, permitioPaso);

                // Lo mostramos en pantalla
                if (textoAvisoResultado != null)
                {
                    textoAvisoResultado.text = mensajeResultado;
                }
            }
            else
            {
                Debug.LogWarning("No hay un cliente actual disponible en el GeneradorClientes.");
            }
        }
        else
        {
            Debug.LogWarning("Falta asignar Evaluador o Generador en ControlInteraccion.");
        }

        // Ejecutar acción física en el NPC
        if (npcActual != null)
        {
            if (permitioPaso)
            {
                npcActual.AccionAceptar();
            }
            else
            {
                npcActual.AccionRechazar();
            }
        }

        CerrarTarjetaTemporal();
    }

    private void CerrarTarjetaTemporal()
    {
        // Ocultamos la tarjeta visualmente al tomar la decisión, pero mantenemos el flujo listo
        if (tarjetaID != null)
        {
            tarjetaID.SetActive(false);
        }
    }
}