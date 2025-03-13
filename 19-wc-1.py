import pygame
import sys

# 屏幕尺寸
SCREEN_WIDTH = 800
SCREEN_HEIGHT = 600

# 游戏状态常量
STATE_MENU = "menu"
STATE_PLAYING = "playing"
STATE_PUZZLE = "puzzle"
STATE_BATTLE = "battle"
STATE_END = "end"

# 初始化 Pygame
pygame.init()
screen = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))
pygame.display.set_caption("Steel Island: Gundam Awakening")
clock = pygame.time.Clock()

# 玩家类：控制玩家移动与绘制
class Player:
    def __init__(self, x, y):
        self.x = x
        self.y = y
        self.speed = 5
        self.image = pygame.Surface((50, 50))
        self.image.fill((255, 0, 0))
        self.rect = self.image.get_rect(center=(x, y))
    
    def move(self, dx, dy):
        self.x += dx * self.speed
        self.y += dy * self.speed
        self.rect.center = (self.x, self.y)
    
    def draw(self, surface):
        surface.blit(self.image, self.rect)

# 谜题类：用于处理各区域谜题逻辑
class Puzzle:
    def __init__(self, puzzle_type):
        self.puzzle_type = puzzle_type
        self.solved = False

    def solve(self):
        # 这里可以加入具体谜题逻辑，目前用按空格键直接解谜模拟
        self.solved = True

    def draw(self, surface):
        font = pygame.font.SysFont("Arial", 24)
        text = font.render("Puzzle: " + self.puzzle_type, True, (255, 255, 255))
        surface.blit(text, (SCREEN_WIDTH/2 - text.get_width()/2, SCREEN_HEIGHT/2 - text.get_height()/2))

# 怪物类：包含简单的追踪玩家逻辑
class Monster:
    def __init__(self, x, y, monster_type):
        self.x = x
        self.y = y
        self.monster_type = monster_type
        self.image = pygame.Surface((40, 40))
        self.image.fill((0, 255, 0))
        self.rect = self.image.get_rect(center=(x, y))
    
    def update(self, player):
        # 简单 AI：向玩家方向移动
        if player.x > self.x:
            self.x += 1
        elif player.x < self.x:
            self.x -= 1
        if player.y > self.y:
            self.y += 1
        elif player.y < self.y:
            self.y -= 1
        self.rect.center = (self.x, self.y)
    
    def draw(self, surface):
        surface.blit(self.image, self.rect)

# 地图类：管理各个区域（对应高达机体的不同部位）
class GameMap:
    def __init__(self):
        # 各区域名称：头部、躯干、臂部、腿部、驾驶舱
        self.areas = ["Head", "Torso", "Arms", "Legs", "Cockpit"]
        self.current_area_index = 0

    def current_area(self):
        return self.areas[self.current_area_index]

    def next_area(self):
        if self.current_area_index < len(self.areas) - 1:
            self.current_area_index += 1

# 游戏主类：整合各模块逻辑与主循环
class Game:
    def __init__(self):
        self.state = STATE_MENU
        self.player = Player(SCREEN_WIDTH // 2, SCREEN_HEIGHT // 2)
        self.game_map = GameMap()
        self.puzzle = None
        self.monsters = []
    
    def start_game(self):
        self.state = STATE_PLAYING
        self.game_map = GameMap()
        self.player = Player(SCREEN_WIDTH // 2, SCREEN_HEIGHT // 2)
        self.spawn_monsters()
    
    def spawn_monsters(self):
        # 根据当前区域简单生成怪物
        self.monsters = []
        area = self.game_map.current_area()
        if area == "Head":
            self.monsters.append(Monster(100, 100, "Mechanical Insect"))
        elif area == "Torso":
            self.monsters.append(Monster(200, 150, "Steel-Armored Crab"))
        elif area == "Arms":
            self.monsters.append(Monster(300, 300, "Mechanical Insect"))
        elif area == "Legs":
            self.monsters.append(Monster(400, 400, "Steel-Armored Crab"))
        elif area == "Cockpit":
            self.monsters.append(Monster(500, 250, "Data Specter"))
    
    def update(self):
        # 根据不同状态更新游戏逻辑
        if self.state == STATE_PLAYING:
            keys = pygame.key.get_pressed()
            dx, dy = 0, 0
            if keys[pygame.K_LEFT]:
                dx = -1
            if keys[pygame.K_RIGHT]:
                dx = 1
            if keys[pygame.K_UP]:
                dy = -1
            if keys[pygame.K_DOWN]:
                dy = 1
            self.player.move(dx, dy)
            # 更新怪物行为
            for monster in self.monsters:
                monster.update(self.player)
            
            # 简单碰撞检测，触发谜题状态
            for monster in self.monsters:
                if self.player.rect.colliderect(monster.rect):
                    self.state = STATE_PUZZLE
                    self.puzzle = Puzzle(self.game_map.current_area() + " Puzzle")
                    break

        elif self.state == STATE_PUZZLE:
            # 模拟谜题解答：按空格键解谜
            keys = pygame.key.get_pressed()
            if keys[pygame.K_SPACE]:
                self.puzzle.solve()
                self.state = STATE_PLAYING
                # 谜题解决后切换区域并刷新怪物
                if self.puzzle.solved:
                    self.game_map.next_area()
                    self.spawn_monsters()
    
    def draw(self, surface):
        surface.fill((0, 0, 0))
        if self.state == STATE_MENU:
            font = pygame.font.SysFont("Arial", 36)
            text = font.render("Press ENTER to Start", True, (255, 255, 255))
            surface.blit(text, (SCREEN_WIDTH / 2 - text.get_width() / 2, SCREEN_HEIGHT / 2 - text.get_height() / 2))
        elif self.state == STATE_PLAYING:
            self.player.draw(surface)
            for monster in self.monsters:
                monster.draw(surface)
            # 显示当前区域信息
            font = pygame.font.SysFont("Arial", 24)
            area_text = font.render("Area: " + self.game_map.current_area(), True, (255, 255, 255))
            surface.blit(area_text, (10, 10))
        elif self.state == STATE_PUZZLE:
            self.puzzle.draw(surface)

    def run(self):
        running = True
        while running:
            for event in pygame.event.get():
                if event.type == pygame.QUIT:
                    running = False
                # 在菜单状态下按 Enter 键开始游戏
                if self.state == STATE_MENU and event.type == pygame.KEYDOWN:
                    if event.key == pygame.K_RETURN:
                        self.start_game()

            self.update()
            self.draw(screen)
            pygame.display.flip()
            clock.tick(60)
        pygame.quit()
        sys.exit()

if __name__ == "__main__":
    game = Game()
    game.run()
