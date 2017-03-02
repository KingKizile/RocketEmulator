using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plus.Communication.Packets.Outgoing.Inventory.Purse
{
    class HabboClubCenterInfoMessageComposer : ServerPacket
    {
        public HabboClubCenterInfoMessageComposer() : base(ServerPacketHeader.HabboClubCenterInfoMessageComposer)
        {
            base.WriteInteger(5);//streakduration in days 
            base.WriteString("01-01-1970");//joindate 
            base.WriteInteger(0); base.WriteInteger(0);//this should be a double 
            base.WriteInteger(0);//unused 
            base.WriteInteger(0);//unused 
            base.WriteInteger(10);//spentcredits 
            base.WriteInteger(20);//streakbonus 
            base.WriteInteger(10);//spentcredits 
            base.WriteInteger(60);//next pay in minutes  
        }
    }
}