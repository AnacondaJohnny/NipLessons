using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tekla.Structures.Plugins;
using TSG = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;




namespace WpfWeldedBeam
{

    public class PluginData
    {
        [StructuresField("BeamHeight")]
        public double beamHeight;

        [StructuresField("BeamWidth")]
        public double beamWidth;

        [StructuresField("BeamFlange")]
        public double beamFlange;

        [StructuresField("BeamWeb")]
        public double beamWeb;

        [StructuresField("Material")]
        public string material;

        [StructuresField("WebClass")]
        public string webClass;

        [StructuresField("FlangeClass")]
        public string flangeClass;

        [StructuresField("Angle1")]
        public double angle1;

        [StructuresField("VerticalOffset")]
        public double verticalOffset;

        [StructuresField("HorizontalOffset")]
        public double horizontalOffset;

        [StructuresField("AssemblyPrefix")]
        public string assemblyPrefix;
    }


    #region Непосредствено бизнес-логика плагина

    #endregion

    // Определяем имя и ссылаемся на интерфейс
    [Plugin("WeldedBeam_EBS")]
    [PluginUserInterface("WpfWeldedBeam.MainWindow")]
    [CustomPartPositioningType(CustomPartPositioningType.POSITIONING_BY_INPUTPOINTS)]
    public class ModelPlugin : CustomPartBase
    {
        //Свойства
        PluginData Data { get; set; }
        TSM.Model Model { get; set; }

        //Конструткор
        public ModelPlugin(PluginData data)
        {   
            Model = new TSM.Model(); //Инициализируем подключение к Текле
            Data = data;
        }

        //Логика
        public override bool Run()
        {

            try
            {
                GetValuesFromDialog();
                #region 2 точки построения детали
                TSG.Point startP = new TSG.Point(this.Positions[0]);
                TSG.Point endP = new TSG.Point(this.Positions[1]);
                #endregion

                #region Определяем 3 балки (3 сварных листа)
                IBeamParameters beamParams = new BeamParameters("Сварная балка", material: Data.material, profile: $"{Data.beamWeb}*{Data.beamHeight - 2 * Data.beamFlange}", color: $"{Data.webClass}");

                Beam wldBeam0 = new Beam(startP, endP, beamParams);
                wldBeam0.DepthEnum = TSM.Position.DepthEnum.MIDDLE;
                wldBeam0.RotationOffset = Data.angle1;
                wldBeam0.DepthOffset = Data.horizontalOffset;
                wldBeam0.PlaneOffset = Data.verticalOffset;
                wldBeam0.AssemblyPrefix = Data.assemblyPrefix;
                wldBeam0.Insert();

                Beam wldBeam1 = new Beam(startP, endP, beamParams);
                wldBeam1.Profile = $"{Data.beamWidth}*{Data.beamFlange}";
                wldBeam1.PlaneOffset = Data.beamHeight / 2 - Data.beamFlange / 2 + Data.verticalOffset ;
                wldBeam1.DepthOffset = Data.horizontalOffset;
                wldBeam1.DepthEnum = TSM.Position.DepthEnum.MIDDLE;
                wldBeam1.RotationOffset = Data.angle1;
                wldBeam1.Color = Data.flangeClass;
                wldBeam1.Insert();

                Beam wldBeam2 = new Beam(startP, endP, beamParams);
                wldBeam2.PlaneOffset = -wldBeam1.PlaneOffset + 2*Data.verticalOffset;
                wldBeam2.DepthOffset = Data.horizontalOffset;
                wldBeam2.DepthEnum = TSM.Position.DepthEnum.MIDDLE;
                wldBeam2.RotationOffset = Data.angle1;
                wldBeam2.Profile = wldBeam1.Profile;
                wldBeam2.Color = Data.flangeClass;
                wldBeam2.Insert();
                #endregion

                #region Привариваем балки друг к другу
                TSM.Weld weld0 = new TSM.Weld();
                weld0.MainObject = wldBeam0.GetBeam();
                weld0.SecondaryObject = wldBeam1.GetBeam();
                weld0.TypeAbove = TSM.BaseWeld.WeldTypeEnum.WELD_TYPE_FILLET;
                weld0.TypeBelow = TSM.BaseWeld.WeldTypeEnum.WELD_TYPE_FILLET;
                weld0.IntermittentType = TSM.BaseWeld.WeldIntermittentTypeEnum.CONTINUOUS;
                weld0.AroundWeld = false;
                weld0.ShopWeld = true;
                weld0.SizeAbove = 10;
                weld0.SizeBelow = 10;
                weld0.Insert();

                weld0.SecondaryObject = wldBeam2.GetBeam();
                weld0.Insert();
                #endregion

                Model.CommitChanges();
                


            }
            catch (Exception ex)
            {

                MessageBox.Show($"Исключение: {ex.Message}");
            }



            return true;
        }
        private void GetValuesFromDialog()
        {
            if (IsDefaultValue(Data.beamHeight)) Data.beamHeight = 800;
            if (IsDefaultValue(Data.beamWidth)) Data.beamWidth = 400;
            if (IsDefaultValue(Data.beamFlange)) Data.beamFlange = 10;
            if (IsDefaultValue(Data.beamWeb)) Data.beamWeb = 14;
            if (IsDefaultValue(Data.material)) Data.material = "Steel_Undefined";
            if (IsDefaultValue(Data.webClass)) Data.webClass = "1";
            if (IsDefaultValue(Data.flangeClass)) Data.flangeClass = "2";
            if (IsDefaultValue(Data.angle1)) Data.angle1 = 0;
            if (IsDefaultValue(Data.verticalOffset)) Data.verticalOffset = 0;
            if (IsDefaultValue(Data.horizontalOffset)) Data.horizontalOffset = 0;
            if (IsDefaultValue(Data.assemblyPrefix)) Data.assemblyPrefix = "Б";
        }
    }
}
