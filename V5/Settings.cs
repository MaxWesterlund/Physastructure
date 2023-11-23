using Raylib_cs;

namespace Settings {
    class Window {
        public const int Size = 800;
        public const int Resolution = 1;

        public static Color backgroundColor = new Color(0, 0, 0, 255);
    }

    class Simulation {
        public const int Size = 400;

        public const int AgentAmount = 10000;
        public const float AgentTurnAngle = 0.5f;
        public const float AgentSensorDistance = 10;
        public const int AgentSporeWitdh = 1;

        public const float DecayRate = 0.95f;
        public const int PheremoneSpreadRadius = 10;
    }
}