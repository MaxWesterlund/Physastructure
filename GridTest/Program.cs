using Raylib_cs;

Raylib.InitWindow(800, 800, "Grid Test");

while (!Raylib.WindowShouldClose()) {
    Raylib.BeginDrawing();
    
    Raylib.ClearBackground(Color.BLACK);
    Raylib.DrawGrid(1000, 10);

    Raylib.EndDrawing();
}