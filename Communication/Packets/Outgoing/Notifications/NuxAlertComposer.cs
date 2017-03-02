/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Rooms.Notifications
{
    class NuxAlertComposer : ServerPacket
    {
        public NuxAlertComposer(int bubble = 0)
            : base(ServerPacketHeader.NuxAlertMessageComposer)
        {
            switch (bubble)
            {
                case 0:
                    {
                        base.WriteString("helpBubble/add/BOTTOM_BAR_INVENTORY/Este es el inventario. Para colocar tus furnis, tan sólo tienes que arrastrarlos hasta el suelo.");
                        break;
                    }
                case 1:
                    {
                        base.WriteString("helpBubble/add/BOTTOM_BAR_NAVIGATOR/Este es el navegador. ¡Úsalo para explorar las miles de salas que hay en el hotel!");
                        break;
                    }
                case 2:
                    {
                        base.WriteString("helpBubble/add/chat_input/Puedes chatear con tus amigos escribiendo aquí.");
                        break;
                    }
                case 3:
                    {
                        base.WriteString("habbopages/welcome_message.txt");
                        break;
                    }
                case 4:
                    {
                        base.WriteString("habbopages/commands.txt");
                        break;
                    }
            }
        }
    }
}
*/