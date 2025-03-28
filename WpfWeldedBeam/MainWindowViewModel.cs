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

        double beamHeight = 0;
        double beamWidth = 0;
        double beamFlange = 0;
        double beamWeb = 0;
        string beamMaterial = string.Empty;
        string webClass = string.Empty;
        string flangeClass = string.Empty;





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

    }
}
