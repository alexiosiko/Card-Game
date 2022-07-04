using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private float distance = 1;
    public Transform bell;
    public List<Transform> hands;
    private int currentPlayerIndex;
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            hands[currentPlayerIndex].GetComponentInChildren<Hand>().isPassed = true;
            NextPlayer();
        }
    }
    public void GetPlayers() {
        hands = new List<Transform>();
        GameObject[] handsArray = GameObject.FindGameObjectsWithTag("Hand");
        for (int i = 0; i < handsArray.Length; i++) {
            hands.Add(handsArray[i].transform);
        }
    }
    public void StartPlayer() {
        // Make all hands false
        for (int i = 0; i < hands.Count; i++)
        {
            hands[i].GetComponent<Hand>().isTurn = false;
        }

        // Make hand[0] true
        currentPlayerIndex = 0;
        hands[currentPlayerIndex].GetComponent<Hand>().isTurn = true;

        // Bell
        AdjustBell();
    }
    public void NextPlayer() {

        hands[currentPlayerIndex].GetComponent<Hand>().isTurn = false;
        // start
        // -- is clockwise, ++ is counter-clockwise
        GetPlayers();
        currentPlayerIndex--;
        if (currentPlayerIndex < 0)
            currentPlayerIndex = hands.Count - 1;
        // end
        hands[currentPlayerIndex].GetComponent<Hand>().isTurn = true;

        // If hand is empty, run this script again
        if (hands[currentPlayerIndex].GetComponent<Hand>().transform.childCount <= 0)
            NextPlayer();
        
        // Bell
        AdjustBell();

        // Finished
        if (hands.Count <= 1) {
            Debug.Log("Finished!");
            this.enabled = false;
        }
    }
    void AdjustBell() {
        CancelInvoke("RingBell");
        // Get angle depending on Hand rotation
        float x = hands[currentPlayerIndex].transform.eulerAngles.z;
        x = Mathf.Sin(x * Mathf.PI/180);

        float y = hands[currentPlayerIndex].transform.eulerAngles.z;
        y = Mathf.Cos(y * Mathf.PI/180);

        bell.GetComponentInChildren<Bell>().target = hands[currentPlayerIndex].transform.position + new Vector3(-distance * x, +distance * y, 0);
        bell.GetComponentInChildren<Bell>().animate = true;
        InvokeRepeating("RingBell", 7, 5);
    }
    void RingBell() {
        bell.GetComponentInChildren<Animator>().Play("Bell Ring");
    }
}
