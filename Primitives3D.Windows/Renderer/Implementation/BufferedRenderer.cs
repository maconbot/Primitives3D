using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives3D.Windows
{
    public class BufferedRenderer : IRenderer
    {
        private const int MaxBuffers = 2;
        private readonly RenderCommandCollection _buffer = new RenderCommandCollection(MaxBuffers);
        private readonly AutoResetEvent _render = new AutoResetEvent(false);
        private readonly SemaphoreSlim _update = new SemaphoreSlim(0, MaxBuffers);
        private readonly CubePrimitive _cubePrimitive;

        public BufferedRenderer(GraphicsDevice device)
        {
            _cubePrimitive = new CubePrimitive(device);
        }

        public void AddCube(Cube primitive)
        {
            var translation = Matrix.CreateFromYawPitchRoll(
                primitive.Rotation.X,
                primitive.Rotation.Y,
                primitive.Rotation.Z) *
                              Matrix.CreateTranslation(primitive.Position);
            
            _buffer.Add(new RenderCommand
                {
                    Color = primitive.Color,
                    Radius = primitive.Radius,
                    World = translation
                });
        }

        public void EndFrame()
        {
            _render.Set();
            _update.Wait();
            _buffer.SwapBuffer();
        }

        public void Draw(Matrix view, Matrix projection)
        {
            _render.WaitOne();

            foreach (var renderingRenderCommand in _buffer)
            {
                _cubePrimitive.Draw(renderingRenderCommand.World, view, projection, renderingRenderCommand.Color);
            }

            _update.Release();
        }
    }
}