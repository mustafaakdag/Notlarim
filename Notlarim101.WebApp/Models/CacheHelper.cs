using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using Notlarim101.BusinessLayer;
using Notlarim101.Entity;

namespace Notlarim101.WebApp.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
           var result= WebCache.Get("category-cache");
           if (result==null)
           {
               CategoryManager cm = new CategoryManager();
               result = cm.List();

               WebCache.Set("category-cache",result,30,true);
           }

           return result;
        }

        public static void RemoveCategoriesFromCache()
        {
            Remove("category-cache");
        }

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}