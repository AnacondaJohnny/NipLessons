using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Plugins;
using TSG = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;




namespace WpfWeldedBeam
{

    public class PluginData
    {

    }


    #region Непосредствено бизнес-логика плагина

    #endregion

    // Определяем имя и ссылкаемся на интерфейс
    [Plugin("WeldedBeam_EBS")]
    [PluginUserInterface("WpfWeldedBeam.MainWindow")]
    [CustomPartInputType(CustomPartInputType.INPUT_2_POINTS)]
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
            
            throw new NotImplementedException();
        }
    }
}
