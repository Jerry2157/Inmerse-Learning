using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EncuestaManager : MonoBehaviour {

    public GameObject[] questions = new GameObject [13];
    public int[] results = new int [13];

    int index = 0;

    string data = "";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void uploadResult( int value)
    {
        results[index] = value;
        if(index == 12)
        {
            CreateTXT();
        }
        
        for(int i = 0; i < questions.Length; i++)
        {
            
            if(i == (index+1))
            {
                questions[i].SetActive(true);
            }
            else
            {
                questions[i].SetActive(false);
            }
        }
        index++;
    }

    private void CreateTXT()
    {
        data = "";
        for(int i = 0; i < questions.Length; i++)
        {
            data += results[i] + "\t";
        }
        StartCoroutine(PutRequest("http://10.12.214.110/"));
    }

    IEnumerator PutRequest(string url)
    {
        byte[] dataToPut = System.Text.Encoding.UTF8.GetBytes(data);
        print(data);
        UnityWebRequest uwr = UnityWebRequest.Put(url, dataToPut);
        
        yield return uwr.SendWebRequest();
        
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
}
