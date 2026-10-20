using UnityEngine;
using TMPro; // Necesario para controlar los textos de TextMeshPro

public class ControlInteraccion : MonoBehaviour
{
    [Header("Referencias de Interfaz (UI)")]
    [Tooltip("Arrastra aquí el Botón de Inspeccionar que creaste en el Canvas")]
    public GameObject botonInspeccionar;

    [Tooltip("Arrastra aquí tu Panel de TarjetaID del Canvas")]
    public GameObject tarjetaID;

    [Header("Texto de Aviso en Pantalla")]
    [Tooltip("Arrastra aquí el texto del Canvas (TextoErrores o TextoAviso) para mostrar los resultados.")]
    public TextMeshProUGUI textoAvisoResultado;

    [Header("Estados de Control")]
    public bool npcEnLaPuerta = false;
    private bool tarjetaAbierta = false;

    private void Start()
    {
        // Asegurarnos de que la UI empiece oculta al iniciar la escena
        if (botonInspeccionar != null) botonInspeccionar.SetActive(false);
        if (tarjetaID != null) tarjetaID.SetActive(false);
        
        // Añade esta línea para que el texto empiece vacío al iniciar el juego:
        if (textoAvisoResultado != null) textoAvisoResultado.text = "";
    }

    private void Update()
    {
        // Detecta la tecla 'E' o 'Espacio' cuando el NPC ya llegó a la puerta
        if (npcEnLaPuerta && !tarjetaAbierta)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                AbrirTarjeta();
            }
        }
    }

    /// <summary>
    /// Llama a esta función desde el script de movimiento del NPC cuando termine de caminar.
    /// </summary>
    public void LlegoNPC()
    {
        npcEnLaPuerta = true;
        if (botonInspeccionar != null)
        {
            botonInspeccionar.SetActive(true);
        }
    }

    /// <summary>
    /// Abre la tarjeta de identificación y oculta el botón de inspeccionar.
    /// </summary>
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
    }

    /// <summary>
    /// Método para asignar al evento OnClick() del botón ACEPTAR en Unity.
    /// </summary>
    public void BotonAceptar()
    {
        ProcesarDecision(true);
    }

    /// <summary>
    /// Método para asignar al evento OnClick() del botón RECHAZAR en Unity.
    /// </summary>
    public void BotonRechazar()
    {
        ProcesarDecision(false);
    }

    /// <summary>
    /// Evalúa la decisión con EvaluadorPortero y actualiza la UI y el NPC.
    /// </summary>
    private void ProcesarDecision(bool permitioPaso)
    {
        MovimientoNPC npcActual = FindObjectOfType<MovimientoNPC>();
        EvaluadorPortero evaluador = FindObjectOfType<EvaluadorPortero>();

        if (evaluador != null)
        {
            // Mensaje provisional visible en UI mientras conectamos el ClienteData exacto
            if (textoAvisoResultado != null)
            {
                textoAvisoResultado.text = permitioPaso ? "Cliente Aceptado" : "¡Error! Cliente rechazado";
            }
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

        CerrarTarjeta();
    }

    /// <summary>
    /// Oculta la tarjeta y restablece los estados.
    /// </summary>
    private void CerrarTarjeta()
    {
        tarjetaAbierta = false;
        npcEnLaPuerta = false;

        if (tarjetaID != null)
        {
            tarjetaID.SetActive(false);
        }
    }
}