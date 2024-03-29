using System.Diagnostics;

namespace Primitives3D.Windows
{
	public class UpdateLoop
	{
		private readonly World _world;
		private readonly Stopwatch _stopwatch;
		private long _lastElapsed;

		public UpdateLoop(IRenderer renderer)
		{
			_world = new World(renderer);
			_stopwatch = new Stopwatch();
		}

		public void Loop()
		{
			_stopwatch.Start();
			while (true)
			{
				Update();
			}
		}

		private void Update()
		{
			var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
			var elapsed = elapsedMilliseconds - _lastElapsed;
			_lastElapsed = elapsedMilliseconds;
			_world.Update(elapsed);
		}
	}
}