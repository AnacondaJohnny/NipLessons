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

        private double beamHeight = 0;
        private double beamWidth = 0;
        private double beamFlange = 0;
        private double beamWeb = 0;
        private string beamMaterial = string.Empty;
        private string webClass = string.Empty;
        private string flangeClass = string.Empty;
        private double angle1 = 0;
        private double verticalOffset = 0;
        private double horizontalOffset = 0;
        private string assemblyPrefix = string.Empty;




        [StructuresDialog("BeamHeight", typeof(TD.Double))]
        public double BeamHeight
        {
            get { return beamHeight; }
            set { beamHeight = value; OnPropertyChanged("BeamHeight"); }

        }

        [StructuresDialog("BeamWidth", typeof(TD.Double))]
        public double BeamWidth
        {
            get { return beamWidth; }
            set { beamWidth = value; OnPropertyChanged("BeamWidth"); }

        }

        [StructuresDialog("BeamFlange", typeof(TD.Double))] //Полка балки
        public double BeamFlange
        {
            get { return beamFlange; }
            set { beamFlange = value; OnPropertyChanged("BeamFlange"); }

        }

        [StructuresDialog("BeamWeb", typeof(TD.Double))] //Стенка балки
        public double BeamWeb
        {
            get { return beamWeb; }
            set { beamWeb = value; OnPropertyChanged("BeamWeb"); }

        }

        [StructuresDialog("Material", typeof(TD.String))]
        public string BeamMaterial
        {
            get { return beamMaterial; }
            set { beamMaterial = value; OnPropertyChanged("BeamMaterial"); }

        }

        [StructuresDialog("WebClass", typeof(TD.String))]
        public string WebClass
        {
            get { return webClass; }
            set { webClass = value; OnPropertyChanged("WebClass"); }

        }



        [StructuresDialog("FlangeClass", typeof(TD.String))]
        public string FlangeClass
        {
            get { return flangeClass; }
            set { flangeClass = value; OnPropertyChanged("FlangeClass"); }

        }

        [StructuresDialog("Angle1", typeof(TD.Double))]
        public double Angle1
        {
            get { return angle1; }
            set { angle1 = value; OnPropertyChanged("Angle1"); }
        }

        [StructuresDialog("VerticalOffset", typeof(TD.Double))]
        public double VerticalOffset
        {
            get { return verticalOffset; }
            set { verticalOffset = value; OnPropertyChanged("VerticalOffset"); }
        }

        [StructuresDialog("HorizontalOffset", typeof(TD.Double))]
        public double HorizontalOffset
        {
            get { return horizontalOffset; }
            set { horizontalOffset = value; OnPropertyChanged("HorizontalOffset"); }
        }

        [StructuresDialog("AssemblyPrefix", typeof(TD.String))]
        public string AssemblyPrefix
        {
            get { return assemblyPrefix; }
            set { assemblyPrefix = value; OnPropertyChanged("AssemblyPrefix"); }

        }

    }
}
