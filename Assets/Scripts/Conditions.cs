using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Conditions : MonoBehaviour
{
    public List<Transform> recentCards = new List<Transform>();
    public List<Transform> highlighted = new List<Transform>();
    protected bool CanPlaceCards(List<Transform> cards) {
        if (cards[0].GetComponentInParent<Hand>().isPassed == true)
            return false;

        highlighted = cards;
        recentCards = FindObjectOfType<GameManager>().recentCards;

        if (recentCards.Count == 0)
            return NewCards();
        if (highlighted.Count == 1) {
            return Single(); 
        }
        if (highlighted.Count == 2)  {
            return TwoPair(); 
        }
        if (highlighted.Count == 3) {
            return ThreePair(); 
        }
        if (highlighted.Count == 4) {
            return FourPair(); 
        }
        return false;
    }
    bool Single() {
        if (highlighted[0].GetComponent<Card>().script.value == 14) // Jack
            return true;
        if (recentCards.Count == 1) 
        {
            if (recentCards[0].GetComponent<Card>().script.value == 13)
                if (highlighted[0].GetComponent<Card>().script.value == 13)
                    return false;
            else if (highlighted[0].GetComponent<Card>().script.value >= recentCards[0].GetComponent<Card>().script.value)
                return true;
        }
        if (recentCards.Count == 2) 
        {
            if (highlighted[0].GetComponent<Card>().script.value == 13 && recentCards[0].GetComponent<Card>().script.value < 13) // and atleast one of recent is les than 2
                return true;
        }
        return false;
    }
    bool TwoPair() {
        if (recentCards.Count == 3) 
        {
            Debug.Log("3");
            if (highlighted[0].GetComponent<Card>().script.value == 13 && highlighted[1].GetComponent<Card>().script.value == 13) // Ace
                return true;
        }

        if (recentCards.Count == 1 && recentCards[0].GetComponent<Card>().script.value == 13)
        {
            if (highlighted[0].GetComponent<Card>().script.value == 13 && highlighted[1].GetComponent<Card>().script.value == 13) // Ace
                return true;
        }

        if (recentCards.Count == 2) {
            Debug.Log("2");
            if (highlighted[0].GetComponent<Card>().script.value == highlighted[1].GetComponent<Card>().script.value)
            {
                if (highlighted[0].GetComponent<Card>().script.value >= recentCards[0].GetComponent<Card>().script.value) {
                    return true;
                }
            }
        }
        return false;
    }
    bool ThreePair() {
        if (highlighted[0].GetComponent<Card>().script.value == highlighted[1].GetComponent<Card>().script.value
        && highlighted[0].GetComponent<Card>().script.value == highlighted[2].GetComponent<Card>().script.value) {
            if (recentCards.Count == 4) {
                if (highlighted[0].GetComponent<Card>().script.value == 14) // 3 2's
                    return true;
            }
            if (recentCards.Count == 3) {
                if (highlighted[0].GetComponent<Card>().script.value >= recentCards[0].GetComponent<Card>().script.value) {
                    return true;
                }
            }
        }
        return false;
    }
    bool FourPair() {
        if (highlighted[0].GetComponent<Card>().script.value == highlighted[1].GetComponent<Card>().script.value
        && highlighted[0].GetComponent<Card>().script.value == highlighted[2].GetComponent<Card>().script.value
        && highlighted[0].GetComponent<Card>().script.value == highlighted[3].GetComponent<Card>().script.value) {
            if (highlighted[0].GetComponent<Card>().script.value >= recentCards[0].GetComponent<Card>().script.value) {
                return true;
            }
        }
        return false;
    }
    bool NewCards() {
        if (highlighted.Count == 1) {
            return true;
        }
        if (highlighted.Count == 2) {
            if (highlighted[0].GetComponent<Card>().script.value == highlighted[1].GetComponent<Card>().script.value)
                return true;
        }
        if (highlighted.Count == 3) {
            if (highlighted[0].GetComponent<Card>().script.value == highlighted[1].GetComponent<Card>().script.value
            && highlighted[0].GetComponent<Card>().script.value == highlighted[2].GetComponent<Card>().script.value)
                return true;
        }
        if (highlighted.Count == 4) {
            if (highlighted[0].GetComponent<Card>().script.value == highlighted[1].GetComponent<Card>().script.value
            && highlighted[0].GetComponent<Card>().script.value == highlighted[2].GetComponent<Card>().script.value
            && highlighted[0].GetComponent<Card>().script.value == highlighted[3].GetComponent<Card>().script.value)
                return true;
        }
        return false;
    }
}
