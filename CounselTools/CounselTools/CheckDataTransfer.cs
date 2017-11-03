using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CounselTools
{
    /// <summary>
    /// 檢查資料
    /// </summary>
    public class CheckDataTransfer
    {
        /// <summary>
        /// SINGLE_ANSWER
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="Items"></param>
        /// <param name="StudentID"></param>
        /// <returns></returns>
        public static int CheckSINGLE_ANSWER_Error(string GroupName,List<string> Items, ClassStudent Student)
        {
            int retError = 0;
            if (Gobal._single_recordDict.ContainsKey(Student.StudentID))
            {
                Dictionary<string, string> chkDict = new Dictionary<string, string>();

                foreach (DataRow dr in Gobal._single_recordDict[Student.StudentID])
                {
                    string key = dr["key"].ToString();
                    if (!chkDict.ContainsKey(key))
                        chkDict.Add(key, dr["data"].ToString().Trim());
                }

                foreach (string ss in Items)
                {
                    string key = GroupName + "_" + ss;
                    if (chkDict.ContainsKey(key))
                    {
                        // 2017/5/18 穎驊 依照 [SH][02] 輔導的報表>綜合紀錄表未輸入完整名單，其中"家庭狀況"，一直顯示2/4或3/4，但檢查填寫狀況，皆有輸入。 項目修改
                        // 正常有兄弟姊妹 其KEY，VALUE 可能為 (家庭狀況_兄弟姊妹_排行,1)， 但若為獨子 其KEY，VALUE 為 (家庭狀況_兄弟姊妹_排行,"")，要另外挑出來 不驗證。
                        if (chkDict[key] == "" && key != "家庭狀況_兄弟姊妹_排行")
                            retError++;
                    }
                    else
                        retError++;
                }
            }
            else
            {
                retError = Items.Count;
            }

            return retError;
        }

        public static int CheckMULTI_ANSWER_Error(string GroupName, List<string> Items, ClassStudent Student)
        {
            int retError = 0;
            if (Gobal._multiple_recordDict.ContainsKey(Student.StudentID))
            {
                Dictionary<string, string> chkDict = new Dictionary<string, string>();

                foreach (DataRow dr in Gobal._multiple_recordDict[Student.StudentID])
                {
                    string key = dr["key"].ToString();
                    if (!chkDict.ContainsKey(key))
                        chkDict.Add(key, dr["data"].ToString().Trim());
                }

                foreach (string ss in Items)
                {
                    string key = GroupName + "_" + ss;
                    if (chkDict.ContainsKey(key))
                    {
                        if (chkDict[key] == "")
                            retError++;
                    }
                    else
                        retError++;
                }
            }
            else
            {
                retError = Items.Count;
            }

            return retError;
        }

        public static int CheckSEMESTER_Error(string GroupName, List<string> Items, ClassStudent Student)
        {
            int retError = 0;
            string sem = K12.Data.School.DefaultSemester;
            if (Gobal._semester_dataDict.ContainsKey(Student.StudentID))
            {
                Dictionary<string, DataRow> chkDict = new Dictionary<string, DataRow>();
                foreach (DataRow dr in Gobal._semester_dataDict[Student.StudentID])
                {
                    string key = dr["key"].ToString();
                    if (!chkDict.ContainsKey(key))
                        chkDict.Add(key, dr);
                }

                foreach (string ss in Items)
                {
                    string key1 = GroupName + "_" + ss;
                    if (chkDict.ContainsKey(key1))
                    {
                        if (Student.GradeYear == 0)
                            retError++;
                        else
                        {
                            if (chkDict[key1] == null)
                                retError++;
                            else
                            {
                                bool err = false;
                                // 年級學期判斷
                                if (Student.GradeYear == 0)
                                    err = true;
                                else
                                {
                                    for (int g = 1; g <= Student.GradeYear; g++)
                                    {
                                        string kk = "s" + g + "a";
                                        string kkb = "s" + g + "b";

                                        // 只有上學期
                                        if (g == Student.GradeYear && sem == "1")
                                            kkb = kk;

                                        if (chkDict[key1][kk] == null || chkDict[key1][kkb] == null)
                                            err = true;
                                        else
                                        {
                                            if (chkDict[key1][kk].ToString() == "" || chkDict[key1][kkb].ToString() == "")
                                            {
                                                err = true;
                                            }
                                        }
                                    }
                                }
                                if (err)
                                    retError++;
                            }

                        }
                    }
                    else
                        retError++;
                }
                
            }
            else
            {
                retError = Items.Count;
            }
            return retError;     
        }

        public static int CheckYEARLY_Error(string GroupName, List<string> Items, ClassStudent Student)
        {
            int retError = 0;
            if (Gobal._yearly_dataDict.ContainsKey(Student.StudentID))
            {
                Dictionary<string, DataRow> chkDict = new Dictionary<string, DataRow>();
                foreach (DataRow dr in Gobal._yearly_dataDict[Student.StudentID])
                {
                    string key = dr["key"].ToString();
                    if (!chkDict.ContainsKey(key))
                        chkDict.Add(key, dr);
                }

                foreach (string ss in Items)
                {
                    string key1 = GroupName + "_" + ss;
                    if (chkDict.ContainsKey(key1))
                    {
                        if (chkDict[key1] == null)
                            retError++;
                        else
                        {
                            bool err = false;
                            // 年級判斷
                            if (Student.GradeYear == 0)
                                err = true;
                            else
                            {
                                for (int g = 1; g <= Student.GradeYear; g++)
                                {
                                    string kk = "g" + g;
                                    if (chkDict[key1][kk] == null)
                                        err = true;
                                    else
                                    {
                                        if (chkDict[key1][kk].ToString() == "")
                                        {
                                            err = true;
                                        }
                                    }
                                }
                            }
                            if (err)
                                retError++;
                        }
                    }
                    else
                        retError++;
                }

            }
            else
            {
                retError = Items.Count;
            }
            return retError;
        }

        public static int CheckPRIORITY_Error(string GroupName, List<string> Items, ClassStudent Student)
        {
            int retError = 0;
            if (Gobal._priority_dataDict.ContainsKey(Student.StudentID))
            {
                Dictionary<string, DataRow> chkDict = new Dictionary<string, DataRow>();
                foreach (DataRow dr in Gobal._priority_dataDict[Student.StudentID])
                {
                    string key = dr["key"].ToString();
                    if (!chkDict.ContainsKey(key))
                        chkDict.Add(key, dr);
                }

                foreach (string ss in Items)
                {
                    string key1 = GroupName + "_" + ss;
                    if (chkDict.ContainsKey(key1))
                    {
                        if (chkDict[key1] == null)
                            retError++;
                        else
                        { 
                            // 檢查優先第一項是否有輸入
                            if (chkDict[key1]["p1"] == null)
                                retError++;
                            else
                                if (chkDict[key1]["p1"].ToString() == "")
                                    retError++;
                        }
                    }
                    else
                        retError++;
                }

            }
            else
            {
                retError = Items.Count;
            }
            return retError;
        }

        public static int CheckRELATIVE_Error(string GroupName, List<string> Items, ClassStudent Student)
        {
            int retError = 0;

            // 2017/5/18 穎驊 依照 [SH][02] 輔導的報表>綜合紀錄表未輸入完整名單，其中"家庭狀況"，一直顯示2/4或3/4，但檢查填寫狀況，皆有輸入。 項目修改
            // 調整舊有問題邏輯(其總是會使錯誤數+1，下面已註解)，現在沒有填寫任何尊親屬資料，填答不完整數 +1
            if (!Gobal._relativeDict.ContainsKey(Student.StudentID))
            {
                retError++;
            }



            //if (Gobal._relativeDict.ContainsKey(Student.StudentID))
            //{
            //    Dictionary<string, DataRow> chkDict = new Dictionary<string, DataRow>();
            //    foreach (DataRow dr in Gobal._relativeDict[Student.StudentID])
            //    {
            //        string key = dr["title"].ToString();
            //        if (!chkDict.ContainsKey(key))
            //            chkDict.Add(key, dr);
            //    }

            //    foreach (string ss in Items)
            //    {
            //        string key1 = GroupName + "_" + ss;
            //        if (chkDict.ContainsKey(key1))
            //        {
            //            if (chkDict[key1] == null)
            //                retError++;
            //            else
            //            { 
            //                // 檢查 name 是否輸入
            //                if (chkDict[key1]["name"] == null)
            //                    retError++;
            //                else
            //                    if (chkDict[key1]["name"].ToString() == "")
            //                        retError++;
            //            }
            //        }
            //        else
            //            retError++;
            //    }

            //}
            //else
            //{
            //    retError = Items.Count;
            //}


            return retError;
        }

        public static int CheckSIBLING_Error(string GroupName, List<string> Items, ClassStudent Student)
        {
            int retError = 0;


            Dictionary<string, string> chkDict_single = new Dictionary<string, string>();

            // 2017 /5/22  穎驊 補上字典含Key 的檢查
            if (Gobal._single_recordDict.ContainsKey(Student.StudentID))
            {
                foreach (DataRow dr in Gobal._single_recordDict[Student.StudentID])
                {
                    string key = dr["key"].ToString();
                    if (!chkDict_single.ContainsKey(key))
                        chkDict_single.Add(key, dr["data"].ToString().Trim());
                }


                // 2017/5/18 穎驊 依照 [SH][02] 輔導的報表>綜合紀錄表未輸入完整名單，其中"家庭狀況"，一直顯示2/4或3/4，但檢查填寫狀況，皆有輸入。 項目修改
                // 調整舊有問題邏輯(其總是會使錯誤數+1，下面已註解)，現在若是非獨子(chkDict_single["家庭狀況_兄弟姊妹_排行"]!="") 又沒有填寫任何兄弟姊妹資料，填答不完整數 +1
                if (chkDict_single.ContainsKey("家庭狀況_兄弟姊妹_排行") && chkDict_single["家庭狀況_兄弟姊妹_排行"] != "")
                {
                    if (!Gobal._siblingDict.ContainsKey(Student.StudentID))
                    {
                        retError++;
                    }
                }
            }
            else 
            {
                retError++;
            }
            

            //if (Gobal._siblingDict.ContainsKey(Student.StudentID))
            //{
            //    Dictionary<string, DataRow> chkDict = new Dictionary<string, DataRow>();
            //    foreach (DataRow dr in Gobal._siblingDict[Student.StudentID])
            //    {
            //        string key = dr["title"].ToString();
            //        if (!chkDict.ContainsKey(key))
            //            chkDict.Add(key, dr);
            //    }

            //    foreach (string ss in Items)
            //    {
            //        string key1 = GroupName + "_" + ss;
            //        if (chkDict.ContainsKey(key1))
            //        {
            //            if (chkDict[key1] == null)
            //                retError++;
            //            else
            //            {
            //                // 檢查 name 是否輸入
            //                if (chkDict[key1]["name"] == null)
            //                    retError++;
            //                else
            //                    if (chkDict[key1]["name"].ToString() == "")
            //                        retError++;
            //            }
            //        }
            //        else
            //            retError++;
            //    }

            //}
            //else
            //{
            //    retError = Items.Count;
            //}


            return retError;
        }
    }
}
