using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Dialog;
using TD = Tekla.Structures.Datatype;

namespace WpfWeldedBeam
{
    public class MainWindowViewModel : BaseViewModel
    {
        [StructuresDialog("BeamHeight", typeof(TD.Integer))]
        public int BeamHeight { get; set; } = 800;
        [StructuresDialog("BeamWidth", typeof(TD.Integer))]
        public int BeamWidth { get; set; } = 300;
        [StructuresDialog("BeamFlnge", typeof(TD.Integer))]
        public int BeamFlange { get; set; } = 20;
        [StructuresDialog("BeamWeb", typeof(TD.Integer))]
        public int BeamWeb { get; set; } = 12;
        [StructuresDialog("Matrial", typeof(TD.String))]
        public string BeamMaterial { get; set; } = "C245";

    }
}
