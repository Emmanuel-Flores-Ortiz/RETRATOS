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
    
    void Awake()                                                
    {
        animator = GetComponent<Animator>();
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
