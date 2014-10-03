using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace eZet.AStar {
    /// <summary>
    /// A PriorityQueue used for the A Star algorithm
    /// </summary>
    /// <typeparam name="TP">The key type to prioritize by</typeparam>
    /// <typeparam name="TV">The value type to store</typeparam>
    public class PriorityQueue<TP, TV> {
        private readonly SortedDictionary<TP, Queue<TV>> _queue = new SortedDictionary<TP, Queue<TV>>();

        /// <summary>
        /// Enqueues a new value, with the give priority
        /// </summary>
        /// <param name="priority">The priority</param>
        /// <param name="value">The value</param>
        public void Enqueue(TP priority, TV value) {
            Queue<TV> q;
            if (!_queue.TryGetValue(priority, out q)) {
                q = new Queue<TV>();
                _queue.Add(priority, q);
            }
            q.Enqueue(value);
        }

        /// <summary>
        /// Dequeues the highest priority value
        /// </summary>
        /// <returns></returns>
        public TV Dequeue() {
            var pair = _queue.First();
            var value = pair.Value.Dequeue();
            if (pair.Value.Count == 0) {
                _queue.Remove(pair.Key);
            }
            return value;
        }

        /// <summary>
        /// Returns true if the queue is empty, otherwise false
        /// </summary>
        public bool IsEmpty {
            get { return !_queue.Any(); }
        }

        /// <summary>
        /// Returns the number of items in the queue
        /// </summary>
        public int Count {
            get { return _queue.Sum(t => t.Value.Count); }
        }
    }
}