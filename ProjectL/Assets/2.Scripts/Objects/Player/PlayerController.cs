using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Card
{
    public string name;
    public int value;
    public string type;

    public GameObject cardObject;


    public Card(string name, int value, string type)
    {
        this.name = name;
        this.value = value;
        this.type = type;

        this.cardObject = new GameObject();
    }
}

public class PlayerController : MonoBehaviour
{

    // Public Variables
    [Header("Player Settings")]

    [SerializeField]
    public int maxCardLength = 4; // Max number of cards in the deck
    public int maxHealth = 20;  // Max health of the player
    public int speed = 0; // Speed of the player

    [Header("Player Stats")]
    public int health = 20; // Current health of the player
    public int currentCardLength = 0; // Current number of cards in the deck

    // Start is called before the first frame update
    void Start()
    {


        // Create a deck of cards
        List<Card> deck = new List<Card>();

        // Add cards to the deck
        for(int i = 0; i<maxCardLength; i++)
        {
            deck.Add(new Card(null, 0, null)); // Add a NULL card to the deck
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
