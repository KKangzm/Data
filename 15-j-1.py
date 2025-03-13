import pygame
import sys

# 初始化 Pygame
pygame.init()

# 设置窗口尺寸与标题
WIDTH, HEIGHT = 800, 600
screen = pygame.display.set_mode((WIDTH, HEIGHT))
pygame.display.set_caption("Mystic Island Expedition")
clock = pygame.time.Clock()

# =====================
# 定义主要游戏角色和对象
# =====================

class Player:
    def __init__(self, x, y):
        self.image = pygame.Surface((50, 50))
        self.image.fill((255, 0, 0))  # 用红色方块表示主角，实际使用角色立绘
        self.rect = self.image.get_rect(topleft=(x, y))
        self.speed = 5
        self.inventory = []  # 简单的物品背包

    def handle_input(self):
        keys = pygame.key.get_pressed()
        if keys[pygame.K_LEFT]:
            self.rect.x -= self.speed
        if keys[pygame.K_RIGHT]:
            self.rect.x += self.speed
        if keys[pygame.K_UP]:
            self.rect.y -= self.speed
        if keys[pygame.K_DOWN]:
            self.rect.y += self.speed

    def update(self):
        self.handle_input()

    def draw(self, surface):
        surface.blit(self.image, self.rect.topleft)

class PuzzleObject:
    def __init__(self, x, y):
        self.rect = pygame.Rect(x, y, 50, 50)  # 谜题机关的位置与尺寸
        self.active = True  # 标记机关是否激活

    def trigger(self):
        print("Puzzle triggered!")
        # 触发谜题逻辑，这里只是示例输出，实际可调用谜题界面等
        self.active = False

    def draw(self, surface):
        if self.active:
            pygame.draw.rect(surface, (0, 255, 0), self.rect)  # 以绿色方块展示机关

# =====================
# 定义场景管理：游戏主场景与谜题场景
# =====================

class GameScene:
    def __init__(self):
        self.player = Player(100, 100)
        self.puzzle_objects = [PuzzleObject(400, 300)]
        self.background = pygame.Surface((WIDTH, HEIGHT))
        self.background.fill((100, 150, 200))  # 模拟海岛背景

    def process_events(self, events):
        for event in events:
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_i:
                    print("打开物品栏")  # 示例：按 I 键打开物品栏，可扩展为实际 UI

    def update(self):
        self.player.update()
        # 检测角色与谜题机关的碰撞
        for puzzle in self.puzzle_objects:
            if puzzle.active and self.player.rect.colliderect(puzzle.rect):
                puzzle.trigger()
                # 此处可以切换到谜题场景
                return "puzzle"
        return "game"

    def draw(self, surface):
        surface.blit(self.background, (0, 0))
        self.player.draw(surface)
        for puzzle in self.puzzle_objects:
            puzzle.draw(surface)

class PuzzleScene:
    def __init__(self):
        self.font = pygame.font.SysFont(None, 36)
        self.message = "Solve the puzzle: Press Y for success, N for failure"
        self.solved = False
        self.failed = False

    def process_events(self, events):
        for event in events:
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_y:
                    print("Puzzle Solved!")
                    self.solved = True
                elif event.key == pygame.K_n:
                    print("Puzzle Failed!")
                    self.failed = True

    def update(self):
        pass

    def draw(self, surface):
        # 绘制简单的谜题界面
        surface.fill((50, 50, 50))
        text = self.font.render(self.message, True, (255, 255, 255))
        surface.blit(text, (50, 50))

# =====================
# 主循环与场景切换逻辑
# =====================

def main():
    # 初始场景为游戏主场景
    current_scene = "game"
    game_scene = GameScene()
    puzzle_scene = None

    running = True
    while running:
        events = pygame.event.get()
        for event in events:
            if event.type == pygame.QUIT:
                running = False

        if current_scene == "game":
            game_scene.process_events(events)
            scene_result = game_scene.update()
            # 如果触发谜题机关则切换到谜题场景
            if scene_result == "puzzle":
                puzzle_scene = PuzzleScene()
                current_scene = "puzzle"
            game_scene.draw(screen)
        elif current_scene == "puzzle":
            puzzle_scene.process_events(events)
            puzzle_scene.update()
            puzzle_scene.draw(screen)
            # 若谜题解决，则返回主游戏场景；否则可设计失败惩罚
            if puzzle_scene.solved:
                current_scene = "game"
                # 此处可移除已解谜的机关或更新背包道具等
            elif puzzle_scene.failed:
                current_scene = "game"
                # 根据失败结果做相应处理，如减少生命值、重置谜题状态等

        pygame.display.flip()
        clock.tick(60)  # 60 FPS

    pygame.quit()
    sys.exit()

if __name__ == "__main__":
    main()
