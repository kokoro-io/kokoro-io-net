using System;
using System.IO;
using System.Threading.Tasks;

namespace KokoroIO
{
    internal static class Polyfills
    {
        public static Task CompletedTask
            => Task.FromResult(0);

        public static Task<TResult> ToTask<TResult>(this Exception exception)
        {
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(exception);

            return tcs.Task;
        }

#if NET45
        public static bool TryGetBuffer(this MemoryStream ms, out ArraySegment<byte> b)
        {
            var buf = ms.GetBuffer();
            b = new ArraySegment<byte>(buf, 0, (int)ms.Length);
            return true;
        }
#endif
    }
}