cpp
#include <iostream>
#include <vector>
#include <map>
#include <string>
#include <random>
#include <algorithm>

class Character {
public:
    std::string name;
    int experience = 0;
    int level = 1;
    int affection = 50; // Initial neutral affection
    std::map<std::string, int> skills;

    Character(std::string name) : name(name) {
        skills["strength"] = 1;
        skills["intelligence"] = 1;
        skills["agility"] = 1;
    }

    void addExperience(int exp) {
        experience += exp;
        while (experience >= 100 * level) {
            levelUp();
            experience -= 100 * level;
        }
    }

    void levelUp() {
        level++;
        for (auto& skill : skills) {
            skill.second++;
        }
    }

    void receiveGift(const std::string& gift, int affectionChange) {
        affection += affectionChange;
        std::cout << name << " received a " << gift << " and is now more " << (affectionChange > 0 ? "happy" : "upset") << " with you." << std::endl;
    }
};

class DialogueOption {
public:
    std::string text;
    std::function<void(Character&)> action;

    DialogueOption(std::string text, std::function<void(Character&)> action) : text(text), action(action) {}
};

class DialogueSystem {
public:
    std::vector<DialogueOption> options;

    void addOption(std::string text, std::function<void(Character&)> action) {
        options.push_back(DialogueOption(text, action));
    }

    void showOptions(Character& character) {
        for (size_t i = 0; i < options.size(); ++i) {
            std::cout << i + 1 << ". " << options[i].text << std::endl;
        }
        int choice;
        std::cout << "Choose an option: ";
        std::cin >> choice;
        if (choice > 0 && choice <= options.size()) {
            options[choice - 1].action(character);
        } else {
            std::cout << "Invalid choice." << std::endl;
        }
    }
};

class IslandMap {
public:
    std::vector<std::string> locations;
    std::map<std::string, std::string> hiddenTreasures;
    std::map<std::string, std::string> events;

    IslandMap() {
        locations = {"Beach", "Forest", "Mountain", "Cave"};
        hiddenTreasures["Beach"] = "Shell";
        hiddenTreasures["Cave"] = "Ancient Coin";
        events["Forest"] = "Encounter a wild animal";
        events["Mountain"] = "Find a mysterious stone tablet";
    }

    void exploreLocation(const std::string& location) {
        std::cout << "Exploring " << location << "..." << std::endl;
        if (hiddenTreasures.find(location) != hiddenTreasures.end()) {
            std::cout << "You found a " << hiddenTreasures[location] << "!" << std::endl;
        }
        if (events.find(location) != events.end()) {
            std::cout << "Event: " << events[location] << std::endl;
        }
    }
};

class Plot {
public:
    std::map<int, std::string> chapters;
    int currentChapter = 1;

    Plot() {
        chapters[1] = "Introduction to the island";
        chapters[2] = "Meeting the locals";
        chapters[3] = "Discovering the ancient ruins";
        chapters[4] = "Final battle against the dark force";
    }

    bool unlockNextChapter() {
        if (currentChapter < chapters.size()) {
            currentChapter++;
            std::cout << "Unlocked new chapter: " << chapters[currentChapter] << std::endl;
            return true;
        }
        return false;
    }

    void playCurrentChapter() {
        std::cout << "Playing chapter " << currentChapter << ": " << chapters[currentChapter] << std::endl;
    }
};

int main() {
    Character player("Player");
    Character npc("NPC");

    DialogueSystem dialogue;
    dialogue.addOption("Ask about the island", [&npc]() { std::cout << npc.name << " tells you about the island's history." << std::endl; });
    dialogue.addOption("Give a gift", [&player, &npc]() { player.receiveGift("Flower", 10); });
    dialogue.addOption("Say goodbye", [&npc]() { std::cout << npc.name << " waves goodbye." << std::endl; });

    IslandMap map;
    Plot plot;

    plot.playCurrentChapter();

    while (true) {
        std::cout << "1. Talk to NPC\n2. Explore the island\n3. Check status\n4. Exit game\n";
        int choice;
        std::cin >> choice;

        switch (choice) {
        case 1:
            dialogue.showOptions(npc);
            break;
        case 2:
            std::cout << "Choose a location to explore:\n";
            for (size_t i = 0; i < map.locations.size(); ++i) {
                std::cout << i + 1 << ". " << map.locations[i] << std::endl;
            }
            int locationChoice;
            std::cin >> locationChoice;
            if (locationChoice > 0 && locationChoice <= map.locations.size()) {
                map.exploreLocation(map.locations[locationChoice - 1]);
                player.addExperience(20); // Gain experience from exploration
            } else {
                std::cout << "Invalid location choice." << std::endl;
            }
            break;
        case 3:
            std::cout << "Player status:\nLevel: " << player.level << "\nExperience: " << player.experience << "\nAffection: " << player.affection << std::endl;
            break;
        case 4:
            std::cout << "Exiting game..." << std::endl;
            return 0;
        default:
            std::cout << "Invalid choice." << std::endl;
        }

        if (player.level >= 5 && !plot.unlockNextChapter()) {
            std::cout << "Congratulations! You have completed the game." << std::endl;
            return 0;
        }
    }

    return 0;
}
