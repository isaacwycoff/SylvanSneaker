using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker
{
    class PlayerController: Controller
    {
        private Entity ControlledEntity { get; set; }

        public PlayerController()
        {

        }

        /*
        public PlayerController(Entity entity)
        {
            this.ControlledEntity = entity;           
        }
        */

        public void SendCommand(EntityCommand command)
        {
            ControlledEntity.SendCommand(command);
        }

    }
}
