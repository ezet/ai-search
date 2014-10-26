using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DevExpress.Mvvm;
using eZet.AStar;
using eZet.AStar.Algorithms;
using Microsoft.Win32;

namespace eZet.Csp.Nonogram.Gui.ViewModels {
    [Export]
    public sealed class ShellViewModel : Screen {
        private int _delay;
        private string _statusText;
        private bool _running;
        private NonogramSolver _solver;
        private AlgorithmResult _result;
        private NonogramModel _model;
        private BindableCollection<NonogramLine> _nodes;

        public ShellViewModel() {
            DisplayName = "Nonogram Solver";
            RunCommand = new DelegateCommand(executeRun, () => Model != null && !Running);
            HaltCommand = new DelegateCommand(executeHalt, () => Model != null && Running);
            OpenCommand = new DelegateCommand(executeOpen);
            SetAlgorithmCommand = new DelegateCommand<string>(executeSetAlgorithm);
            _solver = new NonogramSolver();
            PropertyChanged += OnPropertyChanged;
            Delay = 500;
            Algorithm = new AStar.Algorithms.AStar(Delay);
            StatusText = "Ready";
        }

        public ICommand RunCommand { get; private set; }

        public ICommand HaltCommand { get; private set; }

        public ICommand OpenCommand { get; private set; }

        public ICommand SetAlgorithmCommand { get; private set; }

        public NonogramModel Model {
            get { return _model; }
            private set {
                if (Equals(value, _model)) return;
                _model = value;
                NotifyOfPropertyChange();
            }
        }

        public AlgorithmResult Result {
            get { return _result; }
            private set {
                if (Equals(value, _result)) return;
                _result = value;
                NotifyOfPropertyChange();
            }
        }

        public int Delay {
            get { return _delay; }
            set {
                if (value == _delay) return;
                _delay = value;
                NotifyOfPropertyChange();
            }
        }

        public String StatusText {
            get { return _statusText; }
            private set {
                if (value == _statusText) return;
                _statusText = value;
                NotifyOfPropertyChange();
            }
        }

        public NonogramSolver Solver {
            get { return _solver; }
            private set {
                if (Equals(value, _solver)) return;
                _solver = value;
                NotifyOfPropertyChange();
            }
        }

        public bool Running {
            get { return _running; }
            private set {
                if (value.Equals(_running)) return;
                _running = value;
                NotifyOfPropertyChange();
            }
        }

        public BindableCollection<NonogramLine> Nodes {
            get { return _nodes; }
            private set {
                if (Equals(value, _nodes)) return;
                _nodes = value;
                NotifyOfPropertyChange();
            }
        }

        public ISearchAlgorithm Algorithm { get; private set; }

        public CancellationTokenSource Cts { get; private set; }

        private void executeHalt() {
            Cts.Cancel();
        }

        private async void executeRun() {
            Result = null;
            Running = true;
            StatusText = "Processing...";
            Cts = new CancellationTokenSource();
            Algorithm.CancellationToken = Cts.Token;
            var task = Task.Run(() => _solver.Solve(Model, Algorithm));
            var result = await task;
            Running = false;
            if (Cts.IsCancellationRequested) {
                StatusText = "Operation cancelled";
            }
            else {
                if (result == GacSolvable.Result.Solved) {
                    Result = _solver.GetStatistics();
                    StatusText = "Solution found";
                }
                else if (result == GacSolvable.Result.Failed)
                    StatusText = "No solutions possible";
                else {
                    StatusText = "No solution found";
                }
            }
        }

        private void executeOpen() {
            Result = null;
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault()) {
                try {
                    Model = _solver.Load(dialog.FileName);
                    Nodes =
                        new BindableCollection<NonogramLine>(
                            Model.Nodes.Cast<NonogramLine>().Where(n => n.Type == NonogramLine.LineType.Row));
                    StatusText = "Graph loaded";
                }
                catch (Exception) {
                    StatusText = "Could not load graph: Invalid format";
                    return;
                }
            }
        }

        private void executeSetAlgorithm(string param) {
            if (param == "astar")
                Algorithm = new AStar.Algorithms.AStar(Delay);
            if (param == "bfs")
                Algorithm = new Bfs(Delay);
            if (param == "dfs")
                Algorithm = new Dfs(Delay);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args) {
            if (args.PropertyName == "Delay") {
                if (_solver.Solvable != null) {
                    _solver.Solvable.Algorithm.Throttle = Delay;
                }
            }
        }
    }
}