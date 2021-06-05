using System.Drawing;

namespace UlearnGame.Utilities
{
    public enum Direction
    {
        Down,
        Left,
        Top,
        Right,
        None
    };

    public static class ImageExtentions
    {
        public static Image RotateImage(this Image img)
        {
            var bmp = new Bitmap(img);
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return bmp;
        }
    }
}
