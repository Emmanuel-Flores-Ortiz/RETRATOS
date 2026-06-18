using UnityEngine;
using UnityEngine.InputSystem;

public class PuertaCambioEscena : MonoBehaviour
{
    //PUERTA
    private bool jugadorEnRangoPuerta;
    
    //VARIABLE NOMBRE DE LA ESCENA
    [SerializeField] private string escenaACambiar;
    
    //VARIABLES TIPO ESTRUCTURA
    private Collider2D coll;
    private SistemaDialogos sistemaDeDialogos;
    private CambioEscenas cambioEscenas;
    
    void Awake()                                                
    {
        sistemaDeDialogos = FindFirstObjectByType<SistemaDialogos>();
        cambioEscenas = FindFirstObjectByType<CambioEscenas>();
    }
    
    void OnEnable()                                             
    { 
        // ¡El escudo protector! Solo nos suscribimos si de verdad existen las referencias
        if (sistemaDeDialogos != null && sistemaDeDialogos.acciones != null)
        {
            sistemaDeDialogos.acciones.Player.Interaccion.performed += eventosPuerta;   
        }
    }

    void OnDisable()                                        
    {
        if (sistemaDeDialogos != null && sistemaDeDialogos.acciones != null)
        {
            sistemaDeDialogos.acciones.Player.Interaccion.performed -= eventosPuerta;                         
        }
    }
    
    //PUERTA
    void eventosPuerta(InputAction.CallbackContext ctx)
    {
        if (jugadorEnRangoPuerta)                               //Detecta si el jugador esta en el rango del objeto.
        {
            CambioDeEscena();                                   //Si es verdad, se activa los eventos del dialogo.
        }
    }

    void CambioDeEscena()
    {
        cambioEscenas.Cambiar(escenaACambiar);
    }
    
    //ACCIONADORES DEL CODIGO ANTERIOR.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jugadorEnRangoPuerta = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jugadorEnRangoPuerta = false;
        }
    }
}
