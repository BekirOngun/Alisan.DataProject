namespace DataProject.Domain.Dto
{
    public class PersonelDto : TBL_BI_REPORT_IK_SICIL
    {
        public string AdSoyad
        {
            get
            {
                return $"{Ad} {Soyad}";
            }
        }
         
        public string YoneticiAdSoyad
        {
            get
            {
                return $"{BirincilYoneticiAd} {BirincilYoneticiSoyad}";
            }
        }
    }
}
