using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
