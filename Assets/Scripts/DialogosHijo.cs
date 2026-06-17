using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class DialogosHijo : MonoBehaviour
{
    //VARIABLES RELACIONADAS CON LOS DIALOGOS
    [FormerlySerializedAs("parrafos")] [SerializeField,TextArea(4,6)] private string[] parrafosPorCamara;  //Indica la cantidad de parrafos que tendra el objeto (estos se escriben y crean desde el inspector).
    
    private int ParrafoMostrado;
    
    
    //VARIABLES TIPO ESTRUCTURA
    public CanvaDirector director;
    public DialogosNPC sistemaDialogos;
    
    
    //------------------------------INICIO DE LOGICA------------------------------
    //Activacion de funciones que se usaran en el proyecto de manera constante
    void Awake()                                                
    {
        director = FindObjectOfType<CanvaDirector>();
        sistemaDialogos = FindObjectOfType<DialogosNPC>();
        enabled = false;
    }

    void OnEnable()
    {
        sistemaDialogos.acciones.Player.Interaccion.performed += InteraccionCamara;
    }

    void OnDisable()
    {
        sistemaDialogos.acciones.Player.Interaccion.performed -= InteraccionCamara;
    }
    
    public void InteraccionCamara(InputAction.CallbackContext ctx)
    {
        DialogoPorCamara();
    }

    public void DialogoPorCamara()
    {
        if (!enabled) return;
        
        int ParrafoMostrado = director.camara - 1;

        if (ParrafoMostrado >= 0 && ParrafoMostrado < parrafosPorCamara.Length)
        {
            sistemaDialogos.parrafos = parrafosPorCamara;
            sistemaDialogos.InicioDeDialogo();
            enabled = false;
        } 
    }
}
