using System.Collections.Generic;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Items.Wired;
using Rocket.Communication.Packets.Outgoing.Rooms.Furni.Wired;



namespace Rocket.HabboHotel.Items.Interactor
{
    public class InteractorWired : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            bool flag = Session == null || Item == null;
            if (!flag)
            {
                bool flag2 = !HasRights;
                if (!flag2)
                {
                    IWiredItem wiredItem = null;
                    bool flag3 = !Item.GetRoom().GetWired().TryGet(Item.Id, out wiredItem);
                    if (!flag3)
                    {
                        Item.ExtraData = "1";
                        Item.UpdateState(false, true);
                        Item.RequestUpdate(2, true);
                        bool flag4 = Item.GetBaseItem().WiredType == WiredBoxType.AddonRandomEffect;
                        if (!flag4)
                        {
                            bool flag5 = Item.GetRoom().GetWired().IsTrigger(Item);
                            if (flag5)
                            {
                                List<int> blockedItems = WiredBoxTypeUtility.ContainsBlockedEffect(wiredItem, Item.GetRoom().GetWired().GetEffects(wiredItem));
                                Session.SendMessage(new WiredTriggerConfigComposer(wiredItem, blockedItems));
                            }
                            else
                            {
                                bool flag6 = Item.GetRoom().GetWired().IsEffect(Item);
                                if (flag6)
                                {
                                    List<int> blockedItems2 = WiredBoxTypeUtility.ContainsBlockedTrigger(wiredItem, Item.GetRoom().GetWired().GetTriggers(wiredItem));
                                    Session.SendMessage(new WiredEffectConfigComposer(wiredItem, blockedItems2));
                                }
                                else
                                {
                                    bool flag7 = Item.GetRoom().GetWired().IsCondition(Item);
                                    if (flag7)
                                    {
                                        Session.SendMessage(new WiredConditionConfigComposer(wiredItem));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnWiredTrigger(Item Item)
        {
        }
    }
}
