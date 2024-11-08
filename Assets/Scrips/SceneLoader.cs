using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("Cargando escena: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    public void VolverAlMenuPrincipal()
    {
        SceneManager.LoadScene("Scenes/menuInicio");
    }
public void AbrirOpciones()
    {
        SceneManager.LoadScene("Opciones");  // Reemplaza "Opciones" con el nombre exacto de la escena
    }
}
