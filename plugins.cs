using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using DBObject = Autodesk.AutoCAD.DatabaseServices.DBObject;


namespace Plugins {
  public class Plugins {
    private void ChangeAttributeBlock(BlockReference br, string tag, string value) {
      Document doc = Application.DocumentManager.MdiActiveDocument;
      using (Transaction tr = blkRef.Database.TransactionManager.StartTransaction()) {
        foreach (ObjectId attId in blkRef.AttributeCollection) {
          AttributeReference attRef = (AttributeReference)tr.GetObject(attId, OpenMode.ForWrite);
          if (attRef.Tag == tag) {
            attRef.TextString = value
          }
        }
        tr.Commit();
      }
    }
  }
}
