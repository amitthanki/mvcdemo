using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ENRLReconSystem.Utility
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checking Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            if (obj == null || obj == DBNull.Value || (obj.ToString() == string.Empty))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Convert object into Bytes (0 to 255)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte ToByte(this object obj)
        {
            if (IsNull(obj))
                return 0;
            else
                return Convert.ToByte(obj);
        }

        /// <summary>
        /// Convert object into Int16 (-32768 to 32767)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static short ToInt16(this object obj)
        {
            if (IsNull(obj))
                return 0;
            else
                return Convert.ToInt16(obj);
        }

        /// <summary>
        /// Convert object into Int32 (-2147483648 to 2147483647)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt32(this object obj)
        {
            if (IsNull(obj))
                return 0;
            else
                return Convert.ToInt32(obj);
        }

        /// <summary>
        /// Convert object into Int64 (-9223372036854775808 to 9223372036854775807)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ToInt64(this object obj)
        {
            if (IsNull(obj))
                return 0;
            else
                return Convert.ToInt64(obj);
        }
        /// <summary>
        /// Convert object into Int64 (-9223372036854775808 to 9223372036854775807)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int64 ToLong(this object obj)
        {
            if (IsNull(obj))
                return 0;
            else
                return Convert.ToInt64(obj);
        }

        /// <summary>
        /// Convert object into Boolean (0 false else true)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object obj)
        {
            if (IsNull(obj))
                return false;
            else
                return Convert.ToBoolean(obj);
        }

        /// <summary>
        /// Convert object to datetime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object obj)
        {
            if (IsNull(obj))
                return DateTime.MinValue;
            else
                return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// Convert object to string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string NullToString(this object obj)
        {
            if (IsNull(obj))
                return string.Empty;
            else
                return Convert.ToString(obj);
        }
        /// <summary>
        /// if object is null or empty then true else false
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (IsNull(obj) || string.IsNullOrEmpty(Convert.ToString(obj)))
                return true;
            else
                return string.IsNullOrEmpty(Convert.ToString(obj).Trim());
        }

        ///<summary>
        /// Convert object into Double (-1.79769e+308 to 1.79769e+308)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ToDouble(this object obj)
        {
            if (IsNull(obj))
                return 0;
            else
                return Convert.ToDouble(obj);
        }

        /// <summary>
        /// Convert object into Decimal (-79228162514264337593543950335m to 79228162514264337593543950335m)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object obj)
        {
            if (IsNull(obj))
                return 0;
            else
                return Convert.ToDecimal(obj);
        }
        /// <summary>
        /// Check object is Numeric or Not
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>boolean true/false</returns>
        public static bool IsNumeric(this object obj)
        {
            try
            {
                Convert.ToDouble(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static string ObjToJsonString(this object obj)
        {
            ////serialize response to save to DB
            var objXmlSerializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            var objMemoryStream = new MemoryStream();
            objXmlSerializer.Serialize(objMemoryStream, obj);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(objMemoryStream.ToArray()));
            return JsonConvert.SerializeXmlNode(doc);

            //DataContractJsonSerializer js = new DataContractJsonSerializer(obj.GetType());
            //MemoryStream msObj = new MemoryStream();
            //js.WriteObject(msObj, obj);
            //msObj.Position = 0;
            //StreamReader sr = new StreamReader(msObj);
            //string s = sr.ReadToEnd();
            //return s;
        }
    }

    public static class ZoneLookupUI
    {
        public static List<ZoneLookup> lstZoneLookup { get; set; }
        static ZoneLookupUI()
        {
            lstZoneLookup = new List<ZoneLookup>();
            lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4001, isDayLight = false, timeZoneOffset = -8, alternateLkup = 4002 });
            lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4002, isDayLight = true, timeZoneOffset = -7, alternateLkup = 4002 });
            lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4003, isDayLight = false, timeZoneOffset = -7, alternateLkup = 4004 });
            lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4004, isDayLight = true, timeZoneOffset = -6, alternateLkup = 4004 });
            lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4005, isDayLight = false, timeZoneOffset = -6, alternateLkup = 4006 });
            lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4006, isDayLight = true, timeZoneOffset = -5, alternateLkup = 4006 });
            lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4007, isDayLight = false, timeZoneOffset = -5, alternateLkup = 4008 });
            lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4008, isDayLight = true, timeZoneOffset = -4, alternateLkup = 4008 });
            lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4009, isDayLight = null, timeZoneOffset = -7, alternateLkup = null });
            lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4010, isDayLight = null, timeZoneOffset = 5.5, alternateLkup = null });

        }

        public static DateTime ConvertToTimeZone(this DateTime? dt, long? timezone)

        {
            ZoneLookup objZoneLookup = new ZoneLookup();
            //find time zone offset
            var zoneStd = ZoneLookupUI.lstZoneLookup.Where(x => x.ZoneLookupId == timezone).FirstOrDefault();
            long? zoneStdLkup = timezone;
            if (zoneStd.isDayLight.ToBoolean())
            {
                zoneStdLkup = zoneStd.alternateLkup.ToLong();
                zoneStd = ZoneLookupUI.lstZoneLookup.Where(x => x.ZoneLookupId == zoneStdLkup).FirstOrDefault();
            }
            //time without day light saving
            // DateTime localTime1 = new DateTime(dt.ToDateTime().Ticks);
            // DateTime localTime1 = DateTime; 
            DateTime dt2 = dt ?? DateTime.MinValue;
            DateTime localTime1 = new DateTime(DateTime.SpecifyKind(dt2, DateTimeKind.Utc).Ticks);
            double min = (zoneStd.timeZoneOffset * 60);
            DateTime dt3 = localTime1.AddMinutes(min);
            if (zoneStd.isDayLight == null)
            {
                //day light savings are not applicable in this time zone
                //result.time = localTime1;
                //result.isDayLight = null;
                //result.timeZoneLkup = timeZone;
                return dt3;
            }

            //is day light saving?
            //find date of second Sunday in March 2:00 AM
            //var days = ['Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday'];
            //calculate day light start time

            DateTime dayLighStart = new DateTime(dt3.Year, 3, 8, 2, 0, 0);
            //search for second sunday in march
            var dayOfWeek = (int)dayLighStart.DayOfWeek;
            while (dayOfWeek != 0)
            {
                double dayLighStartMin = dayLighStart.ToDateTime().Minute + 1440;
                //move to next day
                DateTime dayLighStartdt = dayLighStart.AddMinutes(dayLighStartMin);
                dayOfWeek = (int)dayLighStartdt.DayOfWeek;
                dayLighStart = dayLighStartdt;
            }

            //calculate day light end time
            //DateTime dayLightEnd = new DateTime(dt3.Year, 11, 1);
            //int firstSunDayInNov = (8 - (int)dayLightEnd.DayOfWeek);
            DateTime dayLightEnd = new DateTime(dt3.Year, 11, 1, 2, 0, 0);
            //search for first sunday of november
            dayOfWeek = (int)dayLightEnd.DayOfWeek;

            while (dayOfWeek != 0)
            {
                double dayLightEndMin = dayLightEnd.ToDateTime().Minute + 1440;
                //move to next day
                DateTime dayLightEnddt = dayLightEnd.AddMinutes(dayLightEndMin);
                dayOfWeek = (int)dayLightEnddt.DayOfWeek;
                dayLightEnd = dayLightEnddt;
            }

            if (localTime1 < dayLighStart)
            {
                //result.time = localTime1;
                //result.isDayLight = false;
                //result.timeZoneLkup = zoneStdLkup;
                return dt3;
            }
            else if (localTime1 < dayLightEnd)
            {
               
                //add one hour
                localTime1.AddMinutes(dt3.ToDateTime().Minute + 60);
                //result.time = localTime1;
                //result.isDayLight = true;
                //result.timeZoneLkup = zoneStd.alternateLkup;
                return dt3.AddHours(1);
            }
            else
            {
                //result.time = localTime1;
                //result.isDayLight = false;
                //result.timeZoneLkup = zoneStdLkup;
                return dt3;
            }


            //return Convert.ToDateTime(obj);
        }

        static bool IsInDaylightSavingsTime(DateTime date)
        {
            // get second sunday in march
            DateTime _tempDateMar = new DateTime(date.Year, 3, 1);
            int secondSunDayInMar = (8 - (int)_tempDateMar.DayOfWeek) + 7;
            _tempDateMar = new DateTime(date.Year, 3, secondSunDayInMar, 2, 0, 0);

            //get first sunday in november
            DateTime _tempDateNov = new DateTime(date.Year, 11, 1);
            int firstSunDayInNov = (8 - (int)_tempDateNov.DayOfWeek);
            _tempDateNov = new DateTime(date.Year, 11, firstSunDayInNov, 2, 0, 0);

            return (date >= _tempDateMar && date <= _tempDateNov);
        }
        public class ZoneLookup
        {
            public long ZoneLookupId { get; set; }
            public bool? isDayLight { get; set; }
            public double timeZoneOffset { get; set; }
            public long? alternateLkup { get; set; }


        }
    }
    
}
