using System.Drawing;
using UlearnGame.Interfaces;
using UlearnGame.Utilities;

namespace UlearnGame.Models.Bonuses
{
    public class ArmorBonus : IBonus
    {
        private readonly int armor;

        public ArmorBonus(int armor)
        {
            this.armor = armor;
        }

        public int GetBonus() => armor;
        public Image GetImage() => throw new System.NotImplementedException();
        public Vector GetPosition() => throw new System.NotImplementedException();
        public bool OnConflict(Player player) => throw new System.NotImplementedException();
        public void StartMotion() => throw new System.NotImplementedException();
        public void StopMotion() => throw new System.NotImplementedException();
    }
}
