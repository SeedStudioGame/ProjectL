using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Card
{
    public int id; // 카드 아이디
    public string name; // 카드 이름
    public string type; // 카드 타입
    public Define.Attribute attribute; // 카드 속성
    public string imageUrl; // 카드 이미지
    public string description; // 카드 설명
    public int value; // 카드 값
}

class Cards
{
    public List<Card> datas = new List<Card>();
}

public class CardController : MonoBehaviour
{
    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Cards");
        Cards cardsInDeck = JsonUtility.FromJson<Cards>(jsonFile.text);
    }
}
