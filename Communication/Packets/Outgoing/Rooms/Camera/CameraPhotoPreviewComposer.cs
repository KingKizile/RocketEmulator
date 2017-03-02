using System;

namespace Rocket.Communication.Packets.Outgoing.Rooms.Camera
{
    public class CameraPhotoPreviewComposer : ServerPacket
    {
        public CameraPhotoPreviewComposer(string ImageUrl)
            : base(ServerPacketHeader.CameraPhotoPreviewComposer)
        {
            base.WriteString(ImageUrl);
        }
    }
}