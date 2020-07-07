using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ES_CapDien.Helpers
{
    public class HelpHome
    {
        public string UrlQuanTri()
        {
            return "http://admin.rd.vov.truongdientu.vn";
        }
        public static string TimeMoment(DateTime date)
        {
            string kq = "";
            try
            {
                DateTime dateNow = DateTime.Now;
                int check = (int)(dateNow - date).TotalDays;
                if (check == 0)
                {
                    double totalHour = (int)(dateNow - date).TotalHours;
                    if (totalHour == 0)
                    {
                        double totalMinus = (dateNow - date).TotalMinutes;
                        kq = (int)totalMinus + " phút trước";
                    }
                    else
                    {
                        kq = totalHour + " giờ trước";
                    }
                }
                else if (check > 0 && check < 7)
                {
                    double totalDay = (dateNow - date).TotalDays;
                    kq = (int)totalDay + " ngày trước";
                }
                else
                {
                    kq = date.ToString("dd/MM/yyyy HH:mm");
                }
            }
            catch { }
            return kq;
        }
        public string SetUrlFileByUrlQuanTri(string contentText)
        {
            string kq = string.Empty;
            if (!string.IsNullOrEmpty(contentText))
            {
                contentText = Regex.Replace(contentText, @"&nbsp;", "").Trim();
                kq = contentText.Replace("/UploadFolder/", UrlQuanTri() + "/UploadFolder/").Replace("/uploadfolder/", UrlQuanTri() + "/UploadFolder/");
            }
                
            return kq;
        }
        public static string TimeDMY(DateTime date)
        {
            string kq = "";
            try
            {
                kq = date.ToString("dd/MM/yyyy");
            }
            catch { }
            return kq;
        }

        #region Định nghĩa cấu hình rewite url

        protected static string CreateTitle(string strTitle)
        {
            string title = strTitle;
            if (String.IsNullOrEmpty(title)) return "";
            title = RejectMarks(title);
            // remove entities
            title = Regex.Replace(title, @"&\w+;", "");
            // remove anything that is not letters, numbers, dash, or space
            title = Regex.Replace(title, @"[^A-Za-z0-9\-\s]", "");
            // remove any leading or trailing spaces left over
            title = title.Trim();
            // replace spaces with single dash
            title = Regex.Replace(title, @"\s+", "-");
            // if we end up with multiple dashes, collapse to single dash            
            title = Regex.Replace(title, @"\-{2,}", "-");
            // make it all lower case
            title = title.ToLower();
            // if it's too long, clip it
            if (title.Length > 80)
                title = title.Substring(0, 79);
            // remove trailing dash, if there is one
            if (title.EndsWith("-"))
                title = title.Substring(0, title.Length - 1);
            return title;
        }
        protected static string RejectMarks(string text)
        {
            string[] pattern = new string[7];
            char[] replaceChar = new char[14];

            // Khởi tạo giá trị thay thế

            replaceChar[0] = 'a';
            replaceChar[1] = 'd';
            replaceChar[2] = 'e';
            replaceChar[3] = 'i';
            replaceChar[4] = 'o';
            replaceChar[5] = 'u';
            replaceChar[6] = 'y';
            replaceChar[7] = 'A';
            replaceChar[8] = 'D';
            replaceChar[9] = 'E';
            replaceChar[10] = 'I';
            replaceChar[11] = 'O';
            replaceChar[12] = 'U';
            replaceChar[13] = 'Y';

            //Mẫu cần thay thế tương ứng

            pattern[0] = "(á|à|ả|ã|ạ|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ)"; //letter a
            pattern[1] = "đ"; //letter d
            pattern[2] = "(é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ)"; //letter e
            pattern[3] = "(í|ì|ỉ|ĩ|ị)"; //letter i
            pattern[4] = "(ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ)"; //letter o
            pattern[5] = "(ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự)"; //letter u
            pattern[6] = "(ý|ỳ|ỷ|ỹ|ỵ)"; //letter y

            for (int i = 0; i < pattern.Length; i++)
            {
                MatchCollection matchs = Regex.Matches(text, pattern[i], RegexOptions.IgnoreCase);
                foreach (Match m in matchs)
                {
                    if (i == 0)
                    {
                        text = text.Replace(m.Value[0], 'a');
                    }
                    else if (i == 1)
                    {
                        text = text.Replace(m.Value[0], 'd');
                    }
                    else if (i == 2)
                    {
                        text = text.Replace(m.Value[0], 'e');
                    }
                    else if (i == 3)
                    {
                        text = text.Replace(m.Value[0], 'i');
                    }
                    else if (i == 4)
                    {
                        text = text.Replace(m.Value[0], 'o');
                    }
                    else if (i == 5)
                    {
                        text = text.Replace(m.Value[0], 'u');
                    }
                    else if (i == 6)
                    {
                        text = text.Replace(m.Value[0], 'y');
                    }
                }
            }
            return text;
        }
        public static string ConfigUrl(string text, int id, string define)
        {
            string kq = "";
            try
            {
                kq = "/"+CreateTitle(text) + define + id;
            }
            catch { }
            return kq;
        }
        /// <summary>
        /// Định nghĩa
        /// </summary>
        /// <returns></returns>
        public static string defineDetailVideo()
        {
            return "-dtvi-";
        }
        public static string defineMobileDetailVideo()
        {
            return "-mdtvi-";
        }
        public static string defineDetailNews()
        {
            return "-dtnew-";
        }
        public static string defineMobileDetailNews()
        {
            return "-mobiledtnew-";
        }
        public static string defineDetailCate()
        {
            return "-dtcate-";
        }
        public static string defineMobileDetailCate()
        {
            return "-mdtcate-";
        }
        public static string defineDetailChuyenDe()
        {
            return "-dtcd-";
        }
        public static string defineMobileDetailChuyenDe()
        {
            return "-mdtcd-";
        }
        #endregion
        #region kiểm tra thiết bị có phải mobile hay không
        public int Checkdevice()
        {
            int kq = 0;
            string sUserAgent = HttpContext.Current.Request.UserAgent.ToLower();
            if (sUserAgent.Contains("table") || sUserAgent.Contains("ipad"))
            {
                kq = 1;

            }
            if (sUserAgent.Contains("iphone") || sUserAgent.Contains("android"))
            {
                kq = 2;

            }
            return kq;
        }
        #endregion
    }
}