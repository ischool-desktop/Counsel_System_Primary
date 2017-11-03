﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Counsel_System.DAO
{
    /// <summary>
    /// AB表欄位索引使用
    /// </summary>
    public class AB_RptColIdx
    {
        /// <summary>
        /// 學年度
        /// </summary>
        public int SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        public int Semester { get; set; }

        /// <summary>
        /// 年級
        /// </summary>
        public int GradeYear { get; set; }

        /// <summary>
        /// 順序
        /// </summary>
        public int Order { get; set; }
    }
}
