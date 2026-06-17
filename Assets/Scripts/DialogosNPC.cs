using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogosNPC : MonoBehaviour
{
    //VARIABLES DE DETECCION DEL PERSONAJE
    private bool jugadorEnRango;
    
    
    //VARIABLES RELACIONADAS CON LOS DIALOGOS
    [SerializeField,TextArea(4,6)] public string[] parrafos;  //Indica la cantidad de parrafos que tendra el objeto (estos se escriben y crean desde el inspector).

    private bool dialogoIniciado;                              //Si es 1 --> Indica que el dialogo con el objeto se esta realizando.

    private int ParrafoMostrado;                           //Indica el parrafo que se esta reproduciendo en ese momento (se pueden ver desde el inspector)
    
    
    //VARIABLES TIPO ESTRUCTURA
    public InputSystem_Actions acciones;
    public CanvaDirector director;
    
    
    
    //------------------------------INICIO DE LOGICA------------------------------
    //Activacion de funciones que se usaran en el proyecto de manera constante
    void Awake()                                                
    {
        acciones = new InputSystem_Actions();
        director = FindObjectOfType<CanvaDirector>();
    }
    
    void OnEnable()                                             //Mantiene activa las funciones que uno descida colocar en el
    {   
        acciones.Player.Enable();                               //Activa el sistema de acciones que son los Inputs 
        acciones.Player.Interaccion.performed += interaccion;   //Se conecta el boton seleccionado con el sistema de Inputs y se acciona.
    }
    
    void OnDisable()                                            //Desactiva las funciones que uno descida colocar en el
    {
        acciones.Player.Interaccion.performed -= interaccion;   //Desconecta el boton seleccionado (evita que el boton quede flotando)
        acciones.Player.Disable();                              //Desactiva el sistema de acciones
    }
    
    
    
    //------------------------------LOGICA RELACIONADA CON DIALOGOS------------------------------
    //Interacion recibe un argumento el cual es el boton asigando anteriormente, de ser el correcto, realiza lo que tiene adentro.
    public void interaccion(InputAction.CallbackContext ctx)           
    {
        if (jugadorEnRango)                                     //Detecta si el jugador esta en el rango del objeto.
        {
            activarDialogo();                                   //Si es verdad, se activa los eventos del dialogo.
        }
    }

    //Esta funcion permite activar los diferentes parrafos dentro del objeto
    public void activarDialogo()
    {
            if (!dialogoIniciado)                               //Si no hay un dialogo iniciado (1) se cumple
            {
                InicioDeDialogo();                              //Esta funcion activa el primer parrafo, activando el dialogo objeto-player.
            }
            else if (director.textoDeDialogo.text == parrafos[ParrafoMostrado])    //Si el texto escrito en el texto del panel es igual al texto guardado en el parrafo actual escrito en el inspecto se cumple
            {
                SiguienteDialogo();                             //Esta funcion se activa         
            }
    }
    
    
    
    //Iniciamos el dialogo al interactuar con el objeto
    public void InicioDeDialogo()
    {
        Time.timeScale = 0f;                                //Detiene el tiempo en el juego de cualquier cosa incluido el jugador, asique ya se no se puede mover mas
        ParrafoMostrado = 0;
        dialogoIniciado = true;
        director.panelDeDialogos.SetActive(true);           //Se activa del CanvaDirector el panel que muestra los dialogos
        StartCoroutine(eventosDeLosDialogos());      //Activamos la corrutina que tiene guardada los eventos que tendran cada dialogo
    }

    //Cuando se termina un parrafo, empieza otro
    private void SiguienteDialogo()
    {
        ParrafoMostrado++;                                  //Se suma 1 en 1 los parrafos mostrados, asi sabiendo cual se mostrara ahora.
        if (ParrafoMostrado < parrafos.Length)              //Si el valor actual del parrafo es menor (ej.2) a la cantidad maxima de parrafos existentes (ej. 3) se cumple.
        {
            StartCoroutine(eventosDeLosDialogos());  //Se activa la corrutina para ese nuevo parrafo.
        }
        else                                                //De no cumplirse el if, significa que ya ocurrieron cada parrafo posible.
        {
            dialogoIniciado = false;                        //El dialogo se desactiva (0)
            director.panelDeDialogos.SetActive(false);      //El panel se desactiva tambiem
            Time.timeScale = 1f;                            //Reactiva el tiempo de cualquier cosa
        }
    }
    
    //Son los eventos que deben seguir cada parrafo.
    private IEnumerator eventosDeLosDialogos()
    {
        director.textoDeDialogo.text = string.Empty;        //Toma del CanvaDirector el texto definido y lo vacia
        
        foreach (char ch in parrafos[ParrafoMostrado])     //En un ciclo for por cada caracter que este dentro del parrafo actual pasara:
        {
            director.textoDeDialogo.text += ch;             //Agregara de caracter en caracter cada caracter guardado en el parrafo actual y lo mostrara en el texto del panel
            yield return new WaitForSecondsRealtime(0.05f);         //Y entre cada caracter habra un delay de tiempo para que haga el efecto de tipeo
        }
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
