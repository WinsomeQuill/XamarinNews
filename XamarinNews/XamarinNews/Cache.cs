using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNews
{
    public static class Cache
    {
        public static int ID { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string About { get; set; }
        public static string Login { get; set; }
        public static ImageSource CropAvatar { get; set; }
        public static ImageSource FullAvatar { get; set; }
    }
}
