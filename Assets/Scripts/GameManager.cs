using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
public class GameManager : MonoBehaviour
{ 
    [Range(0.5f, 1)] public float dealSpeed = 1;
    public GameObject cardParent;
    public List<Sprite> deck = new List<Sprite>();
    public SpriteAtlas spriteAtlas;
    public List<Transform> recentCards = new List<Transform>();
    private TurnManager turnManager;
    void Awake() {
        turnManager = FindObjectOfType<TurnManager>();
    }
    void Start() {
        // Change the motherfucking sprite childs into sprites :D
        Sprite[] cardSprites = new Sprite[54];
        spriteAtlas.GetSprites(cardSprites);
        for (int i = 0; i < 54; i++)
            deck.Add(cardSprites[i]);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            if (turnManager.hands.Count <= 1)
                Debug.Log("Not enough players");
            else if (deck.Count == 0)
                Debug.Log("Deck is empty!");
            else
                StartCoroutine(Deal());
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            // Get all active hands make the ispassed = false
            for (int i = 0; i < turnManager.hands.Count; i++) {
                turnManager.hands[i].GetComponent<Hand>().isPassed = false;
            }
            ClearTable();
        }
    }
    void AdjustCards() {
        GameObject[] hands = GameObject.FindGameObjectsWithTag("Hand");
        for (int i = 0; i < hands.Length; i++) {
            hands[i].GetComponent<Hand>().AdjustCards();
        }
    }
    public IEnumerator Deal() {
        Debug.Log("Dealing");
        while (deck.Count > 0) 
        {
        GameObject[] hands = GameObject.FindGameObjectsWithTag("Hand");
        for (int i = 0; i < hands.Length; i++) {
            if (deck.Count == 0) {
                yield break; // if IEnumerator ? yield break : return
            }
            Transform parent = hands[i].transform;
            GameObject card = Instantiate<GameObject>(cardParent, new Vector3(5, -5, 0), parent.transform.rotation);

            float distance = Vector2.Distance(card.transform.position, parent.position);
            card.transform.parent = parent;

            // Draw face after some time
            StartCoroutine(card.GetComponentInChildren<Card>().DrawFace());

            // Adjust cards in current hand
            hands[i].GetComponent<Hand>().AdjustCards();

            // COROUTINE
            yield return new WaitForSeconds(1 - dealSpeed);
            }
        }
    }
    public void ClearTable() {
        recentCards.Clear();
        Transform off = GameObject.Find("Off Table").transform;
        Transform center = GameObject.Find("Center").transform;
        while (true) {
            Debug.Log("Clearing");
            if (center.childCount == 0)
                break;
            Card card = center.GetChild(0).GetComponentInChildren<Card>();
            card.target = Vector3.zero;
            card.animate = true;
            StartCoroutine(center.GetChild(0).GetComponentInChildren<Card>().DrawBack());
            center.GetChild(0).parent = off;
        }
    }
}
