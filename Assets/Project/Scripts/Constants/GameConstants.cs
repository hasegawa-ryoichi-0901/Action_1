using System;

public class GameConstants {
    public const int LoadingFPS = 10;
    public const int FPS = 60;

    public const string SceneReload = "Project/Scenes/Reload";
    public const string SceneMain = "Project/Scenes/Main";

    public const string yyMMddHHmmss = "yyMMddHHmmss";
    public const string yyyyMMddHHmmss = "yyyyMMddHHmmss";
    public static TimeSpan AutoSaveInterval = TimeSpan.FromSeconds(600);
    public static TimeSpan SteamInitializeWaitTime = TimeSpan.FromSeconds(1);
    public static TimeSpan InitializeFitTime = TimeSpan.FromSeconds(1.0f);

    public enum CanvasLayer {
        HudLayer = 100, // インスペクターで設定
        Popup = 101,
        Tutorial = 110,
    }
}
