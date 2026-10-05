using UnityEngine;

// Define la vestimenta que puede llevar el cliente
public enum TipoVestimenta 
{ 
    Formal, 
    Casual, 
    Deportiva, 
    Elegante 
}

[CreateAssetMenu(fileName = "NuevoCliente", menuName = "Club Bouncer/Cliente Data")]
public class ClienteData : ScriptableObject
{
    [Header("Datos de Identificación")]
    public string nombreCliente;
    public int edad;
    public Sprite fotoDocumento;
    public bool tieneDocumentoFalso;

    [Header("Atributos del Cliente")]
    public Sprite spriteCuerpoCliente;
    public TipoVestimenta vestimenta;
    public bool estaEbrio;
    public bool portaArmas;

    [Header("Respuesta Narrativa")]
    [TextArea(2, 4)]
    public string dialogoAlEntrar;
    [TextArea(2, 4)]
    public string dialogoAlSerRechazado;
}