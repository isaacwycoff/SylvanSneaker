using Microsoft.Xna.Framework;
using SylvanSneaker.Input;

namespace SylvanSneaker.Slots
{
    public static class UserInputSlot
    {
        private static InputManager Manager;

        public static void Initialize(InputManager manager)
        {
            Manager = manager;
        }

        public static void Shutdown()
        {

        }

        public static void UpdateGame(GameTime gameTime)
        {
            Manager.UpdateGame(gameTime);
        }
    }
}
