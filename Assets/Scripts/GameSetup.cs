using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameSetup : MonoBehaviour
{
    public GameObject handPrefab;
    public List<Transform> hands;
    TurnManager turnManager;
    void Awake() {
        turnManager = FindObjectOfType<TurnManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            Begin();
        }
    }
    public void Begin() {
        AddHand();
        GetHands();
        turnManager.StartPlayer();
    }
    public void AddHand() {
        GameObject hand = Instantiate<GameObject>(handPrefab, Vector3.zero, Quaternion.identity);
    }
    void GetHands() {
        hands = new List<Transform>();
        GameObject[] handsArray = GameObject.FindGameObjectsWithTag("Hand");

        for (int i = 0; i < handsArray.Length; i++) {
            hands.Add(handsArray[i].transform);
        }
        turnManager.hands = hands;
        AdjustHands();
    }
    void AdjustHands() {
        float angle = 0;
        for (int i = 0; i < hands.Count; i++) {
            hands[i].position = new Vector3(3 * Mathf.Cos(angle), 3 * Mathf.Sin(angle), 0);
            hands[i].eulerAngles = new Vector3(0, 0, angle * 180/Mathf.PI + 90);
            angle += 2 * Mathf.PI / hands.Count;
        }
    }
}
