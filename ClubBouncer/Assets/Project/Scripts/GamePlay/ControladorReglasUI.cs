using UnityEngine;
using TMPro;

public class ControladorReglasUI : MonoBehaviour
{
    [Header("Datos de la Noche")]
    public ReglaNoche reglaActual; // Arrastra tu ScriptableObject Regla_Noche1

    [Header("Referencias de UI")]
    public TextMeshProUGUI textoTitulo;
    public TextMeshProUGUI textoReglas;

    void Start()
    {
        ActualizarReglasEnPantalla();
    }

    public void ActualizarReglasEnPantalla()
    {
        if (reglaActual == null) return;

        // 1. Mostrar título
        if (textoTitulo != null)
        {
            textoTitulo.text = reglaActual.tituloNoche;
        }

        // 2. Formatear y mostrar la lista de reglas
        if (textoReglas != null)
        {
            string ebrios = reglaActual.permitirEbrios ? "Permitidos" : "Prohibidos";
            string armas = reglaActual.permitirArmas ? "Permitidas" : "Prohibidas";

            textoReglas.text = 
                $"<b>REGLAS DEL CLUB</b>\n\n" +
                $"• <b>Edad Mínima:</b> {reglaActual.edadMinima} años\n" +
                $"• <b>Ebrios:</b> {ebrios}\n" +
                $"• <b>Armas:</b> {armas}\n" +
                $"• <b>Vestimenta:</b> {reglaActual.vestimentaExigida}";
        }
    }
}