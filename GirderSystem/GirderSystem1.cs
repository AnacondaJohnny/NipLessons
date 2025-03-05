using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API2020;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using System.Collections;

namespace GirderSystem1
{
    public class GirderParameters : IGirderParameters
    {
        public struct BeamParameters : Iparameters //структура для хранения наших параметров балок
        {
        public string Name { get; set; }
        public string Material { get; set; }
        public string Profile { get; set; }
        public string Color { get; set; }
        }

        public Iparameters PrimaryBeam { get; set; } //атрибуты главной балки
        public Iparameters SecondaryBeam { get; set; } //атрибуты второстепенной балки
        public ArrayList Points { get; set; } // набор точек для построения системы 
        public double Width { get; set; } // ширина балочной клетки
        public double Step { get; set; }// шаг второстепенных балок
        public int ConnectionNumber { get; set; }// номер соединения компонента

        //конструктор
        public GirderParameters(double width, double step, string primaryProfile, string secondaryProfile, string material, ArrayList points, int connectionNumber)
        {
            PrimaryBeam = new BeamParameters() { Name = "GeneralBeam", Color = "2", Material = material, Profile = primaryProfile};
            SecondaryBeam = new BeamParameters() { Name = "StepBeam", Color = "20", Material = material, Profile = secondaryProfile };
            Points = points;
            Width = width;
            Step = step;
            ConnectionNumber = connectionNumber;

        }

    }
    
    public class GirderSystem
    {
        public List<Beam> Beams { get; set; } = new List<Beam>();
        public double Width { get; set; } // ширина балочной системы
        public double Step { get; set; }// шаг второстепенных балок
        public double Length { get; set; }// длина балочной системы
        public TSM.TransformationPlane TransformationPlane { get; set; }//положение рабочей плосости для создания системы балок
        int connectionNumber;


        //конструктор
        public GirderSystem(IGirderParameters parameters)
        {
            Width = parameters.Width;
            Step = parameters.Step;
            Length = CalculateLength(parameters.Points);
            connectionNumber = parameters.ConnectionNumber;
            TransformationPlane = CreateTransformationPlane(parameters.Points);
            GenerateGirder(parameters);

        }

        //методы
        private void GenerateGirder(IGirderParameters parameters) //Создание сетки балок
        {
            //Создаем 2 опорные балки нашей площадки
            Beam generalFirstBeam = new Beam(new TSG.Point(), new TSG.Point(Length, 0, 0));
            Beam generalSecondBeam = new Beam(new TSG.Point(0, Width, 0), new TSG.Point(Length, Width, 0));
            generalFirstBeam.Name = parameters.PrimaryBeam.Name;
            generalFirstBeam.Material = parameters.PrimaryBeam.Material;
            generalFirstBeam.Profile = parameters.PrimaryBeam.Profile;
            generalFirstBeam.Color = parameters.PrimaryBeam.Color;
            generalSecondBeam.Name = parameters.PrimaryBeam.Name;
            generalSecondBeam.Material = parameters.PrimaryBeam.Material;
            generalSecondBeam.Profile = parameters.PrimaryBeam.Profile;
            generalSecondBeam.Color = parameters.PrimaryBeam.Color;
            Beams.Add(generalFirstBeam); //добавляем основные балки в список
            Beams.Add(generalSecondBeam); //добавляем основные балки в список
            AddStepBeams(parameters); //добавляем второстепенные балки в список

            
        }

        private void AddStepBeams(IGirderParameters parameters) //Метод добавления второстепенных балок в список
        {
            double count = Math.Floor(Length / Step);
            double startPosition = 0;
            if (Length - (count*Step)>0)
            {
                startPosition = (Length - (count * Step)) / 2;
            }
            else
            {
                count += 1;
            }
            for (int i = 0; i < count; i++)
            {
                Beam b = new Beam(new TSG.Point(startPosition, 0, 0), new TSG.Point(startPosition, Width, 0));
                b.Name = parameters.SecondaryBeam.Name;//заполнение параметров 
                b.Profile = parameters.SecondaryBeam.Profile;
                b.Material = parameters.SecondaryBeam.Material;
                b.Color = parameters.SecondaryBeam.Color;

                Beams.Add(b); //добавляем второстепенные балки в список
                startPosition += Step; // инкрементируем стартовую позицию

            }

            
        }

        private TSM.TransformationPlane CreateTransformationPlane(ArrayList points) //Создание рабочей плоскости по двум точкам пришедшим из IGirderParameters
        {
            TSG.Point p1 = points[0] as TSG.Point;
            TSG.Point p2 = points[1] as TSG.Point;
            TSG.Vector vX = new TSG.Vector(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);// создаем вектор на основе 2-х точек
            vX.Normalize();//нормализуем вектор vX, который мы создали выше
            TSG.Vector vZ = new TSG.Vector(0, 0, 1);
            TSG.Vector vY = vZ.Cross(vX); // метод .Cross() возвращет вектор перпендикулярный vZ b Vx
            return new TSM.TransformationPlane(p1, vX, vY);


        }

        private double CalculateLength(ArrayList points) // возвращает длину нашей балочной клетки по двум точкам пришедшим из IGirderParameters
        {
            TSG.Point p1 = points[0] as TSG.Point;
            TSG.Point p2 = points[1] as TSG.Point;
            double length = Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
            return length;
        }
        public void Insert()
        {
            Beams.ForEach(b => b.Insert());
            CreateConnections();

        }

        private void CreateConnections()
        {
            TSM.Beam b1 = Beams[0].GetBeam();
            TSM.Beam b2 = Beams[1].GetBeam();
            TSM.Connection connection = new TSM.Connection();
            connection.Name = "Beam to beam connection";
            connection.Number = connectionNumber;
            connection.UpVector = new TSG.Vector(0, 0, 1);
            connection.PositionType = Tekla.Structures.PositionTypeEnum.COLLISION_PLANE;
            for (int c = 2; c < Beams.Count; c++)
            {
                TSM.Beam b = Beams[c].GetBeam();
                connection.SetPrimaryObject(b1);
                connection.SetSecondaryObject(b);
                connection.Insert();
                connection.SetPrimaryObject(b2);
                connection.Insert();


            }
        }
    }
}
