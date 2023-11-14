using Raylib_cs;
using Settings;

static class Program {
    static void Main() {
        Raylib.InitWindow(Window.Size, Window.Size, "Physastructure");
        while (!Raylib.WindowShouldClose()) {
            Step();
            Draw();
        }
        Raylib.CloseWindow();
    }

    static void Step() {

    }

    static void Draw() {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Window.backgroundColor);
        Raylib.EndDrawing();
    }
}