using System.Collections.Generic;
using UnityEngine;

public class GeneradorClientes : MonoBehaviour
{
    public List<ClienteData> listaClientes;
    public ControladorUI controladorUI;
    
    [Header("Panel de Victoria")]
    public GameObject panelVictoria; // Arrastra aquí tu PanelVictoria desde la jerarquía

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

            // Le pasamos el cliente al MovimientoNPC
            MovimientoNPC npcMovimiento = FindObjectOfType<MovimientoNPC>();
            if (npcMovimiento != null)
            {
                npcMovimiento.EstablecerCliente(clienteActual);
            }
        }
        else
        {
            Debug.Log("¡Se terminaron los clientes de la fila por hoy!");
            
            if (controladorUI != null)
            {
                controladorUI.ToggleTarjeta(false);
            }

            // === AQUÍ ACTIVamos LA VICTORIA AUTOMÁTICAMENTE ===
            if (panelVictoria != null)
            {
                panelVictoria.SetActive(true);
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