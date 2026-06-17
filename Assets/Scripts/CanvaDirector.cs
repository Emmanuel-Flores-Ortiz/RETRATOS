using UnityEngine;
using TMPro;

public class CanvaDirector : MonoBehaviour
{
    //VARIABLES PARA LOS DAILOGOS
    [SerializeField] public GameObject panelDeDialogos;
    [SerializeField] public TMP_Text textoDeDialogo;
    
    //VARIABLES PARA LA CAMARA
    public int camara = 0;

    public void AumentoCamara()
    {
        camara++;
    }
}
