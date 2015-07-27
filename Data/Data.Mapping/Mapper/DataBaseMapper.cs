using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CM.Tools.Misellaneous;
using CMData.Schemas;

namespace Data.Mapping.Mapper
{
    public class DataBaseMapper
    {
        #region Declaraciones

        public delegate void ProgressValueChangedHandler(string Text, int MaxValue, int Value);
        public delegate void ProgressIncrementHandler(ref bool Cancel);

        public event ProgressValueChangedHandler ProgressProcessChanged;
        public event ProgressValueChangedHandler ProgressActionChanged;
        public event ProgressIncrementHandler ProgressIncrementProcess;
        public event ProgressIncrementHandler ProgressIncrementAction;

        #endregion

        #region Constructores

        //public DataBaseMapper(XsdDataBase.TBL_ConnectionRow nConnection)
        //{
        //    _MapDataBase = new XsdDataBase();
        //    _Connection = nConnection;
        //}

        public DataBaseMapper(XsdDataBase nMapDataBase, XsdDataBase.TBL_ConnectionRow nConnection)
        {
            Log = new StringBuilder();
            MapDataBase = nMapDataBase;
            Connection = nConnection;
        }

        #endregion

        #region Propiedades

        public StringBuilder Log { get; set; }

        public XsdDataBase.TBL_ConnectionRow Connection { get; set; }

        public XsdDataBase MapDataBase { get; set; }

        #endregion

        #region Metodos

        public void LoadAndRefreshAllObjects(bool recursiveSelectedObjects, bool AddNewsOnly)
        {
            CMData.Manager.DBManager DBM = null;
            bool Cancel = false;

            try
            {
                if (Connection != null)
                {
                    Log = new StringBuilder();

                    if (Connection.IsConnection_TypeNull())
                    {
                        DBM = new CMData.Manager.DBManager(Connection.Connection_String);
                    }
                    else
                    {
                        var dbType = (CMData.DataBase.DataBaseType)(Enum.Parse(typeof(CMData.DataBase.DataBaseType), Connection.Connection_Type));
                        DBM = new CMData.Manager.DBManager(dbType, Connection.Connection_String);
                    }

                    DBM.Connection_Open();

                    // Guardar antigua configuracion actual para ser actualizada en los casos en los que sea posible
                    var newMapDataBase = new XsdDataBase();

                    if (ProgressProcessChanged != null) ProgressProcessChanged("Descargando", 3, 0);
                    
                    // Cargar lista de objetos
                    var Filtros = new List<string>();
                    foreach (CMData.Schemas.XsdDataBase.TBL_SchemaRow SchemaRow in MapDataBase.TBL_Schema)
                    {
                        if (SchemaRow.Maping_Schema)
                            Filtros.Add(SchemaRow.Schema_Name);
                    }

                    DBM.DataBase.FillDataBaseTables(newMapDataBase.TBL_Object, Connection, Filtros);
                    DBM.DataBase.FillDataBaseViews(newMapDataBase.TBL_Object, Connection, Filtros);
                    DBM.DataBase.FillDataBaseStoredProcedures(newMapDataBase.TBL_Object, Connection, Filtros);

                    //Eliminar objetos no utilizados
                    var deleteObjects = new List<XsdDataBase.TBL_ObjectRow>();

                    foreach (XsdDataBase.TBL_ObjectRow originalObject in MapDataBase.TBL_Object.Rows)
                    {
                        var existsObj = FindObjectRow(newMapDataBase, originalObject.Catalog_Name, originalObject.Schema_Name, originalObject.Object_Name);

                        if (existsObj == null)
                        {
                            if (originalObject.Generic_Type == "Table")                            
                                Log.AppendLine("La tabla " + originalObject.Object_Name + " no fue encontrada en la base de datos");                            
                            else if (originalObject.Generic_Type == "View")                            
                                Log.AppendLine("La vista " + originalObject.Object_Name + " no fue encontrada en la base de datos");                            
                            else                            
                                Log.AppendLine("El procedimiento almacenado " + originalObject.Object_Name + " no fue encontrado en la base de datos");                            

                            deleteObjects.Add(originalObject);
                        }
                    }

                    foreach (var deleteObject in deleteObjects)
                    {
                        deleteObject.Delete();
                    }

                    //Agregar nuevos objetos
                    foreach (XsdDataBase.TBL_ObjectRow newObject in newMapDataBase.TBL_Object.Rows)
                    {
                        var existsObj = FindObjectRow(MapDataBase, newObject.Catalog_Name, newObject.Schema_Name, newObject.Object_Name);

                        if (existsObj == null)
                        {
                            bool mapped = false;

                            try { mapped = newObject.Mapped; }
                            catch { }

                            MapDataBase.TBL_Object.AddTBL_ObjectRow(Connection.id_Connection, newObject.Generic_Type, newObject.Catalog_Name, newObject.Schema_Name, newObject.Object_Type, newObject.Object_Name, newObject.Selected, mapped);

                            if (newObject.Generic_Type == "Table")
                                Log.AppendLine("La tabla " + newObject.Object_Name + " ha sido agregado recientemente");
                            else if (newObject.Generic_Type == "View")
                                Log.AppendLine("La vista " + newObject.Object_Name + " ha sido agregado recientemente");
                            else
                                Log.AppendLine("El procedimiento almacenado " + newObject.Object_Name + " ha sido agregado recientemente");
                        }
                    }

                    // Se sale si solo se requiere cargar los nuevos
                    if (AddNewsOnly) return;

                    if (ProgressActionChanged != null) ProgressActionChanged("Leyendo estructura de tablas", MapDataBase.TBL_Object.Rows.Count, 0);

                    var newObjectRows = FindObjects(MapDataBase, Connection.id_Connection, "Table");

                    foreach (var newObject in newObjectRows)
                    {
                        //Restaurar la configuracion temporalmente guardada
                        if (newObject.Selected)
                        {
                            if (recursiveSelectedObjects)
                            {
                                var TempObject = newObject;
                                RefreshObjectTable(ref TempObject);
                                newObject.Mapped = true;
                            }
                            else
                            {
                                newObject.Mapped = false;
                            }
                        }

                        if (ProgressIncrementAction != null) ProgressIncrementAction(ref Cancel);
                        if (Cancel) throw new Exception("Cancelada por el usuario");
                    }

                    if (ProgressIncrementProcess != null) ProgressIncrementProcess(ref Cancel);
                    if (Cancel) throw new Exception("Cancelada por el usuario");

                    // Actualizar las vistas
                    newObjectRows = FindObjects(MapDataBase, Connection.id_Connection, "View");

                    if (ProgressActionChanged != null) ProgressActionChanged("Leyendo Vistas", newObjectRows.Length, 0);

                    foreach (var newObject in newObjectRows)
                    {
                        //Restaurar la configuracion temporalmente guardada
                        if (newObject.Selected)
                        {
                            if (recursiveSelectedObjects)
                            {
                                var TempObject = newObject;
                                RefreshObjectTable(ref TempObject);
                                newObject.Mapped = true;
                            }
                            else
                            {
                                newObject.Mapped = false;
                            }
                        }

                        if (ProgressIncrementAction != null) ProgressIncrementAction(ref Cancel);
                        if (Cancel) throw new Exception("Cancelada por el usuario");
                    }

                    if (ProgressIncrementProcess != null) ProgressIncrementProcess(ref Cancel);
                    if (Cancel) throw new Exception("Cancelada por el usuario");

                    // Actualizar los procedimientos almacenados
                    var newSpRows = FindObjects(MapDataBase, Connection.id_Connection, "StoredProcedure");

                    if (ProgressActionChanged != null) ProgressActionChanged("Leyendo Procedimientos", newSpRows.Length, 0);

                    foreach (var newObject in newSpRows)
                    {
                        if (newObject.Selected)
                        {
                            if (recursiveSelectedObjects)
                            {
                                var TempObject = newObject;
                                RefreshStoredProcedure(ref TempObject);
                                newObject.Mapped = true;
                            }
                            else
                            {
                                newObject.Mapped = false;
                            }
                        }

                        if (ProgressIncrementAction != null) ProgressIncrementAction(ref Cancel);
                        if (Cancel) throw new Exception("Cancelada por el usuario");
                    }

                    if (ProgressIncrementProcess != null) ProgressIncrementProcess(ref Cancel);
                    if (Cancel) throw new Exception("Cancelada por el usuario");

                    DBM.Connection_Close();
                }
                else
                {
                    throw new Exception("Debe seleccionar una conexión");
                }
            }
            catch (Exception ex)
            {
                if (DBM != null) DBM.Connection_Close();

                throw new Exception(ex.Message, ex);
            }
        }

        public void RefreshStoredProcedure(ref XsdDataBase.TBL_ObjectRow nOriginalStoredProcedure)
        {
            // Consultar la nueva configuracion
            var newMapStoredProcedure = LoadAndRestoreStoredProcedure(nOriginalStoredProcedure, Log);

            //Restaurar informacion basica
            nOriginalStoredProcedure.Generic_Type = newMapStoredProcedure.Generic_Type;
            nOriginalStoredProcedure.Object_Type = newMapStoredProcedure.Object_Type;
            nOriginalStoredProcedure.Catalog_Name = newMapStoredProcedure.Catalog_Name;
            nOriginalStoredProcedure.Schema_Name = newMapStoredProcedure.Schema_Name;
            nOriginalStoredProcedure.Object_Name = newMapStoredProcedure.Object_Name;

            // Restaurar parametros
            var fields = nOriginalStoredProcedure.GetTBL_FieldRows();

            foreach (var field in fields)
            {
                MapDataBase.TBL_Field.RemoveTBL_FieldRow(field);
            }

            try
            {
                fields = newMapStoredProcedure.GetTBL_FieldRows();

                foreach (var parameter in fields)
                {
                    MapDataBase.TBL_Field.AddTBL_FieldRow(nOriginalStoredProcedure,
                        parameter.Field_Name,
                        parameter.Field_Type,
                        parameter.Specific_Type,
                        parameter.Is_Nullable,
                        parameter.Max_Length,
                        parameter.Precision,
                        parameter.Scale,
                        parameter.PrimaryKey_Order,
                        parameter.Direction);
                }
            }
            catch (Exception ex)
            {
                Log.AppendLine(ex.Message);
            }
        }

        public void RefreshObjectTable(ref XsdDataBase.TBL_ObjectRow nOriginalTable)
        {
            // Consultar la nueva configuracion
            var newMapTable = LoadAndRestoreObjectTable(nOriginalTable, Log);

            // Restaurar informacion basica
            nOriginalTable.Generic_Type = newMapTable.Generic_Type;
            nOriginalTable.Object_Type = newMapTable.Object_Type;
            nOriginalTable.Catalog_Name = newMapTable.Catalog_Name;
            nOriginalTable.Schema_Name = newMapTable.Schema_Name;
            nOriginalTable.Object_Name = newMapTable.Object_Name;

            nOriginalTable.Selected = newMapTable.Selected;
            nOriginalTable.Mapped = newMapTable.Mapped;

            // Restaurar columnas
            var fields = nOriginalTable.GetTBL_FieldRows();

            foreach (var field in fields)
            {
                field.Delete();
            }

            try
            {
                fields = newMapTable.GetTBL_FieldRows();

                foreach (var newMapField in fields)
                {
                    var field = MapDataBase.TBL_Field.AddTBL_FieldRow(nOriginalTable,
                        newMapField.Field_Name,
                        newMapField.Field_Type,
                        newMapField.Specific_Type,
                        newMapField.Is_Nullable,
                        newMapField.Max_Length,
                        newMapField.Precision,
                        newMapField.Scale,
                        newMapField.PrimaryKey_Order,
                        newMapField.Direction);
                    try
                    {
                        var relations = newMapField.GetTBL_RelationRows();

                        foreach (var newMapRelation in relations)
                        {
                            MapDataBase.TBL_Relation.AddTBL_RelationRow(field,
                                newMapRelation.Relation_Name,
                                newMapRelation.Table_Name,
                                newMapRelation.Column_Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.AppendLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AppendLine(ex.Message);
            }

            // Restaurar filtros
            var filters = nOriginalTable.GetTBL_FilterRows();

            foreach (var nFilter in filters)
            {
                nFilter.Delete();
            }

            try
            {
                filters = newMapTable.GetTBL_FilterRows();

                foreach (var newMapFilter in filters)
                {
                    var nFilter = MapDataBase.TBL_Filter.AddTBL_FilterRow(nOriginalTable, newMapFilter.Name);

                    try
                    {
                        var filterFields = newMapFilter.GetTBL_Filter_FieldRows();

                        foreach (var newFilterField in filterFields)
                        {
                            MapDataBase.TBL_Filter_Field.AddTBL_Filter_FieldRow(nFilter,
                                newFilterField.Filter_Order,
                                newFilterField.Field_Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.AppendLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AppendLine(ex.Message);
            }
        }

        public void RemoveObjectsReturnedInStoredProcedures(XsdDataBase ds, int id_Connection)
        {
            var spRows = FindObjects(ds, id_Connection, "StoredProcedure");

            foreach (var spRow in spRows)
            {
                var oldSPReturns = (XsdDataBase.TBL_SP_ReturnRow[])ds.TBL_SP_Return.Select("fk_Object = " + spRow.id_Object);

                if (oldSPReturns.Length > 0)
                {
                    var Return_Type = (ReturnType)(Enum.Parse(typeof(ReturnType), oldSPReturns[0].Return_Type));

                    if (Return_Type == ReturnType.TablaTipada)
                    {
                        ds.TBL_SP_Return.RemoveTBL_SP_ReturnRow(oldSPReturns[0]);
                    }
                }
            }
        }

        public void RemoveObjects(XsdDataBase ds, int id_Connection, string objectType)
        {
            DataRow[] rows = ds.TBL_Object.Select("fk_Connection = " + id_Connection + " AND Generic_Type = '" + objectType + "'");

            foreach (var row in rows)
            {
                row.Delete();
            }
        }

        public void CompareAndRestoreObjectStoredProcedure(XsdDataBase.TBL_ObjectRow originalObject, XsdDataBase.TBL_ObjectRow newObject, ref StringBuilder log)
        {
            var strLogObject = new StringBuilder("");

            if (originalObject != null)
            {
                newObject.Catalog_Name = originalObject.Catalog_Name;
                newObject.Schema_Name = originalObject.Schema_Name;
                newObject.Object_Type = originalObject.Object_Type;
                newObject.Object_Name = originalObject.Object_Name;
                newObject.Selected = originalObject.Selected;
                newObject.Mapped = originalObject.Mapped;

                if (strLogObject.ToString() != "")
                {
                    log.AppendLine("Se encontraron errores en el procedimiento almacenado [" + originalObject.Object_Name + "]");
                    log.AppendLine(strLogObject.ToString());
                }
            }
            else
            {
                log.AppendLine("El procedimiento almacenado [" + newObject.Object_Name + "] ha sido agregado recientemente");
            }            
        }

        public void RestoreOriginalObjectStoredProcedure(XsdDataBase originalDataBase, XsdDataBase.TBL_ObjectRow oldObject, XsdDataBase.TBL_ObjectRow newObject, ref StringBuilder log, XsdDataBase dtsDataBase)
        {
            var strLogObject = new StringBuilder("");

            if (oldObject != null)
            {
                newObject.Selected = oldObject.Selected;

                // Recuperar las columnas
                var oldColumns = (XsdDataBase.TBL_FieldRow[])(originalDataBase.TBL_Field.Select("fk_Object = " + oldObject.id_Object));

                foreach (var oldColumn in oldColumns)
                {
                    var newColumn = dtsDataBase.TBL_Field.NewTBL_FieldRow();

                    newColumn.fk_Object = newObject.id_Object;
                    newColumn.Field_Name = oldColumn.Field_Name;
                    newColumn.Field_Type = oldColumn.Field_Type;
                    newColumn.Specific_Type = oldColumn.Specific_Type;
                    newColumn.Is_Nullable = oldColumn.Is_Nullable;
                    newColumn.Max_Length = oldColumn.Max_Length;
                    newColumn.Precision = oldColumn.Precision;
                    newColumn.Scale = oldColumn.Scale;
                    newColumn.PrimaryKey_Order = oldColumn.PrimaryKey_Order;
                    newColumn.Direction = oldColumn.Direction;
                    dtsDataBase.TBL_Field.AddTBL_FieldRow(newColumn);
                }

                newObject.Mapped = true;

                if (strLogObject.ToString() != "")
                {
                    log.AppendLine("Se encontraron errores en el procedimiento almacenado [" + oldObject.Object_Name + "]");
                    log.AppendLine(strLogObject.ToString());
                    newObject.Mapped = false;
                }
            }
            else
            {
                log.AppendLine("El procedimiento almacenado [" + newObject.Object_Name + "] ha sido agregado recientemente");
                newObject.Mapped = false;
            }            
        }

        public void RestoreObjectsReturnedInStoredProceduresOld(XsdDataBase dsOriginal, XsdDataBase dsNew, int id_Connection, ref StringBuilder log)
        {
            var spOriginalObjects = FindObjects(dsOriginal, id_Connection, "StoredProcedure");

            foreach (var spOriginalObject in spOriginalObjects)
            {
                var strLog = new StringBuilder("");
                var originalReturn = (XsdDataBase.TBL_SP_ReturnRow[])(dsOriginal.TBL_SP_Return.Select("fk_Object = " + spOriginalObject.id_Object));

                if (originalReturn.Length > 0)
                {
                    var spNewObject = FindObjectRow(dsNew, spOriginalObject.Catalog_Name, spOriginalObject.Schema_Name, spOriginalObject.Object_Name);

                    if (spNewObject != null)
                    {
                        var newReturn = dsNew.TBL_SP_Return.NewTBL_SP_ReturnRow();

                        newReturn.fk_Object = spNewObject.id_Object;
                        newReturn.Return_Type = originalReturn[0].Return_Type;
                        newReturn.Data_Type_Returned = originalReturn[0].Data_Type_Returned;
                        dsNew.TBL_SP_Return.AddTBL_SP_ReturnRow(newReturn);

                        var Return_Type = (ReturnType)(Enum.Parse(typeof(ReturnType), originalReturn[0].Return_Type));

                        if (Return_Type == ReturnType.TablaTipada)
                        {
                            newReturn.Schema_Name_Returned = originalReturn[0].Schema_Name_Returned;
                            newReturn.Object_Name_Returned = originalReturn[0].Object_Name_Returned;
                        }

                        if (strLog.ToString() != "")
                        {
                            dsNew.TBL_SP_Return.RemoveTBL_SP_ReturnRow(newReturn);
                            log.AppendLine("Se Eliminó el tipo de dato retornado del objeto [" + spNewObject.Object_Name + "] ");
                            log.AppendLine(strLog.ToString());
                        }
                    }
                    else
                    {
                        log.AppendLine("El Objeto [" + spOriginalObject.Object_Name + "] no encontrado en la nueva base de datos");
                    }
                }
            }
        }

        public void CompareOriginalVsNewObjects(XsdDataBase originalDataBase, XsdDataBase newDataBase, ref StringBuilder log, XsdDataBase.TBL_ConnectionRow SelectedConnection)
        {
            var originalobjects = FindObjects(originalDataBase, SelectedConnection.id_Connection, "Table");

            foreach (var originalObject in originalobjects)
            {
                var newObject = FindObjectRow(newDataBase, originalObject.Catalog_Name, originalObject.Schema_Name, originalObject.Object_Name);

                if (newObject == null)
                {
                    log.AppendLine("La tabla [" + originalObject.Object_Name + "] ha sido eliminada de la base de datos ");
                }
            }

            originalobjects = FindObjects(originalDataBase, SelectedConnection.id_Connection, "View");

            foreach (var originalObject in originalobjects)
            {
                var newObject = FindObjectRow(newDataBase, originalObject.Catalog_Name, originalObject.Schema_Name, originalObject.Object_Name);

                if (newObject == null)
                {
                    log.AppendLine("La vista [" + originalObject.Object_Name + "] ha sido eliminada de la base de datos ");
                }
            }

            originalobjects = FindObjects(originalDataBase, SelectedConnection.id_Connection, "StoredProcedure");

            foreach (var originalObject in originalobjects)
            {
                var newObject = FindObjectRow(newDataBase, originalObject.Catalog_Name, originalObject.Schema_Name, originalObject.Object_Name);

                if (newObject == null)
                {
                    log.AppendLine("El procedimiento [" + originalObject.Object_Name + "] ha sido eliminado de la base de datos ");
                }
            }
        }

        #endregion

        #region Funciones

        public XsdDataBase.TBL_ObjectRow LoadAndRestoreStoredProcedure(XsdDataBase.TBL_ObjectRow nOriginalStoredProcedure, StringBuilder nLog)
        {
            CMData.Manager.DBManager DBM = null;

            try
            {
                if (Connection != null)
                {
                    if (Connection.IsConnection_TypeNull())
                    {
                        DBM = new CMData.Manager.DBManager(Connection.Connection_String);
                    }
                    else
                    {
                        var dbType = (CMData.DataBase.DataBaseType)(Enum.Parse(typeof(CMData.DataBase.DataBaseType), Connection.Connection_Type));
                        DBM = new CMData.Manager.DBManager(dbType, Connection.Connection_String);
                    }

                    DBM.Connection_Open();

                    var newDataBase = new XsdDataBase();
                    var newObject = newDataBase.TBL_Object.NewTBL_ObjectRow();

                    newObject.Generic_Type = nOriginalStoredProcedure.Generic_Type;
                    newObject.Object_Type = nOriginalStoredProcedure.Object_Type;
                    newObject.Catalog_Name = nOriginalStoredProcedure.Catalog_Name;
                    newObject.Schema_Name = nOriginalStoredProcedure.Schema_Name;
                    newObject.Object_Name = nOriginalStoredProcedure.Object_Name;

                    newDataBase.TBL_Object.AddTBL_ObjectRow(newObject);

                    DBM.DataBase.FillDataBaseParameters(newDataBase.TBL_Field, newObject);

                    try { newObject.Selected = nOriginalStoredProcedure.Selected; }
                    catch { }
                    newObject.Mapped = true;

                    try
                    {
                        var originalSpReturn = nOriginalStoredProcedure.GetTBL_SP_ReturnRows()[0];
                        var newSpReturn = newDataBase.TBL_SP_Return.NewTBL_SP_ReturnRow();

                        newSpReturn.fk_Object = newObject.id_Object;
                        newSpReturn.Return_Type = originalSpReturn.Return_Type;
                        newSpReturn.Data_Type_Returned = originalSpReturn.Data_Type_Returned;
                        newSpReturn.Schema_Name_Returned = originalSpReturn.Schema_Name_Returned;
                        newSpReturn.Object_Name_Returned = originalSpReturn.Object_Name_Returned;

                        newDataBase.TBL_SP_Return.AddTBL_SP_ReturnRow(newSpReturn);
                    }
                    catch { }

                    DBM.Connection_Close();

                    return newObject;
                }

                throw new Exception("Se debe seleccionar una conexión");
            }
            catch (Exception ex)
            {
                if (DBM != null) DBM.Connection_Close();
                throw new Exception(ex.Message);
            }
        }

        public XsdDataBase.TBL_ObjectRow LoadAndRestoreObjectTable(XsdDataBase.TBL_ObjectRow nOriginalTable, StringBuilder nLog)
        {
            CMData.Manager.DBManager DBM = null;

            try
            {
                if (Connection != null)
                {
                    if (Connection.IsConnection_TypeNull())
                    {
                        DBM = new CMData.Manager.DBManager(Connection.Connection_String);
                    }
                    else
                    {
                        var dbType = (CMData.DataBase.DataBaseType)(Enum.Parse(typeof(CMData.DataBase.DataBaseType), Connection.Connection_Type));
                        DBM = new CMData.Manager.DBManager(dbType, Connection.Connection_String);
                    }

                    DBM.Connection_Open();

                    var newDataBase = new XsdDataBase();
                    var newObject = newDataBase.TBL_Object.NewTBL_ObjectRow();

                    newObject.Generic_Type = nOriginalTable.Generic_Type;
                    newObject.Object_Type = nOriginalTable.Object_Type;
                    newObject.Catalog_Name = nOriginalTable.Catalog_Name;
                    newObject.Schema_Name = nOriginalTable.Schema_Name;
                    newObject.Object_Name = nOriginalTable.Object_Name;

                    newDataBase.TBL_Object.AddTBL_ObjectRow(newObject);

                    DBM.DataBase.FillDataTableColumns(newDataBase.TBL_Field, newDataBase.TBL_Relation, newObject);
                    DBM.Connection_Close();

                    try { newObject.Selected = nOriginalTable.Selected; }
                    catch { }
                    newObject.Mapped = true;

                    //Restaurar filtros
                    try
                    {
                        var filters = nOriginalTable.GetTBL_FilterRows();

                        foreach (var originalFilter in filters)
                        {
                            var newFilter = newDataBase.TBL_Filter.NewTBL_FilterRow();

                            newFilter.fk_Object = newObject.id_Object;
                            newFilter.Name = originalFilter.Name;
                            newDataBase.TBL_Filter.AddTBL_FilterRow(newFilter);

                            var filterFields = originalFilter.GetTBL_Filter_FieldRows();
                            bool isNewFilterFieldsComplete = true;

                            foreach (var originalFilterField in filterFields)
                            {
                                var fieldRows = newDataBase.TBL_Field.Select("Field_Name = '" + originalFilterField.Field_Name + "'");

                                if (fieldRows.Length > 0)
                                {
                                    var newFilterField = newDataBase.TBL_Filter_Field.NewTBL_Filter_FieldRow();

                                    newFilterField.fk_Filter = newFilter.id_Filter;
                                    newFilterField.Field_Name = originalFilterField.Field_Name;
                                    newFilterField.Filter_Order = originalFilterField.Filter_Order;

                                    newDataBase.TBL_Filter_Field.AddTBL_Filter_FieldRow(newFilterField);
                                }
                                else
                                {
                                    nLog.AppendLine(ControlChars.Tab + "El campo con nombre [" + originalFilterField.Field_Name + "] no fue encontrado en la base de datos");
                                    isNewFilterFieldsComplete = false;
                                }
                            }

                            if (!isNewFilterFieldsComplete)
                            {
                                nLog.AppendLine(ControlChars.Tab + "El filtro con nombre [" + originalFilter.Name + "] no fue agregado debido a que no coinciden sus campos de filtrado");
                                newFilter.Delete();
                            }
                        }
                    }
                    catch { }

                    return newObject;
                }

                throw new Exception("Se debe seleccionar una conexión");
            }
            catch (Exception ex)
            {
                if (DBM != null) DBM.Connection_Close();
                throw new Exception(ex.Message, ex);
            }
        }

        public XsdDataBase.TBL_ObjectRow[] FindObjects(XsdDataBase ds, int id_Connection, string Generic_Type)
        {
            return (XsdDataBase.TBL_ObjectRow[])(ds.TBL_Object.Select("fk_Connection = " + id_Connection + " AND Generic_Type = '" + Generic_Type + "'"));
        }

        public XsdDataBase.TBL_ObjectRow FindObjectRow(XsdDataBase ds, int id_Object)
        {
            var objects = (XsdDataBase.TBL_ObjectRow[])(ds.TBL_Object.Select("id_Object = " + id_Object));

            return objects.Length > 0 ? objects[0] : null;
        }

        public XsdDataBase.TBL_ObjectRow FindObjectRow(XsdDataBase ds, string catalogName, string schemaName, string objectName)
        {
            var objects = (XsdDataBase.TBL_ObjectRow[])(ds.TBL_Object.Select("Catalog_Name = '" + catalogName + "' AND Schema_Name = '" + schemaName + "' AND Object_Name = '" + objectName + "'"));

            return objects.Length > 0 ? objects[0] : null;
        }

        #endregion
    }
}