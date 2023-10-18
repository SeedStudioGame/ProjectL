using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class CardSet
{
    public int id; // 카드 아이디
    public string name; // 카드 이름
    public string type; // 카드 타입
    public Define.Attribute attribute; // 카드 속성
    public string imageUrl; // 카드 이미지
    public string description; // 카드 설명
    public int value; // 카드 값

    public CardSet(int id, string name, string type, Define.Attribute attribute, string imageUrl, string description, int value)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.attribute = attribute;
        this.imageUrl = imageUrl;
        this.description = description;
        this.value = value;
    }

    public CardSet()
    {
        this.id = 0;
        this.name = "";
        this.type = "";
        this.attribute = Define.Attribute.fire;
        this.imageUrl = "";
        this.description = "";
    }
}



public class CardController : MonoBehaviour
{
    [SerializeField]
    List<CardSet> cardsInArray = new List<CardSet>();
    List<CardSet> cardsInHand = new List<CardSet>();
    List<CardSet> cardsInDeck = new List<CardSet>();

    void Start()
    {
        // Load cards from json file
        TextAsset jsonFile = Resources.Load<TextAsset>("Cards");
        cardsInDeck = JsonUtility.FromJson<List<CardSet>>(jsonFile.text);

        Debug.Log(cardsInDeck.Count);
        // Shuffle cards
        Shuffle(cardsInDeck);

        // Draw 5 cards
        for (int i = 0; i < 5; i++)
        {
            cardsInHand.Add(cardsInDeck[0]);
            cardsInDeck.RemoveAt(0);
        }

        // Show cards in hand
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            Debug.Log(cardsInHand[i].name);
        }
    }

    void Shuffle(List<CardSet> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            CardSet temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

}
