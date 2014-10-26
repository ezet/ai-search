using System.Collections.Generic;
using System.Linq.Dynamic;
using eZet.Csp.Constraints;

namespace eZet.Csp {
    /// <summary>
    /// A specialized queue that ensures that only one instance of each Variable and Constraint task pair is queued.
    /// </summary>
    public class ReviseQueue : Queue<FilterTask> {

        private readonly Queue<FilterTask> _queue;

        private readonly Dictionary<IVariable, HashSet<IConstraint>> _hash;

        public ReviseQueue() {
            _queue = new Queue<FilterTask>();
            _hash = new Dictionary<IVariable, HashSet<IConstraint>>();
        }

        public new void Enqueue(FilterTask task) {
            HashSet<IConstraint> set;
            if (_hash.TryGetValue(task.Node, out set)) {
                if (set.Contains(task.Constraint))
                    return;
            } else {
                _hash.Add(task.Node, new HashSet<IConstraint>());
            }
            _hash[task.Node].Add(task.Constraint);
            _queue.Enqueue(task);
        }

        public new FilterTask Dequeue() {
            var task = _queue.Dequeue();
            _hash[task.Node].Remove(task.Constraint);
            return task;
        }

        public bool Any() {
            return _queue.Any();
        }
    }
}
