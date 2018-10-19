using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ENRLReconSystem.Helpers
{
    public static class HtmlHelperExtender
    {
        public static MvcHtmlString DateTimeFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, bool isTimeApplicable, object htmlAttributes = null)
        {
            StringBuilder output = new StringBuilder();
            string _dPart = "_DPart";
            string _tPart = "_TPart";
            string _zPart = "_ZPart";
            string controlName = ((MemberExpression)expression.Body).Member.Name;
            string CssClassAttribute_CT = "class";
            
            string _currentDate = "_CurrentDate";
            string _clearDate = "_ClearDate";

            output.Append(string.Format("<input id=\"{0}\" name=\"{0}\" type=\"text\" ", controlName + _dPart));
            if (isTimeApplicable)
            {
                output.Append(RenderAttribute(CssClassAttribute_CT, "datepicker", "date_input"));
            }
            else
            {
                output.Append(RenderAttribute(CssClassAttribute_CT, "datepicker", "dateInputWithoutTime"));
            }

            if (htmlAttributes != null)
            {
                foreach (var item in ((RouteValueDictionary)htmlAttributes))
                {
                    output.Append(RenderAttribute(item.Key.ToString(), item.Value.ToString()));
                }
            }

            output.Append(" />");

            if (isTimeApplicable)
            {
                //adding time label
                output.Append(string.Format("<input id=\"{0}\" name=\"{0}\" type=\"text\" ", controlName + _tPart));
                output.Append(RenderAttribute(CssClassAttribute_CT, "timepicker", "time_input"));

                if (htmlAttributes != null)
                {
                    foreach (var item in ((RouteValueDictionary)htmlAttributes))
                    {
                        if (item.Key.ToString() == "data-val-required")
                        {
                            output.Append(RenderAttribute(item.Key.ToString(), "Time for " +item.Value.ToString()));
                        }
                        else
                        {
                            output.Append(RenderAttribute(item.Key.ToString(), item.Value.ToString()));
                        }
                    }
                }

                output.Append(" />");

            }

            //adding Current date image button
            output.Append(string.Format("<button id=\"{0}\" type=\"button\" alt=\"Current Date\" onclick=\"javascript:SetCurrentDateTime('{1}');\" title=\"Current Date\" ", controlName + _currentDate, controlName));
            output.Append(RenderAttribute(CssClassAttribute_CT, "datetime_icon"));
            output.Append(" ><img src =\"/Images/todays.png\" /></button>");

            //adding Clear date image button
            output.Append(string.Format("<button id=\"{0}\" type=\"button\" alt=\"Clear Date\" onclick=\"javascript:ClearDateTime('{1}');\" title=\"Clear Date\" ", controlName + _clearDate, controlName));
            output.Append(RenderAttribute(CssClassAttribute_CT, "datetime_icon"));
            output.Append(" ><img src =\"/Images/clear_date.png\" /></button>");
            
            return new MvcHtmlString(output.ToString());
        }

        public static string RenderAttribute(string attribute, string attributeValue, string defaultValue = null)
        {
            string value = string.Empty;
            if (string.IsNullOrEmpty(attributeValue) == false)
            {
                value = attributeValue;
            }
            else if (string.IsNullOrEmpty(defaultValue) == false)
            {
                value = defaultValue;
            }
            if (string.IsNullOrEmpty(value) == false)
            {
                if (string.IsNullOrEmpty(attribute))
                {
                    return value;
                }
                else
                {
                    return string.Concat(attribute, "=\"", value, "\" ");
                }
            }
            return "";
        }

        public static MvcHtmlString HourMinDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string type, object htmlAttributes = null)
        {
            try
            {
                List<SelectListItem> lstHourMin = new List<SelectListItem>();
                if (type == "Hour")
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        lstHourMin.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                }
                else if (type == "Minute")
                {
                    for (int i = 0; i <= 55; i += 5)
                    {
                        lstHourMin.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                }
                else if (type == "AMPM")
                {
                    lstHourMin.Add(new SelectListItem() { Text = "AM", Value = "AM" });
                    lstHourMin.Add(new SelectListItem() { Text = "PM", Value = "PM" });
                }
                if (htmlAttributes != null && htmlAttributes is RouteValueDictionary)
                {
                    RouteValueDictionary htmlAttr = htmlAttributes as RouteValueDictionary;
                    return helper.DropDownListFor(expression, lstHourMin, "<---Select--->", htmlAttr);
                }
                return helper.DropDownListFor(expression, lstHourMin, htmlAttributes);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static MvcHtmlString ActionLinkForQueueCount<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression,long queueLkup,long discCat)
        {
            StringBuilder output = new StringBuilder();
            long count = (long)ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;
            QueueSummary Model = helper.ViewData.Model as QueueSummary;
            if (Model.lstUserAccessQueueLkups != null && Model.lstUserAccessQueueLkups.Contains(queueLkup) && count != 0)
            {
                HtmlString anchorTag;
                if (discCat == (long)DiscripancyCategory.OOA || discCat == (long)DiscripancyCategory.SCC || discCat == (long)DiscripancyCategory.TRR)
                {
                    anchorTag = LinkExtensions.ActionLink(helper, count.ToString(), "SearchFromHome", "Common", new { @ComplianceStartDate = Model.StartDate, @ComplianceEndDate = Model.EndDate, @Queue = queueLkup, @data = discCat }, new { @class = "count-button", @Title = "View Queue" });
                }
                else
                {
                    anchorTag = LinkExtensions.ActionLink(helper, count.ToString(), "SearchFromHome", "Common", new { @CaseCreationStartDate = Model.StartDate, @CaseCreationEndDate = Model.EndDate, @Queue = queueLkup, @data = discCat }, new { @class = "count-button", @Title = "View Queue" });
                }
                output.Append(anchorTag);
            }
            else
            {
                output.Append(string.Format(count.ToString()));
            }
            return new MvcHtmlString(output.ToString());
        }

        //Custom Dropdown binds data according to lookup type
        public static MvcHtmlString CustomDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression,
                                         long lookUpTypeId, string optionalLabel, object htmlAttributes = null, bool IsVerificationState = false)
        {
            try
            {
                IEnumerable<SelectListItem> selectList = null;
                List<DOCMN_LookupMaster> lstLookups = null;

                lstLookups=CacheUtility.GetAllLookupsFromCache(lookUpTypeId);
                selectList = (IEnumerable<SelectListItem>)new SelectList(lstLookups, "CMN_LookupMasterId", "LookupValue");
                if (htmlAttributes != null && htmlAttributes is RouteValueDictionary)
                {
                    RouteValueDictionary htmlAttr = htmlAttributes as RouteValueDictionary;
                    if (!string.IsNullOrEmpty(optionalLabel))
                        return helper.DropDownListFor(expression, selectList, optionalLabel, htmlAttr);
                    else
                        return helper.DropDownListFor(expression, selectList, htmlAttr);
                }
                if (!string.IsNullOrEmpty(optionalLabel))
                    return helper.DropDownListFor(expression, selectList, optionalLabel, htmlAttributes);
                else
                    return helper.DropDownListFor(expression, selectList, htmlAttributes);
            }
            catch (Exception ex)
            {
               
                return null;
            }

        }
    }
}