using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PuertaCambioEscena : MonoBehaviour
{
    //PUERTA
    private bool jugadorEnRangoPuerta;
    
    //VARIABLE NOMBRE DE LA ESCENA
    [SerializeField] private string escenaACambiar;
    
    //VARIABLES TIPO ESTRUCTURA
    Animator animator;
    private Collider2D coll;
    private SistemaDialogos sistemaDeDialogos;
    private CambioEscenas cambioEscenas;
    
    void Start()                                                
    {
        animator = GetComponent<Animator>();
        sistemaDeDialogos = FindFirstObjectByType<SistemaDialogos>();
        cambioEscenas = FindFirstObjectByType<CambioEscenas>();

        // Forzamos la suscripción aquí en el Start
        if (sistemaDeDialogos != null && sistemaDeDialogos.acciones != null)
        {
            // Limpiamos suscripciones viejas por si acaso y nos conectamos
            sistemaDeDialogos.acciones.Player.Interaccion.performed -= eventosPuerta;   
            sistemaDeDialogos.acciones.Player.Interaccion.performed += eventosPuerta;   
            Debug.Log($"[PUERTA] {gameObject.name} se conectó con éxito al Input System.");
        }
        else
        {
            Debug.LogError($"[PUERTA] {gameObject.name} NO encontró el SistemaDialogos en la escena. El Space no funcionará aquí.");
        }
    }
    
    //======== FUNCIONES EXTRAS ===========
    //ESTA FUNCION ME AYUDA A SABER CUANTO TIEMPO VA A REALIZARCE UNA ANIMACION
    private float Tiempo_Animacion()
    {
        AnimatorStateInfo infoAnimacion = animator.GetCurrentAnimatorStateInfo(0);
        float tiempoTotal = infoAnimacion.length;
        return tiempoTotal;
    }
    
    
    //PUERTA
    void eventosPuerta(InputAction.CallbackContext ctx)
    {
        if (jugadorEnRangoPuerta)                               //Detecta si el jugador esta en el rango del objeto.
        {
            jugadorEnRangoPuerta = false;
            StartCoroutine(CorrutinaPuerta()); //Si es verdad, se activa los eventos del dialogo.
        }
    }

    IEnumerator CorrutinaPuerta()
    {
        animator.SetBool("AbrirPuerta", true);
        yield return null;
        float tiempoEspera = Tiempo_Animacion();
        yield return new WaitForSecondsRealtime(tiempoEspera);
        CambioDeEscena(); 
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
            Debug.Log("En rango");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jugadorEnRangoPuerta = false;
            Debug.Log("Fuera de rango");
        }
    }
}
