class HexGrid:
    def __init__(self, size):
        self.grid = {}
        # 生成可旋转的六边形岛屿
        for q in range(-size, size+1):
            r1 = max(-size, -q - size)
            r2 = min(size, -q + size)
            for r in range(r1, r2+1):
                self.grid[(q, r)] = {
                    'type': random.choice(['data_node', 'enemy', 'totem']),
                    'rotation': 0,
                    'entropy': 0
                }

    def rotate_sector(self, center_q, center_r):
        # 六边形区块旋转逻辑
        sectors = {}
        for (q, r) in self.grid:
            dx = q - center_q
            dr = r - center_r
            if abs(dx) + abs(dr) <= 2:  # 3层影响范围
                new_q = int(center_q + (dx * 0.5 - dr))
                new_r = int(center_r + (dx + dr * 0.5))
                sectors[(new_q, new_r)] = self.grid[(q, r)]
        self.grid.update(sectors)