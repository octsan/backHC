using Dapper;
using ERP_MaxysHC.Maxys.Model;
using System.Data.SqlClient;

namespace ERP_MaxysHC.Maxys.Data.RepositoriesSQL
{
    public class OCDocumentosRepository : IOCDocumentosRepository
    {
        private readonly SQLConfiguration _connectionString;

        public OCDocumentosRepository(SQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<OCDocumentos>> GetAllOCDocumentos()
        {
            var db = GetConnection();
            var query = "select CFECHA, CSERIEDOCUMENTO, CFOLIO, CRAZONSOCIAL, CTOTAL from admDocumentos where CSERIEDOCUMENTO = 'OCUA' and CIDDOCUMENTODE = 17 and CFECHA between '20240101' and '20251231' order by CFECHA desc";
            var documentos = await db.QueryAsync<OCDocumentos>(query);
            return documentos;
        }
    }
}
