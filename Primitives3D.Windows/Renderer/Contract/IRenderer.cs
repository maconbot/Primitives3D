using Microsoft.Xna.Framework;

namespace Primitives3D.Windows
{
    public interface IRenderer
    {
        void AddCube(Cube primitive);
        void EndFrame();
        void Draw(Matrix view, Matrix projection);
    }
}