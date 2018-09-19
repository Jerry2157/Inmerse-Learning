using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [Tooltip("Aqui va el objeto de menu principal.")]
    public GameObject mainMenu;
    [Tooltip("Aqui va el objeto de menu principal.")]
    public GameObject materias;
    [Tooltip("Aqui va el objeto de menu principal.")]
    public GameObject lecciones;
    [Tooltip("Aqui va el objeto de menu principal.")]
    public GameObject diccionario;
    [Tooltip("Aqui va el objeto de menu principal.")]
    public GameObject preguntas;
    [Tooltip("Aqui va el objeto de menu principal.")]
    public GameObject cuestionario;

    [Header("Variables de lecciones")]
    [Tooltip("Aqui va el objeto de menu principal.")]
    public GameObject cn;
    [Tooltip("Aqui va el objeto de menu principal.")]
    public GameObject estadistica;
    [Tooltip("Aqui va el objeto de menu principal.")]
    public GameObject historia;
    [Tooltip("Aqui va el objeto de menu principal.")]
    public GameObject salud;


    private GameObject[] assignatures = new GameObject[4];

    private GameObject[] menus = new GameObject[6];

    public GameObject camera;

    public void ActivateCamera()
    {
        camera.SetActive(false);
        camera.SetActive(true);
    }

    public void Start()
    {
        menus[0] = mainMenu;
        menus[1] = materias;
        menus[2] = lecciones;
        menus[3] = diccionario;
        menus[4] = preguntas;
        menus[5] = cuestionario;

        assignatures[0] = cn;
        assignatures[1] = estadistica;
        assignatures[2] = historia;
        assignatures[3] = salud;
    }


    public void showMainMenu()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (i == 0)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }

    public void showMaterias()
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if (i == 1)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
    public void showLecciones()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (i == 2)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
    public void showDiccionario()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (i == 3)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
    public void showPreguntas()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (i == 4)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
    public void showCuestionario()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (i == 5)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
    public void showAssignature(int codeAssig)
    {
        showLecciones();
        for(int i = 0; i < assignatures.Length; i++)
        {
            if(i == codeAssig)
            {
                assignatures[i].SetActive(true);
            }
            else
            {
                assignatures[i].SetActive(false);
            }
        }
    }
    public void loadDownloader()
    {
        SceneManager.LoadScene("VideoDownload", LoadSceneMode.Single);
    }
}
