class EntropyEventManager:
    def __init__(self):
        self.event_stack = [
            (5, 'drone_drop'),
            (8, 'data_storm'), 
            (12, 'ancient_awake')
        ]
        
    def check_events(self, current_entropy):
        triggered = []
        for threshold, event in self.event_stack:
            if current_entropy >= threshold:
                triggered.append(event)
                self.event_stack.remove((threshold, event))
        return triggered