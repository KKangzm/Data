#include <iostream>
#include <vector>
#include <map>
#include <memory>
#include <thread>
#include <mutex>
#include <queue>

// ==================== 卡牌管理系统 ====================
class Card {
public:
    std::string name;
    int energyCost;
    std::function<void()> effect;

    Card(std::string n, int cost, std::function<void()> eff)
        : name(n), energyCost(cost), effect(eff) {}
};

class CardManager {
    std::vector<std::unique_ptr<Card>> collection;
    std::vector<Card*> deck;
    
public:
    void addToCollection(Card* card) {
        collection.emplace_back(card);
    }

    void buildDeck() {
        // 简单的卡组构建逻辑
        for(int i=0; i<5 && i<collection.size(); ++i) {
            deck.push_back(collection[i].get());
        }
    }

    void useCard(int index) {
        if(index >= 0 && index < deck.size()) {
            deck[index]->effect();
        }
    }
};

// ==================== 战斗系统 ====================
class BattleSystem {
    enum class BattleState { PlayerTurn, EnemyTurn, Ended };
    BattleState state;
    int playerEnergy;
    int enemyHP;
    int playerHP;

public:
    BattleSystem() : playerEnergy(3), enemyHP(30), playerHP(25) {}

    void startBattle() {
        state = BattleState::PlayerTurn;
        battleLoop();
    }

private:
    void battleLoop() {
        while(state != BattleState::Ended) {
            if(state == BattleState::PlayerTurn) {
                handlePlayerTurn();
                state = BattleState::EnemyTurn;
            } else {
                handleEnemyTurn();
                state = BattleState::PlayerTurn;
            }
            checkBattleEnd();
        }
    }

    void handlePlayerTurn() {
        std::cout << "玩家回合，当前能量：" << playerEnergy << "\n";
        // 卡牌使用逻辑需要与卡牌管理器结合
    }

    void handleEnemyTurn() {
        std::cout << "敌人发动攻击！\n";
        playerHP -= 5;
    }

    void checkBattleEnd() {
        if(enemyHP <= 0 || playerHP <= 0) {
            state = BattleState::Ended;
        }
    }
};

// ==================== 解密系统 ====================
class Puzzle {
public:
    virtual ~Puzzle() = default;
    virtual bool checkSolution() = 0;
    virtual void showHint() = 0;
};

class CombinationPuzzle : public Puzzle {
    std::vector<std::string> requiredItems;
    std::vector<std::string> playerItems;
public:
    bool checkSolution() override {
        return requiredItems == playerItems;
    }

    void showHint() override {
        std::cout << "需要组合：" << requiredItems.size() << "个物品\n";
    }
};

// ==================== 角色成长系统 ====================
class CharacterGrowth {
    int level;
    int experience;
    std::map<int, std::string> unlockTable;

public:
    CharacterGrowth() : level(1), experience(0) {
        unlockTable = {
            {2, "新技能：火焰冲击"},
            {3, "装备槽+1"},
            {5, "终极奥义解锁"}
        };
    }

    void gainExp(int amount) {
        experience += amount;
        while(experience >= getRequiredExp()) {
            levelUp();
        }
    }

private:
    int getRequiredExp() const {
        return level * level * 100;
    }

    void levelUp() {
        ++level;
        std::cout << "升级到等级 " << level << "! ";
        if(unlockTable.count(level)) {
            std::cout << "解锁：" << unlockTable[level];
        }
        std::cout << "\n";
    }
};

// ==================== 多人对战系统 ====================
class MatchmakingSystem {
    std::queue<std::string> matchQueue;
    std::mutex queueMutex;

public:
    void joinQueue(const std::string& player) {
        std::lock_guard<std::mutex> lock(queueMutex);
        matchQueue.push(player);
        tryMatchPlayers();
    }

private:
    void tryMatchPlayers() {
        if(matchQueue.size() >= 2) {
            std::string p1 = matchQueue.front(); matchQueue.pop();
            std::string p2 = matchQueue.front(); matchQueue.pop();
            startMatch(p1, p2);
        }
    }

    void startMatch(const std::string& p1, const std::string& p2) {
        std::cout << "开始对战：" << p1 << " vs " << p2 << "\n";
    }
};

// ==================== UI系统 ====================
class UISystem {
public:
    void showMainMenu() {
        std::cout << "1. 开始战斗\n2. 卡组编辑\n3. 多人对战\n";
    }

    void animateCardPlay() {
        std::cout << "播放卡牌使用动画...\n";
    }
};

// ==================== 游戏主系统 ====================
class GameCore {
    CardManager cardManager;
    BattleSystem battleSystem;
    CharacterGrowth growthSystem;
    MatchmakingSystem matchmaking;
    UISystem uiSystem;

public:
    void initialize() {
        initializeCards();
        cardManager.buildDeck();
    }

    void startGame() {
        uiSystem.showMainMenu();
        // 示例游戏流程
        growthSystem.gainExp(150);
        battleSystem.startBattle();
        matchmaking.joinQueue("Player1");
    }

private:
    void initializeCards() {
        cardManager.addToCollection(
            new Card("火球术", 2, []{ std::cout << "造成5点伤害！\n"; })
        );
        cardManager.addToCollection(
            new Card("治疗术", 1, []{ std::cout << "恢复3点生命！\n"; })
        );
    }
};

int main() {
    GameCore game;
    game.initialize();
    game.startGame();
    return 0;
}