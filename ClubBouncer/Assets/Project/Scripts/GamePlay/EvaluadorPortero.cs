using UnityEngine;

public class EvaluadorPortero : MonoBehaviour
{
    [Header("Configuración Activa")]
    public ReglaNoche reglaActual;

    [Header("Referencias UI")]
    public ControladorUI controladorUI; // Referencia para actualizar los textos del dinero/errores en pantalla

    [Header("Estadísticas del Turno")]
    public int clientesProcesados = 0;
    public int aciertos = 0;
    public int errores = 0;
    public int dineroAcumulado = 0;

    private void Start()
    {
        if (controladorUI == null)
        {
            controladorUI = FindObjectOfType<ControladorUI>();
        }
        
        // Actualizar la pantalla al iniciar la noche
        ActualizarHUD();
    }

    /// <summary>
    /// Evalúa si el cliente cumple absolutamente TODAS las reglas de la noche.
    /// Devuelve TRUE si debe entrar, FALSE si debe ser rechazado.
    /// </summary>
    public bool EvaluarCliente(ClienteData cliente)
    {
        if (cliente == null || reglaActual == null)
        {
            Debug.LogError("Error: Falta asignar el cliente o la regla actual en EvaluadorPortero.");
            return false;
        }

        // Regla 1: Edad mínima
        if (cliente.edad < reglaActual.edadMinima)
        {
            return false; // Rechazado por menor de edad
        }

        // Regla 2: Documento falso
        if (cliente.tieneDocumentoFalso)
        {
            return false; // Rechazado por documento falso
        }

        // Regla 3: Portación de armas
        if (cliente.portaArmas && !reglaActual.permitirArmas)
        {
            return false; // Rechazado por armamento
        }

        // Regla 4: Estado de ebriedad
        if (cliente.estaEbrio && !reglaActual.permitirEbrios)
        {
            return false; // Rechazado por ebrio
        }

        // Regla 5: Código de vestimenta
        if (cliente.vestimenta != reglaActual.vestimentaExigida)
        {
            return false; // Rechazado por vestimenta incorrecta
        }

        // Si pasa todas las validaciones, el cliente es apto para entrar
        return true;
    }

    /// <summary>
    /// Procesa la decisión del portero (Aceptar o Rechazar) y devuelve un mensaje con el resultado para mostrar en UI.
    /// </summary>
    public string DecidirEntrada(ClienteData cliente, bool permitioPaso)
    {
        bool clienteEsValido = EvaluarCliente(cliente);
        clientesProcesados++;
        string mensajeResultado = "";

        int penalizacion = (reglaActual != null) ? reglaActual.penalizacionDineroError : 10;

        if (permitioPaso == clienteEsValido)
        {
            // ¡Decisión correcta!
            aciertos++;
            dineroAcumulado += 20; // Recompensa por acierto
            mensajeResultado = $"¡Acierto! Decisión correcta con {cliente.nombreCliente}. Dinero: ${dineroAcumulado}";
            Debug.Log($"<color=green>{mensajeResultado}</color>");
        }
        else
        {
            // Decisión errónea
            errores++;
            dineroAcumulado -= penalizacion; // Penalización por error
            mensajeResultado = $"¡Error! Decisión equivocada con {cliente.nombreCliente}. Errores: {errores}/3. Dinero: ${dineroAcumulado}";
            Debug.LogWarning($"<color=red>{mensajeResultado}</color>");
        }

        // Actualizamos los contadores visuales en pantalla
        ActualizarHUD();

        return mensajeResultado;
    }

    private void ActualizarHUD()
    {
        if (controladorUI != null)
        {
            controladorUI.ActualizarEstadisticas(dineroAcumulado, errores);
        }
    }
}