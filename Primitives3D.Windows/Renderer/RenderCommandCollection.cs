using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Primitives3D.Windows
{
    public class RenderCommandCollection : ICollection<RenderCommand>
    {
        private static readonly int MaxCommands = Constants.CubeCount;
        private readonly int _bufferCount;
        private readonly int[] _bufferLength;
        private readonly RenderCommand[][] _buffer;
        private int _activeBuffer;

        public RenderCommandCollection(int buffers)
        {
            _bufferCount = buffers;
            _bufferLength = new int[buffers];
            _buffer = new RenderCommand[buffers][];
            
            for (int i = 0; i < buffers; i++)
            {
                _buffer[i] = new RenderCommand[MaxCommands];
            }
        }

        private IEnumerable<RenderCommand> GetCurrentCollection()
        {
            var activeBuffer = _activeBuffer;
            var bufferLength = _bufferLength[activeBuffer];
            var newBuffer = new RenderCommand[bufferLength];

            Array.Copy(_buffer[activeBuffer], newBuffer, bufferLength);

            return newBuffer;
        }
        
        public IEnumerator<RenderCommand> GetEnumerator()
        {
            return GetCurrentCollection().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(RenderCommand item)
        {
            var activeBuffer = _activeBuffer;
            var bufferLength = _bufferLength[activeBuffer]++;

            if (bufferLength < MaxCommands)
            {
                _buffer[activeBuffer][bufferLength] = item;
            }
            else
            {
                throw new Exception("Buffer is full");
            }
        }

        public void SwapBuffer()
        {
            var newBuffer = (_activeBuffer + 1)%_bufferCount;
            _bufferLength[newBuffer] = 0;
            Interlocked.Exchange(ref _activeBuffer, newBuffer);
        }

        public void Clear()
        {
            _bufferLength[_activeBuffer] = 0;
        }

        public bool Contains(RenderCommand item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(RenderCommand[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(RenderCommand item)
        {
            throw new NotImplementedException();
        }

        public int Count { get { return _bufferLength[_activeBuffer]; } }
        public bool IsReadOnly { get { return false; } }
    }
}