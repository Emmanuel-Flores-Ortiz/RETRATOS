using UnityEngine;
using System.Collections;

public class ObjetosColeccionables : MonoBehaviour
{
    //VARIABLES DE LOS OBJETOS
    public enum TipoObjeto {camara, NPC}
    public bool tomarObjeto;
    [SerializeField, TextArea(4,6)] private string[] parrafosCamaras;
    
    
    //VARIABLES TIPO ESTRUCTURA
    private Collider2D coll;
    [SerializeField] private TipoObjeto tipoObjeto;
    private CanvaDirector director;
    private DialogosNPC sistemaDeDialogos;
    
    
    //INICIALIZO TODAS MIS VARIABLES QUE USARE EN TODO MOMENTO
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        director = FindFirstObjectByType<CanvaDirector>();
        sistemaDeDialogos = FindFirstObjectByType<DialogosNPC>();
    }
    
    
    //CAMARA
    private IEnumerator eventosCamara()
    {
        // Cambiamos el signo a != (Diferente de null / Significa: Si SÍ EXISTEN)
        if (director != null && sistemaDeDialogos != null)
        {
            if (director.camara < 3)
            {
                coll.enabled = false;
                director.AumentoCamara();
            
                sistemaDeDialogos.parrafos = parrafosCamaras;
                int parrafoMostradoCamara = director.camara - 1;
            
                if (parrafoMostradoCamara >= 0 && parrafoMostradoCamara < sistemaDeDialogos.parrafos.Length)
                {
                    sistemaDeDialogos.ParrafoMostrado = parrafoMostradoCamara;
                    sistemaDeDialogos.InicioDeDialogo();
                    Debug.Log("Se inicia el sistema de accion del dialogo");
                    yield return new WaitForSecondsRealtime(3f);
                    Debug.Log("Pasaron los 5 segundos");

                    sistemaDeDialogos.dialogoIniciado = false;      //El dialogo se desactiva (0)
                    director.panelDeDialogos.SetActive(false);      //El panel se desactiva tambiem
                    Time.timeScale = 1f;
                }
                Destroy(gameObject);
                Debug.Log("Se borro el objeto");

                                           //Reactiva el tiempo de cualquier cosa
            }
            else
            {
                Debug.Log("LA CAMARA SE RECOLECTO 3 VECES");
                Destroy(gameObject);
            }
        }
        else
        {
            // Este bloque solo se ejecutará si de verdad faltara alguno en el inspector
            Debug.LogError("No se pudo iniciar el evento porque CanvaDirector o DialogosNPC son nulos.");
        }
    }
    
    
    //NPC
    void eventosNPC()
    {
        coll.enabled = false;
        Debug.Log("NPC destruido");
        Destroy(gameObject);
    }
    
    
    //ONTRIGGER
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (tipoObjeto)
            {
                case TipoObjeto.camara:
                    StartCoroutine(eventosCamara());
                    break;
                case TipoObjeto.NPC:
                    eventosNPC();
                    break;
            }
        }
    }
}
