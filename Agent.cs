using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

public class Agent {
    public int XCoord;
    public int YCoord;
    public float Heading;

    float xPos;
    float yPos;

    public Agent(int xStart, int yStart, float heading) {
        XCoord = xStart;
        YCoord = yStart;
        Heading = heading;

        xPos = xStart;
        yPos = yStart;
    }

    public void Move(float[,] decayMap) {
        xPos = xPos + MathF.Cos(Heading) * Settings.AgentSpeed;
        yPos = yPos + MathF.Sin(Heading) * Settings.AgentSpeed;

        float left = GetMapValue(decayMap, xPos, yPos, Heading - Settings.AgentSensorAngle, Settings.AgentSensorDistance);
        float center = GetMapValue(decayMap, xPos, yPos, Heading, Settings.AgentSensorDistance);
        float right = GetMapValue(decayMap, xPos, yPos, Heading + Settings.AgentSensorAngle, Settings.AgentSensorDistance);

        XCoord = (int)MathF.Round(xPos);
        YCoord = (int)MathF.Round(yPos);
        XCoord = Math.Clamp(XCoord, 0, Settings.WIDTH - 1);
        YCoord = Math.Clamp(YCoord, 0, Settings.HEIGHT - 1);

        if (XCoord == 0 || XCoord == Settings.WIDTH - 1 || YCoord == 0 || YCoord == Settings.HEIGHT - 1) {
            Heading += Settings.AgentSensorAngle;
        }
        else if (center == left && left == right) {
            Heading += Settings.AgentSensorAngle * new Random().Next(-1, 2);
        }
        else if (center < left && center < right) {
            if (left == right) {
                Heading += Settings.AgentSensorAngle * new Random().Next(0, 2) == 1 ? 1 : -1;
            }
            else if (left > right) {
                Heading -= Settings.AgentSensorAngle;
            }
            else {
                Heading += Settings.AgentSensorAngle;
            }
        }
    }

    float GetMapValue(float[,] map, float xOrig, float yOrig, float a, float l) {
        int x = Math.Clamp((int)MathF.Round(xOrig + MathF.Cos(a) * l), 0, Settings.WIDTH - 1);
        int y = Math.Clamp((int)MathF.Round(yOrig + MathF.Sin(a) * l), 0, Settings.HEIGHT - 1);

        float value = map[x, y];
        
        return value;
    }
}