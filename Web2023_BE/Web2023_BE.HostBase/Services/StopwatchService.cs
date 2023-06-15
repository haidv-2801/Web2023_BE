
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.Domain.Shared.Debugs;

namespace Web2023_BE.Cache
{
    public class StopwatchService : IStopwatchService
    {
        private readonly Dictionary<string, long> _time;
        private readonly Stopwatch _sw;
        public StopwatchService()
        {
            _time = new Dictionary<string, long>();
            _sw = new Stopwatch();
        }

        public void Capture(string actionKey, Action action)
        {
            //_sw.Restart();
            action.Invoke();
            return;
            var time = _sw.ElapsedMilliseconds;

            if (_time.ContainsKey(actionKey))
            {
                _time[actionKey] += time;
            }
            else
            {
                _time[actionKey] = time;
            }
        }

        public async Task Capture(string actionKey, Func<Task> action)
        {
            //_sw.Restart();
            await action.Invoke();
            return;
            var time = _sw.ElapsedMilliseconds;

            if (_time.ContainsKey(actionKey))
            {
                _time[actionKey] += time;
            }
            else
            {
                _time[actionKey] = time;
            }
        }

        public T Capture<T>(string actionKey, Func<T> action)
        {
            //_sw.Restart();
            var result = action.Invoke();
            return result;
            var time = _sw.ElapsedMilliseconds;

            if (_time.ContainsKey(actionKey))
            {
                _time[actionKey] += time;
            }
            else
            {
                _time[actionKey] = time;
            }

            return result;
        }

        public async Task<T> Capture<T>(string actionKey, Func<Task<T>> action)
        {
            //_sw.Restart();
            var result = await action.Invoke();
            return result;
            var time = _sw.ElapsedMilliseconds;

            if (_time.ContainsKey(actionKey))
            {
                _time[actionKey] += time;
            }
            else
            {
                _time[actionKey] = time;
            }

            return result;
        }

        public object GetResult()
        {
            return _time;
        }
    }
}
