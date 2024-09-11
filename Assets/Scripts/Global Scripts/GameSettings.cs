using System;

public static class GameSettings
{
    #region Attributes
    private static string towerdefenseBackendPath = "/minigames/towerdefense/api/v1";
    #endregion

    public static string GetTowerdefenseBackendPath()
    {
        return towerdefenseBackendPath;
    }
}