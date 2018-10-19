using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENRLReconSystem.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime DateTimeExtended(this DateTime dt)
        {
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            /*
            if (ConstantValues.PreferredTimeZoneID != null)
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt, ConstantValues.PreferredTimeZoneID);
            else
            */
                return dt;
        }
        public static DateTime PreferredDateTime(object dateTimeToConvert)
        {
            DateTime dateTime = Convert.ToDateTime(dateTimeToConvert);
            /*
            if (ConstantValues.PreferredTimeZoneID != null)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, ConstantValues.PreferredTimeZoneID);
            }
            else
            {
                return dateTime;
            }
            */
            return dateTime;            
        }
        public static DateTime PreferredDateTime(DateTime dateTimeToConvert)
        {
            DateTime dateTime = Convert.ToDateTime(dateTimeToConvert);
            /*
            if (ConstantValues.PreferredTimeZoneID != null)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, ConstantValues.PreferredTimeZoneID);
            }
            else
            {
                return dateTime;
            }
            */
            return dateTime;
        }
        public static DateTime PreferredDateTime(object dateTimeToConvert, long? timeZoneLkup)
        {
            string preferredTimeZoneID = null;
            preferredTimeZoneID = GetTimeZoneIdByLookup(timeZoneLkup);
            DateTime dateTime;
            DateTime.TryParse(dateTimeToConvert.ToString(), out dateTime);
            if (preferredTimeZoneID != null && dateTime != null)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, preferredTimeZoneID);
            }
            else
            {
                return dateTime;
            }
        }
        public static DateTime PreferredDateTime(DateTime dateTimeToConvert, long? timeZoneLkup)
        {
            string preferredTimeZoneID = null;
            preferredTimeZoneID = GetTimeZoneIdByLookup(timeZoneLkup);
            if (preferredTimeZoneID != null && dateTimeToConvert != null)
            {
                dateTimeToConvert = DateTime.SpecifyKind(dateTimeToConvert, DateTimeKind.Utc);
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTimeToConvert, preferredTimeZoneID);
            }
            else
            {
                return dateTimeToConvert;
            }
        }
        public static DateTime ConvertDateTimeToUTC(DateTime dateTime, string sourceTimeZoneID)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneID), TimeZoneInfo.Utc);
        }
        public static string GetTimeZoneIdByLookup(long? timeZoneLkup)
        {
            if (timeZoneLkup == null)
                timeZoneLkup = 0;
            string timeZoneId = null;
            /*
            switch (timeZoneLkup)
            {
                case (long)Enumerations.LookUpMaster.India:
                    {
                        timeZoneId = ConstantTexts.India;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.CentralStandardTime:
                    {
                        timeZoneId = ConstantTexts.CentralStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.CentralDaylightTime:
                    {
                        timeZoneId = ConstantTexts.CentralStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.EasternDaylightTime:
                    {
                        timeZoneId = ConstantTexts.EasternStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.EasternStandardTime:
                    {
                        timeZoneId = ConstantTexts.EasternStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.MountainDaylightTime:
                    {
                        timeZoneId = ConstantTexts.MountainStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.MountainStandardTime:
                    {
                        timeZoneId = ConstantTexts.MountainStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.PacificDaylightTime:
                    {
                        timeZoneId = ConstantTexts.PacificStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.PacificStandardTime:
                    {
                        timeZoneId = ConstantTexts.PacificStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.AtlanticStandardTime:
                    {
                        timeZoneId = ConstantTexts.AtlanticStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.AlaskanStandardTime:
                    {
                        timeZoneId = ConstantTexts.AlaskanStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.HawaiiAleutianStandardTime:
                    {
                        timeZoneId = ConstantTexts.HawaiiAleutianStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.SamoaStandardTime:
                    {
                        timeZoneId = ConstantTexts.SamoaStandardTime;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.ChamorroStandardTime:
                    {
                        timeZoneId = ConstantTexts.ChamorroStandardTime;
                        break;
                    }
            }
            */
            return timeZoneId;
        }
        public static string GetTimeZoneShortFormByLookup(long? timeZoneLkup)
        {
            if (timeZoneLkup == null)
                timeZoneLkup = 0;
            string timeZoneId = null;
            /*
            switch (timeZoneLkup)
            {
                case (long)Enumerations.LookUpMaster.India:
                    {
                        timeZoneId = ConstantTexts.IST;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.CentralStandardTime:
                    {
                        timeZoneId = ConstantTexts.CST;
                        break;
                    }

                case (long)Enumerations.LookUpMaster.EasternStandardTime:
                    {
                        timeZoneId = ConstantTexts.EST;
                        break;
                    }

                case (long)Enumerations.LookUpMaster.MountainStandardTime:
                    {
                        timeZoneId = ConstantTexts.MST;
                        break;
                    }

                case (long)Enumerations.LookUpMaster.PacificStandardTime:
                    {
                        timeZoneId = ConstantTexts.PST;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.AtlanticStandardTime:
                    {
                        timeZoneId = ConstantTexts.AST;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.AlaskanStandardTime:
                    {
                        timeZoneId = ConstantTexts.AKST;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.HawaiiAleutianStandardTime:
                    {
                        timeZoneId = ConstantTexts.HAST;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.SamoaStandardTime:
                    {
                        timeZoneId = ConstantTexts.SST;
                        break;
                    }
                case (long)Enumerations.LookUpMaster.ChamorroStandardTime:
                    {
                        timeZoneId = ConstantTexts.CHST;
                        break;
                    }
                default:
                    {
                        timeZoneId = ConstantTexts.UTC;
                        break;
                    }
            }
            */
            return timeZoneId;
        }
    }
}