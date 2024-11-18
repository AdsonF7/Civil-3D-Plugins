namespace Plugins
{
  public void SetXData(DBObject dbobject, List<Tuple<string, dynamic, int>> data, Transaction tr)
  {
    Database db = dbobject.Database;
    RegAppTable regaapptable = (RegAppTable)tr.GetObject(db.RegAppTableId, OpenMode.ForRead);
    ResultBuffer resultbuffer = new ResultBuffer(new TypedValue((int)DxfCode.ExtendedDataRegAppName, regAppName));

    if (!regaapptable.Has(regAppName))
    {
      regaapptable.UpgradeOpen();
      RegAppTableRecord regapptablerecord = new RegAppTableRecord();
      regapptablerecord.Name = regAppName;
      regaapptable.Add(regapptablerecord);
      tr.AddNewlyCreatedDBObject(regapptablerecord, true);
    }

    foreach (Tuple<string, dynamic, int> item in data)
    {
      resultbuffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, item.Item1));
      resultbuffer.Add(new TypedValue(item.Item3, item.Item2));
    }

    dbobject.XData = resultbuffer;
    resultbuffer.Dispose();
  }
}
