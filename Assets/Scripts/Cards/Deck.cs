using UnityEngine;

/// <summary>
/// Manages the deck of cards, including generation, folding, and removal.
/// </summary>
public class Deck : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Header("Deck Configuration")]
    [Tooltip("Prefab used for card generation.")]
    public GameObject cardPrefab;

    [Tooltip("Number of cards in the deck.")]
    public int numberOfCards = 7;

    [Tooltip("Array holding the instantiated card objects.")]
    public GameObject[] cards;

    [Tooltip("Animator controlling the deck animations.")]
    public Animator animator;

    //  ------------------ Private ------------------

    private bool _isFolded = false;

    /// <summary>
    /// Folds the deck, triggering the fold animation.
    /// </summary>
    public void FoldDeck() {
        _isFolded = true;
        foreach (GameObject item in cards)
        {
            UICard card = item.GetComponent<UICard>();
            card.UnFlipCard();
        }
        if(_isFolded) animator.Play("DeckFold");
    }

    /// <summary>
    /// Removes all cards from the deck and returns them to the pool.
    /// </summary>
    public void RemoveDeck() {
        Debug.Log("Remove Deck called!");
        foreach (GameObject card in cards) PoolManager.Instance.ReturnObject(card);
    }

    /// <summary>
    /// Generates a new deck with the specified number of cards.
    /// </summary>
    public void GenerateDeck() {
        cards = new GameObject[numberOfCards];
        for (int i = 0; i < numberOfCards; i++) {
            cards[i] = PoolManager.Instance.GetObject(cardPrefab, Vector3.zero , Quaternion.identity, transform);
            Card card = cards[i].GetComponent<Card>();
            card.Init();
        }
        _isFolded = false;
    }
}