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

    [Header("Cliente Actual de Prueba")]
    public ClienteData clienteActual;

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
        
        // Si tienes la variable 'estaEbrio' en tu ClienteData
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
            // Llama a DecidirEntrada de tu EvaluadorPortero
            evaluador.DecidirEntrada(clienteActual, aceptado);
            ActualizarPantalla();
            
            // Avanza al siguiente cliente de la lista
            generador.SiguienteCliente();
            
            // Cierra la tarjeta emergente automáticamente
            ToggleTarjeta(false);
        }
    }
}