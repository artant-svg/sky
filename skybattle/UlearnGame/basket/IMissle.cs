using System.Windows.Forms;
using UlearnGame.Utilities;

namespace UlearnGame.Interfaces
{
    public interface IMissle
    {
        public Direction Direction { get; set; }
        public int MissleSpeed { get; set; }
        public PictureBox MissleImage { get; }
        public int Damage { get; set; }
        public Vector GetPosition();
        public void StartMissle();
        public void StopMissle();
        public void SetPosition(int x, int y);
    }
}
