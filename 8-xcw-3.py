class CyberCard:
    def __init__(self, base_type):
        self.base = base_type  # 'corals' | 'corporate' | 'chaos'
        self.evolution = None
        self.effects = CARDS_DATA[base_type]
        
    def mutate(self, element):
        # 元素融合变异逻辑
        evolution_map = {
            ('corals', 'coolant'): 'acid_coral',
            ('corporate', 'data_storm'): 'hacked_drone',
            ('chaos', 'memory_fragment'): 'soul_override'
        }
        if (self.base, element) in evolution_map:
            self.evolution = evolution_map[(self.base, element)]
            self.update_effects()

    def entropy_effect(self, entropy_level):
        # 熵值影响卡牌效果
        if self.base == 'chaos':
            self.effects['power'] *= (1 + entropy_level*0.2)