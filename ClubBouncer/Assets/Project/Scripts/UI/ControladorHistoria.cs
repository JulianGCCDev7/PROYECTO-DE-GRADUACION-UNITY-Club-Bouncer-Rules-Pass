using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ControladorHistoria : MonoBehaviour
{
    [Header("Imágenes de la Intro")]
    public Image imagenFondo; 
    public List<Sprite> listaSpritesFondo; 

    [Header("Elementos del Juego a Activar al Terminar")]
    public GameObject gameManagerObject; // Arrastra aquí tu GameManager
    public GameObject panelReglasObject; // Arrastra aquí tu PanelReglas

    private int indiceImagen = 0;

    void Start()
    {
        ActualizarImagen();
        
        // Nos aseguramos de que el juego y las reglas comiencen apagados
        if (gameManagerObject != null) gameManagerObject.SetActive(false);
        if (panelReglasObject != null) panelReglasObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AvanzarSecuencia();
        }
    }

    void AvanzarSecuencia()
    {
        if (listaSpritesFondo == null || listaSpritesFondo.Count == 0 || imagenFondo == null) return;

        indiceImagen++;

        if (indiceImagen < listaSpritesFondo.Count)
        {
            ActualizarImagen();
        }
        else
        {
            ComenzarJuegoReal();
        }
    }

    void ActualizarImagen()
    {
        if (listaSpritesFondo.Count > indiceImagen && listaSpritesFondo[indiceImagen] != null)
        {
            imagenFondo.sprite = listaSpritesFondo[indiceImagen];
        }
    }

    void ComenzarJuegoReal()
    {
        if (gameManagerObject != null)
        {
            gameManagerObject.SetActive(true);

            // Forzamos a que el generador y la UI muestren el primer cliente con su diálogo real
            GeneradorClientes generador = gameManagerObject.GetComponent<GeneradorClientes>();
            if (generador != null)
            {
                // Asegúrate de llamar al método que asigna y muestra el cliente actual
                // generador.CargarClienteActual(); o similar
            }
        }

        if (panelReglasObject != null)
        {
            panelReglasObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}