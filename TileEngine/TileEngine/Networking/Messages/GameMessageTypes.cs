namespace TileEngine.Networking.Messages
{
    /// <summary>
    /// The game message types.
    /// </summary>
    public enum GameMessageTypes
    {
        RequestBannerState,

        AddBannerState,

        ColorSwitchState,

        PlayerBannerState,

        PlayerDisconnectedState,

        SetTitleState,

        InGameState,

        UpdateEntityState,

        RequestTileState,

        SnapTileState,

        ReleaseTileState,
     
        GetEntityState,

        RemoveEntityState,

        RotateEntityState,

        UpdateScoreState,

        AddPointingAnimationState
    }
}