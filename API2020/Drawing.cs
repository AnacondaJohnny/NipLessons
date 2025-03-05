using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;


namespace API2020
{
    public class Drawing
    {
        

        //Поля и свойства
        TSD.Drawing drawing = new TSD.GADrawing();
        List<Beam> beams = new List<Beam>();

        //Конструкторы
        public Drawing(): this("Балочная клетка", new TSD.Size(420,297), new List<Beam>()) //цепочка методов
        {

        }
        public Drawing(string name, TSD.Size size, List<Beam> beams)
        {
            drawing.Layout.LoadAttributes("100_Выборка_металла"); // преднастроенная компоновка 
            drawing.Name = name;
            drawing.Layout.SheetSize = size;
            this.beams = beams;

        }

        //Методы
        public void Insert() // метод создания чертежа
        {
            drawing.Insert(); // Вставляем чертеж
            new TSD.DrawingHandler().SetActiveDrawing(drawing);//Делаем активным текущий чертеж. Это необходимо для того, чтобы втавить метки
            AddViews();

        }

        private void AddViews() // метод создания видов на чертеже
        {
            TSG.Point originCSPoint = new TSG.Point(); // точка отсчета для координатных систем (по умолчанию в нуле)
            //создаем координатные системы для наших видов
            
            TSG.CoordinateSystem csPlane = new TSG.CoordinateSystem(originCSPoint, new TSG.Vector(1, 0, 0), new TSG.Vector(0, 1, 0)); //Координатная система для вида в плане (глобальная система координат)
            TSG.CoordinateSystem csFront = new TSG.CoordinateSystem(originCSPoint, new TSG.Vector(1, 0, 0), new TSG.Vector(0, 0, 1)); //Координатная система для вида спереди (глобальная система координат)
            TSG.CoordinateSystem csLeft = new TSG.CoordinateSystem(originCSPoint, new TSG.Vector(0, -1, 0), new TSG.Vector(0, 0, 1)); //Координатная система для вида слева (глобальная система координат)

            TSM.TransformationPlane tP = new TSM.TransformationPlane(new TSG.CoordinateSystem());//в этот момент мы еще не вернули координатную систему в ноль в MinWindow.xaml.cs поэтому можно инициализировать TransformationPlane по умолчанию (в нулевой точке)
            if (beams.Count > 0)// корректируем Origin points для коориднатных систем планов (переставляем в начало нашей балочной клетки)
            {
                csPlane.Origin = tP.TransformationMatrixToGlobal.Transform(beams[0].StartPoint);//загвостка где-то тут!!!! Да, проблема в том как текла откладывает эти точки
                csFront.Origin = tP.TransformationMatrixToGlobal.Transform(beams[0].StartPoint);
                csLeft.Origin = tP.TransformationMatrixToGlobal.Transform(beams[0].StartPoint);
            }

            //Определяем координаты точек в системе параллельной глобальной системе координат (для более точного определения границ Restriction box)
            TSM.TransformationPlane csPlaneTP = TrPlaneGlobal(csPlane); //Transformation plane соответствующий нашей системе координат csPlane
            TSG.Point minPointPlane = new TSG.Point(beams[0].GetBeam().GetSolid().MinimumPoint);//Объявление и инициализация точек минимумуа и максимума в координатах рабочей плоскости
            TSG.Point maxPointPlane = new TSG.Point(beams[1].GetBeam().GetSolid().MaximumPoint);
            minPointPlane = new TSG.Point(csPlaneTP.TransformationMatrixToLocal.Transform(tP.TransformationMatrixToGlobal.Transform(minPointPlane)));//Точка минимума плана (координата точки в координатной системе csPlane)
            maxPointPlane = new TSG.Point(csPlaneTP.TransformationMatrixToLocal.Transform(tP.TransformationMatrixToGlobal.Transform(maxPointPlane)));//Точка максимума плана (координата точки в координатной системе csPlane)

            //Объявляем и редактируем границы restriction box для дальнейшей подачи в вид
            TSG.AABB planeAABB = new TSG.AABB(new TSG.Point(0, 0, -500), new TSG.Point(6000, 3000, 500)); //выносим restriction box для вида в плане
            if (beams.Count > 0)//корректируем граничные точки для restriction box в соответствии с солидом нашей балочной клетки
            {
                planeAABB.MinPoint.Y = minPointPlane.Y;
                planeAABB.MinPoint.X = minPointPlane.X;
                planeAABB.MinPoint.Z = beams[0].GetBeam().GetSolid().MinimumPoint.Z;

                planeAABB.MaxPoint.Y = maxPointPlane.Y;
                planeAABB.MaxPoint.X = maxPointPlane.X;
                planeAABB.MaxPoint.Z = beams[1].GetBeam().GetSolid().MaximumPoint.Z;
            }
            //Объявление и инициализация вида в плане. Этот класс работает с глобальными координатами!!!!!
            TSD.View viewPlane = new TSD.View(drawing.GetSheet(), csPlane, csPlane, planeAABB);
            viewPlane.Attributes.Scale = 50;
            viewPlane.Name = "Вид сверху";
            viewPlane.Origin = new TSG.Point(30, 150);
            viewPlane.Insert();
            //
            var parts = viewPlane.GetAllObjects(typeof(TSD.Part));
            while(parts.MoveNext()) //метод увеличивающий итератор внутри инумератора
            {
                TSD.ModelObject p = parts.Current as TSD.ModelObject; // приводим нашу деталь к ModelObject для того чтобы потом подать на вход в метку
                var b = beams.Where(w => w.GetBeam().Identifier.ID == p.ModelIdentifier.ID).FirstOrDefault(); //ВОТ С ЭТИМ ДЕРЬМОМ РАЗОБРАТЬСЯ!!!!! Запрашиваем балку, идентификатор которой равен идентификатору нашего ModelObject
                if (b != null)
                {
                    TSD.Mark mark = new TSD.Mark(p);
                    mark.Attributes.LoadAttributes("standard");
                    mark.Placing = new TSD.AlongLinePlacing(b.StartPoint, b.EndPoint);
                    mark.InsertionPoint = new TSG.Point(b.StartPoint.X + ((b.EndPoint.X - b.StartPoint.X) / 2), b.StartPoint.Y + ((b.EndPoint.Y - b.StartPoint.Y) / 2));
                }
            }




            TSG.AABB frontAABB = new TSG.AABB(new TSG.Point(0, -500, -500), new TSG.Point(6000, 100, 500));//выносим restriction box для вида спереди
            if (beams.Count > 0)//корректируем граничные точки для restriction box в соответствии с солидом нашей балочной клетки
            {
                frontAABB.MinPoint.Y = beams[0].GetBeam().GetSolid().MinimumPoint.Z;
                frontAABB.MinPoint.Z = beams[0].GetBeam().GetSolid().MinimumPoint.Y;
                frontAABB.MaxPoint.Y = beams[0].GetBeam().GetSolid().MaximumPoint.Z;
                frontAABB.MaxPoint.Z = beams[0].GetBeam().GetSolid().MaximumPoint.Y;
            }
            TSD.View viewFront = new TSD.View(drawing.GetSheet(), csFront, csFront, frontAABB);//Объявление и инициализация вида спереди
            viewFront.Attributes.Scale = 50;
            viewFront.Name = "Вид спереди";
            viewFront.Origin = new TSG.Point(30, 120);
            viewFront.Insert();

            //Создаем размер
            double length = 0;
            beams[0].GetBeam().GetReportProperty("LENGTH", ref length);
            TSD.StraightDimension dimensionLength = new TSD.StraightDimension(viewFront, beams[0].StartPoint, beams[0].EndPoint, new TSG.Vector(0,-1,0), length);
            dimensionLength.Insert();

            

            TSG.AABB leftAABB = new TSG.AABB(new TSG.Point(-3000, -500, -500), new TSG.Point(0, 100, 1500));//выносим restriction box для вида слева
            if (beams.Count > 0)//корректируем граничные точки для restriction box в соответствии с солидом нашей балочной клетки
            {
                leftAABB.MinPoint.X = -beams[1].GetBeam().GetSolid().MaximumPoint.Y;
                leftAABB.MinPoint.Y = beams[0].GetBeam().GetSolid().MinimumPoint.Z;
                if (beams[2].GetBeam().GetSolid().MinimumPoint.X < 0)
                    leftAABB.MinPoint.Z = -beams[2].GetBeam().GetSolid().MinimumPoint.X;
                else
                    leftAABB.MinPoint.Z = 0;
                leftAABB.MaxPoint.X = -beams[0].GetBeam().GetSolid().MinimumPoint.Y;
                leftAABB.MaxPoint.Y = beams[0].GetBeam().GetSolid().MaximumPoint.Z;
                leftAABB.MaxPoint.Z = -beams[2].GetBeam().GetSolid().MaximumPoint.X;
            }
            TSD.View viewLeft = new TSD.View(drawing.GetSheet(), csLeft, csLeft, leftAABB);//Объявление и инициализация вида слева
            viewLeft.Attributes.Scale = 50;
            viewLeft.Name = "Вид сбоку";
            viewLeft.Origin = new TSG.Point(250, 120);
            viewLeft.Insert();

            //Создаем размер
            double height = 0;
            beams[0].GetBeam().GetReportProperty("HEIGHT", ref height);
            TSD.StraightDimension dimensionHeight = new TSD.StraightDimension(viewLeft, beams[0].StartPoint, new TSG.Point(beams[0].StartPoint.X, beams[0].StartPoint.Z - height), new TSG.Vector(1, 0, 0), height);
            dimensionHeight.Insert();

            //Создаем прямоугольник
            TSD.Rectangle rectangle = new TSD.Rectangle(drawing.GetSheet(), new TSG.Point(20, 5), new TSG.Point(70, 30)); //Используем хитрость! Так как TSD.ContainerView наследуется от TSD.ViewBase, то обращаемся к нему как к TSD.ViewBase
            rectangle.Attributes.Line.Color = TSD.DrawingColors.Blue;
            rectangle.Insert();

            //Создаем текст
            TSD.Text text = new TSD.Text(drawing.GetSheet(), new TSG.Point(45, 15), "Примечание");
            text.Insert();
        }

      
        private TSM.TransformationPlane TrPlaneGlobal(CoordinateSystem targetCoordinateSystem)
        {
            TSG.Point csPlaneY = new TSG.Point(0, 1, 0); // точка в системе координат csPlane
            TSG.Point csPlaneX = new TSG.Point(1, 0, 0);
            TSG.Matrix fromCsPlane = TSG.MatrixFactory.FromCoordinateSystem(targetCoordinateSystem); // матрица перехода из координатной системы csPlane в текущую рабочую плоскость
            csPlaneY = new TSM.TransformationPlane(new TSG.CoordinateSystem()).TransformationMatrixToLocal.Transform(fromCsPlane.Transform(csPlaneY)); //точка в системе координат рабочей плоскости
            csPlaneX = new TSM.TransformationPlane(new TSG.CoordinateSystem()).TransformationMatrixToLocal.Transform(fromCsPlane.Transform(csPlaneX)); //точка в системе координат рабочей плоскости
            TSM.TransformationPlane tPGlobal = new TSM.TransformationPlane(new TSG.Point(), new TSG.Vector(csPlaneX), new TSG.Vector(csPlaneY));
            return tPGlobal;
        }
    }
}
