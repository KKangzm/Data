// 简化流体动力学算法
function simulateWaterFlow(grid, deltaTime):
    for each cell in grid:
        if cell.isWaterSource:
            cell.pressure = 1.0
        else:
            cell.pressure = 0.0

    for 3 iterations:
        for each cell in grid:
            avgPressure = average(neighborPressures)
            cell.pressure = lerp(cell.pressure, avgPressure, 0.2)

    for each cell in grid:
        flowDirection = calculateGradient(cell)
        cell.velocity = flowDirection * cell.pressure * deltaTime

// 重量感应系统
class PressurePlateSensor:
    def __init__(self, threshold):
        self.current_mass = 0
        self.activation_threshold = threshold
        
    def on_collision(obj):
        self.current_mass += obj.GetComponent<Rigidbody>().mass
        if self.current_mass >= self.activation_threshold:
            trigger_mechanism()