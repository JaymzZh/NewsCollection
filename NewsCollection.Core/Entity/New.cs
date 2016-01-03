﻿using System;
﻿using System.ComponentModel;
﻿using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace NewsCollention.Entity
{
    /// <summary>新闻</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("新闻")]
    [BindIndex("PK_New_Id", true, "Id")]
    [BindIndex("IX_New_Url", false, "Url")]
    [BindIndex("IX_New_AuthorId", false, "AuthorId")]
    [BindRelation("AuthorId", false, "Author", "Id")]
    [BindTable("New", Description = "新闻", ConnName = "DefaultConn", DbType = DatabaseType.SqlServer)]
    public partial class New : INew
    {
        #region 属性
        private Int32 _Id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("ID")]
        [DataObjectField(false, true, false, 10)]
        [BindColumn(1, "Id", "ID", null, "int", 10, 0, false)]
        public virtual Int32 Id
        {
            get { return _Id; }
            set { if (OnPropertyChanging(__.Id, value)) { _Id = value; OnPropertyChanged(__.Id); } }
        }

        private String _Title;
        /// <summary></summary>
        [DisplayName("Title")]
        [Description("标题")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn(2, "Title", "标题", null, "varchar(255)", 0, 0, false, Master=true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
        }

        private String _Url;
        /// <summary></summary>
        [DisplayName("Url")]
        [Description("链接")]
        [DataObjectField(true, false, true, 255)]
        [BindColumn(3, "Url", "链接", null, "varchar(255)", 0, 0, false)]
        public virtual String Url
        {
            get { return _Url; }
            set { if (OnPropertyChanging(__.Url, value)) { _Url = value; OnPropertyChanged(__.Url); } }
        }

        private String _Content;
        /// <summary></summary>
        [DisplayName("Content")]
        [Description("内容")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn(4, "Content", "内容", null, "text", 0, 0, false)]
        public virtual String Content
        {
            get { return _Content; }
            set { if (OnPropertyChanging(__.Content, value)) { _Content = value; OnPropertyChanged(__.Content); } }
        }

        private DateTime _CreateTime;
        /// <summary></summary>
        [DisplayName("CreateTime")]
        [Description("发布时间")]
        [DataObjectField(false, false, false, 7)]
        [BindColumn(5, "CreateTime", "发布时间", null, "datetime", 7, 0, false)]
        public virtual DateTime CreateTime
        {
            get { return _CreateTime; }
            set { if (OnPropertyChanging(__.CreateTime, value)) { _CreateTime = value; OnPropertyChanged(__.CreateTime); } }
        }

        private Int32 _AuthorId;
        /// <summary></summary>
        [DisplayName("AuthorId")]
        [Description("作者")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "AuthorId", "作者", null, "int", 10, 0, false)]
        public virtual Int32 AuthorId
        {
            get { return _AuthorId; }
            set { if (OnPropertyChanging(__.AuthorId, value)) { _AuthorId = value; OnPropertyChanged(__.AuthorId); } }
        }
        #endregion

        #region 获取/设置 字段值
        /// <summary>
        /// 获取/设置 字段值。
        /// 一个索引，基类使用反射实现。
        /// 派生实体类可重写该索引，以避免反射带来的性能损耗
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case __.Id : return _Id;
                    case __.Title : return _Title;
                    case __.Url : return _Url;
                    case __.Content : return _Content;
                    case __.CreateTime : return _CreateTime;
                    case __.AuthorId : return _AuthorId;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Id : _Id = Convert.ToInt32(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.Url : _Url = Convert.ToString(value); break;
                    case __.Content : _Content = Convert.ToString(value); break;
                    case __.CreateTime : _CreateTime = Convert.ToDateTime(value); break;
                    case __.AuthorId : _AuthorId = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得New字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field Id = FindByName(__.Id);

            ///<summary></summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary></summary>
            public static readonly Field Url = FindByName(__.Url);

            ///<summary></summary>
            public static readonly Field Content = FindByName(__.Content);

            ///<summary></summary>
            public static readonly Field CreateTime = FindByName(__.CreateTime);

            ///<summary></summary>
            public static readonly Field AuthorId = FindByName(__.AuthorId);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得New字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String Id = "Id";

            ///<summary></summary>
            public const String Title = "Title";

            ///<summary></summary>
            public const String Url = "Url";

            ///<summary></summary>
            public const String Content = "Content";

            ///<summary></summary>
            public const String CreateTime = "CreateTime";

            ///<summary></summary>
            public const String AuthorId = "AuthorId";

        }
        #endregion
    }

    /// <summary>New接口</summary>
    /// <remarks></remarks>
    public partial interface INew
    {
        #region 属性
        /// <summary></summary>
        Int32 Id { get; set; }

        /// <summary></summary>
        String Title { get; set; }

        /// <summary></summary>
        String Url { get; set; }

        /// <summary></summary>
        String Content { get; set; }

        /// <summary></summary>
        DateTime CreateTime { get; set; }

        /// <summary></summary>
        Int32 AuthorId { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}