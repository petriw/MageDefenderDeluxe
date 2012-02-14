using Microsoft.Xna.Framework;

namespace MageDefenderDeluxe.Interfaces
{
    interface ICamera
    {
        Matrix ViewMatrix { get; set; }
        Matrix ProjectionMatrix { get; set; }
        Vector3 Position { get; set; }
        Vector3 LookAt { get; set; }
    }
}