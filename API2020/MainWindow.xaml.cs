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

namespace API2020
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TSM.Model model;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_CreateBeam_Click(object sender, RoutedEventArgs e)
        {
            model = model ?? new TSM.Model();
            if (model.GetConnectionStatus())
            {
                Beam beam = new Beam();
                try
                {
                    beam.Insert();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                model.CommitChanges();
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к Tekla!");
            }
        }

        private void btn_CreatePlate_Click(object sender, RoutedEventArgs e)
        {
            model = model ?? new TSM.Model();
            if (model.GetConnectionStatus())
            {
                ContourPlate plate = new ContourPlate();
                try
                {
                    plate.Insert();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                model.CommitChanges();
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к Tekla!");
            }
            

        }

        private void btn_CreateAssembly_Click(object sender, RoutedEventArgs e)
        {
            model = model ?? new TSM.Model();
            if (model.GetConnectionStatus())
            {
                AssemblyColomn assemblyColomn = new AssemblyColomn();
                try
                {
                    assemblyColomn.Insert();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                model.CommitChanges();
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к Tekla!");
            }
        }

        private void btn_MoveWorkPlane_Click(object sender, RoutedEventArgs e)
        {
            model = model ?? new TSM.Model();
            if (model.GetConnectionStatus())
            {
                TSMUI.Picker picker = new TSMUI.Picker();
                var obj = picker.PickObject(TSMUI.Picker.PickObjectEnum.PICK_ONE_OBJECT);
                if (obj.GetType() == typeof(TSM.Beam))
                {
                    var beam = obj as TSM.Beam;
                    TSG.CoordinateSystem objCoordinateSystem = beam.GetCoordinateSystem();
                    TSM.TransformationPlane transformationPlane = new TSM.TransformationPlane(objCoordinateSystem);
                    model.GetWorkPlaneHandler().SetCurrentTransformationPlane(transformationPlane);
                    
                }
                if (obj.GetType() == typeof (TSM.Detail))
                {
                    var det = obj as TSM.Detail;
                    Hashtable ht = new Hashtable();
                    det.GetAllUserProperties(ref ht);
                    
                }
                
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к Tekla!");
            }

        }

        private void btn_CreteDetail_Click(object sender, RoutedEventArgs e)
        {
            model = model ?? new TSM.Model();
            if (model.GetConnectionStatus())
            {
                TSMUI.Picker picker = new TSMUI.Picker();
                var obj = picker.PickObject(TSMUI.Picker.PickObjectEnum.PICK_ONE_OBJECT);
                if (obj.GetType() == typeof(TSM.Beam))
                {
                    var beam = obj as TSM.Beam;
                    Detail detail = new Detail(beam as TSM.Part, 1002);
                    try
                    {
                        detail.Insert();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    
                    model.CommitChanges();

                }
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к Tekla!");
            }

        }

        private void btn_CreateWeldAndBolts_Click(object sender, RoutedEventArgs e)
        {
            TSMUI.Picker picker = new TSMUI.Picker();
            var obj = picker.PickObjects(TSMUI.Picker.PickObjectsEnum.PICK_N_PARTS);        //выбираем объекты из Теклы

            TSM.Beam beam = new TSM.Beam();
            TSM.Beam column = new TSM.Beam();
            TSM.ContourPlate plate = new TSM.ContourPlate();

            while (obj.MoveNext())       //перебор выбранных объектов
            {
                if (obj.Current.GetType().Equals(typeof(TSM.Beam))) //если тип объекта равен TSM.Beam, то зайдет в if
                {
                    TSM.Beam b = obj.Current as TSM.Beam;
                    if (b.Type.Equals(TSM.Beam.BeamTypeEnum.COLUMN)) // если тип объекта колонна, то запишет экземпляр b в column
                    {
                        column = b;
                    }
                    else beam = b;
                }
                else if (obj.Current.GetType().Equals(typeof(TSM.ContourPlate))) // если тип объекта пластина, то запишет экземпляр b в column
                {
                    TSM.ContourPlate p = obj.Current as TSM.ContourPlate;
                    plate = p;
                }
            }



            //Сварной шов
            TSM.Weld weld = new TSM.Weld();
            weld.MainObject = beam; // указываем главную деталь для сварки
            weld.SecondaryObject = plate; // указываем второстепенную деталь для сварки
            weld.TypeAbove = TSM.BaseWeld.WeldTypeEnum.WELD_TYPE_FILLET; // угловой шов
            weld.IntermittentType = TSM.BaseWeld.WeldIntermittentTypeEnum.CONTINUOUS; // форма сварнго шва "непрерывный"
            weld.AroundWeld = false; // кромка или по периметру
            weld.ShopWeld = true; // заводской
            weld.Insert();

            //Болты
            TSM.BoltArray boltArray = new TSM.BoltArray();
            boltArray.PartToBeBolted = column;
            boltArray.PartToBoltTo = plate;

            boltArray.FirstPosition = new TSG.Point(22880.54, 159, 6700);
            boltArray.SecondPosition = new TSG.Point(22880.54, 159, 7000);
            boltArray.BoltSize = 20; // размер
            boltArray.Tolerance = 3.00; // допуск
            boltArray.BoltStandard = "7798"; // стандарт болта
            boltArray.BoltType = TSM.BoltGroup.BoltTypeEnum.BOLT_TYPE_SITE; // заводской или монтажный
            boltArray.CutLength = 100; // длина разреза

            boltArray.Length = 100; // длина для упоров нельсона
            boltArray.ExtraLength = 15; // доп длина
            boltArray.ThreadInMaterial = TSM.BoltGroup.BoltThreadInMaterialEnum.THREAD_IN_MATERIAL_NO; // болт или нить???? ЧЗХ???

            boltArray.Position.Rotation = TSM.Position.RotationEnum.BELOW;
            boltArray.StartPointOffset.Dx = 150; // смещение по dX

            boltArray.Bolt = true;
            boltArray.Washer1 = true;
            boltArray.Washer2 = true;
            boltArray.Washer3 = true;
            boltArray.Nut1 = true;
            boltArray.Nut2 = true;

            boltArray.Hole1 = true;//специальные отверстия
            boltArray.Hole2 = true;//специальные отверстия
            boltArray.Hole3 = true;//специальные отверстия
            boltArray.Hole4 = true;//специальные отверстия
            boltArray.Hole5 = true;//специальные отверстия

            boltArray.AddBoltDistX(100);//интервал по x
            boltArray.AddBoltDistY(100);//интервал по у
            boltArray.Insert();


            model = model ?? new TSM.Model();
            model.CommitChanges();


            

        }

        private void btn_CreateDrawing_Click(object sender, RoutedEventArgs e)
        {
            model = model ?? new TSM.Model();
            Drawing drawing = new Drawing();
            drawing.Insert();
            model.CommitChanges();

        }
    }
}
