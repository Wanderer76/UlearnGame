using System.Drawing;
using UlearnGame.Utilities;

namespace UlearnGame.Models
{
    public class EnemyMissle : CommonMissle
    {
        public EnemyMissle(Image image, Direction direction, int missleSpeed, int damage, int width, int height, int maxHeight, int maxWidth, int x, int y)
        :base(image, direction, missleSpeed, damage, width, height, maxHeight, maxWidth, x, y)
        {
           
        }

    }
}
