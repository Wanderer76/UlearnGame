using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UlearnGame.Models;
using UlearnGame.Utilities;

namespace UlearnGame.Interfaces
{

    public interface IEnemy
    {
        public int Damage { get; set; }
        public Vector GetPosition();
        public void Shoot();
        public void MoveToPoint(Vector position);
        public void MoveFromPoint(Vector position);
        public Image GetImage();
        public PictureBox GetSource();
        public bool DeadInConflict(IEnumerable<PlayerMissle>missle);
    }
}
