using System.Collections.Generic;
using UnityEngine;

public class GeneradorClientes : MonoBehaviour
{
    [Header("Lista de Clientes de la Noche")]
    public List<ClienteData> listaClientes = new List<ClienteData>();

    [Header("Referencias")]
    public ControladorUI controladorUI;

    private int indiceActual = 0;

    private void Start()
    {
        // Carga al primer cliente al iniciar la escena
        if (listaClientes.Count > 0)
        {
            CargarClienteActual();
        }
    }

    public void PasaraSiguienteCliente()
    {
        indiceActual++;

        if (indiceActual < listaClientes.Count)
        {
            CargarClienteActual();
        }
        else
        {
            Debug.Log("<color=yellow>¡Noche terminada! No hay más clientes en la fila.</color>");
            // Aquí luego pondremos la pantalla de Resumen de Fin de Noche
        }
    }

    private void CargarClienteActual()
    {
        if (controladorUI != null && listaClientes[indiceActual] != null)
        {
            controladorUI.CargarCliente(listaClientes[indiceActual]);
        }
    }
}