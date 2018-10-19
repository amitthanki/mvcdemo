using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ENRLReconSystem.Models
{
    public static class UIExtensions
    {
        //public static bool IsNull(this object obj)
        //{
        //    if (obj == null || obj == DBNull.Value || (obj.ToString() == string.Empty))
        //        return true;
        //    else
        //        return false;
        //}
        public static DateTime ConvertToUTC(this DateTime dateToConvert, TimeZoneUTCDiffernece timeZone)
        {
            return dateToConvert.AddMinutes(-1 * (double)timeZone);
        }

        public static DateTime ConvertFromUTC(this DateTime dateToConvert, TimeZoneUTCDiffernece timeZone)
        {
            return dateToConvert.AddMinutes((double)timeZone);
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
            double min = dt.ToDateTime().Minute + zoneStd.timeZoneOffset * 60;
            DateTime dt3 =localTime1.AddMinutes(min);
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
            DateTime dayLighStart = new DateTime(dt3.Year, 3, 1);
            int secondSunDayInMar = (8 - (int)dayLighStart.DayOfWeek) + 7;
            dayLighStart = new DateTime(dt3.Year, 3, secondSunDayInMar, 2, 0, 0);
            //search for second sunday in march
            var dayOfWeek = dayLighStart.Day;
            //while (dayOfWeek != 0)
            //{
            //    double dayLighStartMin = dayLighStart.ToDateTime().Minute + 1440;
            //    //move to next day
            //    DateTime dayLighStartdt =  dayLighStart.AddMinutes(dayLighStartMin);
            //    dayOfWeek = dayLighStartdt.Day;
            //}           

            //calculate day light end time
            DateTime dayLightEnd = new DateTime(dt3.Year, 11, 1);
            int firstSunDayInNov = (8 - (int)dayLightEnd.DayOfWeek);
            dayLightEnd = new DateTime(dt3.Year, 11, firstSunDayInNov, 2, 0, 0);
            //search for first sunday of november
            dayOfWeek = dayLightEnd.Day;

            //while (dayOfWeek != 0)
            //{
            //    double dayLightEndMin = dayLightEnd.ToDateTime().Minute + 1440;
            //    //move to next day
            //    DateTime dayLightEnddt = dayLightEnd.AddMinutes(dayLightEndMin);
            //    dayOfWeek = dayLightEnddt.Day;
            //}
            
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
                return dt3;
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

        public static class ZoneLookupUI
        {
            public static List<ZoneLookup> lstZoneLookup { get; set; }
            static ZoneLookupUI()
            {
                lstZoneLookup = new List<ZoneLookup>();
                lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4001, isDayLight = false, timeZoneOffset =  -8, alternateLkup = 4002 });
                lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4002, isDayLight = true, timeZoneOffset = -7,   alternateLkup = 4002 });
                lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4003, isDayLight = false, timeZoneOffset =  -7, alternateLkup = 4004 });
                lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4004, isDayLight = true, timeZoneOffset = -6,    alternateLkup = 4004 });
                lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4005, isDayLight = false, timeZoneOffset =  -6,  alternateLkup = 4006 });
                lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4006, isDayLight = true, timeZoneOffset = -5,    alternateLkup = 4006 });
                lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4007, isDayLight = false, timeZoneOffset =  -5,  alternateLkup = 4008 });
                lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4008, isDayLight = true, timeZoneOffset = -4,    alternateLkup = 4008 });
                lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4009, isDayLight = null, timeZoneOffset = -7,    alternateLkup=null });
                lstZoneLookup.Add(new ZoneLookup { ZoneLookupId = 4010, isDayLight = null, timeZoneOffset = -5.5,  alternateLkup = null });

            }
        }



    }
}