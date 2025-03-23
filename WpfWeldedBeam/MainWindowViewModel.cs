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
        [StructuresDialog("BeamHeight", typeof(TD.Double))]
        public double BeamHeight { get; set; } = 800;

        [StructuresDialog("BeamWidth", typeof(TD.Double))]
        public double BeamWidth { get; set; } = 300;

        [StructuresDialog("BeamFlange", typeof(TD.Double))] //Полка балки
        public double BeamFlange { get; set; } = 20;

        [StructuresDialog("BeamWeb", typeof(TD.Double))] //Стенка балки
        public double BeamWeb { get; set; } = 12;

        [StructuresDialog("Material", typeof(TD.String))]
        public string BeamMaterial { get; set; } = "C245";

    }
}
