using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using eZet.AStar.Grid;

namespace eZet.AStar.Gui.ViewModels {

    [Export(typeof(IShell))]
    public class ShellViewModel : Screen, IShell {
        private BindableCollection<Grid2DNode> _nodes;

        public AStarSolver Solver { get; set; }

        public BindableCollection<Grid2DNode> Nodes {
            get { return _nodes; }
            set {
                if (Equals(value, _nodes)) return;
                _nodes = value;
                NotifyOfPropertyChange();
            }
        }

        public ShellViewModel() {
            DisplayName = "AStar GUI";
            Solver = new AStarSolver();
            
            var grid = GridLoader.Load("");
            Nodes = new BindableCollection<Grid2DNode>(grid.Nodes);
            Task.Run(() => Solver.Solve(grid));
        }

        public void Init() {
        }


    }
}