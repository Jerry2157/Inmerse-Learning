using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoDownloader : MonoBehaviour {

    public VideoPlayer player;

    public string url = "";

    public GameObject warningVideo;
    // Use this for initialization
    void Start () {
        
        StartCoroutine(Download());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Download()
    {


        print("no existe");
        warningVideo.SetActive(true);
        using (WWW www = new WWW(url))
        {
            yield return www;
            print("downloaded video");
            player.playOnAwake = false;
            //var www = new WWW("http://Sameer.com/SampleVideo_360x240_2mb.mp4");
            File.WriteAllBytes(Application.persistentDataPath + "/testtwo.mp4", www.bytes);
            player.url = Application.persistentDataPath + "/testtwo.mp4";

            warningVideo.SetActive(false);
            player.Play();
            //Renderer renderer = GetComponent<Renderer>();
            //renderer.material.mainTexture = www.texture;
        }


        
        /*if (System.IO.File.Exists(Application.persistentDataPath + "/testtwo.mp4"))
        {
            print("si existe");
            warningVideo.SetActive(false);
            player.url = Application.persistentDataPath + "/testtwo.mp4";
            player.Play();
        }
        else
        {
            print("no existe");
            warningVideo.SetActive(true);
            using (WWW www = new WWW(url))
            {
                yield return www;
                print("downloaded video");
                player.playOnAwake = false;
                //var www = new WWW("http://Sameer.com/SampleVideo_360x240_2mb.mp4");
                File.WriteAllBytes(Application.persistentDataPath + "/testtwo.mp4", www.bytes);
                player.url = Application.persistentDataPath + "/testtwo.mp4";

                warningVideo.SetActive(false);
                player.Play();
                Invoke("loadMainMenu", 90f);

            }
        }*/
            
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

}
