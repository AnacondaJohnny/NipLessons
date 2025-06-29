﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

using TSG = Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Plugins;

namespace WPFPlugin
{
    public class PluginData
    {
        #region Fields
        //
        // Define the fields specified on the Form.
        //
        [StructuresField("name")]
        public string partName;

        [StructuresField("profile")]
        public string profile;

        [StructuresField("offset")]
        public double offset;

        [StructuresField("material")]
        public string material;

        [StructuresField("componentname")]
        public string componentname;

        [StructuresField("componentnumber")]
        public int componentnumber;

        [StructuresField("lengthfactor")]
        public int lengthfactor;

        [StructuresField("angle1")]
        public double Angle1;
        #endregion
    }

    [Plugin("WPFPlugin")]
    [PluginUserInterface("WPFPlugin.MainWindow")]
    public class WPFPlugin : PluginBase
    {
        #region Fields
        private Model _Model;
        private PluginData _Data;
        //
        // Define variables for the field values.
        //
        private string _PartName = string.Empty;
        private string _Profile = string.Empty;
        private string _Material = string.Empty;
        private double _Offset = 0.0;
        private int _LengthFactor = 0;
        private double _Angle1 = 0;
        #endregion

        #region Properties
        private Model Model
        {
            get { return this._Model; }
            set { this._Model = value; }
        }

        private PluginData Data
        {
            get { return this._Data; }
            set { this._Data = value; }
        }
        #endregion

        #region Constructor
        public WPFPlugin(PluginData data)
        {
            Model = new Model();
            Data = data;
        }
        #endregion

        #region Overrides
        public override List<InputDefinition> DefineInput()
        {
            //
            // This is an example for selecting two points; change this to suit your needs.
            //
            List<InputDefinition> PointList = new List<InputDefinition>();
            Picker Picker = new Picker();
            ArrayList PickedPoints = Picker.PickPoints(Picker.PickPointEnum.PICK_TWO_POINTS);

            PointList.Add(new InputDefinition(PickedPoints));

            return PointList;
        }

        public override bool Run(List<InputDefinition> Input)
        {
            try
            {
                GetValuesFromDialog();

                ArrayList Points = (ArrayList)Input[0].GetInput();
                TSG.Point StartPoint = Points[0] as TSG.Point;
                TSG.Point EndPoint = Points[1] as TSG.Point;

                TSG.Point LengthVector = new TSG.Point(EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y, EndPoint.Z - StartPoint.Z);

                if (_LengthFactor > 0)
                {
                    EndPoint.X = _LengthFactor * LengthVector.X + StartPoint.X;
                    EndPoint.Y = _LengthFactor * LengthVector.Y + StartPoint.Y;
                    EndPoint.Z = _LengthFactor * LengthVector.Z + StartPoint.Z;
                }

                Beam beam = new Beam(StartPoint, EndPoint);
                beam.Position.PlaneOffset = _Offset;
                beam.Name = _PartName;
                beam.Profile.ProfileString = _Profile;
                beam.Material.MaterialString = _Material;
                beam.Insert();


                #region Моделируем поворот балки через Tekla.Structures.Model.Operation
                double angle1 = _Angle1 / ((double)180 / Math.PI); //приводим угол поворота к радианам

                TSG.CoordinateSystem coordSys1 = new TSG.CoordinateSystem(beam.GetCoordinateSystem().Origin, beam.GetCoordinateSystem().AxisX, beam.GetCoordinateSystem().AxisY);
                TSG.Matrix rotationMatrix = TSG.MatrixFactory.Rotate(angle1, beam.GetCoordinateSystem().AxisX); //Создаем матрицу поворота

                TSG.Point pointNew1 = rotationMatrix.Transform(beam.GetCoordinateSystem().AxisY); //Применяем матрицу к оси Y
                TSG.CoordinateSystem coordSys2 = new TSG.CoordinateSystem(new TSG.Point(0, 0, 0), beam.GetCoordinateSystem().AxisX, new TSG.Vector(pointNew1));//Применяем поворот к координатной системе 

                
                Operation.MoveObject(beam, coordSys1, coordSys2);
                #endregion

                Operation.DisplayPrompt("Selected component " + _Data.componentname + " : " + _Data.componentnumber.ToString());

                
            }
            catch (Exception Exc)
            {
                MessageBox.Show(Exc.ToString());
            }

            return true;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Gets the values from the dialog and sets the default values if needed
        /// </summary>
        private void GetValuesFromDialog()
        {
            _PartName = Data.partName;
            _Profile = Data.profile;
            _Material = Data.material;
            _Offset = Data.offset;
            _LengthFactor = Data.lengthfactor + 1;
            _Angle1 = Data.Angle1;

            if (IsDefaultValue(_PartName))
                _PartName = "TEST";
            if (IsDefaultValue(_Profile))
                _Profile = "HEA200";
            if (IsDefaultValue(_Material))
                _Material = "STEEL_UNDEFINED";
            if (IsDefaultValue(_Offset))
                _Offset = 0;
            if (IsDefaultValue(_LengthFactor) || _LengthFactor == 0)
                _LengthFactor = 1;
            if (IsDefaultValue(_Angle1))
                _Angle1 = 0;
        }

        // Write your private methods here.

        #endregion
    }
}
