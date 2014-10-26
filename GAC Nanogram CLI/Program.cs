using System;
using System.Linq;
using eZet.Csp;

namespace GAC_Nanogram_CLI {
    public class Program {
        static void Main(string[] args) {

            var solver = new NanogramSolver();
            var model = solver.load();
            var gac = new GacSolvable(model);
            var result = gac.Solve();
            Console.WriteLine(result);
            var lines = gac.Model.Nodes.Cast<NanogramLine>().Where(n => n.Type == NanogramLine.LineType.Row).Reverse();

            foreach (var line in lines) {
                foreach (var cell in line.DomainValues.Cast<LinePattern>().ToList().Single().BlockArray) {
                    Console.Write(cell ? "#" : " ");
                }
                Console.WriteLine();
            }
        }
    }
}
