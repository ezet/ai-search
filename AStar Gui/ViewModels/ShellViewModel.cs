using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Mvvm;
using eZet.AStar.Grid;
using eZet.AStar.Gui.Models;
using Microsoft.Win32;

namespace eZet.AStar.Gui.ViewModels {

    [Export(typeof(IShell))]
    public class ShellViewModel : Screen, IShell {
        private readonly IWindowManager _windowManager;
        private BindableCollection<Grid2DNode> _nodes;
        private IList<Grid2DNode> _solution;
        private string _statusText;
        private AlgorithmResult _result;
        private int _delay;
        private bool _running;

        public bool Running {
            get { return _running; }
            set {
                if (value.Equals(_running)) return;
                _running = value;
                NotifyOfPropertyChange();
            }
        }

        public Solver Solver { get; set; }

        public Grid2D CurrentGrid { get; private set; }

        public int Delay {
            get { return _delay; }
            set {
                if (value == _delay) return;
                _delay = value;
                NotifyOfPropertyChange();
            }
        }

        public BindableCollection<String> Log { get; private set; }

        public ICommand EnableThrottleCommand { get; set; }

        public CancellationTokenSource Cts { get; private set; }

        public AlgorithmResult Result {
            get { return _result; }
            private set {
                if (Equals(value, _result)) return;
                _result = value;
                NotifyOfPropertyChange();
            }
        }

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

        public ICommand OpenCommand { get; private set; }

        public ICommand NewCommand { get; private set; }

        public ICommand HaltCommand { get; private set; }

        public ICommand SetAlgorithmCommand { get; private set; }


        [ImportingConstructor()]
        public ShellViewModel(IWindowManager windowManager) {
            _windowManager = windowManager;
            DisplayName = "eZet Graph Solver";
            Delay = 100;
            Solver = new Solver(new AStar(Delay));
            RunCommand = new DelegateCommand(ExecuteRun, () => CurrentGrid != null && !Running);
            HaltCommand = new DelegateCommand(ExecuteHalt, () => Running);
            SetAlgorithmCommand = new DelegateCommand<string>(ExecuteSetAlgorithm);
            //CurrentGrid = GridLoader.Load(GridLoader.GridBank.First());
            OpenCommand = new DelegateCommand(ExecuteOpen);
            NewCommand = new DelegateCommand(ExecuteNew);
            StatusText = "Ready";
            PropertyChanged += OnPropertyChanged;
        }

        private void ExecuteHalt() {
            Cts.Cancel(true);
        }

        private void ExecuteNew() {
            var vm = IoC.Get<NewGridViewModel>();
            if (_windowManager.ShowDialog(vm).GetValueOrDefault()) {
                try {
                    CurrentGrid = GridLoader.Load(vm.Data);
                    Nodes = new BindableCollection<Grid2DNode>(CurrentGrid.Nodes);
                    StatusText = "New grid loaded";
                } catch (Exception e) {
                    StatusText = "Could not load grid: Invalid format";
                }

            }
        }

        private void ExecuteOpen() {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault()) {
                string data;
                using (var file = dialog.OpenFile()) {
                    data = file.ReadString();
                }
                try {
                    CurrentGrid = GridLoader.Load(data);
                    Nodes = new BindableCollection<Grid2DNode>(CurrentGrid.Nodes);
                    StatusText = "Grid loaded from file";
                } catch (Exception e) {
                    StatusText = "Could not load grid: Invalid format";
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) {
            if (propertyChangedEventArgs.PropertyName == "Delay") {
                Solver.Algorithm.Throttle = Delay;
            }
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
            Running = true;
            StatusText = "Processing...";
            var sw = new Stopwatch();
            sw.Start();
            Cts = new CancellationTokenSource();
            var task = Task.Run(() => Solver.Solve(CurrentGrid, Cts.Token));
            var path = await task.ConfigureAwait(false);
            Running = false;
            sw.Stop();
            if (Cts.IsCancellationRequested) {
                StatusText = "Operation cancelled";
                CurrentGrid = null;
            } else {
                if (path != null) {
                    Solution = path.Cast<Grid2DNode>().ToList();
                    Result = new AlgorithmResult(Solution.Count, Solver.Algorithm.ExpandedNodes.Count, sw.Elapsed);
                    StatusText = "Solution found";
                } else {
                    StatusText = "Solution not found";
                }
            }
        }

    }
}