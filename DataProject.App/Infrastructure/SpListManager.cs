using System.Linq;
using DataProject.Dal.Managers;
using DataProject.Domain.Constants;
using Microsoft.SharePoint.Client;

namespace DataProject.App.Infrastructure
{
    public class SpListManager
    {
        public static void SpSaveAylikIseGirenler()
        {
            var sicilManager = new SicilManager();

            var list = sicilManager.GetAylikIseGirenler();

            if (list != null && list.Any())
            {
                var spList = SpClientContext.Client().Web.Lists.GetByTitle(SpListNames.BuAyIseBaslayanPersoneller);
                var itemCreateInfo = new ListItemCreationInformation();

                DeleteAllList(spList);

                list.ForEach(t =>
                {
                    var listItem = spList.AddItem(itemCreateInfo);
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.SicilNo] = t.SicilNo;
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.AdSoyad] = t.AdSoyad;
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.PozisyonAdi] = t.PozisyonAdı;
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.KurumAdi] = t.KurumAdi;
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.IsYeriAdi] = t.IsYeriAdi;
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.BolumAdi] = t.BolumAdi;
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.AltBolumAdi] = t.AltBolumAdi;
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.BordroKodu] = t.BordroKodu;
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.IseGirisTarihi] = t.IseGirisTarihi;
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.BirincilYoneticiSicilNo] = t.BirincilYoneticiSicilNo;
                    listItem[SpListFieldNames.PersonelAylikIseGirenler.BirinciYoneticiAdSoyad] = t.YoneticiAdSoyad;


                    listItem.Update();
                    SpClientContext.Client().ExecuteQuery();
                });

            }
        }

        public static void SpSaveHaftalikIseGirenler()
        {

        }

        public static void DeleteAllList(List list)
        {
            var queryLimit = 4000;
            var batchLimit = 100;
            var moreItems = true;

            var viewXml = string.Format(@"
        <View>
            <Query><Where></Where></Query>
            <ViewFields>
                <FieldRef Name='ID' />
            </ViewFields>
            <RowLimit>{0}</RowLimit>
        </View>", queryLimit);

            var camlQuery = new CamlQuery();
            camlQuery.ViewXml = viewXml;

            while (moreItems)
            {
                var listItems = list.GetItems(camlQuery); // CamlQuery.CreateAllItemsQuery());

                SpClientContext.Client().Load(listItems, eachItem => eachItem.Include(
                         item => item,
                         item => item["ID"]));

                SpClientContext.Client().ExecuteQuery();

                var totalListItems = listItems.Count;

                if (totalListItems > 0)
                {
                    // Console.WriteLine("Deleting {0} items from {1}...", totalListItems, myList.Id);
                    for (var i = totalListItems - 1; i > -1; i--)
                    {
                        listItems[i].DeleteObject();

                        if (i % batchLimit == 0)
                            SpClientContext.Client().ExecuteQuery();
                    }

                    SpClientContext.Client().ExecuteQuery();
                }
                else
                {
                    moreItems = false;
                }
            }

        }
    }
}
