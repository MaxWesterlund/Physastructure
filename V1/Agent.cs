using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

public class Agent {
    public int XCoord;
    public int YCoord;
    public float Heading;
    public float PheremoneStrength;

    float xPos;
    float yPos;

    public Agent(int xStart, int yStart, float heading) {
        XCoord = xStart;
        YCoord = yStart;
        Heading = heading;

        xPos = xStart;
        yPos = yStart;
    }

    public void Move(CoordData[,] map) {
        xPos = xPos + MathF.Cos(Heading) * Settings.AgentSpeed;
        yPos = yPos + MathF.Sin(Heading) * Settings.AgentSpeed;

        XCoord = (int)MathF.Round(xPos);
        YCoord = (int)MathF.Round(yPos);
        XCoord = Math.Clamp(XCoord, 0, Settings.WIDTH - 1);
        YCoord = Math.Clamp(YCoord, 0, Settings.HEIGHT - 1);

        float left = GetMapValue(map, xPos, yPos, Heading - Settings.AgentSensorAngle, Settings.AgentSensorDistance);
        float center = GetMapValue(map, xPos, yPos, Heading, Settings.AgentSensorDistance);
        float right = GetMapValue(map, xPos, yPos, Heading + Settings.AgentSensorAngle, Settings.AgentSensorDistance);
        
        if (XCoord == 0 || XCoord == Settings.WIDTH - 1 || YCoord == 0 || YCoord == Settings.HEIGHT - 1) {
            Heading += Settings.AgentSensorAngle * new Random().Next(0, 2) == 1 ? 1 : -1;
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

        PheremoneStrength *= Settings.PheremoneDecayRate;
    }

    float GetMapValue(CoordData[,] map, float xOrig, float yOrig, float a, float l) {
        int x = Math.Clamp((int)MathF.Round(xOrig + MathF.Cos(a) * l), 0, Settings.WIDTH - 1);
        int y = Math.Clamp((int)MathF.Round(yOrig + MathF.Sin(a) * l), 0, Settings.HEIGHT - 1);

        float spore = map[x, y].SporeStrength;
        float pheremone = map[x, y].PheremoneStrength;
        int heightDiff = (int)MathF.Abs(map[x, y].Height - map[XCoord, YCoord].Height);

        float value = spore * Settings.SporeWeight + pheremone * Settings.PheremoneWeight - heightDiff * Settings.HeightWeight;

        return value;
    }
}