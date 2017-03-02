using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Offer
{
    class TargetOfferMessageComposer : ServerPacket
    {
        public TargetOfferMessageComposer() : base(ServerPacketHeader.TargetOfferMessageComposer)
        {
            base.WriteInteger(1);
            base.WriteInteger(190);
            base.WriteString("bf16_tko_gr1");
            base.WriteString("bf16_tko1");
            base.WriteInteger(105); //Credits
            base.WriteInteger(105); //Diamonds
            base.WriteInteger(5);
            base.WriteInteger(2);
            base.WriteInteger(RocketEmulador.Oferta); //3 Days ... time in seconds
            base.WriteString(RocketEmulador.RocketData().data["oferta.title"]); //Title
            base.WriteString(RocketEmulador.RocketData().data["oferta.desc"]); //Description
            base.WriteString(RocketEmulador.RocketData().data["oferta.image"]); //Image Large
            base.WriteString(RocketEmulador.RocketData().data["oferta.image1"]); //Image on Close Notification
            base.WriteInteger(1);
            base.WriteInteger(15);
            base.WriteString("wf_act_mute_triggerer"); //1 Month HC
            base.WriteString("wf_xtra_random"); //Snack
            base.WriteString("wf_act_leave_team"); //10 Credits 
            base.WriteString("wf_trg_game_ends"); //Roof Building
            base.WriteString("wf_trg_game_ends"); //Snack
            base.WriteString("wf_trg_game_ends"); //10 Credits
            base.WriteString("wf_trg_game_ends"); //10 Credits
            base.WriteString("wf_trg_game_ends"); //Building 1
            base.WriteString("wf_trg_game_ends"); //Building 2
            base.WriteString("wf_trg_game_ends"); //10 Credits
            base.WriteString("wf_trg_game_ends"); //Clothes Scarf
            base.WriteString("wf_trg_game_ends"); //10 Credits
            base.WriteString("wf_trg_game_ends"); //10 Credits
            base.WriteString("wf_trg_game_ends"); //
            base.WriteString("wf_trg_game_ends"); //10 Credits 
        }
    }
}