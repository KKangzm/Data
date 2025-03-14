import pygame
import random
from collections import deque
from datetime import datetime, timedelta

class TimeController:
    def __init__(self):
        self.game_time = datetime(2023, 1, 1, 8, 0)
        self.weather = "sunny"
        self.time_scale = 60  # 1秒游戏时间=1分钟现实时间
        self.history = deque(maxlen=100)  # 时间回溯记录

    def update(self):
        # 更新时间并保存状态快照
        prev_state = self.get_state()
        self.game_time += timedelta(minutes=self.time_scale)
        self.history.append(prev_state)

        # 动态天气变化
        if random.random() < 0.005:
            self.weather = random.choice(["rainy", "sunny", "foggy"])

    def get_state(self):
        return {
            "time": self.game_time,
            "weather": self.weather
        }

    def rewind_time(self):
        if self.history:
            state = self.history.pop()
            self.game_time = state["time"]
            self.weather = state["weather"]

class InventorySystem:
    def __init__(self):
        self.items = {}
        self.combination_recipes = {
            ("key", "rusty_key"): "golden_key",
            ("paper", "ink"): "document"
        }

    def add_item(self, item_name):
        self.items[item_name] = self.items.get(item_name, 0) + 1

    def combine_items(self, item1, item2):
        for recipe, result in self.combination_recipes.items():
            if {item1, item2} == set(recipe):
                self.items[item1] -= 1
                self.items[item2] -= 1
                self.add_item(result)
                return result
        return None

class SymbolPuzzle:
    def __init__(self, pattern, solution):
        self.pattern = pattern
        self.solution = solution
        self.player_input = []

    def check_solution(self):
        return self.player_input == self.solution

    def reset(self):
        self.player_input = []

class GameEntity(pygame.sprite.Sprite):
    def __init__(self, image_path, position, interactable=False):
        super().__init__()
        self.image = pygame.image.load(image_path)
        self.rect = self.image.get_rect(center=position)
        self.interactable = interactable
        self.rotation = 0
        self.original_image = self.image

    def rotate(self, degrees):
        self.rotation = (self.rotation + degrees) % 360
        self.image = pygame.transform.rotate(self.original_image, self.rotation)
        self.rect = self.image.get_rect(center=self.rect.center)

class AudioManager:
    def __init__(self):
        pygame.mixer.init()
        self.sounds = {
            "click": pygame.mixer.Sound("click.wav"),
            "success": pygame.mixer.Sound("success.wav")
        }
        self.current_music = None

    def play_music(self, track, loop=True):
        if self.current_music != track:
            pygame.mixer.music.load(track)
            pygame.mixer.music.play(-1 if loop else 0)
            self.current_music = track

class GameCore:
    def __init__(self):
        pygame.init()
        self.screen = pygame.display.set_mode((800, 600))
        self.clock = pygame.time.Clock()
        self.running = True
        self.time_controller = TimeController()
        self.inventory = InventorySystem()
        self.audio = AudioManager()
        self.entities = pygame.sprite.Group()
        self.current_puzzle = None
        self.collected_documents = []

        # 初始化示例实体
        self.entities.add(GameEntity("chest.png", (400, 300), True))
        self.entities.add(GameEntity("key.png", (100, 100), True))

    def handle_interaction(self, event):
        if event.type == pygame.MOUSEBUTTONDOWN:
            pos = pygame.mouse.get_pos()
            clicked_sprites = [s for s in self.entities if s.rect.collidepoint(pos)]
            if clicked_sprites:
                self.audio.sounds["click"].play()
                return clicked_sprites[0]
        return None

    def handle_puzzle_input(self, event):
        if self.current_puzzle and event.type == pygame.KEYDOWN:
            if event.key in [pygame.K_UP, pygame.K_DOWN, pygame.K_LEFT, pygame.K_RIGHT]:
                direction = {
                    pygame.K_UP: "up",
                    pygame.K_DOWN: "down",
                    pygame.K_LEFT: "left",
                    pygame.K_RIGHT: "right"
                }[event.key]
                self.current_puzzle.player_input.append(direction)
                if self.current_puzzle.check_solution():
                    self.audio.sounds["success"].play()
                    print("Puzzle Solved!")
                    self.current_puzzle = None

    def update_environment(self):
        # 根据时间和天气更新视觉效果
        if self.time_controller.weather == "rainy":
            self.screen.fill((50, 50, 100))
        elif self.time_controller.weather == "foggy":
            self.screen.fill((150, 150, 150))
        else:
            hour = self.time_controller.game_time.hour
            if 6 <= hour < 18:
                self.screen.fill((135, 206, 235))  # 白天
            else:
                self.screen.fill((30, 30, 60))     # 夜晚

    def run(self):
        while self.running:
            for event in pygame.event.get():
                if event.type == pygame.QUIT:
                    self.running = False
                elif event.type == pygame.KEYDOWN and event.key == pygame.K_r:
                    self.time_controller.rewind_time()

                # 处理交互和谜题输入
                selected_object = self.handle_interaction(event)
                self.handle_puzzle_input(event)

                # 文档收集示例
                if event.type == pygame.KEYDOWN and event.key == pygame.K_d:
                    self.collected_documents.append("日记内容...")

            # 更新游戏状态
            self.time_controller.update()
            self.update_environment()
            
            # 绘制游戏实体
            self.entities.draw(self.screen)
            
            # 更新显示
            pygame.display.flip()
            self.clock.tick(30)

        pygame.quit()

if __name__ == "__main__":
    game = GameCore()
    game.run()