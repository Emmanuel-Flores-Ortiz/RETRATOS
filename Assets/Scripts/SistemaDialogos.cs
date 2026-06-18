using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class SistemaDialogos : MonoBehaviour
{
    //VARIABLES RELACIONADAS CON LOS DIALOGOS
    [HideInInspector] public string[] parrafos;  //Indica la cantidad de parrafos que tendra el objeto (estos se escriben y crean desde el inspector).

    public bool dialogoIniciado;                              //Si es 1 --> Indica que el dialogo con el objeto se esta realizando.

    public int ParrafoMostrado;                           //Indica el parrafo que se esta reproduciendo en ese momento (se pueden ver desde el inspector)
    
    [HideInInspector] public SistemaNPC npcActual;
    
    //VARIABLES TIPO ESTRUCTURA
    public InputSystem_Actions acciones;
    public CanvaDirector director;
    
    
    
    //------------------------------INICIO DE LOGICA------------------------------
    //Activacion de funciones que se usaran en el proyecto de manera constante
    void Awake()                                                
    {
        acciones = new InputSystem_Actions();
        director = FindFirstObjectByType<CanvaDirector>();
    }
    
    void OnEnable()                                             //Mantiene activa las funciones que uno descida colocar en el
    {   
        acciones.Player.Enable();                               //Activa el sistema de acciones que son los Inputs 
        acciones.Player.Interaccion.performed += AvanzarDialogo;   //Se conecta el boton seleccionado con el sistema de Inputs y se acciona.
    }
    
    void OnDisable()                                            //Desactiva las funciones que uno descida colocar en el
    {
        acciones.Player.Interaccion.performed -= AvanzarDialogo;   //Desconecta el boton seleccionado (evita que el boton quede flotando)
        acciones.Player.Disable();                              //Desactiva el sistema de acciones
    }
    
    
    
    //------------------------------LOGICA RELACIONADA CON DIALOGOS------------------------------
    //Interacion recibe un argumento el cual es el boton asigando anteriormente, de ser el correcto, realiza lo que tiene adentro.
    public void AvanzarDialogo(InputAction.CallbackContext ctx)           
    {
        if (dialogoIniciado && director.textoDeDialogo.text == parrafos[ParrafoMostrado])    
        {
            SiguienteDialogo();                                   //Si es verdad, se activa los eventos del dialogo.
        }
    }
    
    //Iniciamos el dialogo al interactuar con el objeto
    public void InicioDeDialogo()
    {
        Time.timeScale = 0f;                                //Detiene el tiempo en el juego de cualquier cosa incluido el jugador, asique ya se no se puede mover mas
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
            
            if (npcActual != null)
            {
                npcActual.ActivarNPC();
                npcActual = null; // Vaciamos la variable para el siguiente que hable
            }
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
    
}
