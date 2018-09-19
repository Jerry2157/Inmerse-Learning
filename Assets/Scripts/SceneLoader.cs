using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {


    public float tiempoRestante;
	// Use this for initialization
	void Start () {
        if (tiempoRestante > 0)
        {
            Invoke("LoadMainMenu", tiempoRestante);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void loadCN()
    {
        SceneManager.LoadScene("Ciencias Naturales", LoadSceneMode.Single);
    }
    public void loadEst()
    {
        SceneManager.LoadScene("Estadistica", LoadSceneMode.Single);
    }
    public void loadHistoria()
    {
        SceneManager.LoadScene("Historia", LoadSceneMode.Single);
    }
    public void loadSalud()
    {
        SceneManager.LoadScene("Salud", LoadSceneMode.Single);
    }
    public void loadDownloadManager()
    {
        SceneManager.LoadScene("VideoDownload", LoadSceneMode.Single);
    }
    
}
