using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DevExpress.Xpf.Mvvm;
using eZet.AStar.Grid;

namespace eZet.AStar.Gui.ViewModels {

    [Export(typeof(IShell))]
    public class ShellViewModel : Screen, IShell {
        private BindableCollection<Grid2DNode> _nodes;
        private IList<Grid2DNode> _solution;
        private string _statusText;

        public Solver Solver { get; set; }

        public int Delay { get; set; }

        public ICommand EnableThrottleCommand { get; set; }

        public String StatusText {
            get { return _statusText; }
            set {
                if (value == _statusText) return;
                _statusText = value;
                NotifyOfPropertyChange();
            }
        }

        public IList<Grid2DNode> Solution {
            get { return _solution; }
            private set {
                if (Equals(value, _solution)) return;
                _solution = value;
                NotifyOfPropertyChange();
            }
        }

        public BindableCollection<Grid2DNode> Nodes {
            get { return _nodes; }
            set {
                if (Equals(value, _nodes)) return;
                _nodes = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand RunCommand { get; private set; }

        public ICommand HaltCommand { get; private set; }

        public ICommand SetAlgorithmCommand { get; private set; }

        public ShellViewModel() {
            DisplayName = "eZet Graph Solver";
            Delay = 100;
            Solver = new Solver(new AStar(Delay));
            RunCommand = new DelegateCommand(ExecuteRun);
            SetAlgorithmCommand = new DelegateCommand<string>(ExecuteSetAlgorithm);
            StatusText = "Ready";
        }

        private void ExecuteSetAlgorithm(string param) {
            if (param == "astar")
                Solver.Algorithm = new AStar(Delay);
            if (param == "bfs")
                Solver.Algorithm = new Bfs(Delay);
            if (param == "dfs")
                Solver.Algorithm = new Dfs(Delay);
            Nodes = null;
        }

        private async void ExecuteRun() {
            StatusText = "Processing...";
            var grid = GridLoader.Load(GridLoader.GridBank.First());
            Nodes = new BindableCollection<Grid2DNode>(grid.Nodes);
            var path = await Task.Run(() => Solver.Solve(grid));
            Solution = path.Cast<Grid2DNode>().ToList();
            StatusText = "Solution found.";
        }

    }
}