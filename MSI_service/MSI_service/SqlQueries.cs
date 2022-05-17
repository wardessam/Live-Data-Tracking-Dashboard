using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSI_service
{
    public class SqlQueries
    {
        public static string ADD_DATA_To_Store_Transaction_TABLE = "insert into Store_Transaction(T_ID , T_Data , Date , Cost , Branch_ID) values(@T_ID , @T_Data , @Date , @Cost , @Branch_ID);";
        //Ward
        public static string SaveNormalRecommdations = "insert into Recommended_Items (Item,Season,Amount,Branch_ID,Date) values (@Item,@Season,@Amount,@Branch_ID,@Date)";
        public static string SelectAllFromSpecific = "select * from Specific_FrequentItems where Season=@Season";
        public static string DeletePreviousRecommendations = "delete from Recommended_Items where Season= @Season";

        public static string SaveSpecificRecommdations = "insert into Recommended_Items (Item,Season,Amount,Branch_ID) values (@Item,@Season,@Amount,@Branch_ID)";
        //sorting
        public static string SelectAllFromFrequentItems = "select Frequent_Item,Amount,Season,Branch_ID from Frequent_Items";


        ///
        public static string DeleteFromSpecific = "delete from Specific_FrequentItems where Season= @Season";
        public static string SaveToSpecific = "insert into Specific_FrequentItems values(@Frequent_Item,@Amount,@Season,@Branch_ID)";
        //


    }
}
