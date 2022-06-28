using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hand : Conditions
{
    public bool isPassed = false;
    public float spacing = 0.3f;
    public bool isTurn = false;
    public TurnManager turnManager;
    void Awake() {
        turnManager = FindObjectOfType<TurnManager>();
    }
    void Start() {
        // Enable for NETWORKING
        //FindObjectOfType<GameSetup>().Begin();
    }
    public void AdjustCards() {
        if (transform.childCount == 0) {
            enabled = false;
            return;
        }
        BoxCollider2D box = transform.GetChild(0).GetComponentInChildren<BoxCollider2D>();
        float offset = transform.childCount / 7.75f;
        for (int i = 0; i < transform.childCount; i++){
            Card card = transform.GetChild(i).GetComponentInChildren<Card>();

            transform.GetChild(i).GetComponentInChildren<SpriteRenderer>().sortingOrder = -i;
            
            // Set card animation
            card.target = new Vector3(-i * spacing + offset, 0, 0);
            card.animate = true;

            // Set card home
            card.parentPos = new Vector3(-i * spacing + offset, 0, 0);

            // Set box colliders
            box.size = new Vector2(0.3f, 1.25f);
            box.offset = new Vector2(-0.3f, 0);
        }
        // First card collider
        box.size = new Vector2(0.933f, 1.267f);
        box.offset = new Vector2(0, 0);
    }
    void OnDisable() {
        gameObject.SetActive(false);
    }
}
