﻿using System;
using System.Data.SqlClient;

namespace WinperUpdateDAO
{
    public class CnaVersiones : SpDao
    {
        public new SqlDataReader Execute()
        {
            SpName = @" select * from Versiones";
            try
            {
                return Connector.ExecuteQuery(SpName, ParmsDictionary);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Error al ejecutar {0}: {1}", "Excute", ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}