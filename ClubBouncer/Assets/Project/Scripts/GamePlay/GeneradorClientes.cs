using System.Collections.Generic;
using UnityEngine;

public class GeneradorClientes : MonoBehaviour
{
    public List<ClienteData> listaClientes;
    public ControladorUI controladorUI;
    
    private int indiceActual = 0;

    private void Start()
    {
        CargarClienteActual();
    }

    public void CargarClienteActual()
    {
        if (listaClientes != null && indiceActual < listaClientes.Count)
        {
            ClienteData clienteActual = listaClientes[indiceActual];

            if (controladorUI != null)
            {
                controladorUI.MostrarCliente(clienteActual);
            }

            // Actualizar el Sprite del NPC físico en la escena (Usando el sprite de caminata)
            MovimientoNPC npcMovimiento = FindObjectOfType<MovimientoNPC>();
            if (npcMovimiento != null && clienteActual.spriteLateralCaminando != null)
            {
                SpriteRenderer sr = npcMovimiento.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = clienteActual.spriteLateralCaminando;
                }
            }
        }
        else
        {
            Debug.Log("¡Se terminaron los clientes de la fila por hoy!");
            if (controladorUI != null)
            {
                controladorUI.ToggleTarjeta(false);
            }
        }
    }

    public void SiguienteCliente()
    {
        indiceActual++;
        CargarClienteActual();
    }

    public ClienteData ObtenerClienteActual()
    {
        if (listaClientes != null && indiceActual < listaClientes.Count)
            return listaClientes[indiceActual];
        return null;
    }
}