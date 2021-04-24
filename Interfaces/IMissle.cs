using System.Drawing;
using UlearnGame.Utilities;

namespace UlearnGame.Interfaces
{
   public interface IMissle
    {
        public Direction Direction { get; set; }
        public Vector GetPosition();
        public int Damage { get; set; }
        public int MissleSpeed { get; }
        public bool InConflict(IEnemy enemy);
    }
}
