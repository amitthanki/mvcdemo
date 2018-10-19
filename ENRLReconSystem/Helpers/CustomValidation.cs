using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace ENRLReconSystem.Helpers
{
    public enum RegexType
    {
        AlphabetsWithSpaces,
        AlphabetsPlusNumeric,
        Alphabets,
        AlphabetsWithNumeric,
        Numeric,
        Email,
        NumericWithComma,
        AlphabetsWithUnderscore,
        AlphabetsWithUnderscoreAndSpaces,
        AlphabetsWithNumericUnderscoreAndSpaces,
        AlphabetsWithSpacesandSpecialCharacters,
        AlphabetsWithUnderscoreSpacesAmpersand,
        AlphabetsWithUnderscoreSpaceAmpersandHyphen,
        AlphabetsWithNumericUnderscoreHyphenAmpersandAtSignAndSpaces,
        AlphabetsWithNumericUnderscoreHyphenAmpersandAndSpaces,
        AlphabetsWithNumericUnderscoreHyphenAndSpaces,
        AlphabetsWithNumericUnderscoreHyphenSlashColonAtSignAmpersandSpace,
        Url,
        AlphabetsWithNumericSpacesAndAmpersand,
        MBI,
        StrictAlphaNumeric,
        AlphabetsWithNumericUnderscoreHyphenSlashColonAtSignAmpersandSpaceandDot,
        MemberNameOld,
        MemberName
    }
    public class CustomValidation
    {

        public CustomValidation()
        {
            KeyValuePair<string, string> defaultAttr = new KeyValuePair<string, string>("data-val", "true");
            _Validators = new List<KeyValuePair<string, string>>();
            _Validators.Add(defaultAttr);
        }

        private List<KeyValuePair<string, string>> _Validators;
        private List<KeyValuePair<string, string>> Validators
        {
            get { return _Validators; }
        }
        public RouteValueDictionary validationAttributes
        {
            get
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                if (_Validators != null)
                {
                    foreach (KeyValuePair<string, string> kvp in _Validators)
                    {
                        routeValueDictionary.Add(kvp.Key, kvp.Value);
                    }
                }
                return routeValueDictionary;
            }
        }
        public CustomValidation Required(string errorMessage = "This field is required.")
        {
            if (errorMessage != string.Empty)
                _Validators.Add(new KeyValuePair<string, string>("data-val-required", errorMessage));
            return this;
        }
        public CustomValidation NotRequired()
        {
            _Validators.Remove(new KeyValuePair<string, string>("data-val", "true"));
            _Validators.Add(new KeyValuePair<string, string>("data-val", "false"));
            return this;
        }
        public CustomValidation StringMaxLength(UInt16 maxLength, string fieldName)
        {
            if (maxLength > 0)
            {
                string msg;
                if (!String.IsNullOrEmpty(_Validators.Find(m => m.Key == "data-val-length").Key))
                {
                    msg = string.Format("{0}{1}{2}", _Validators.Find(m => m.Key == "data-val-length").Value, Environment.NewLine, "This field must be a string with a maximum length of " + maxLength.ToString());
                    _Validators.Remove(_Validators.Find(m => m.Key == "data-val-length"));
                }
                else
                {
                    msg = fieldName + "  must have maximum length of  " + maxLength.ToString();
                }
                _Validators.AddRange(new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string, string>("data-val-length-max",maxLength.ToString()),
                new KeyValuePair<string, string>("data-val-length", msg)});
            }
            return this;
        }
        public CustomValidation StringMinLength(UInt16 minLength)
        {
            if (minLength > 0)
            {
                string msg;
                if (!String.IsNullOrEmpty(_Validators.Find(m => m.Key == "data-val-length").Key))
                {
                    msg = string.Format("{0}{1}{2}", _Validators.Find(m => m.Key == "data-val-length").Value, Environment.NewLine, "This field must be a string with a minimum length of " + minLength.ToString());
                    _Validators.Remove(_Validators.Find(m => m.Key == "data-val-length"));
                }
                else
                {
                    msg = "This field must be a string with a minimum length of " + minLength.ToString();
                }
                _Validators.AddRange(new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string, string>("data-val-length-min",minLength.ToString()),
            new KeyValuePair<string, string>("data-val-length", msg)});
            }
            return this;
        }
        public CustomValidation StringLength(UInt16 Length, string ControlName)
        {
            if (Length > 0)
            {
                string msg = "";
                if (!String.IsNullOrEmpty(_Validators.Find(m => m.Key == "data-val-length").Key))
                {
                    _Validators.Remove(_Validators.Find(m => m.Key == "data-val-length"));
                }
                if (!String.IsNullOrEmpty(_Validators.Find(m => m.Key == "data-val-length-min").Key))
                {
                    _Validators.Remove(_Validators.Find(m => m.Key == "data-val-length-min"));
                }
                if (!String.IsNullOrEmpty(_Validators.Find(m => m.Key == "data-val-length-max").Key))
                {
                    _Validators.Remove(_Validators.Find(m => m.Key == "data-val-length-max"));
                }
                msg = ControlName + " length should be " + Length.ToString();

                _Validators.AddRange(new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string, string>("data-val-length-min",Length.ToString()),
                new KeyValuePair<string, string>("data-val-length-max",Length.ToString()),
            new KeyValuePair<string, string>("data-val-length", msg)});
            }
            return this;
        }
        public CustomValidation Range(UInt16 minLength, UInt16 maxLength)
        {
            if (maxLength > 0 && minLength > 0)
            {
                _Validators.AddRange(new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string, string>("data-val-range-min",minLength.ToString()),
                new KeyValuePair<string, string>("data-val-range-max",maxLength.ToString()),
                new KeyValuePair<string,string>("data-val-range","This field must be between "+minLength.ToString()+" and "+maxLength.ToString())});
            }
            return this;
        }
        public CustomValidation RegularExpression(string expression)
        {
            if (expression != string.Empty)
            {

                _Validators.AddRange(new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string,string>("data-val-regex-pattern",expression),
                new KeyValuePair<string,string>("data-val-regex","This field must match the regular expression "+expression)
            });
            }
            return this;
        }
        public CustomValidation RegularExpression(RegexType regexType, string controlName)
        {
            string expression;
            string expressionTypeName;
            switch (regexType)
            {
                case RegexType.Alphabets:
                    {
                        expression = "^[a-zA-Z]+$";
                        expressionTypeName = "Alphabets";
                        break;
                    }
                case RegexType.AlphabetsWithSpaces:
                    {
                        expression = "^[a-zA-Z ]+$";
                        expressionTypeName = "Alphabets With Spaces";
                        break;
                    }
                case RegexType.Email:
                    {
                        expression = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                        expressionTypeName = "Email";
                        break;
                    }
                case RegexType.AlphabetsWithSpacesandSpecialCharacters:
                    {
                        expression = "^[a-zA-Z& ]+$";
                        expressionTypeName = "Alphabets With Spaces And Ampersand Symbol";
                        break;
                    }
                case RegexType.AlphabetsWithUnderscoreSpacesAmpersand:
                    {
                        expression = "^[a-zA-Z&_ ]+$";
                        expressionTypeName = "Alphabets With Underscore,Spaces And Ampersand Symbol";
                        break;
                    }
                case RegexType.AlphabetsWithNumeric:
                    {
                        expression = "^[a-zA-Z0-9]+$";
                        expressionTypeName = "Alphabets With Numeric";
                        break;
                    }
                case RegexType.AlphabetsWithUnderscore:
                    {
                        expression = "^[a-zA-Z_]+$";
                        expressionTypeName = "Alphabets With Underscores";
                        break;
                    }
                case RegexType.AlphabetsWithUnderscoreAndSpaces:
                    {
                        expression = "^[a-zA-Z_ ]+$";
                        expressionTypeName = "Alphabets With Underscores And Spaces";
                        break;
                    }
                case RegexType.AlphabetsWithNumericUnderscoreAndSpaces:
                    {
                        expression = "^[a-zA-Z0-9_ ]+$";
                        expressionTypeName = "Alphabets,Numeric,Underscores And Space";
                        break;
                    }
                case RegexType.AlphabetsWithNumericUnderscoreHyphenAmpersandAtSignAndSpaces:
                    {
                        expression = "^[a-zA-Z0-9_@& -]+$";
                        expressionTypeName = "Alphabets,Numeric,Underscores,Hyphen,Ampersand,At-Sign And Space";
                        break;
                    }
                case RegexType.AlphabetsWithNumericUnderscoreHyphenAndSpaces:
                    {
                        expression = "^[a-zA-Z0-9_ -]+$";
                        expressionTypeName = "Alphabets,Numeric,Underscores,Hyphen And Space";
                        break;
                    }
                case RegexType.AlphabetsWithUnderscoreSpaceAmpersandHyphen:
                    {
                        expression = "^[a-zA-Z_& -]+$";
                        expressionTypeName = "Alphabets,Underscores,Hyphen,Ampersand And Space";
                        break;
                    }
                case RegexType.AlphabetsWithNumericUnderscoreHyphenSlashColonAtSignAmpersandSpace:
                    {
                        expression = "^[a-zA-Z0-9_&@:\\\\/ -]+$";
                        expressionTypeName = "Alphabets,Numeric,Underscores,Hyphen,Ampersand,Slash,Colon,At Sign And Space";
                        break;
                    }
                case RegexType.AlphabetsWithNumericUnderscoreHyphenAmpersandAndSpaces:
                    {
                        expression = "^[a-zA-Z0-9_& -]+$";
                        expressionTypeName = "Alphabets,Numeric,Underscores,Hyphen,Ampersand And Space";
                        break;
                    }
                case RegexType.Numeric:
                    {
                        expression = "^[0-9]+$";
                        expressionTypeName = "Numeric";
                        break;
                    }
                case RegexType.NumericWithComma:
                    {
                        expression = "^[0-9,]+$";
                        expressionTypeName = "Numeric With Comma";
                        break;
                    }
                case RegexType.Url:
                    {
                        expression = @"^(https?:\/\/)([\da-z\.-]+)\.([a-z\.]{2,6})(\:[0-9]{4,5})?([\/\w \.-]*)*(\?[a-zA-Z0-9&=]*)?\/?$";
                        expressionTypeName = "URL";
                        break;
                    }
                case RegexType.AlphabetsWithNumericSpacesAndAmpersand:
                    {
                        expression = @"^[a-zA-Z0-9$ ]+$";
                        expressionTypeName = "Alphanumeric, Spaces and Symbols('$')";
                        break;
                    }
                case RegexType.AlphabetsPlusNumeric:
                    {
                        expression = @"^[a-zA-Z0-9$ ]+$";
                        expressionTypeName = "Alphabates Plus Numeric";
                        break;
                    }
                case RegexType.MBI:
                    {
                        expression = @"^[1-9]{1}[A,C-H,J-K,M-N,P-R,T-Y]{1}[A,C-H,J-K,M-N,P-R,T-Y,0-9]{1}[0-9]{1}[A,C-H,J-K,M-N,P-R,T-Y]{1}[A,C-H,J-K,M-N,P-R,T-Y,0-9]{1}[0-9]{1}[A,C-H,J-K,M-N,P-R,T-Y]{2}[0-9]{2}$";
                        expressionTypeName = "Valid MBI";
                        break;
                    }
                case RegexType.StrictAlphaNumeric:
                    {
                        expression = @"^([0-9]+[a-zA-Z]+|[a-zA-Z]+[0-9]+)[0-9a-zA-Z]*$";
                        expressionTypeName = "Combination of Alphabets and Number";
                        break;
                    }
                case RegexType.AlphabetsWithNumericUnderscoreHyphenSlashColonAtSignAmpersandSpaceandDot:
                    {
                        expression = "^[a-zA-Z0-9_&@:\\\\// -.||]+$";
                        expressionTypeName = "Alphabets,Numeric,Underscores,Hyphen,Ampersand,Slash,Colon,At Sign,Space And Dot";
                        break;
                    }
                case RegexType.MemberNameOld:
                    {
                        expression = "^[a-zA-Z- .]+$";
                        expressionTypeName = "Alphabets, Hyphen, Space And Dot";
                        break;
                    }
                case RegexType.MemberName:
                    {
                        expression = "^[a-zA-Z- , . ' _ ]+$";
                        expressionTypeName = "Alphabets, Hyphen, Space,Comma,apostrophe And Dot";
                        break;
                    }
                default:
                    {
                        expression = "";
                        expressionTypeName = "";
                        break;
                    }
            }
            _Validators.AddRange(new List<KeyValuePair<string, string>>(){
                new KeyValuePair<string,string>("data-val-regex-pattern",expression),
                new KeyValuePair<string,string>("data-val-regex",string.Format("{0} should have {1} value only.",controlName,expressionTypeName))
            });
            return this;
        }
        public CustomValidation AddCssClass(params string[] classNames)
        {
            if (classNames.Length > 0)
            {
                StringBuilder sbClasses = new StringBuilder();
                foreach (string strclass in classNames)
                {
                    sbClasses.Append(strclass + " ");
                }
                _Validators.Add(new KeyValuePair<string, string>("class", sbClasses.ToString().TrimEnd(' ')));
            }
            return this;
        }
        public CustomValidation CustomAttributes(string attrKey, string attrValue)
        {
            if (attrKey != string.Empty && attrValue != string.Empty)
            {
                _Validators.Add(new KeyValuePair<string, string>(attrKey, attrValue));
            }
            return this;
        }

    }
}