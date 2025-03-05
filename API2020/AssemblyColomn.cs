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
    class AssemblyColomn
    {
        Beam mainPart;
        ContourPlate secodaryPart;
        TSM.Assembly assembly;

        public AssemblyColomn()
        {
            mainPart = new Beam(new TSG.Point(0, 0, 0), new TSG.Point(0, 0, 6000));
            secodaryPart = new ContourPlate(CalculatePlatePoints(), null);
            
            
        }
        public void Insert()
        {
            mainPart.Insert();
            secodaryPart.Insert();
            assembly = mainPart.GetBeam().GetAssembly();
            assembly.Add(secodaryPart.GetPlate());
            assembly.Modify();
        }

        private ArrayList CalculatePlatePoints()
        {
            ArrayList arrayList = new ArrayList();
            TSG.Point bP = mainPart.StartPoint;
            
            TSM.ContourPoint p1 = new TSM.ContourPoint(new TSG.Point(bP.X - 250, bP.Y - 250, bP.Z), null);
            TSM.ContourPoint p2 = new TSM.ContourPoint(new TSG.Point(bP.X - 250, bP.Y + 250, bP.Z), null);
            TSM.ContourPoint p3 = new TSM.ContourPoint(new TSG.Point(bP.X + 250, bP.Y + 250, bP.Z), null);
            TSM.ContourPoint p4 = new TSM.ContourPoint(new TSG.Point(bP.X + 250, bP.Y - 250, bP.Z), null);
            arrayList.Add(p1);
            arrayList.Add(p2);
            arrayList.Add(p3);
            arrayList.Add(p4);
            
            
            return arrayList;
        }
    }
}
