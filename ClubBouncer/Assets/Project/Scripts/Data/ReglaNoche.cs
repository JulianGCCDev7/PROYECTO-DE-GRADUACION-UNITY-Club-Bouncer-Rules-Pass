using UnityEngine;

[CreateAssetMenu(fileName = "ReglaNoche_1", menuName = "Club Bouncer/Regla Noche")]
public class ReglaNoche : ScriptableObject
{
    [Header("Reglas de la Noche")]
    public int edadMinima = 21;
    public bool permitirEbrios = false;
    public bool permitirArmas = false;
    public TipoVestimenta vestimentaExigida = TipoVestimenta.Formal;

    [Header("Configuración de Noche")]
    public string tituloNoche = "Noche VIP";
    public int metaClientesCorrectos = 10;
    public int penalizacionDineroError = 50;
}