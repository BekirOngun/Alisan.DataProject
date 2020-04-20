using DataProject.Dal.Infrastructure;
using DataProject.Domain.Dto;
using IKYS.Dal.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataProject.Dal.Managers
{
    public class SicilManager
    {
        public List<PersonelDto> GetAylikIseGirenler()
        {
            var result = new List<PersonelDto>();

            try
            {
                using (var db = new Database())
                {
                    var repository = new SicilRepository(db.Context);
                    result = repository.GetAylikIseGirenler();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public DataTable GetHaftalikIseGirenler()
        {
            var result = new DataTable();

            try
            {
                using (var db = new Database())
                {
                    var repository = new SicilRepository(db.Context);
                    result = repository.GetHaftalikIseGirenler();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
