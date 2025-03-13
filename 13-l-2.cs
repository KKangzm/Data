using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>(); // 玩家卡组
    public List<Card> hand = new List<Card>(); // 当前手牌
    public int maxHandSize = 5; // 最大手牌数

    // 随机抽一张卡
    public void DrawCard()
    {
        if (deck.Count > 0 && hand.Count < maxHandSize)
        {
            int index = Random.Range(0, deck.Count);
            Card drawnCard = deck[index];
            hand.Add(drawnCard);
            deck.RemoveAt(index);
            Debug.Log($"Drew card: {drawnCard.cardName}");
        }
        else
        {
            Debug.Log("No more cards to draw or hand is full.");
        }
    }

    // 使用手牌
    public void PlayCard(int handIndex, Player player)
    {
        if (handIndex >= 0 && handIndex < hand.Count)
        {
            hand[handIndex].UseCard(player);
            hand.RemoveAt(handIndex); // 卡牌用完后消失
        }
    }
}
