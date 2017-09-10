using System;

namespace Shipwreck.KokoroIO
{
    public sealed class EventArgs<T> : EventArgs
    {
        public EventArgs(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }
}