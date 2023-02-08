using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class CardScriptableObject : ScriptableObject
{
    public Sprite face;
    public int value;
    public void Start() {
        GetFace();
        GetValue(); // Face:   [3,4,5,6,7,8,9,10,J, Q, K, A, 2, JOKER]
                    // Value:  [1,2,3,4,5,6,7,8 ,9,10,11,12,13, 14   ]
    }
    void GetFace() {
        List<Sprite> deck = FindObjectOfType<GameManager>().deck;
        face = FindObjectOfType<GameManager>().deck[Random.Range(0, deck.Count)];
        deck.Remove(face);
    }
    void GetValue() {
        Debug.Log(face.name + " length: " + face.name.Length);  
        string temp = face.name[face.name.Length - 8].ToString();
        if (temp == "1")
            value = 10;
        else if (temp == "J")
            value = 11;
        else if (temp == "Q")
            value = 12;
         else if (temp == "K")
            value = 13;
        else if (temp == "A")
            value = 14;
        else if (temp == "2")
            value = 15;
        else if (temp == "Z")
            value = 16;
        else
            value = int.Parse(temp);
    
        value -= 2;
    }
}
