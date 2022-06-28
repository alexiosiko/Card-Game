using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AdjustCards();
    }
    public void AdjustCards() {
        for (int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Center";
            transform.GetChild(i).GetComponentInChildren<SpriteRenderer>().sortingOrder = i;
        }
    }
}
