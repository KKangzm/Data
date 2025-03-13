import random
import time

class GameCharacter:
    def __init__(self, name, health=100, attack_power=10):
        self.name = name
        self.health = health
        self.attack_power = attack_power
        self.items = []
    
    def move(self, direction):
        print(f"{self.name} moves {direction}.")
    
    def attack(self, target):
        damage = random.randint(1, self.attack_power)
        target.health -= damage
        print(f"{self.name} attacks {target.name} for {damage} damage.")
    
    def use_item(self, item):
        if item in self.items:
            self.items.remove(item)
            item.use(self)
        else:
            print(f"{self.name} doesn't have this item.")
    
    def add_item(self, item):
        self.items.append(item)
        print(f"{self.name} picks up {item.name}.")

class Item:
    def __init__(self, name, effect):
        self.name = name
        self.effect = effect
    
    def use(self, character):
        print(f"{character.name} uses {self.name}.")
        self.effect(character)

class HealingPotion(Item):
    def __init__(self):
        super().__init__("Healing Potion", lambda char: setattr(char, 'health', min(char.health + 20, 100)))

class Key(Item):
    def __init__(self):
        super().__init__("Key", lambda char: print(f"{char.name} unlocks the door."))

class Enemy(GameCharacter):
    def ai(self, player):
        if random.random() < 0.5:
            self.move(random.choice(['left', 'right', 'up', 'down']))
        else:
            self.attack(player)

class Puzzle:
    def __init__(self, question, answer):
        self.question = question
        self.answer = answer
    
    def solve(self, guess):
        if guess == self.answer:
            print("Puzzle solved!")
            return True
        else:
            print("Incorrect answer.")
            return False

class Scene:
    def __init__(self, name, description, puzzles=None, enemies=None):
        self.name = name
        self.description = description
        self.puzzles = puzzles or []
        self.enemies = enemies or []
    
    def enter(self, player):
        print(f"Entering {self.name}: {self.description}")
        for puzzle in self.puzzles:
            if not puzzle.solve(input("Solve the puzzle: ")):
                return
        for enemy in self.enemies:
            while enemy.health > 0 and player.health > 0:
                player.attack(enemy)
                if enemy.health > 0:
                    enemy.ai(player)
                if player.health <= 0:
                    print(f"{player.name} has been defeated.")
                    return
        print(f"{player.name} clears the scene {self.name}.")

class Story:
    def __init__(self, scenes):
        self.scenes = scenes
        self.current_scene_index = 0
    
    def next_scene(self, player):
        if self.current_scene_index < len(self.scenes):
            current_scene = self.scenes[self.current_scene_index]
            current_scene.enter(player)
            self.current_scene_index += 1
            time.sleep(2)  # Simulate loading time
        else:
            print("The game is over!")

# Example usage
player1 = GameCharacter("Hero1")
player2 = GameCharacter("Hero2")

healing_potion = HealingPotion()
key = Key()

player1.add_item(healing_potion)
player2.add_item(key)

puzzle1 = Puzzle("What has keys but can't open locks?", "keyboard")
enemy1 = Enemy("Goblin", health=50, attack_power=5)

scene1 = Scene("Forest", "A dense forest with mysterious sounds.", [puzzle1], [enemy1])
scene2 = Scene("Castle", "An ancient castle with a locked door.", [], [])

story = Story([scene1, scene2])

# Start the game
story.next_scene(player1)
story.next_scene(player2)
