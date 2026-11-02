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
    public TextMeshProUGUI textoArma; // <--- 1. NUEVA REFERENCIA PARA EL ARMA
    public Image imagenFotoPerfil; 

    [Header("Elementos de Diálogo / Narrativa")]
    public TextMeshProUGUI textoDialogoNPC; 

    [Header("Estadísticas en Pantalla")]
    public TextMeshProUGUI textoDinero;
    public TextMeshProUGUI textoErrores;

    [Header("Aviso de Resultado en Pantalla")]
    [Tooltip("Arrastra aquí tu objeto TextoAvisoResultado del Canvas")]
    public TextMeshProUGUI textoAvisoResultado;

    [Header("Cliente Actual de Prueba")]
    public ClienteData clienteActual;
    private string textoRechazoPendiente = "";

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

    private void Update()
    {
        // Si hay un texto de rechazo en pantalla, si el jugador hace clic en cualquier lado, avanzamos
        if (!string.IsNullOrEmpty(textoRechazoPendiente) && Input.GetMouseButtonDown(0))
        {
            textoRechazoPendiente = "";
            if (generador != null)
            {
                generador.SiguienteCliente();
            }
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

        // <--- 2. LÓGICA PARA MOSTRAR SI PORTA ARMAS --->
        if (textoArma != null)
        {
            string estadoArma = cliente.portaArmas ? "Arma: Sí (PELIGRO)" : "Arma: No";
            textoArma.text = estadoArma;
            
            // Opcional: Pone el texto en rojo si lleva arma y en blanco si está limpio
            textoArma.color = cliente.portaArmas ? Color.red : Color.white;
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

            if (textoDialogoNPC != null)
            {
                if (!aceptado)
                {
                    // Guardamos y pintamos el texto de rechazo a la fuerza
                    textoRechazoPendiente = clienteActual.dialogoAlSerRechazado;
                    textoDialogoNPC.text = textoRechazoPendiente;
                }
                else
                {
                    textoDialogoNPC.text = clienteActual.dialogoAlEntrar;
                    textoRechazoPendiente = "";
                }
            }

            if (textoAvisoResultado != null)
            {
                textoAvisoResultado.text = mensajeResultado;
            }

            ActualizarPantalla();
            ToggleTarjeta(false);
            
            // NO llamamos al siguiente cliente de golpe si fue rechazado, 
            // dejamos que el texto se muestre.
            if (aceptado && generador != null)
            {
                generador.SiguienteCliente();
            }
        }
    }
}