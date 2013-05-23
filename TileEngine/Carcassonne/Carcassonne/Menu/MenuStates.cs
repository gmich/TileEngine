using System;

namespace Carcassonne.Menu
{
    public enum MenuStates
    {
        //Waiting for selection
        IDLE,

        MainMenu,
        Profile,
            UseSteamAPI,
            ProfileNameUpdated,
        Play,
            CreateLobby,
                    ExpansionLobby,
                    Start,
                    Color,
            JoinLobby,
                    IPUpdated,
                Ready,
        About,
        Disconnected,
        Exit
    }
}
