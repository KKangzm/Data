cpp
#include <iostream>
#include <vector>
#include <string>
#include <map>
#include <algorithm>
#include <random>
#include <ctime>

// 基础类定义
class Card {
public:
    std::string name;
    int attack;
    int defense;
    int cost;

    Card(std::string name, int attack, int defense, int cost) : name(name), attack(attack), defense(defense), cost(cost) {}
};

class Player {
public:
    std::string name;
    int health;
    int energy;
    int experience;
    int level;
    std::vector<Card> deck;
    std::map<std::string, int> skills;

    Player(std::string name, int health, int energy) : name(name), health(health), energy(energy), experience(0), level(1) {}

    void addCardToDeck(Card card) {
        deck.push_back(card);
    }

    bool useCard(int index) {
        if (index >= 0 && index < deck.size() && deck[index].cost <= energy) {
            energy -= deck[index].cost;
            // 卡牌效果
            std::cout << "使用了卡牌: " << deck[index].name << std::endl;
            return true;
        }
        return false;
    }

    void gainExperience(int exp) {
        experience += exp;
        while (experience >= 100 * level) {
            levelUp();
        }
    }

private:
    void levelUp() {
        level++;
        std::cout << name << "升级到了" << level << "级！" << std::endl;
    }
};

// 战斗系统
void battle(Player& player1, Player& player2) {
    while (player1.health > 0 && player2.health > 0) {
        std::cout << player1.name << "的回合" << std::endl;
        for (int i = 0; i < player1.deck.size(); ++i) {
            std::cout << i << ": " << player1.deck[i].name << " (" << player1.deck[i].attack << "/" << player1.deck[i].defense << ") 花费: " << player1.deck[i].cost << std::endl;
        }
        int choice;
        std::cout << "选择卡牌: ";
        std::cin >> choice;
        if (player1.useCard(choice)) {
            player2.health -= player1.deck[choice].attack;
            std::cout << player2.name << "受到了" << player1.deck[choice].attack << "点伤害，剩余生命值: " << player2.health << std::endl;
        }

        if (player2.health <= 0) {
            std::cout << player1.name << "胜利！" << std::endl;
            player1.gainExperience(50);
            break;
        }

        std::cout << player2.name << "的回合" << std::endl;
        // 简单的AI
        std::random_device rd;
        std::mt19937 gen(rd());
        std::uniform_int_distribution<> dis(0, player2.deck.size() - 1);
        int aiChoice = dis(gen);
        if (player2.useCard(aiChoice)) {
            player1.health -= player2.deck[aiChoice].attack;
            std::cout << player1.name << "受到了" << player2.deck[aiChoice].attack << "点伤害，剩余生命值: " << player1.health << std::endl;
        }

        if (player1.health <= 0) {
            std::cout << player2.name << "胜利！" << std::endl;
            player2.gainExperience(50);
            break;
        }
    }
}

// 主函数
int main() {
    srand(time(0));
    Player player1("Player1", 100, 10);
    Player player2("Player2", 100, 10);

    player1.addCardToDeck(Card("Fireball", 10, 0, 5));
    player1.addCardToDeck(Card("Shield", 0, 10, 3));
    player2.addCardToDeck(Card("Ice Shard", 8, 0, 4));
    player2.addCardToDeck(Card("Armor Up", 0, 8, 2));

    battle(player1, player2);

    return 0;
}
