class DataCenterBoss:
    def __init__(self):
        self.discard_pile = []
        
    def protocol_field(self, player_action):
        # 吞噬弃牌堆生成技能
        if player_action['type'] == 'discard':
            self.discard_pile.append(player_action['card'])
            
            if len(self.discard_pile) >= 3:
                combined_skill = self.create_skill()
                return combined_skill
        return None

    def create_skill(self):
        # 组合弃牌生成新技能
        types = [card.type[:3] for card in self.discard_pile]
        return SkillMatrix[''.join(types)]