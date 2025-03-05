using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using TSMUI = Tekla.Structures.Model.UI;

namespace API2020
{
    class Detail// компоненты типа "узел" в библиотеке Trimble представлены типом TSM.Detail class
    {
        private TSM.Detail detail;

        public Detail(TSM.Part part, int numberComponent)
        {
            detail = new TSM.Detail();
            detail.Name = "End Plate";
            detail.Number = numberComponent;
            detail.UpVector = new TSG.Vector();
            detail.DetailType = Tekla.Structures.DetailTypeEnum.END;
            detail.AutoDirectionType = Tekla.Structures.AutoDirectionTypeEnum.AUTODIR_DETAIL;
            detail.PositionType = Tekla.Structures.PositionTypeEnum.MIDDLE_PLANE;
            detail.SetPrimaryObject(part);
            //detail.SetReferencePoint(new TSG.Point(0, 0, 0));
            detail.SetReferencePoint(((TSM.Beam)part).GetCoordinateSystem().Origin);
            detail.SetAttribute("hpl1", 950);
            detail.SetAttribute("bpl1", 800);

            detail.LoadAttributesFromFile("standard");

        }
        public void Insert()
        {
            detail.Insert();
        }

    }
}
