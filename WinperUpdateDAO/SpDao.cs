using System.Data.SqlClient;
using ConnectorDB;

namespace WinperUpdateDAO
{
    public abstract class SpDao
    {
        /// <summary>
        /// Atributo que almacena el contenido (datos) rescatados desde la BD.
        /// </summary>
        private SqlDataReader SqlDr { get; set; }

        /// <summary>
        /// Conector para acceder a SQL-Server 2008R2
        /// </summary>
        protected DbConnector Connector { get; set; }

        /// <summary>
        /// Listado de parámetros que se utilizarán en la  invocación del SP.
        /// </summary>
        //protected Dictionary<string, object> ParmsDictionary { get; private set; }
        protected ThDictionary ParmsDictionary { get; set; }

        /// <summary>
        /// Nombre del SP. P.e: "dbo.sp_mi_sp".
        /// </summary>
        protected string SpName { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa las propiedades con los valores por defecto.
        /// </summary>
        protected SpDao()
        {
            ParmsDictionary = new ThDictionary();
            Connector = new DbConnector();
            SpName = "";
        }

        /// <summary>
        /// Método genérico para implementar en las clases derivadas.
        /// </summary>
        /// <returns></returns>
        public virtual SqlDataReader Execute()
        {
            return SqlDr;
        }

        /// <summary>
        /// Imprime el nombre del SP que ejecuta la clase derivada.
        /// </summary>
        /// <returns> string con el nombre de un SP. </returns>
        public override string ToString()
        {
            return SpName ?? "";
        }
    }
}
