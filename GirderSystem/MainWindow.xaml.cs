using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using TSMUI = Tekla.Structures.Model.UI;
using System.Collections;
using GirderSystem1;
using API2020;

namespace GirderSystemProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   //Свойства привязываемые к нашей форме
        public string Step { get; set; } = "1000,0";
        public string GirderWidth { get; set; } = "3000,0";
        public string Material { get; set; } = "C245";
        public string PrimaryProfile { get; set; } = "I45B2_20_93";
        public string SecondaryProfile { get; set; } = "I30B2_20_93";
        public string ConnectionNumber { get; set; } = "185";
        public bool IsCreateDrawing { get; set; } = true;



        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;//DataContext определяет, откуда будут браться данные для привязки. Это поле обязательно
        }

        private void btn_CreateGirder_Click(object sender, RoutedEventArgs e)
        {
            TSM.Model model = new TSM.Model();

            if (model.GetConnectionStatus())
            {
                TSMUI.Picker picker = new TSMUI.Picker();
                ArrayList array = picker.PickPoints(TSMUI.Picker.PickPointEnum.PICK_TWO_POINTS);
                GirderParameters girderParameters = new GirderParameters(Convert.ToDouble(GirderWidth), Convert.ToDouble(Step), PrimaryProfile, SecondaryProfile, Material, array, Convert.ToInt32(ConnectionNumber));
                TSM.TransformationPlane originTP = model.GetWorkPlaneHandler().GetCurrentTransformationPlane();
                GirderSystem girderSystem = new GirderSystem(girderParameters);
                model.GetWorkPlaneHandler().SetCurrentTransformationPlane(girderSystem.TransformationPlane);//перемещение рабочей плоскости в начало координат нашей балочной клетки
                girderSystem.Insert();
                
                if (IsCreateDrawing)
                {
                    API2020.Drawing drawing = new API2020.Drawing("Балочная клетка", new Tekla.Structures.Drawing.Size(420,297), girderSystem.Beams);
                    drawing.Insert();
                }
                

                model.GetWorkPlaneHandler().SetCurrentTransformationPlane(originTP);//Возвращение рабочей плоскости в начало координат модели
                model.CommitChanges();


                

            }

        }
    }
}
