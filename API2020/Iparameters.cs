using System.Collections;

namespace API2020
{
    public interface Iparameters //атрибуты для класса балки
    {
        string Name { get; set; }
        string Material { get; set; }
        string Profile { get; set; }
        string Color { get; set; }

    }
    public interface IGirderParameters
    {
        Iparameters PrimaryBeam { get; set; } //атрибуты главной балки
        Iparameters SecondaryBeam { get; set; } //атрибуты второстепенной балки
        ArrayList Points { get; set; } // набор точек для построения системы 
        double Width { get; set; } // ширина балочной клетки
        double Step { get; set; }// шаг второстепенных балок
        int ConnectionNumber { get; set; }// номер соединения компонента


    }
}
