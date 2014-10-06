using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker
{
    class PlayerController: Controller
    {

        public PlayerController(Entity entity)
        {
            this.ControlledEntity = entity;           
        }

        public void SendCommand(EntityCommand command)
        {
            ControlledEntity.SendCommand(command);
        }

        private Entity ControlledEntity { get; set; }
    }
}
