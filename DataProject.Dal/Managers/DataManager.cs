using DataProject.Domain.Dto;

namespace DataProject.Dal.Managers
{
    public class DataManager
    {
        static DataManager dataManager;
        DataManager()
        {

        }

        public static DataManager Instance()
        {
            if (dataManager == null)
                dataManager = new DataManager();

            return dataManager;
        }

        public ResponseDto Start()
        {
            return null;
        }

        public void Dispose()
        {

        }
    }
}
