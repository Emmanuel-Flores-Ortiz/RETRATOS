using UnityEngine;
using UnityEngine.InputSystem;

public class SistemaNPC : MonoBehaviour
{
    //VARIABLES DE DETECCION DEL PERSONAJE
    private bool jugadorEnRango;
    
    //VARIABLE DEL DIALOGO
    [SerializeField, TextArea(4,6)] private string[] parrafosNPC;
    
    //VARIABLES TIPO ESTRUCTURA
    private SistemaDialogos sistemaDialogos;

    
    void Awake()
    {
        sistemaDialogos = FindObjectOfType<SistemaDialogos>();
    }
    
    void OnEnable()                                             //Mantiene activa las funciones que uno descida colocar en el
    {   
        sistemaDialogos.acciones.Player.Interaccion.performed += Interaccion;   //Se conecta el boton seleccionado con el sistema de Inputs y se acciona.
    }
    
    void OnDisable()                                            //Desactiva las funciones que uno descida colocar en el
    {
        sistemaDialogos.acciones.Player.Interaccion.performed -= Interaccion;   //Desconecta el boton seleccionado (evita que el boton quede flotando)
    }

    
    void Interaccion(InputAction.CallbackContext ctx)
    {
        if (jugadorEnRango && !sistemaDialogos.dialogoIniciado)
        {
            sistemaDialogos.npcActual = this;
            sistemaDialogos.parrafos = parrafosNPC;
            sistemaDialogos.ParrafoMostrado = 0;
            sistemaDialogos.InicioDeDialogo();
            this.enabled = false;
        }
    }
    
    public void ActivarNPC()
    {
        this.enabled = true;
    }
    
    //ACCIONADORES DEL CODIGO ANTERIOR.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jugadorEnRango = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jugadorEnRango = false;
        }
    }


}
