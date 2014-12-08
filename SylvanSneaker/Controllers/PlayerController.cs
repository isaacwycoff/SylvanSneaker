using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker
{
    class PlayerController: Controller
    {
        public Entity ControlledEntity { get; set; }

        public PlayerController() { }

        public void SendCommand(EntityCommand command)
        {
            ControlledEntity.SendCommand(command);
        }

    }
}
