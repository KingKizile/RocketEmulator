using System.Linq;
using System.Collections.Generic;

using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Incoming;
using Rocket.Communication.Packets.Outgoing.Inventory.Purse;
using Rocket.Communication.Packets.Outgoing.Quests;

using Rocket.Database.Interfaces;
using log4net;
using Rocket.HabboHotel.Items;

namespace Rocket.HabboHotel.Camera
{
    public class CameraPhotoManager
    {
        private static readonly ILog log = LogManager.GetLogger("Plus.HabboHotel.Camera.CameraPhotoManager");

        private Dictionary<int, CameraPhotoPreview> _previews;

        private string _previewPath = "preview/{1}-{0}.png";
        private string _purchasedPath = "purchased/{1}-{0}.png";
        private int _maxPreviewCacheCount = 1000;

        private int _purchaseCoinsPrice = 999;
        private int _purchaseDucketsPrice = 999;
        private int _publishDucketsPrice = 999;

        private ItemData _photoPoster;

        public int PurchaseCoinsPrice
        {
            get
            {
                return this._purchaseCoinsPrice;
            }
        }

        public int PurchaseDucketsPrice
        {
            get
            {
                return this._purchaseDucketsPrice;
            }
        }

        public int PublishDucketsPrice
        {
            get
            {
                return this._publishDucketsPrice;
            }
        }

        public ItemData PhotoPoster
        {
            get
            {
                return this._photoPoster;
            }
        }

        public CameraPhotoManager()
        {
            _previews = new Dictionary<int, CameraPhotoPreview>();
        }

        public void Init(ItemDataManager itemDataManager)
        {
            RocketEmulador.RocketData().data.TryGetValue("camera.path.preview", out this._previewPath);
            RocketEmulador.RocketData().data.TryGetValue("camera.path.purchased", out this._purchasedPath);

            if (RocketEmulador.RocketData().data.ContainsKey("camera.preview.maxcache"))
            {
    this._maxPreviewCacheCount = int.Parse(RocketEmulador.RocketData().data["camera.preview.maxcache"]);
                }

            if (RocketEmulador.GetDBConfig().DBData.ContainsKey("camera.photo.purchase.price.coins"))
            {
    this._purchaseCoinsPrice = int.Parse(RocketEmulador.GetDBConfig().DBData["camera.photo.purchase.price.coins"]);
                }

            if (RocketEmulador.GetDBConfig().DBData.ContainsKey("camera.photo.purchase.price.duckets"))
            {
    this._purchaseDucketsPrice = int.Parse(RocketEmulador.GetDBConfig().DBData["camera.photo.purchase.price.duckets"]);
                }

            if (RocketEmulador.GetDBConfig().DBData.ContainsKey("camera.photo.publish.price.duckets"))
            {
    this._publishDucketsPrice = int.Parse(RocketEmulador.GetDBConfig().DBData["camera.photo.publish.price.duckets"]);
                }

            int ItemId = int.Parse(RocketEmulador.GetDBConfig().DBData ["camera.photo.purchase.item_id"]);

            if (!itemDataManager.GetItem(ItemId, out this._photoPoster))
            {
                log.Error("Couldn't load photo poster item " + ItemId + ", no furniture record found.");
                }

            //log.Info("Camera Photo Manager -> LOADED");
        }

        public CameraPhotoPreview GetPreview(int PhotoId)
        {
            if (!this._previews.ContainsKey(PhotoId))
            {
                    return null;
                }

            return this._previews [PhotoId];
        }

        public void AddPreview(CameraPhotoPreview preview)
        {
            if (this._previews.Count >= this._maxPreviewCacheCount)
            {
    this._previews.Remove(this._previews.Keys.First());
                }

            this._previews.Add(preview.Id, preview);
        }

        public string GetPath(CameraPhotoType type, int PhotoId, int CreatorId)
        {
            string path = "{1}-{0}.png";

            switch (type)
            {
                case CameraPhotoType.PREVIEW:
                    path = this._previewPath;
                    break;
                case CameraPhotoType.PURCHASED:
                    path = this._purchasedPath;
                    break;
            }

            return string.Format(path, PhotoId, CreatorId);
        }
    }
}