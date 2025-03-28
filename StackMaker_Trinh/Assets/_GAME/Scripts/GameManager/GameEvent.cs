namespace _GAME.Scripts.GameManager
{
    using System;
    using System.Reflection.Emit;

    public static class GameEvent
    {
        public static Action OnPlayerOutOfBricks;
        public static Action<bool> OnInputPermissionChanged;
    }
}