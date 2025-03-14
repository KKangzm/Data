#include <iostream>
#include <vector>
#include <map>
#include <functional>
#include <random>

// ==================== 解密系统 ====================
class Puzzle {
public:
    virtual ~Puzzle() = default;
    virtual bool checkCondition(const std::string& input) = 0;
    virtual void triggerEffect() = 0;
};

class MathPuzzle : public Puzzle {
    int answer;
public:
    MathPuzzle(int correct) : answer(correct) {}
    bool checkCondition(const std::string& input) override {
        return std::stoi(input) == answer;
    }
    void triggerEffect() override {
        std::cout << "Math Puzzle Solved!\n";
    }
};

class MusicPuzzle : public Puzzle {
    std::string sequence;
public:
    MusicPuzzle(const std::string& seq) : sequence(seq) {}
    bool checkCondition(const std::string& input) override {
        return input == sequence;
    }
    void triggerEffect() override {
        std::cout << "Melody Played Correctly!\n";
    }
};

// ==================== 角色互动系统 ====================
class DialogueTree {
    struct Node {
        std::string text;
        std::map<int, Node*> options;
    };

    Node* root;
    Node* current;
public:
    DialogueTree() : root(new Node), current(root) {}
    
    void addOption(int choice, const std::string& response) {
        current->options[choice] = new Node{response, {}};
    }

    void selectOption(int choice) {
        if (current->options.find(choice) != current->options.end()) {
            current = current->options[choice];
            std::cout << current->text << "\n";
        }
    }
};

class GiftSystem {
    std::map<std::string, int> affinity;
public:
    void giveGift(const std::string& npc, const std::string& gift) {
        int value = calculateGiftValue(npc, gift);
        affinity[npc] += value;
        std::cout << npc << " affinity +" << value << "\n";
    }

private:
    int calculateGiftValue(const std::string& npc, const std::string& gift) {
        // 实现基于NPC喜好的计算逻辑
        return 10;
    }
};

// ==================== 角色成长系统 ====================
class SkillNode {
public:
    std::string name;
    bool unlocked;
    std::vector<SkillNode*> prerequisites;

    SkillNode(std::string name) : name(name), unlocked(false) {}
};

class SkillTree {
    std::map<std::string, SkillNode*> skills;
public:
    void addSkill(const std::string& name, 
                const std::vector<std::string>& requires) {
        SkillNode* node = new SkillNode(name);
        for (auto& req : requires) {
            node->prerequisites.push_back(skills[req]);
        }
        skills[name] = node;
    }

    bool unlockSkill(const std::string& name) {
        if (skills[name]->prerequisites.empty() || 
            std::all_of(skills[name]->prerequisites.begin(), 
                       skills[name]->prerequisites.end(),
                       [](SkillNode* n){ return n->unlocked; })) {
            skills[name]->unlocked = true;
            return true;
        }
        return false;
    }
};

// ==================== 岛屿探索系统 ====================
class IslandMap {
    std::vector<std::vector<char>> grid;
    std::mt19937 rng;

public:
    IslandMap(int size) : rng(std::random_device{}()) {
        generateProceduralMap(size);
    }

    void generateProceduralMap(int size) {
        grid.resize(size, std::vector<char>(size));
        std::uniform_int_distribution<int> dist(0, 100);
        
        for (auto& row : grid) {
            for (auto& cell : row) {
                cell = dist(rng) < 30 ? 'T' : 
                      (dist(rng) < 10 ? 'X' : '.');
            }
        }
    }

    void printMap() const {
        for (const auto& row : grid) {
            for (char cell : row) {
                std::cout << cell;
            }
            std::cout << "\n";
        }
    }
};

// ==================== 剧情系统 ====================
class StoryManager {
    struct Chapter {
        std::string name;
        bool unlocked;
        std::function<bool()> condition;
    };

    std::vector<Chapter> chapters;
    int currentChapter;

public:
    StoryManager() : currentChapter(0) {
        chapters.push_back({"Arrival", true, []{ return true; }});
        chapters.push_back({"Lab Discovery", false, 
                           []{ /* 解锁条件 */ return true; }});
    }

    void updateProgress() {
        if (chapters[currentChapter+1].condition()) {
            currentChapter++;
            std::cout << "New Chapter Unlocked: " 
                     << chapters[currentChapter].name << "\n";
        }
    }
};

// ==================== 游戏主系统 ====================
class GameSystem {
    SkillTree skills;
    IslandMap map;
    StoryManager story;
    GiftSystem gifts;

public:
    GameSystem() : map(20) {}

    void run() {
        std::cout << "=== Starting Island Adventure ===\n";
        map.printMap();
        
        // 示例交互
        gifts.giveGift("Scientist", "Herb");
        skills.addSkill("Botany", {});
        skills.unlockSkill("Botany");
        
        story.updateProgress();
    }
};

int main() {
    GameSystem game;
    game.run();
    return 0;
}