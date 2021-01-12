using RestSharp;

namespace EFurni.Presentation.Extensions
{
    public static class RestSharpExtensions
    {
        public static IRestRequest AddFilter(this RestRequest instance,object filterQuery)
        {
            if (filterQuery == null)
                return instance;
            
            var fields = filterQuery.GetType().GetProperties();

            foreach (var field in fields)
            {
                var fieldValue = field.GetValue(filterQuery);
                
                if(fieldValue == null)
                    continue;

                instance.AddParameter(field.Name, fieldValue, ParameterType.QueryString);
            }

            return instance;
        }

        public static IRestRequest AddPagination(this RestRequest instance, object paginationQuery)
        {
            if (paginationQuery == null)
                return instance;
            
            var fields = paginationQuery.GetType().GetProperties();

            foreach (var field in fields)
            {
                var fieldValue = field.GetValue(paginationQuery);
                
                if(fieldValue == null)
                    continue;

                instance.AddParameter(field.Name, fieldValue, ParameterType.QueryString);
            }

            return instance;
        }
        
    }
}