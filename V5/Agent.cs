using System.Numerics;
using Settings;

public class Agent {
    Vector2 position;
    Vector2 direction;
    float heading;

    public Agent(int x, int y, float head) {
        position = new Vector2(x, y);
        heading = head;
    }

    public void Move(float[,] decayMap) {
        Sensor[] sensors = new Sensor[3];
        for (int i = 0; i < sensors.Length; i++) {
            float angle = heading - Simulation.AgentTurnAngle + i * Simulation.AgentTurnAngle;
            Vector2 p = Simulation.AgentSensorDistance * DirectionFromAngle(angle);
            int x = Math.Clamp((int)p.X, 0, Simulation.Size - 1);
            int y = Math.Clamp((int)p.Y, 0, Simulation.Size - 1);
            Sensor sensor = new Sensor(
                angle,
                decayMap[x, y]
            );

            sensors[i] = sensor;
        }

        Sensor bestChoice = sensors[0];
        for (int i = 1; i < sensors.Length; i++) {
            Sensor sensor = sensors[i];
            if (sensor.Pheremone > bestChoice.Pheremone) {
                bestChoice = sensor;
            }
        }
        heading = bestChoice.Angle;
        direction = DirectionFromAngle(heading);

        if (position.X < 0 || position.X >= Simulation.Size - 1) {
            direction.Y *= -1;
        }
        if (position.Y < 0 || position.Y >= Simulation.Size - 1) {
            direction.X *= -1;
        }
        
        Vector2 wantedPos = position + direction;
        
        position = wantedPos;
    }

    public void LeaveSpore(float[,] decayMap) {
        // for (int y = -Simulation.AgentSporeWitdh; y < Simulation.AgentSporeWitdh; y++) {
        //     for (int x = -Simulation.AgentSporeWitdh; x < Simulation.AgentSporeWitdh; x++) {
        //         Vector2 coordVector = new Vector2(position.X + x, position.Y + y);
        //         if (Vector2.Distance(coordVector, position) <= Simulation.AgentSporeWitdh) {
        //             decayMap[Math.Clamp((int)coordVector.X, 0, Simulation.Size - 1), Math.Clamp((int)coordVector.Y, 0, Simulation.Size - 1)] = 1;
        //         }
        //     }
        // }
        decayMap[Math.Clamp((int)position.X, 0, Simulation.Size - 1), Math.Clamp((int)position.Y, 0, Simulation.Size - 1)] = 1;
    }

    Vector2 DirectionFromAngle(float a) {
        return new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
    }

    struct Sensor {
        public float Angle;
        public float Pheremone;

        public Sensor(float angle, float pheremone) {
            Angle = angle;
            Pheremone = pheremone;
        }
    }
}