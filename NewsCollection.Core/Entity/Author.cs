﻿using System;
﻿using System.ComponentModel;
﻿using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace NewsCollention.Entity
{
    /// <summary>作者</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("作者")]
    [BindIndex("PK_Author_Id", true, "Id")]
    [BindIndex("PK_Author_Name", false, "Name")]
    [BindRelation("Id", true, "New", "AuthorId")]
    [BindTable("Author", Description = "作者", ConnName = "DefaultConn", DbType = DatabaseType.SqlServer)]
    public partial class Author : IAuthor
    {
        #region 属性
        private Int32 _Id;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("ID")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "Id", "ID", null, "int", 10, 0, false)]
        public virtual Int32 Id
        {
            get { return _Id; }
            set { if (OnPropertyChanging(__.Id, value)) { _Id = value; OnPropertyChanged(__.Id); } }
        }

        private String _Name;
        /// <summary></summary>
        [DisplayName("Name")]
        [Description("名称")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn(2, "Name", "名称", null, "varchar(255)", 0, 0, false, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private String _Url;
        /// <summary></summary>
        [DisplayName("Url")]
        [Description("链接")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(3, "Url", "链接", null, "varchar(255)", 0, 0, false)]
        public virtual String Url
        {
            get { return _Url; }
            set { if (OnPropertyChanging(__.Url, value)) { _Url = value; OnPropertyChanged(__.Url); } }
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
                    case __.Name : return _Name;
                    case __.Url : return _Url;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Id : _Id = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Url : _Url = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Author字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field Id = FindByName(__.Id);

            ///<summary></summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary></summary>
            public static readonly Field Url = FindByName(__.Url);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Author字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String Id = "Id";

            ///<summary></summary>
            public const String Name = "Name";

            ///<summary></summary>
            public const String Url = "Url";

        }
        #endregion
    }

    /// <summary>Author接口</summary>
    /// <remarks></remarks>
    public partial interface IAuthor
    {
        #region 属性
        /// <summary></summary>
        Int32 Id { get; set; }

        /// <summary></summary>
        String Name { get; set; }

        /// <summary></summary>
        String Url { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}