#include <iostream>
#include <vector>
#include <unordered_map>
#include <set>
#include <utility>

// 情绪类型枚举
enum class EmotionType { SHY, BRAVE, ANGRY, CALM, HAPPY, NEUTRAL };

// 情绪共鸣系统
class EmotionResonanceSystem {
private:
    struct EmotionState {
        EmotionType type;
        int intensity;
    };
    
    EmotionState playerEmotion;
    std::unordered_map<std::string, EmotionState> npcEmotions;

public:
    // 更新NPC情绪状态
    void updateNpcEmotion(const std::string& npcId, EmotionType type, int intensity) {
        npcEmotions[npcId] = { type, std::clamp(intensity, 0, 100) };
    }

    // 检查共鸣条件
    bool checkResonance(const std::string& npcId) const {
        auto it = npcEmotions.find(npcId);
        if (it == npcEmotions.end()) return false;

        const auto& npcEmo = it->second;
        
        // 情绪互补规则
        static const std::unordered_map<EmotionType, EmotionType> resonanceRules = {
            {EmotionType::SHY, EmotionType::BRAVE},
            {EmotionType::ANGRY, EmotionType::CALM},
            {EmotionType::HAPPY, EmotionType::NEUTRAL}
        };

        auto ruleIt = resonanceRules.find(npcEmo.type);
        if (ruleIt == resonanceRules.end()) return false;

        return playerEmotion.type == ruleIt->second && 
               abs(npcEmo.intensity - playerEmotion.intensity) < 30;
    }

    // 生成迷宫路径（简化的3x3矩阵示例）
    std::vector<std::vector<int>> generateMazePath(const std::string& npcId) const {
        if (checkResonance(npcId)) {
            return {{1,0,1}, {0,1,0}, {1,0,1}}; // 开放路径
        }
        return {{0,1,0}, {1,0,1}, {0,1,0}};    // 防御迷宫
    }

    void setPlayerEmotion(EmotionType type, int intensity) {
        playerEmotion = {type, std::clamp(intensity, 0, 100)};
    }
};

// 人格魔方系统
class PersonalityCube {
private:
    std::string currentFacet = "surface";
    std::set<std::string> requiredItems = {"mechanical_keyboard", "hologram_jade", "nano_brush"};

public:
    void flipDimension(const std::string& newFacet) {
        currentFacet = newFacet;
    }

    bool checkItems(const std::set<std::string>& collected) const {
        for (const auto& item : requiredItems) {
            if (collected.find(item) == collected.end()) return false;
        }
        return true;
    }

    bool unlockAbility(bool& abilityLocked) const {
        if (abilityLocked) {
            abilityLocked = false;
            return true;
        }
        return false;
    }
};

// 羁绊组合系统
class BondAbilitySystem {
private:
    using AbilityPair = std::pair<std::string, std::string>;
    
    std::unordered_map<AbilityPair, std::string, PairHash> combinationRules = {
        {{"laser_cut", "poison_mist"}, "corrosion_beam"},
        {{"time_stop", "plant_growth"}, "instant_garden"},
        {{"telekinesis", "electric_shock"}, "plasma_manipulation"}
    };

public:
    // 自定义pair哈希器
    struct PairHash {
        template <class T1, class T2>
        std::size_t operator()(const std::pair<T1, T2>& p) const {
            auto h1 = std::hash<T1>{}(p.first);
            auto h2 = std::hash<T2>{}(p.second);
            return h1 ^ (h2 << 1);
        }
    };

    std::string combineAbilities(const std::string& a1, const std::string& a2) const {
        auto it = combinationRules.find({a1, a2});
        return (it != combinationRules.end()) ? it->second : "";
    }

    bool solvePuzzle(const std::string& required, 
                    const std::vector<std::string>& used) const {
        if (used.size() != 2) return false;
        return combineAbilities(used[0], used[1]) == required;
    }
};

// 游戏控制器
class GameController {
private:
    EmotionResonanceSystem emotionSystem;
    PersonalityCube cubeSystem;
    BondAbilitySystem bondSystem;

public:
    void handleDialogChoice(const std::string& npcId, const std::string& choice) {
        static const std::unordered_map<std::string, std::pair<EmotionType, int>> emotionMap = {
            {"aggressive", {EmotionType::ANGRY, 70}},
            {"humorous", {EmotionType::HAPPY, 40}},
            {"curious", {EmotionType::BRAVE, 30}}
        };
        
        auto it = emotionMap.find(choice);
        if (it != emotionMap.end()) {
            emotionSystem.setPlayerEmotion(it->second.first, it->second.second);
        }
    }

    bool attemptPuzzle(const std::string& puzzleType, 
                      const std::string& npcId,
                      const std::set<std::string>& items) {
        if (puzzleType == "emotion_maze") {
            auto path = emotionSystem.generateMazePath(npcId);
            return !path.empty(); // 简化的导航检查
        }
        else if (puzzleType == "personality_cube") {
            return cubeSystem.checkItems(items);
        }
        return false;
    }
};

int main() {
    // 测试情绪共鸣系统
    EmotionResonanceSystem ers;
    ers.setPlayerEmotion(EmotionType::BRAVE, 40);
    ers.updateNpcEmotion("tech_maiden", EmotionType::SHY, 65);
    std::cout << "Resonance check: " 
              << ers.checkResonance("tech_maiden") << std::endl;

    // 测试人格魔方系统
    PersonalityCube cube;
    cube.flipDimension("gamer_room");
    std::set<std::string> collected = {"mechanical_keyboard", "hologram_jade"};
    std::cout << "Items validation: " 
              << cube.checkItems(collected) << std::endl;

    // 测试羁绊组合系统
    BondAbilitySystem bond;
    std::cout << "Ability combo: " 
              << bond.combineAbilities("laser_cut", "poison_mist") << std::endl;

    return 0;
}