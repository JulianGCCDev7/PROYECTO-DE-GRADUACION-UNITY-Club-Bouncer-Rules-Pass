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
    public Image imagenFotoPerfil; // <-- Arrastra aquí tu objeto "FotoPerfil" de la tarjeta

    [Header("Elementos de Diálogo / Narrativa")]
    public TextMeshProUGUI textoDialogoNPC; // <-- Arrastra aquí el texto del diálogo en pantalla si lo usas

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
        if (textoAvisoResultado != null)
        {
            textoAvisoResultado.text = "";
        }
        if (textoDialogoNPC != null)
        {
            textoDialogoNPC.text = "";
        }
    }

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

        // CARGAR LA FOTO DEL DOCUMENTO EN LA TARJETA AUTOMÁTICAMENTE
        if (imagenFotoPerfil != null && cliente.fotoDocumento != null)
        {
            imagenFotoPerfil.sprite = cliente.fotoDocumento;
        }

        // MOSTRAR DIÁLOGO DE ENTRADA AUTOMÁTICAMENTE
        if (textoDialogoNPC != null)
        {
            textoDialogoNPC.text = cliente.dialogoAlEntrar;
        }
    }

    public void ActualizarEstadisticas(int dinero, int errores)
    {
        if (textoDinero != null) textoDinero.text = "Dinero: $" + dinero;
        if (textoErrores != null) textoErrores.text = "Errores: " + errores + "/3";
    }

    public void ActualizarPantalla()
    {
        if (evaluador != null)
        {
            ActualizarEstadisticas(evaluador.dineroAcumulado, evaluador.errores);
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
        if (evaluador != null && clienteActual != null)
        {
            string mensajeResultado = evaluador.DecidirEntrada(clienteActual, aceptado);

            if (!aceptado && textoDialogoNPC != null)
            {
                textoDialogoNPC.text = clienteActual.dialogoAlSerRechazado;
            }

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
            Debug.LogWarning("Falta asignar el Evaluador o el Cliente Actual en ControladorUI.");
        }
    }
}