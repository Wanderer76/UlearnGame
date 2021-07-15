using System.Drawing;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public class PlayerMissle : CommonMissle
    {
        public const int Width = 20;
        public const int Height = 25;



        public PlayerMissle(Image image, Direction direction, int missleSpeed, int maxHeight, int maxWidth, int x, int y)
        : base(image, direction, missleSpeed, 1, Width, Height, maxHeight, maxWidth, x, y)
        {

        }


    }
}
