using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MiniGun.Models
{
    public class ExtendedDataHolder
    {
        private static ExtendedDataHolder ourInstance = new ExtendedDataHolder();

        private Dictionary<string, object> extras = new Dictionary<string, object>();

        private ExtendedDataHolder()
        {

        }

        public static ExtendedDataHolder GetInstance()
        {
            return ourInstance;
        }

        public void PutExtra(string name, object obj)
        {
            if (HasExtra(name))
                RemoveExtra(name);

            extras.Add(name, obj);
        }

        public bool HasExtra(String name)
        {
            return extras.ContainsKey(name);
        }

        public object GetExtra(string name)
        {
            return extras[name];
        }

        public void Clear()
        {
            extras.Clear();
        }

        public void RemoveExtra(string name)
        {
            extras.Remove(name);
        }
    }
}