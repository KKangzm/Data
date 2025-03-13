import pygame
import sys

# 初始化 Pygame
pygame.init()

# 屏幕尺寸与帧率设置
SCREEN_WIDTH = 800
SCREEN_HEIGHT = 600
FPS = 60

# 颜色定义
WHITE = (255, 255, 255)
BLACK = (0, 0, 0)

# 创建屏幕
screen = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))
pygame.display.set_caption("岛屿谜踪 - Island Mystery")
clock = pygame.time.Clock()


# 玩家类：负责玩家的移动、跳跃以及物理模拟
class Player(pygame.sprite.Sprite):
    def __init__(self, x, y):
        super(Player, self).__init__()
        self.image = pygame.Surface((40, 60))
        self.image.fill((0, 128, 255))
        self.rect = self.image.get_rect(topleft=(x, y))
        self.vel_y = 0
        self.speed = 5
        self.jump_strength = -15
        self.on_ground = False
        self.double_jump_available = True

    def update(self, platforms):
        keys = pygame.key.get_pressed()
        dx = 0
        dy = 0

        # 水平移动
        if keys[pygame.K_LEFT]:
            dx = -self.speed
        if keys[pygame.K_RIGHT]:
            dx = self.speed

        # 处理跳跃逻辑
        if keys[pygame.K_SPACE]:
            if self.on_ground:
                self.vel_y = self.jump_strength
                self.on_ground = False
            elif self.double_jump_available:
                self.vel_y = self.jump_strength
                self.double_jump_available = False

        # 重力效果
        self.vel_y += 0.8
        if self.vel_y > 10:
            self.vel_y = 10
        dy += self.vel_y

        # 检测与平台的碰撞
        self.on_ground = False
        # 水平碰撞检测
        self.rect.x += dx
        for platform in platforms:
            if self.rect.colliderect(platform.rect):
                if dx > 0:
                    self.rect.right = platform.rect.left
                elif dx < 0:
                    self.rect.left = platform.rect.right

        # 垂直碰撞检测
        self.rect.y += dy
        for platform in platforms:
            if self.rect.colliderect(platform.rect):
                if self.vel_y > 0:
                    self.rect.bottom = platform.rect.top
                    self.vel_y = 0
                    self.on_ground = True
                    self.double_jump_available = True
                elif self.vel_y < 0:
                    self.rect.top = platform.rect.bottom
                    self.vel_y = 0

# 平台类：用于构建可供跳跃的平台
class Platform(pygame.sprite.Sprite):
    def __init__(self, x, y, width, height):
        super(Platform, self).__init__()
        self.image = pygame.Surface((width, height))
        self.image.fill((100, 100, 100))
        self.rect = self.image.get_rect(topleft=(x, y))

# 谜题元素类：代表场景中可交互的机关或道具
class PuzzleElement(pygame.sprite.Sprite):
    def __init__(self, x, y, width, height, puzzle_id):
        super(PuzzleElement, self).__init__()
        self.image = pygame.Surface((width, height))
        self.image.fill((255, 200, 0))
        self.rect = self.image.get_rect(topleft=(x, y))
        self.puzzle_id = puzzle_id
        self.activated = False

    def activate(self):
        self.activated = True
        self.image.fill((0, 255, 0))
        print(f"Puzzle {self.puzzle_id} activated!")

# 关卡类：管理不同区域的场景数据与谜题机关
class Level:
    def __init__(self, level_id):
        self.level_id = level_id
        self.platforms = pygame.sprite.Group()
        self.puzzle_elements = pygame.sprite.Group()
        self.create_level()

    def create_level(self):
        if self.level_id == 1:
            # 海滩区：基础平台与简单机关
            self.platforms.add(Platform(0, SCREEN_HEIGHT - 50, SCREEN_WIDTH, 50))
            self.platforms.add(Platform(200, SCREEN_HEIGHT - 150, 100, 20))
            self.platforms.add(Platform(400, SCREEN_HEIGHT - 250, 150, 20))
            self.puzzle_elements.add(PuzzleElement(220, SCREEN_HEIGHT - 190, 30, 30, puzzle_id=1))
        elif self.level_id == 2:
            # 热带丛林：增加难度与更多跳跃平台
            self.platforms.add(Platform(0, SCREEN_HEIGHT - 50, SCREEN_WIDTH, 50))
            self.platforms.add(Platform(150, SCREEN_HEIGHT - 120, 120, 20))
            self.platforms.add(Platform(350, SCREEN_HEIGHT - 200, 100, 20))
            self.platforms.add(Platform(550, SCREEN_HEIGHT - 300, 100, 20))
            self.puzzle_elements.add(PuzzleElement(370, SCREEN_HEIGHT - 240, 30, 30, puzzle_id=2))
        elif self.level_id == 3:
            # 古遗迹：更复杂的布局和机关谜题
            self.platforms.add(Platform(0, SCREEN_HEIGHT - 50, SCREEN_WIDTH, 50))
            self.platforms.add(Platform(100, SCREEN_HEIGHT - 100, 150, 20))
            self.platforms.add(Platform(300, SCREEN_HEIGHT - 150, 150, 20))
            self.platforms.add(Platform(500, SCREEN_HEIGHT - 200, 150, 20))
            self.puzzle_elements.add(PuzzleElement(320, SCREEN_HEIGHT - 190, 30, 30, puzzle_id=3))
        elif self.level_id == 4:
            # 火山内穴：动态平台与时间限制挑战
            self.platforms.add(Platform(0, SCREEN_HEIGHT - 50, SCREEN_WIDTH, 50))
            self.platforms.add(Platform(200, SCREEN_HEIGHT - 120, 100, 20))
            self.platforms.add(Platform(400, SCREEN_HEIGHT - 180, 100, 20))
            self.platforms.add(Platform(600, SCREEN_HEIGHT - 240, 100, 20))
            self.puzzle_elements.add(PuzzleElement(620, SCREEN_HEIGHT - 280, 30, 30, puzzle_id=4))

    def draw(self, surface):
        self.platforms.draw(surface)
        self.puzzle_elements.draw(surface)

# 游戏主类：负责整体流程管理与关卡切换
class Game:
    def __init__(self):
        self.player = Player(50, SCREEN_HEIGHT - 100)
        self.current_level_id = 1
        self.level = Level(self.current_level_id)
        # 用于统一管理所有精灵
        self.all_sprites = pygame.sprite.Group()
        self.all_sprites.add(self.player)
        self.all_sprites.add(self.level.platforms)
        self.all_sprites.add(self.level.puzzle_elements)

    def run(self):
        running = True
        while running:
            clock.tick(FPS)
            for event in pygame.event.get():
                if event.type == pygame.QUIT:
                    running = False

            # 更新玩家状态
            self.player.update(self.level.platforms)

            # 检测玩家与谜题元素的交互
            for puzzle in self.level.puzzle_elements:
                if self.player.rect.colliderect(puzzle.rect) and not puzzle.activated:
                    puzzle.activate()

            # 绘制场景
            screen.fill(WHITE)
            self.level.draw(screen)
            screen.blit(self.player.image, self.player.rect)
            pygame.display.flip()

            # 示例：玩家跑到屏幕右侧时切换到下一关
            if self.player.rect.x > SCREEN_WIDTH:
                self.current_level_id += 1
                if self.current_level_id > 4:
                    print("Game Completed!")
                    running = False
                else:
                    # 重置玩家位置，并加载新关卡
                    self.player.rect.topleft = (50, SCREEN_HEIGHT - 100)
                    self.level = Level(self.current_level_id)
                    self.all_sprites.empty()
                    self.all_sprites.add(self.player)
                    self.all_sprites.add(self.level.platforms)
                    self.all_sprites.add(self.level.puzzle_elements)

        pygame.quit()
        sys.exit()


if __name__ == '__main__':
    game = Game()
    game.run()
