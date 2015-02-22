using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Input
{
    [Flags]
    public enum GameCommand
    {
        

    }

    public class InputManager : Controller
    {
        // For right now we're going to send EntityCommands around
        // This is NOT a complete solution, since there are commands
        // that don't have to do with moving an Entity around
        Dictionary<Keys, EntityCommand> KeyLookup;
        Dictionary<Buttons, EntityCommand> ButtonLookup;
        public Entity ControlledEntity { get; set; }

        public InputManager ()
	    {
            KeyLookup = new Dictionary<Keys, EntityCommand>() {
                { Keys.Down, EntityCommand.MoveSouth },
                { Keys.Right, EntityCommand.MoveEast },
                { Keys.Left, EntityCommand.MoveWest },
                { Keys.Up, EntityCommand.MoveNorth },
                { Keys.Space, EntityCommand.AttackPrimary },
            };
	    }

        public void SetControlledEntity (Entity entity)
        {
            ControlledEntity = entity;
        }

        public void UpdateGame(GameTime gameTime)
        {
            EntityCommand command = 0;

            var keys = Keyboard.GetState().GetPressedKeys();

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                command = command | EntityCommand.MoveSouth;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                command = command | EntityCommand.MoveEast;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                command = command | EntityCommand.MoveWest;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                command = command | EntityCommand.MoveNorth;
            }

            SendCommand(command);            
        }

        private void SendCommand(EntityCommand command)
        {
            if(ControlledEntity != null)
            {
                ControlledEntity.SendCommand(command);
            }
        }
    }
}
