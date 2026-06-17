using UnityEngine;
using System.Collections;

public class ObjetosColeccionables : MonoBehaviour
{
    //VARIABLES DE LOS OBJETOS
    public enum TipoObjeto {camara}
    public bool tomarObjeto;
    
    
    //VARIABLES TIPO ESTRUCTURA
    public Collider2D coll;
    [SerializeField] private TipoObjeto tipoObjeto;
    private CanvaDirector director;
    
    
    //INICIALIZO TODAS MIS VARIABLES QUE USARE EN TODO MOMENTO
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        director = FindObjectOfType<CanvaDirector>();
    }
    
    
    //CAMARA
    void eventosCamara()
    {
        if (director.camara < 3)
        {
            coll.enabled = false;
            if (director != null)
            {
                director.AumentoCamara();
                DialogosHijo scriptDialogos = FindObjectOfType<DialogosHijo>();
                if (scriptDialogos != null)
                {
                    scriptDialogos.DialogoPorCamara();
                }
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("LA CAMARA SE RECOLECTO 3 VECES");
        }
    }
    
    
    //ONTRIGGER
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (tipoObjeto)
        {
            case TipoObjeto.camara:
                eventosCamara();
                break;
        }
    }

}
