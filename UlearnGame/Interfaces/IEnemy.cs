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

        public int CurrentShootDelay { get; set; }

        public Vector GetPosition();
        public Image GetImage();
        public PictureBox GetSource();
        public IEnumerable<IMissle> GetMissles();
        public bool OnMissleConflict(IEnumerable<PlayerMissle> missle);
        public int GetHealth();
        public void Shoot();
        public void MoveToPoint(Vector position);
        public void MoveFromPoint(Vector position);
    }
}
