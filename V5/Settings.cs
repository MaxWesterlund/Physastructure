using Raylib_cs;

namespace Settings {
    class Window {
        public const int Size = 800;
        public const int Resolution = 4;

        public static Color backgroundColor = new Color(0, 0, 0, 255);
    }

    class Simulation {
        public const int Size = 400;

        public const int AgentAmount = 100;

        public const float DecayRate = 0.999f;
        public const int PheremoneSpreadRadius = 5;
    }
}