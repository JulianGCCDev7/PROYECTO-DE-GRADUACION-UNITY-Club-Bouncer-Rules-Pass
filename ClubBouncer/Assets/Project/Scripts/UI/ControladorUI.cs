using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControladorUI : MonoBehaviour
{
    [Header("Panel Principal de la Tarjeta")]
    public GameObject panelTarjetaID;

    [Header("Referencias de Scripts")]
    public EvaluadorPortero evaluador;
    public GeneradorClientes generador;

    [Header("Elementos de la Tarjeta ID")]
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoEdad;
    public TextMeshProUGUI textoVestimenta;
    public TextMeshProUGUI textoEstado;
    public Image imagenFoto;

    [Header("Estadísticas en Pantalla")]
    public TextMeshProUGUI textoDinero;
    public TextMeshProUGUI textoErrores;

    [Header("Aviso de Resultado en Pantalla")]
    [Tooltip("Arrastra aquí tu objeto TextoAvisoResultado del Canvas")]
    public TextMeshProUGUI textoAvisoResultado;

    [Header("Cliente Actual de Prueba")]
    public ClienteData clienteActual;

    private void Start()
    {
        // Limpiamos el texto de aviso al iniciar la escena
        if (textoAvisoResultado != null)
        {
            textoAvisoResultado.text = "";
        }
    }

    // Método para abrir / cerrar la tarjeta como Pop-up
    public void ToggleTarjeta(bool mostrar)
    {
        if (panelTarjetaID != null)
        {
            panelTarjetaID.SetActive(mostrar);
        }
    }

    public void MostrarCliente(ClienteData cliente)
    {
        clienteActual = cliente;
        if (cliente == null) return;

        if (textoNombre != null) textoNombre.text = "Nombre: " + cliente.nombreCliente;
        if (textoEdad != null) textoEdad.text = "Edad: " + cliente.edad + " años";
        if (textoVestimenta != null) textoVestimenta.text = "Ropa: " + cliente.vestimenta;
        
        if (textoEstado != null)
        {
            textoEstado.text = "Estado: " + (cliente.estaEbrio ? "Ebrio" : "Sobrio");
        }
    }

    public void ActualizarPantalla()
    {
        if (evaluador != null)
        {
            if (textoDinero != null) textoDinero.text = "Dinero: $" + evaluador.dineroAcumulado;
            if (textoErrores != null) textoErrores.text = "Errores: " + evaluador.errores + "/3";
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

    private void ProcesarDecision(bool aceptado)
    {
        if (generador != null && evaluador != null && clienteActual != null)
        {
            // Capturamos el mensaje detallado que devuelve el EvaluadorPortero
            string mensajeResultado = evaluador.DecidirEntrada(clienteActual, aceptado);

            // Mostramos el mensaje directamente en la pantalla del juego
            if (textoAvisoResultado != null)
            {
                textoAvisoResultado.text = mensajeResultado;
            }

            ActualizarPantalla();
            
            // Avanza al siguiente cliente de la lista
            generador.SiguienteCliente();
            
            // Cierra la tarjeta emergente automáticamente
            ToggleTarjeta(false);
        }
    }
    private void ProcesarDecision(bool aceptado)
    {
        // Si el clienteActual no se asignó desde el NPC, intentamos buscarlo o evitar el error
        if (evaluador != null && clienteActual != null)
        {
            string mensajeResultado = evaluador.DecidirEntrada(clienteActual, aceptado);

            if (textoAvisoResultado != null)
            {
                textoAvisoResultado.text = mensajeResultado;
            }

            ActualizarPantalla();
            
            if (generador != null)
            {
                generador.SiguienteCliente();
            }
            
            ToggleTarjeta(false);
        }
        else
        {
            Debug.LogWarning("No hay un cliente activo asignado en ControladorUI para evaluar.");
        }
    }
}