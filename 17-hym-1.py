import pygame
import sys

# 初始化 Pygame
pygame.init()

# 屏幕设置
WIDTH, HEIGHT = 800, 600
screen = pygame.display.set_mode((WIDTH, HEIGHT))
pygame.display.set_caption("Two Player Adventure")
clock = pygame.time.Clock()

# 颜色定义
WHITE = (255, 255, 255)
BLACK = (0, 0, 0)
RED = (255, 0, 0)
BLUE = (0, 0, 255)

# 角色类
class Player:
    def __init__(self, x, y, color, controls):
        self.rect = pygame.Rect(x, y, 40, 60)
        self.color = color
        self.speed = 5
        self.controls = controls
        self.inventory = []

    def move(self, keys):
        if keys[self.controls['left']]:
            self.rect.x -= self.speed
        if keys[self.controls['right']]:
            self.rect.x += self.speed
        if keys[self.controls['up']]:
            self.rect.y -= self.speed
        if keys[self.controls['down']]:
            self.rect.y += self.speed
    
    def draw(self, screen):
        pygame.draw.rect(screen, self.color, self.rect)

# 定义玩家
player1_controls = {'left': pygame.K_a, 'right': pygame.K_d, 'up': pygame.K_w, 'down': pygame.K_s}
player2_controls = {'left': pygame.K_LEFT, 'right': pygame.K_RIGHT, 'up': pygame.K_UP, 'down': pygame.K_DOWN}
player1 = Player(100, 500, RED, player1_controls)
player2 = Player(600, 500, BLUE, player2_controls)

# 物品类
class Item:
    def __init__(self, x, y, name):
        self.rect = pygame.Rect(x, y, 30, 30)
        self.name = name

    def draw(self, screen):
        pygame.draw.rect(screen, (0, 255, 0), self.rect)

# 示例物品
items = [Item(300, 400, "Key"), Item(500, 300, "Potion")]

# 游戏循环
running = True
while running:
    screen.fill(WHITE)
    keys = pygame.key.get_pressed()
    
    # 事件监听
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False
    
    # 玩家移动
    player1.move(keys)
    player2.move(keys)
    
    # 绘制
    player1.draw(screen)
    player2.draw(screen)
    
    for item in items:
        item.draw(screen)
    
    pygame.display.flip()
    clock.tick(30)

pygame.quit()
sys.exit()
