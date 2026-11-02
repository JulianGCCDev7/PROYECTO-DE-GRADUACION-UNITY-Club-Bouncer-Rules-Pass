using UnityEngine;
using UnityEngine.SceneManagement;

public class EvaluadorPortero : MonoBehaviour
{
    [Header("Configuración Activa")]
    public ReglaNoche reglaActual;

    [Header("Referencias UI")]
    public ControladorUI controladorUI; // Referencia para actualizar los textos del dinero/errores en pantalla

    [Header("Límites de Errores")]
    public int erroresMaximos = 3;

    [Header("Estadísticas del Turno")]
    public int clientesProcesados = 0;
    public int aciertos = 0;
    public int errores = 0;
    public int dineroAcumulado = 0;

    [Header("Paneles de Fin de Juego (UI)")]
    public GameObject panelGameOver;
    public GameObject panelVictoria;

    private void Start()
    {
        if (controladorUI == null)
        {
            controladorUI = FindObjectOfType<ControladorUI>();
        }
        
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);

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
            mensajeResultado = $"¡Error! Decisión equivocada con {cliente.nombreCliente}. Errores: {errores}/{erroresMaximos}. Dinero: ${dineroAcumulado}";
            Debug.LogWarning($"<color=red>{mensajeResultado}</color>");

            // Comprobamos si se llega al límite de errores para el Game Over
            if (errores >= erroresMaximos)
            {
                TriggerGameOver();
                return "¡GAME OVER! Has alcanzado los 3 errores permitidos.";
            }
        }

        // Actualizamos los contadores visuales en pantalla
        ActualizarHUD();

        return mensajeResultado;
    }

    /// <summary>
    /// Se llama automáticamente cuando la lista de clientes se agota por completo sin perder.
    /// </summary>
    public void VerificarFinDeLista()
    {
        if (errores < erroresMaximos)
        {
            TriggerVictoria();
        }
    }

    private void TriggerGameOver()
    {
        Debug.Log("¡GAME OVER! El juego ha terminado por errores.");
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }
        Time.timeScale = 0f; // Pausa el tiempo del juego
    }

    private void TriggerVictoria()
    {
        Debug.Log("¡VICTORIA! Has completado la lista de clientes.");
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }
        Time.timeScale = 0f; // Pausa el tiempo del juego
    }

    /// <summary>
    /// Método para reiniciar la escena desde el botón UI de "Jugar de nuevo".
    /// </summary>
    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ActualizarHUD()
    {
        if (controladorUI != null)
        {
            controladorUI.ActualizarEstadisticas(dineroAcumulado, errores);
        }
    }
}