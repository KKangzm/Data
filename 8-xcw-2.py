class QuantumTurnSystem:
    def __init__(self):
        self.neuro_compute = 6  # 初始神经算力
        self.entropy = 0
        
    def player_turn(self, action):
        cost_map = {'play_card':3, 'move':2, 'hack':1}
        if self.neuro_compute >= cost_map[action]:
            self.neuro_compute -= cost_map[action]
            self.entropy += 1 if action == 'hack' else 0
            return True
        return False

    def entropy_check(self):
        if self.entropy > 8:
            # 触发数据风暴事件
            self.shuffle_effects()
            return "DataStorm"
        return None