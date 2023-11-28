using System.Numerics;
using Settings;

public class Agent {
    Vector2 position;
    Vector2 direction;
    float heading;

    public Agent(int x, int y, float head) {
        position = new Vector2(x, y);
        heading = head;
        direction = DirectionFromAngle(heading);
    }

    public void Move(float[,] decayMap, bool[,] pointMap) {
        Sensor[] sensors = new Sensor[3];
        for (int i = 0; i < sensors.Length; i++) {
            float angle = heading - Simulation.AgentTurnAngle + i * Simulation.AgentTurnAngle;
            Vector2 p = position + Simulation.AgentSensorDistance * DirectionFromAngle(angle);
            int x = Math.Clamp((int)Math.Round(p.X), 0, Simulation.Size - 1);
            int y = Math.Clamp((int)Math.Round(p.Y), 0, Simulation.Size - 1);

            float value = pointMap[x, y] ? 2 : decayMap[x, y];

            Sensor sensor = new Sensor(
                angle,
                value
            );

            sensors[i] = sensor;
        }

        Sensor bestChoice = sensors[1];
        for (int i = 0; i < sensors.Length; i++) {
            Sensor sensor = sensors[i];
            if (sensor.Pheremone > bestChoice.Pheremone) {
                bestChoice = sensor;
            }
        }
        if (bestChoice != null) {
            heading = bestChoice.Angle;
            direction = DirectionFromAngle(heading);
        }

        if ((int)Math.Round(position.X + direction.X) <= 0 || (int)Math.Round(position.X + direction.X) >= Simulation.Size - 1) {
            direction.X *= -1;
            heading = MathF.Atan2(direction.Y, direction.X);
        }
        if ((int)Math.Round(position.Y + direction.Y) <= 0 || (int)Math.Round(position.Y + direction.Y) >= Simulation.Size - 1) {
            direction.Y *= -1;
            heading = MathF.Atan2(direction.Y, direction.X);
        }
        
        Vector2 wantedPos = position + direction;
        
        position = wantedPos;
    }

    public void LeaveSpore(float[,] decayMap) {
        decayMap[Math.Clamp((int)Math.Round(position.X), 0, Simulation.Size - 1), Math.Clamp((int)Math.Round(position.Y), 0, Simulation.Size - 1)] = 1;
    }

    Vector2 DirectionFromAngle(float a) {
        return new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
    }

    class Sensor {
        public float Angle;
        public float Pheremone;

        public Sensor(float angle, float pheremone) {
            Angle = angle;
            Pheremone = pheremone;
        }
    }
}