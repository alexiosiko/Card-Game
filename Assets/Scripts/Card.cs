using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Card : Conditions
{
    public Sprite back;
    public Color highlightColor = new Color(1,0.8f,1,1);
    public Color defaultColor = new Color(1,1,1,1);
    private float spacing = 0.3f;
    public CardScriptableObject script;
    private Animator animator;
    public Vector3 parentPos;
    public Vector3 target;
    private bool highlight = false;
    public bool animate = false;
    public float animationSpeed = 6;
    private Vector3 mousePos;
    private Vector3 mouseDragOffset;
    private GameManager gameManager;
    private TurnManager turnManager;
    private new SpriteRenderer renderer;
    void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        turnManager = FindObjectOfType<TurnManager>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        script = ScriptableObject.CreateInstance<CardScriptableObject>();
        script.Start();
    }
    void Update() {
        if (animate == true) {
            Debug.Log("animating");
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, target, Time.deltaTime * animationSpeed);
            if (Vector2.Distance(transform.parent.localPosition, target) < 0.001f)
                animate = false;
        }
    }
    public IEnumerator DrawFace() {
        yield return new WaitForSeconds(0.4f);
        renderer.sprite = script.face;
    }
    public IEnumerator DrawBack() {
        yield return new WaitForSeconds(0.5f);
        renderer.sprite = back;
    }
    public void OnMouseDrag() {

        animator.Play("Empty");
        if (highlight == true)
            renderer.color = new Color(highlightColor.r, highlightColor.g, highlightColor.b, 0.6f);
        else
            renderer.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0.6f);
        // Drag card with offset
        transform.parent.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10)
        + mouseDragOffset;
    }
    void OnMouseDown() {
            
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mouseDragOffset = transform.position - mousePos;
    }
    void Hightlight() {
        if (highlight == false) {
            highlight = true;
            // Also referenced in ResetCard()
            renderer.color = highlightColor;

            // Add to list
            transform.parent.parent.GetComponent<Hand>().highlighted.Add(transform);
        }
        else {
            highlight = false;
            renderer.color = defaultColor;

            // Remove from list
            transform.parent.parent.GetComponent<Hand>().highlighted.Remove(transform);
        }
    }
    public void OnMouseOver() {
            
        animator.Play("Card Up");
    }
    void OnMouseUp() {
            
        // Check if a click and not a highlight by calculating distance from mounseDown mouseUp
        if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), mousePos) < 0.1f) {
            Hightlight();
            ResetCard();
            return;
        }
        // Check if current turn
        if (transform.parent.parent.GetComponent<Hand>().isTurn == false) {
            ResetCard();
            return;
        }

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider != null) {
                if (hits[i].collider.tag == "Center") {

                    // Store parent
                    Transform parent = transform.parent.parent;
                    Hand hand = parent.GetComponent<Hand>();

                    // If current card is not in list, then add to list
                    if (hand.highlighted.Contains(transform) == false) {
                        hand.highlighted.Add(transform);
                    }

                    // Check conditions
                    if (CanPlaceCards(hand.highlighted) == false) {
                        ResetCard();
                        hand.highlighted.Remove(transform);
                        return;
                    }

                    // Clear recents
                    gameManager.recentCards.Clear();

                    // Get angle depending on Hand rotation
                    float x = transform.parent.parent.eulerAngles.z;
                    x = Mathf.Cos(x * Mathf.PI/180);
                    float y = transform.parent.parent.eulerAngles.z;
                    y = Mathf.Sin(y * Mathf.PI/180);

                    int count = hand.highlighted.Count;
                    for  (int a = 0; a < count; a++) {
                        hand.highlighted[a].GetComponent<SpriteRenderer>().color = defaultColor;
                        hand.highlighted[a].parent.parent = GameObject.Find("Center").transform;
                        
                        // Animations
                        hand.highlighted[a].GetComponent<Card>().target = hits[i].collider.transform.position + new Vector3(x * spacing * (a - count / 2), y * spacing * (a - count / 2), 0);
                        hand.highlighted[a].GetComponent<Card>().animate = true;

                        // Disable collider
                        hand.highlighted[a].GetComponent<BoxCollider2D>().enabled = false;
                        // Get random rotation
                        hand.highlighted[a].transform.localRotation = Quaternion.Euler(0, 0,  Random.Range(-20, 20));
                    
                        // Add to recent cards
                        gameManager.recentCards.Add(hand.highlighted[a].transform);
                    }
                    // Clear List<Transform>
                    hand.highlighted.Clear();
                    // Adjust center order
                    hits[i].collider.gameObject.GetComponent<Center>().AdjustCards();
                    // Adjust hand order
                    hand.AdjustCards();
                    // Next turn
                    if (turnManager.enabled == true)
                        turnManager.NextPlayer();
                }
            }
        }
        // If box collider is disabled -> also means that card is in Center
        // and "out of bounds"
        if (GetComponent<BoxCollider2D>().isActiveAndEnabled == true) {
            ResetCard();
        }
    }
    void OnMouseExit() {
        animator.Play("Card Down");
    }
    void ResetCard() {
        if (highlight == true)
            renderer.color = highlightColor;
        else
            renderer.color = defaultColor;
        target = parentPos;
        animate = true;
    }
}