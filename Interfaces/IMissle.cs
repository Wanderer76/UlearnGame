using System.Drawing;
using UlearnGame.Utilities;

namespace UlearnGame.Interfaces
{
    interface IMissle
    {
        public Direction Direction { get; set; }
        public int Damage { get; set; }
        public int MissleSpeed { get; }
    }
}
