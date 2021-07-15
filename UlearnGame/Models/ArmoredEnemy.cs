using System.Drawing;
using System.Windows.Forms;

namespace UlearnGame.Models
{
    public class ArmoredEnemy : CommonEnemy
    {
        public const int MissleWidth = 30;
        public const int MissleHeight = 50;
        public const int ArmoredEnemySize = 60;
        public const int Health = 50;

        public ArmoredEnemy(Form form, int missleCount, int missleSpeed = 3, int shootInterval = 1000, int damage = 20, int speed = 1)
            : base(form, missleCount, Health, new Size(MissleWidth, MissleHeight), new Size(ArmoredEnemySize, ArmoredEnemySize), missleSpeed, shootInterval, damage, speed)
        {

        }


    }
}
