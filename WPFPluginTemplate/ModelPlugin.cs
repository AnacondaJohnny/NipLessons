using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using TSMUI = Tekla.Structures.Model.UI;
using Tekla.Structures.Plugins;
using System.Collections;
using System.Windows;
using Tekla.Structures.Model;

namespace WPFPluginTemplate
{
    //Здесь реализуется логика работы нашего плагина (бизнес-логика)

    public class PluginData //Здесь хранятся свойства, которые мы передаем на вход нашей логике
    {
        [StructuresField("Profile")]
        public string Profile;
        [StructuresField("Material")]
        public string Material;
        [StructuresField("RebarSize")]
        public int RebarSize;
        [StructuresField("RebarStep")]
        public double RebarStep;

    }

    [Plugin("WPFPluginTemplate")] //Создаем имя нашего плагина (отображается в окне приложения и компоненты)
    [PluginUserInterface("WPFPluginTemplate.MainWindow")] //Указываем вызываемый интерфейс (без этого атрибута окно не будет вызываться)
    
    public class ModelPlugin : PluginBase //без этого наследования Текла не сможет прочитать нашу библиотеку
    {
        TSM.Model Model  { get; set; }
        PluginData Data { get; set; }
        public ModelPlugin(PluginData data)
        {
            Model = new TSM.Model();
            Data = data;
        }
        public override List<InputDefinition> DefineInput() //Этот метод определяет входные объекты (в нашем случае точки) для подачи в главный метод
        {
            List<InputDefinition> pointList = new List<InputDefinition>();
            TSMUI.Picker picker = new TSMUI.Picker();
            ArrayList pickedPoints = picker.PickPoints(TSMUI.Picker.PickPointEnum.PICK_TWO_POINTS); //Вот тут выбираем 2 точки
            pointList.Add(new InputDefinition(pickedPoints)); //Добавляем эти точки в наш дефинишн
            return pointList;
        }

        public override bool Run(List<InputDefinition> input) //Это главный метод нашего плагина. Здесь происходит вся логика
        {
            try
            {
                ArrayList array = input[0].GetInput() as ArrayList;
                CreateBeam(array);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return true;
        }

        private void CreateBeam(ArrayList array)
        {
            TSG.Point p1 = array[0] as TSG.Point;
            TSG.Point p2 = array[1] as TSG.Point;
            TSM.Beam b = new TSM.Beam(p1, p2);
            b.Profile.ProfileString = Data.Profile;
            b.Material.MaterialString = Data.Material;

            b.Insert();
            CrateSingleRebar(b);
            CreateGroupRebar(b);
            Model.CommitChanges();

        }

        private void CreateGroupRebar(Beam b)
        {
            //Получаем минимальную и максимальную точки описывающей объект рамки
            TSG.Point minP = b.GetSolid().MinimumPoint;
            TSG.Point maxP = b.GetSolid().MaximumPoint;

            //Вычисляем точки сечения
            //Сечение у минимальной точки
            TSG.Point p1 = minP;
            TSG.Point p2 = new TSG.Point(minP.X, minP.Y, maxP.Z);
            TSG.Point p3 = new TSG.Point(minP.X, maxP.Y, maxP.Z);
            TSG.Point p4 = new TSG.Point(minP.X, maxP.Y, minP.Z);
            //Сечение у максимальной точки
            TSG.Point p5 = new TSG.Point(maxP.X, minP.Y, minP.Z);
            TSG.Point p6 = new TSG.Point(maxP.X, minP.Y, maxP.Z);
            TSG.Point p7 = maxP;
            TSG.Point p8 = new TSG.Point(maxP.X, maxP.Y, minP.Z);

            TSM.RebarGroup rebarGroup = new TSM.RebarGroup();

            TSM.Polygon polygon = new TSM.Polygon();
            polygon.Points.Add(p1);
            polygon.Points.Add(p2);
            polygon.Points.Add(p3);
            polygon.Points.Add(p4);
            polygon.Points.Add(p1);
            TSM.Polygon polygon2 = new TSM.Polygon();
            polygon2.Points.Add(p5);
            polygon2.Points.Add(p6);
            polygon2.Points.Add(p7);
            polygon2.Points.Add(p8);
            polygon2.Points.Add(p5);

            rebarGroup.Polygons.Add(polygon);
            rebarGroup.Polygons.Add(polygon2);
            rebarGroup.RadiusValues = new ArrayList() { 25.0 };
            rebarGroup.SpacingType = BaseRebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_TARGET_SPACE;
            rebarGroup.Spacings = new ArrayList() { Data.RebarStep };
            rebarGroup.ExcludeType = BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            rebarGroup.Size = "12";
            rebarGroup.Father = b;
            rebarGroup.Name = "Group rebar";
            rebarGroup.Class = 9;
            rebarGroup.Grade = "А500";
            rebarGroup.NumberingSeries.Prefix = "rg";
            rebarGroup.NumberingSeries.StartNumber = 1;
            rebarGroup.OnPlaneOffsets = new ArrayList() { 25.0, 25.0, 25.0, 25.0 };
            rebarGroup.FromPlaneOffset = 25.0;
            rebarGroup.StartHook.Shape = RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            rebarGroup.StartHook.Angle = 90;
            rebarGroup.StartHook.Length = 100;
            rebarGroup.StartHook.Radius = 25;
            rebarGroup.EndHook.Shape = RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            rebarGroup.EndHook.Angle = 90;
            rebarGroup.EndHook.Length = 100;
            rebarGroup.EndHook.Radius = 25;
            rebarGroup.StartPointOffsetValue = 25;
            rebarGroup.EndPointOffsetValue = 25;
            rebarGroup.Insert();
        }

        private void CrateSingleRebar(Beam b)
        {
            //Получаем минимальную и максимальную точки описывающей объект рамки
            TSG.Point minP = b.GetSolid().MinimumPoint;
            TSG.Point maxP = b.GetSolid().MaximumPoint;

            //Вычисляем точки сечения
            //Сечение у минимальной точки
            TSG.Point p1 = minP;
            TSG.Point p2 = new TSG.Point(minP.X, minP.Y, maxP.Z);
            TSG.Point p3 = new TSG.Point(minP.X, maxP.Y, maxP.Z);
            TSG.Point p4 = new TSG.Point(minP.X, maxP.Y, minP.Z);
            //Сечение у максимальной точки
            TSG.Point p5 = new TSG.Point(maxP.X, minP.Y, minP.Z);
            TSG.Point p6 = new TSG.Point(maxP.X, minP.Y, maxP.Z);
            TSG.Point p7 = maxP;
            TSG.Point p8 = new TSG.Point(maxP.X, maxP.Y, minP.Z);

            double halfSize = Math.Round(Data.RebarSize / 2.0, 0);

            TSM.SingleRebar single = new TSM.SingleRebar();
            TSM.Polygon polygon = new TSM.Polygon();
            polygon.Points.Add(p2);
            polygon.Points.Add(p6);
            single.Polygon = polygon;
            single.Class = 2;
            single.Name = "Single rebar";
            single.NumberingSeries.Prefix = "sr";
            single.NumberingSeries.StartNumber = 1;
            single.Father = b;
            single.Grade = "А500";
            single.Size = Data.RebarSize.ToString();
            single.FromPlaneOffset = -25.00 - halfSize;
            single.OnPlaneOffsets = new ArrayList() { 25.00 + halfSize };
            single.Insert();

            single.OnPlaneOffsets[0] = -25.00 - halfSize;
            polygon.Points.Clear();
            polygon.Points.Add(p1);
            polygon.Points.Add(p5);
            single.Insert();

            single.OnPlaneOffsets[0] = 25.00 + halfSize;
            single.FromPlaneOffset = 25.00 + halfSize;
            polygon.Points.Clear();
            polygon.Points.Add(p3);
            polygon.Points.Add(p7);
            single.Insert();

            single.OnPlaneOffsets[0] = -25.00 - halfSize;
            single.FromPlaneOffset = 25.00 + halfSize;
            polygon.Points.Clear();
            polygon.Points.Add(p4);
            polygon.Points.Add(p8);
            single.Insert();





        }


    }
}
