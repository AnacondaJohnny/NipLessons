using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Dialog;
using TD = Tekla.Structures.Datatype;

namespace WPFPluginTemplate
{

    public class MainWindowViewModel : BaseViewModel
    {
        //Все эти атрибуты нужны чтобы связать ViewModel с Model. Чтобы Текла могла их сопоставить 
        //Сопоставляем типы, без этого работать не будет. Обяхательно!

        [StructuresDialog("Profile", typeof(TD.String))]
        public string Profile { get; set; } = "500*500";

        [StructuresDialog("Material", typeof(TD.String))]
        public string Material { get; set; } = "B25";

        [StructuresDialog("RebarStep", typeof(TD.Double))]
        public double RebarStep { get; set; } = 200.0;

        [StructuresDialog("RebarSize", typeof(TD.Integer))]
        public int RebarSize { get; set; } = 25;

    }
}
