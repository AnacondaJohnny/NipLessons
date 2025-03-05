using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using System.Collections;

namespace API2020
{
    class ContourPlate
    {
        //свойства
        private TSM.ContourPlate plate;

        //имя балки
        public string Name 
        {
            get { return plate.Name; } // get => plate.Name; (Эти формы записи идентичны)
            set { plate.Name = value; }// set => plate.Name; = value (Эти формы записи идентичны)
        }

        // класс пластина
        public string Color
        {
            get => plate.Class;
            set => plate.Class = value;
        }
        // профиль пластины
        public string Profile
        {
            get => plate.Profile.ProfileString;
            set => plate.Profile.ProfileString = value;
        }
        //Материал пластины
        public string Material
        {
            get => plate.Material.MaterialString;
            set => plate.Material.MaterialString = value;
        }
        //префикс сборки
        public string AssemblyPrefix
        {
            get => plate.AssemblyNumber.Prefix;
            set => plate.AssemblyNumber.Prefix = value;
        }
        //начальный номер сборки
        public int AssemblyStartNumber
        {
            get => plate.AssemblyNumber.StartNumber;
            set => plate.AssemblyNumber.StartNumber = value;
        }
        //перфикс детали
        public string PartPrefix
        {
            get => plate.PartNumber.Prefix;
            set => plate.PartNumber.Prefix = value;
        }
        //начальный номер детали
        public int PartStartNumber
        {
            get => plate.PartNumber.StartNumber;
            set => plate.PartNumber.StartNumber = value;
        }
        //положение по глубине
        public TSM.Position.DepthEnum DepthEnum
        {
            get => plate.Position.Depth;
            set => plate.Position.Depth = value;
        }
        //Отступ по глубине
        public double DepthOffset
        {
            get => plate.Position.DepthOffset;
            set => plate.Position.DepthOffset = value;
        }

        public ContourPlate() : this(new ArrayList { new TSM.ContourPoint(new TSG.Point(0, 0, 0), null),
                                                    new TSM.ContourPoint(new TSG.Point(0, 500, 0), null),
                                                    new TSM.ContourPoint(new TSG.Point(500, 500, 0), null),
                                                    new TSM.ContourPoint(new TSG.Point(500, 0, 0),null )}
                                                    , null) { }
        

        
        public ContourPlate(ArrayList points, Iparameters parameters)
        {
            plate = new TSM.ContourPlate();
            plate.Contour.ContourPoints = points;
            if (parameters is null)
            {
                SetDefaultValue();

            }
            else
            {
                SetParameters();
            }
            
        }

        


        //конструкторы
        //методы
        private void SetDefaultValue()
        {
            Name = "Plate";
            AssemblyPrefix = "NIPPL";
            AssemblyStartNumber = 1;
            PartPrefix = "NIPPL";
            PartStartNumber = 1;
            Material = "C245";
            Profile = "PL100";
            DepthEnum = TSM.Position.DepthEnum.BEHIND;
            DepthOffset = 0;

        }
        private void SetParameters() { }

        public void Insert()=>plate.Insert();
        public TSM.ContourPlate GetPlate() => plate;
        
    }
}
