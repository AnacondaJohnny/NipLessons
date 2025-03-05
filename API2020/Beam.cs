using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;

namespace API2020
{
    public class Beam
    {
        //свойства
        private TSM.Beam beam;

        //имя балки
        public string Name
        {
            get { return beam.Name; } // get => beam.Name; (Эти формы записи идентичны)
            set { beam.Name = value; }// set => beam.Name; = value (Эти формы записи идентичны)
        }
        // класс балки
        public string Color
        {
            get => beam.Class;
            set => beam.Class = value;
        }
        // профиль балки
        public string Profile
        {
            get => beam.Profile.ProfileString;
            set => beam.Profile.ProfileString = value;
        }
        //Материал балки
        public string Material
        {
            get => beam.Material.MaterialString;
            set => beam.Material.MaterialString = value;
        }
        //Префикс сборки
        public string AssemblyPrefix
        {
            get => beam.AssemblyNumber.Prefix;
            set => beam.AssemblyNumber.Prefix = value;
        }
        //Начальный номер сборки
        public int AssemblyStartNumber
        {
            get => beam.AssemblyNumber.StartNumber;
            set => beam.AssemblyNumber.StartNumber = value;
        }
        //Префикс детали
        public string PartPrefix
        {
            get => beam.PartNumber.Prefix;
            set => beam.PartNumber.Prefix = value;
        }
        //Начальный номер детали
        public int PartStartNumber
        {
            get => beam.PartNumber.StartNumber;
            set => beam.PartNumber.StartNumber = value;
        }
        //Начальная точка
        public TSG.Point StartPoint
        {
            get => beam.StartPoint;
            set => beam.StartPoint = value;
        }
        //Конечная точка
        public TSG.Point EndPoint
        {
            get => beam.EndPoint;
            set => beam.EndPoint = value;
        }
        //Позиция по глубине
        public TSM.Position.DepthEnum DepthEnum
        {
            get => beam.Position.Depth;
            set => beam.Position.Depth = value;
        }
        //Отступ по глубине
        public double DepthOffset
        {
            get => beam.Position.DepthOffset;
            set => beam.Position.DepthOffset = value;
        }
        //Позиция в плоскости план
        public TSM.Position.PlaneEnum PlaneEnum
        {
            get => beam.Position.Plane;
            set => beam.Position.Plane = value;
        }
        //Отступ в плосокоти план
        public double PlaneOffset
        {
            get => beam.Position.PlaneOffset;
            set => beam.Position.PlaneOffset = value;
        }
        //Поворот
        public TSM.Position.RotationEnum RotationEnum
        {
            get => beam.Position.Rotation;
            set => beam.Position.Rotation = value;
        }
        //Отступ для поворота
        public double RotationOffset
        {
            get => beam.Position.RotationOffset;
            set => beam.Position.RotationOffset = value;
        }

        //конструктор
        public Beam() : this(TSM.Beam.BeamTypeEnum.BEAM)
        {

        }
        public Beam(TSM.Beam.BeamTypeEnum beamTypeEnum)
        {
            beam = beamTypeEnum switch
            {
                TSM.Beam.BeamTypeEnum.BEAM => new TSM.Beam(TSM.Beam.BeamTypeEnum.BEAM),
                TSM.Beam.BeamTypeEnum.COLUMN => new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN),
                TSM.Beam.BeamTypeEnum.PAD_FOOTING => new TSM.Beam(TSM.Beam.BeamTypeEnum.PAD_FOOTING),
                TSM.Beam.BeamTypeEnum.PANEL => new TSM.Beam(TSM.Beam.BeamTypeEnum.PANEL),
                TSM.Beam.BeamTypeEnum.STRIP_FOOTING => new TSM.Beam(TSM.Beam.BeamTypeEnum.STRIP_FOOTING),
                _ => new TSM.Beam()
            };
            SetDefaultValue();

        }
        public Beam(TSG.Point startP, TSG.Point endP)
        {
            beam = new TSM.Beam(startP, endP);
            SetDefaultValue(startP, endP);

        }
        public Beam(TSG.Point startP, TSG.Point endP, Iparameters parameter)
        {
            beam = new TSM.Beam(startP, endP);
            SetDefaultValue(startP, endP);
            SetParameters(parameter);
        }

        private void SetParameters(Iparameters parameters)
        {
            beam.Name = parameters.Name;
            beam.Profile.ProfileString = parameters.Profile;
            beam.Material.MaterialString = parameters.Material;
            beam.Class = parameters.Color;
            
        }




        //методы
        private void SetDefaultValue() => SetDefaultValue(new TSG.Point(0, 0, 0), new TSG.Point(6000, 0, 0)); // аналогичный способ ввода методов из 1 строки
        public void Insert() => beam.Insert(); // аналогичный способ ввода методов из 1 строки


        private void SetDefaultValue(TSG.Point startPoint, TSG.Point endPoint)
        {
            Name = "Beam";
            AssemblyPrefix = "B";
            AssemblyStartNumber = 1;
            PartPrefix = "DET";
            PartStartNumber = 1;
            Profile = "I30B2_20_93";
            Material = "C245";
            StartPoint = startPoint;
            EndPoint = endPoint;
            DepthEnum = TSM.Position.DepthEnum.BEHIND;
            DepthOffset = 0;
            PlaneEnum = TSM.Position.PlaneEnum.MIDDLE;
            PlaneOffset = 0;
            RotationEnum = TSM.Position.RotationEnum.BELOW;
            RotationOffset = 0;

        }
        public TSM.Beam GetBeam() 
        {
            return beam;
            
        }
        

        
        
            
        

    }
}
