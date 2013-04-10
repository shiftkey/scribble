using System;
using System.Diagnostics;

namespace Scribble.CodeSnippets.Infrastructure
{
    public class TimingScope : IDisposable
    {
        readonly string message;
        Stopwatch sw;

        TimingScope(string message)
        {
            this.message = message;
        }

        public static TimingScope Start(string message)
        {
            var scope = new TimingScope(message);
            scope.StartInternal();
            return scope;
        }

        void StartInternal()
        {
#if DEBUG
            sw = new Stopwatch();
            sw.Start();
#endif
        }

        void StopInternal()
        {
#if DEBUG
            sw.Stop();
            Trace.WriteLine(string.Format("{0} took {1}ms", message, sw.ElapsedMilliseconds));
#endif
        }

        public void Dispose()
        {
#if DEBUG
            StopInternal();
#endif
        }
    }
}
