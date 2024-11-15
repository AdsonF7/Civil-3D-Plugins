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
    
    //Mudar atributo de uma referencia de bloco
    private void ChangeAttributeBlock(BlockReference blkRef, string tag, string value)
    {
      Document doc = Application.DocumentManager.MdiActiveDocument;
      using (Transaction tr = blkRef.Database.TransactionManager.StartTransaction())
      {
        foreach (ObjectId attId in blkRef.AttributeCollection)
        {
          AttributeReference attRef = (AttributeReference)tr.GetObject(attId, OpenMode.ForWrite);
          if (attRef.Tag == tag)
          {
            attRef.TextString = value
          }
        }
        tr.Commit();
      }
    }

    //Recuperar Atributo de Bloco Dinamico
    public GetDynamicBlockParameterValues(BlockReference blkRef, string property)
    {
      string value = null;
      if (blkRef.IsDynamicBlock)
      {
        DynamicBlockReferencePropertyCollection propCollection = blkRef.DynamicBlockReferencePropertyCollection;
        foreach (DynamicBlockReferenceProperty prop in propCollection)
        {
          if (prop.PropertyName == property)
          {
            value = prop.Value.ToString();
          }
        }
      }
      else {
        System.Console.WriteLine("Is not dynamic block");
      }
      return value;
    }

    //Recuperar Atributo de Texto
    private string GetCustomAttributeValue(BlockReference blkRef, string attributeName)
    {
      string attributeValue = null;
      using (Transaction tr = blkRef.Database.TransactionManager.StartTransaction())
      {
        foreach (ObjectId attId in blkRef.AttributeCollection)
        {
          AttributeReference attRef = (AttributeReference)tr.GetObject(attId, OpenMode.ForRead);
          if (attRef.Tag.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
          {
            attributeValue = attRef.TextString;
            break;
          }
        }
        tr.Commit();
      }    
      return attributeValue ?? "Atributo não encontrado";
    }

    //Recuperar Atributo de Dicionario
    private string GetCustomDataFromExtensionDictionary(BlockReference blkRef, string key)
    {
      string customData = null;
      Document doc = Application.DocumentManager.MdiActiveDocument;
      if (blkRef.ExtensionDictionary.IsNull)
      {
        return "Nenhum dicionário de extensão encontrado.";
      }
      using (Transaction tr = blkRef.Database.TransactionManager.StartTransaction())
      { 
        DBDictionary extDict = (DBDictionary)tr.GetObject(blkRef.ExtensionDictionary, OpenMode.ForRead);
        if (extDict.Contains(key))
        {
          DBObject obj = tr.GetObject(extDict.GetAt(key), OpenMode.ForRead);
          if (obj is Xrecord xrec)
          {
            ResultBuffer rb = xrec.Data;
            foreach (TypedValue tv in rb)
            {
              if (tv.TypeCode == (int)DxfCode.Text)
              {
                customData = tv.Value.ToString();
                break;
              }
            }
          }
        }
        tr.Commit();
      }
      return customData ?? "não encontrado"; // Exemplo de retorno
    }
  }
}
