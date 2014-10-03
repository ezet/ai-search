using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace eZet.AStar.Gui.ViewModels {
    [Export]
    public class NewGridViewModel : Screen {
        public string Data { get; set; }
    }
}