using System.Drawing;
using UlearnGame.Utilities;

namespace UlearnGame.Interfaces
{

    interface IEnemy
    {
        int Damage { get; set; }
        Vector GetPosition();
        public void Shoot();
        public void MoveToPoint(Vector position);

        public Image GetImage();
    }
}
