using UnityEngine;
using UnityEngine.SceneManagement;

   
public class MainMenu : MonoBehaviour {

    public bool isSingle;
    public bool isMulti;
    public bool isQuit;
	void OnMouseUp() {
        GetComponent<Renderer>().material.color = Color.cyan;

        if (isSingle)
        {
            SceneManager.LoadScene(4);
        }
        else if (isMulti) {
            SceneManager.LoadScene(7);
        }
        else if (isQuit) {
            Application.Quit();
        }
    }

}
