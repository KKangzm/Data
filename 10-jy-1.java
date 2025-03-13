import java.util.*;
import java.util.concurrent.ThreadLocalRandom;

public class IslandSurvivalGame {
    // 地图生成系统
    static class MapGenerator {
        private static final int MAP_SIZE = 20;
        private final Random rand = new Random();
        private final String[][] terrainMap = new String[MAP_SIZE][MAP_SIZE];
        
        enum TerrainType {
            FOREST, BEACH, MOUNTAIN, RUINS, LAKE
        }

        public void generateIsland() {
            for (int x = 0; x < MAP_SIZE; x++) {
                for (int y = 0; y < MAP_SIZE; y++) {
                    double noise = rand.nextDouble();
                    if (noise < 0.3) terrainMap[x][y] = TerrainType.FOREST.name();
                    else if (noise < 0.5) terrainMap[x][y] = TerrainType.BEACH.name();
                    else if (noise < 0.7) terrainMap[x][y] = TerrainType.MOUNTAIN.name();
                    else terrainMap[x][y] = TerrainType.RUINS.name();
                }
            }
            terrainMap[10][10] = TerrainType.LAKE.name(); // 中央湖泊
        }

        public String getTerrain(int x, int y) {
            return terrainMap[x][y];
        }
    }

    // 解谜系统
    static class PuzzleSystem {
        interface Puzzle {
            boolean solve(Object input);
        }

        Map<String, Puzzle> activePuzzles = new HashMap<>();

        class SymbolPuzzle implements Puzzle {
            private final String pattern;

            SymbolPuzzle(String pattern) {
                this.pattern = pattern;
            }

            @Override
            public boolean solve(Object input) {
                return input.toString().equals(pattern);
            }
        }

        public void addPuzzle(String id, Puzzle puzzle) {
            activePuzzles.put(id, puzzle);
        }
    }

    // 资源管理系统
    static class ResourceManager {
        enum ResourceType { WOOD, STONE, HERBS, FOOD }
        private final Map<ResourceType, Integer> inventory = new EnumMap<>(ResourceType.class);

        public void gatherResource(ResourceType type, int amount) {
            inventory.put(type, inventory.getOrDefault(type, 0) + amount);
        }

        public boolean useResources(ResourceType type, int amount) {
            if (inventory.getOrDefault(type, 0) >= amount) {
                inventory.put(type, inventory.get(type) - amount);
                return true;
            }
            return false;
        }
    }

    // 角色成长系统
    static class PlayerProgression {
        private int experience;
        private int level;
        private final Set<String> unlockedAbilities = new HashSet<>();

        public void gainExperience(int exp) {
            experience += exp;
            if (experience >= getRequiredExp()) {
                level++;
                unlockNewAbility();
                experience = experience % getRequiredExp();
            }
        }

        private int getRequiredExp() {
            return level * 100 + 50;
        }

        private void unlockNewAbility() {
            String[] abilities = {"Climbing", "Swimming", "Crafting"};
            if (level <= abilities.length) {
                unlockedAbilities.add(abilities[level - 1]);
            }
        }
    }

    // 对话系统
    static class DialogueSystem {
        class DialogueTree {
            class Node {
                String text;
                Map<Integer, Node> options = new HashMap<>();
                Runnable callback;
            }

            Node currentNode;
            
            public void startDialogue(Node root) {
                currentNode = root;
            }

            public void selectOption(int choice) {
                if (currentNode.options.containsKey(choice)) {
                    currentNode = currentNode.options.get(choice);
                    if (currentNode.callback != null) currentNode.callback.run();
                }
            }
        }
    }

    // 天气和时间系统
    static class EnvironmentSystem {
        enum Weather { SUNNY, RAINY, STORM }
        enum TimePeriod { MORNING, DAY, EVENING, NIGHT }
        
        private Weather currentWeather = Weather.SUNNY;
        private TimePeriod currentTime = TimePeriod.MORNING;
        
        public void updateEnvironment() {
            // 每10分钟游戏时间更新一次
            currentTime = TimePeriod.values()[(currentTime.ordinal() + 1) % 4];
            if (ThreadLocalRandom.current().nextDouble() < 0.3) {
                currentWeather = Weather.values()[
                    ThreadLocalRandom.current().nextInt(Weather.values().length)];
            }
        }
    }

    // 结局和成就系统
    static class GameState {
        enum EndingType { GOOD, NEUTRAL, BAD }
        private final Set<String> achievedEndings = new HashSet<>();
        private final Set<String> unlockedAchievements = new HashSet<>();

        public void recordChoice(String choiceId) {
            // 根据选择记录结局走向
            if (choiceId.startsWith("ETHICAL_")) achievedEndings.add("GOOD");
            if (choiceId.startsWith("SELFISH_")) achievedEndings.add("BAD");
        }

        public void checkAchievements() {
            // 成就检测逻辑
            if (achievedEndings.size() >= 3) {
                unlockedAchievements.add("Master Explorer");
            }
        }
    }

    public static void main(String[] args) {
        // 初始化游戏系统
        MapGenerator map = new MapGenerator();
        PuzzleSystem puzzles = new PuzzleSystem();
        ResourceManager resources = new ResourceManager();
        PlayerProgression player = new PlayerProgression();
        EnvironmentSystem environment = new EnvironmentSystem();
        GameState gameState = new GameState();

        // 生成初始地图
        map.generateIsland();
        
        // 添加示例谜题
        puzzles.addPuzzle("temple_door", 
            new PuzzleSystem.SymbolPuzzle("αβγ"));
        
        // 游戏主循环
        for (int day = 1; day <= 7; day++) {
            environment.updateEnvironment();
            
            // 示例资源采集
            resources.gatherResource(ResourceManager.ResourceType.WOOD, 5);
            
            // 示例经验获取
            player.gainExperience(30);
        }
        
        // 记录结局
        gameState.recordChoice("ETHICAL_SAVING");
        gameState.checkAchievements();
        
        System.out.println("Game completed with " + 
            gameState.achievedEndings.size() + " endings unlocked!");
    }
}