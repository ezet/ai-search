using System.Collections.Generic;
using System.Linq;

namespace eZet.AStar {
    public class PriorityQueue<TP, TV> {

        private readonly SortedDictionary<TP, Queue<TV>> _queue = new SortedDictionary<TP, Queue<TV>>();

        public void Enqueue(TP priority, TV value) {
            Queue<TV> q;
            if (!_queue.TryGetValue(priority, out q)) {
                q = new Queue<TV>();
                _queue.Add(priority, q);
            }
            q.Enqueue(value);
        }

        public TV Dequeue() {
            var pair = _queue.First();
            var value = pair.Value.Dequeue();
            if (pair.Value.Count == 0) {
                _queue.Remove(pair.Key);
            }
            return value;
        }

        public bool IsEmpty { get { return !_queue.Any(); } }

    }
}
