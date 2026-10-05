using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorUI : MonoBehaviour
{
    [Header("Referencias de Scripts")]
    public EvaluadorPortero evaluador;

    [Header("Elementos de la Tarjeta ID")]
    public TMP_Text textoNombre;
    public TMP_Text textoEdad;
    public TMP_Text textoVestimenta;
    public TMP_Text textoEstado;
    public Image imagenFoto;

    [Header("Estadísticas en Pantalla")]
    public TMP_Text textoDinero;
    public TMP_Text textoErrores;

    [Header("Cliente Actual de Prueba")]
    public ClienteData clienteActual;

    private void Start()
    {
        ActualizarPantalla();
    }

    public void CargarCliente(ClienteData nuevoCliente)
    {
        clienteActual = nuevoCliente;
        
        if (clienteActual != null)
        {
            textoNombre.text = "Nombre: " + clienteActual.nombreCliente;
            textoEdad.text = "Edad: " + clienteActual.edad + " años";
            textoVestimenta.text = "Ropa: " + clienteActual.vestimenta.ToString();
            textoEstado.text = clienteActual.estaEbrio ? "Estado: EBRIO" : "Estado: Sobrio";
            
            if (clienteActual.fotoDocumento != null)
            {
                imagenFoto.sprite = clienteActual.fotoDocumento;
            }
        }
    }

    public void PresionarAceptar()
    {
        if (clienteActual == null) return;
        evaluador.DecidirEntrada(clienteActual, true);
        ActualizarPantalla();
    }

    public void PresionarRechazar()
    {
        if (clienteActual == null) return;
        evaluador.DecidirEntrada(clienteActual, false);
        ActualizarPantalla();
    }

    private void ActualizarPantalla()
    {
        if (evaluador != null)
        {
            if (textoDinero != null) textoDinero.text = "Dinero: $" + evaluador.dineroAcumulado;
            if (textoErrores != null) textoErrores.text = "Errores: " + evaluador.errores + "/3";
        }
    }
}