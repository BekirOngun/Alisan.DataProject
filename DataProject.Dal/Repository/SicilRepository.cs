using DataProject.Domain;
using DataProject.Domain.Dto;
using IKYS.Dal.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataProject.Dal.Infrastructure
{
    public class SicilRepository : BaseRepository<LogoDataModel, TBL_BI_REPORT_IK_SICIL>
    {
        public SicilRepository(LogoDataModel context) : base(context)
        {
        }

        #region methods

        public List<PersonelDto> GetAylikIseGirenler()
        {
            var pDate = $"{DateTime.Now.Month}/{DateTime.Now.Year}";

            var list = _context.TBL_BI_REPORT_IK_SICIL.Where(t => t.IseGirisTarihi.Contains(pDate)).Select(t =>
                new PersonelDto
                {
                    SicilNo = t.SicilNo,
                    Ad = t.Ad,
                    Soyad = t.Soyad,
                    PozisyonAdı = t.PozisyonAdı,
                    KurumAdi = t.KurumAdi,
                    IsYeriAdi = t.IsYeriAdi,
                    BolumAdi = t.BolumAdi,
                    AltBolumAdi = t.AltBolumAdi,
                    BordroKodu = t.BordroKodu,
                    IseGirisTarihi = t.IseGirisTarihi,
                    BirincilYoneticiSicilNo = t.BirincilYoneticiSicilNo,
                    BirincilYoneticiAd = t.BirincilYoneticiAd,
                    BirincilYoneticiSoyad = t.BirincilYoneticiSoyad,
                });

            return list.ToList();
        }

        public DataTable GetHaftalikIseGirenler()
        {
            var query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append(" [SicilNo],");
            query.Append(" [Ad],");
            query.Append(" [Soyad],");
            query.Append(" [KurumKodu],");
            query.Append(" [IsYeriAdi],");
            query.Append(" [PozisyonAdı],");
            query.Append(" [BordroKodu],");
            query.Append(" [IseGirisTarihi],");
            query.Append(" [BirincilYoneticiAd],");
            query.Append(" [BirincilYoneticiSoyad] ");
            query.Append(" FROM TBL_BI_REPORT_IK_SICIL");
            query.Append(" WHERE YEAR(CONVERT(date, IseGirisTarihi, 103)) = YEAR(GETDATE()) and MONTH(CONVERT(date, IseGirisTarihi, 103)) = month(GETDATE()) ");

            var con = new MSSQL();
            var dt = con.Select(query.ToString());

            return dt;
        }

        #endregion
    }
}
