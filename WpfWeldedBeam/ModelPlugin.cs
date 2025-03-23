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
        
    }


    #region Непосредствено бизнес-логика плагина

    #endregion

    // Определяем имя и ссылкаемся на интерфейс
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
                TSG.Point startP = new TSG.Point(this.Positions[0]);
                TSG.Point endP = new TSG.Point(this.Positions[1]);

                IBeamParameters beamParams = new BeamParameters("Сварная балка",material: Data.material, profile: $"{Data.beamWeb}*{Data.beamHeight - 2*Data.beamFlange}", color: "3");

                Beam wldBeam0 = new Beam(startP, endP, beamParams);
                wldBeam0.DepthEnum = TSM.Position.DepthEnum.MIDDLE;
                wldBeam0.Insert();

                Beam wldBeam1 = new Beam(startP, endP, beamParams);
                wldBeam1.Profile = $"{Data.beamWidth}*{Data.beamFlange}";
                wldBeam1.PlaneOffset = Data.beamHeight/2 - Data.beamFlange/2;
                wldBeam1.DepthEnum = TSM.Position.DepthEnum.MIDDLE;
                wldBeam1.Insert();

                Beam wldBeam2 = new Beam(startP, endP, beamParams);
                wldBeam2.PlaneOffset = -wldBeam1.PlaneOffset;
                wldBeam2.DepthEnum = TSM.Position.DepthEnum.MIDDLE;
                wldBeam2.Profile = wldBeam1.Profile;
                wldBeam2.Insert();

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


                Model.CommitChanges();



            }
            catch (Exception ex)
            {

                MessageBox.Show($"Исключение: {ex.Message}");
            }



            return true;
        }
    }
}
