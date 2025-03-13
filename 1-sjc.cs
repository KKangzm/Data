using System;

namespace GameSystems
{
    public class SurvivalSystem
    {
        private float bodyTemperature;
        private int infectionLevel;
        private int mentalState;

        public SurvivalSystem(float initialBodyTemperature = 37.0f, int initialInfectionLevel = 0, int initialMentalState = 100)
        {
            bodyTemperature = initialBodyTemperature;
            infectionLevel = initialInfectionLevel;
            mentalState = initialMentalState;
        }

        public void UpdateBodyTemperature(float change)
        {
            bodyTemperature += change;
            Console.WriteLine($"体温更新为: {bodyTemperature}°C");
        }

        public void UpdateInfectionLevel(int change)
        {
            infectionLevel += change;
            Console.WriteLine($"感染等级更新为: {infectionLevel}");
        }

        public void UpdateMentalState(int change)
        {
            mentalState += change;
            Console.WriteLine($"精神状态更新为: {mentalState}");
        }
    }

    public class EnvironmentPuzzle
    {
        private bool isSolved;

        public EnvironmentPuzzle(bool solved = false)
        {
            isSolved = solved;
        }

        public void SolvePuzzle()
        {
            isSolved = true;
            Console.WriteLine("环境谜题已解决！");
        }

        public bool CheckIfSolved()
        {
            return isSolved;
        }
    }

    public class DynamicEnvironment
    {
        private string currentCondition;

        public DynamicEnvironment(string initialCondition = "晴朗")
        {
            currentCondition = initialCondition;
        }

        public void ChangeCondition(string newCondition)
        {
            currentCondition = newCondition;
            Console.WriteLine($"环境条件变更为: {currentCondition}");
        }

        public string GetCurrentCondition()
        {
            return currentCondition;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var survival = new SurvivalSystem();
            survival.UpdateBodyTemperature(0.5f);
            survival.UpdateInfectionLevel(-10);
            survival.UpdateMentalState(20);

            var puzzle = new EnvironmentPuzzle();
            puzzle.SolvePuzzle();

            var environment = new DynamicEnvironment();
            environment.ChangeCondition("暴风雨");

            // 检查谜题是否解决
            if (puzzle.CheckIfSolved())
            {
                Console.WriteLine("恭喜，你解决了谜题！");
            }

            Console.WriteLine("当前环境条件是: " + environment.GetCurrentCondition());
        }
    }
}
