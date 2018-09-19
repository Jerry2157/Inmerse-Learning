using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicManager : MonoBehaviour {

    public GameObject[] indexes;

    private string[] words = {"Avión","Aguacate","Anillo","Árbol","Araña","Abeja","Arcoiris",
        "bebé","bañera","balón","ballena","búho","buzón",
        "casa","caballo","calabaza","cazo","cereza","coco","conejo",
        "dados","diente","diamante","dinosaurio","doncella","doctor","dominó",
        "espejo","escalera","escopeta","estrella","elefante","erizo","embudo"
    };

    int actualIndex = 0;

    public textToS texttos;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void selectLetter(int letter)
    {
        actualIndex = letter;
        print("recibi: " + letter);
        
        for(int i = 1; i < 6; i++)
        {
            print("i: " + i + " letter: " + letter);
            if(i == letter)
            {
                indexes[i].SetActive(true);
            }
            else
            {
                indexes[i].SetActive(false);
            }
        }
        indexes[0].SetActive(false);
    }
    public void showLetters()
    {
        indexes[actualIndex].SetActive(false);
        indexes[0].SetActive(true);
    }
    public void selectWord(int word)
    {
        texttos.conviertVoz(words[word]);
    }
}
