using System.Drawing;
using System.Windows.Forms;

namespace UlearnGame.Models
{
    public class LightEnemy : CommonEnemy
    {
        public const int MissleWidth = 20;
        public const int MissleHeight = 25;
        public const int LightEnemySize = 50;
        public const int Health = 30;

        public LightEnemy(Form form, int missleCount, int missleSpeed = 2, int shootInterval = 1000, int damage = 15, int speed = 1)
        : base(form, missleCount, 30, new Size(MissleWidth, MissleHeight), new Size(LightEnemySize, LightEnemySize), missleSpeed, shootInterval, damage, speed)
        { }
    }
}
