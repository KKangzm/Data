using System;
using UnityEngine;

// 定义卡牌类型
public enum CardType { Exploration, Combat, Building, Puzzle }

// 卡牌基类
[Serializable]
public class Card
{
    public string cardName;
    public CardType type;
    public string description;
    public int effectValue; // 影响值，如伤害、恢复值
    public Sprite icon;

    public virtual void UseCard(Player player)
    {
        Debug.Log($"{cardName} used.");
    }
}

// 示例：战斗卡牌（继承 Card）
public class CombatCard : Card
{
    public int attackPower;

    public override void UseCard(Player player)
    {
        Debug.Log($"Player attacks with {cardName}, dealing {attackPower} damage.");
        // 这里可以调用敌人受伤的逻辑
    }
}
