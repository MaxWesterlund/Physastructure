public struct CoordinateData {
    public float Strength;
    public bool IsPoint;

    public CoordinateData() {
        Strength = 0;
        IsPoint = false;
    }

    public float Evaluate() {
        float value = IsPoint ? 2 : Strength;
        return value;
    }
}