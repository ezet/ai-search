namespace eZet.AStar {
    public interface IAlgorithm {

        Path Run(ISolvable solvable);

        int Throttle { get; set; }
    }
}