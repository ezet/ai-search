using System;
using System.Collections.Generic;
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
using eZet.Csp;
using eZet.Csp.GraphColouring;
using eZet.Gac.Gui.Models;
using Microsoft.Win32;
using MoreLinq;
using ICanvasObject = eZet.Csp.GraphColouring.ICanvasObject;

namespace eZet.Gac.Gui.ViewModels {
    [Export]
    public class ShellViewModel : Screen {
        private int _delay;
        private string _statusText;
        private VertexColorSolver _solver;
        private Graph _graph;
        private int _domainValueCount;
        private bool _running;
        private VertexColorResult _result;
        private BindableCollection<ICanvasObject> _canvasObjects;

        public int Delay {
            get { return _delay; }
            set {
                if (value == _delay) return;
                _delay = value;
                NotifyOfPropertyChange();
            }
        }

        public ShellViewModel() {
            DisplayName = "GAC with Incremental Search";
            RunCommand = new DelegateCommand(executeRun, () => Graph != null && !Running);
            HaltCommand = new DelegateCommand(executeHalt, () => Graph != null && Running);
            OpenCommand = new DelegateCommand(executeOpen);
            SetAlgorithmCommand = new DelegateCommand<string>(executeSetAlgorithm);
            PropertyChanged += OnPropertyChanged;
            _solver = new VertexColorSolver();
            DomainValueCount = 4;
            Delay = 50;
            Algorithm = new AStar.Algorithms.AStar(Delay);
            StatusText = "Ready";
        }

        public String StatusText {
            get { return _statusText; }
            private set {
                if (value == _statusText) return;
                _statusText = value;
                NotifyOfPropertyChange();
            }
        }


        public Graph Graph {
            get { return _graph; }
            private set {
                if (Equals(value, _graph)) return;
                _graph = value;
                NotifyOfPropertyChange();
            }
        }

        public int DomainValueCount {
            get { return _domainValueCount; }
            set {
                if (value == _domainValueCount) return;
                _domainValueCount = value;
                NotifyOfPropertyChange();
            }
        }


        public ICommand RunCommand { get; private set; }

        public ICommand HaltCommand { get; private set; }

        public ICommand OpenCommand { get; private set; }

        public ICommand SetAlgorithmCommand { get; private set; }

        public bool Running {
            get { return _running; }
            private set {
                if (value.Equals(_running)) return;
                _running = value;
                NotifyOfPropertyChange();
            }
        }

        public VertexColorResult Result {
            get { return _result; }
            private set {
                if (Equals(value, _result)) return;
                _result = value;
                NotifyOfPropertyChange();
            }
        }

        public BindableCollection<ICanvasObject> CanvasObjects {
            get { return _canvasObjects; }
            private set {
                if (Equals(value, _canvasObjects)) return;
                _canvasObjects = value;
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
            var task = Task.Run(() => _solver.Solve(Graph, DomainValueCount, Algorithm));
            var result = await task;
            Running = false;
            if (Cts.IsCancellationRequested) {
                StatusText = "Operation cancelled";
            } else {
                if (result == GacSolvable.Result.Solved) {
                    Result = _solver.GetStatistics();
                    StatusText = "Solution found";
                } else if (result == GacSolvable.Result.Failed)
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
                    Graph = VertexColorSolver.Load(dialog.FileName);
                    StatusText = "Graph loaded";
                } catch (Exception) {
                    StatusText = "Could not load graph: Invalid format";
                    return;
                }
                var list = Graph.Nodes.Cast<ICanvasObject>().ToList();
                transformGraph(list.Cast<SimpleVariable>().ToList());
                list.AddRange(Graph.Edges.Cast<ICanvasObject>());
                CanvasObjects = new BindableCollection<ICanvasObject>(list);
            }
        }

        private void transformGraph(IEnumerable<SimpleVariable> variables) {
            // transform negative coordinates
            var minX = variables.MinBy(v => v.X).X;
            var minY = variables.MinBy(v => v.Y).Y;
            foreach (var v in variables) {
                if (minX < 0)
                    v.X += Math.Abs(minX);
                if (minY < 0)
                    v.Y += Math.Abs(minY);
            }
            // fit to 1000 x 1000 px
            var maxX = variables.MaxBy(v => v.X).X;
            var maxY = variables.MaxBy(v => v.X).Y;
            var x = maxX / 100;
            var y = maxY / 100;
            foreach (var v in variables) {
                if (x > 1)
                    v.X = v.X / x;
                if (x > 1)
                    v.Y = v.Y / y;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args) {
            if (args.PropertyName == "Delay") {
                if (_solver.GacSolvable != null && _solver.GacSolvable.Algorithm != null)
                    _solver.GacSolvable.Algorithm.Throttle = Delay;
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
    }
}