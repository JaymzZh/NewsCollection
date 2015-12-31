﻿using System;
﻿using System.ComponentModel;
﻿using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace NewsCollention.Entity
{
    /// <summary>新闻标签</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("新闻标签")]
    [BindIndex("IX_NewTag", true, "NewId,TagId")]
    [BindRelation("NewId", false, "New", "Id")]
    [BindRelation("TagId", false, "Tag", "Id")]
    [BindTable("NewTag", Description = "新闻标签", ConnName = "DefaultConn", DbType = DatabaseType.SqlServer)]
    public partial class NewTag : INewTag
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

        private Int32 _NewId;
        /// <summary></summary>
        [DisplayName("NewId")]
        [Description("NewId")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(1, "NewId", "NewId", null, "int", 10, 0, false)]
        public virtual Int32 NewId
        {
            get { return _NewId; }
            set { if (OnPropertyChanging(__.NewId, value)) { _NewId = value; OnPropertyChanged(__.NewId); } }
        }

        private Int32 _TagId;
        /// <summary></summary>
        [DisplayName("TagId")]
        [Description("TagId")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "TagId", "TagId", null, "int", 10, 0, false)]
        public virtual Int32 TagId
        {
            get { return _TagId; }
            set { if (OnPropertyChanging(__.TagId, value)) { _TagId = value; OnPropertyChanged(__.TagId); } }
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
                    case __.Id: return _Id;
                    case __.NewId : return _NewId;
                    case __.TagId : return _TagId;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Id : _Id = Convert.ToInt32(value); break;
                    case __.NewId : _NewId = Convert.ToInt32(value); break;
                    case __.TagId : _TagId = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得NewTag字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field Id = FindByName(__.Id);

            ///<summary></summary>
            public static readonly Field NewId = FindByName(__.NewId);

            ///<summary></summary>
            public static readonly Field TagId = FindByName(__.TagId);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得NewTag字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String Id = "Id";

            ///<summary></summary>
            public const String NewId = "NewId";

            ///<summary></summary>
            public const String TagId = "TagId";

        }
        #endregion
    }

    /// <summary>NewTag接口</summary>
    /// <remarks></remarks>
    public partial interface INewTag
    {
        #region 属性
        /// <summary></summary>
        Int32 Id { get; set; }

        /// <summary></summary>
        Int32 NewId { get; set; }

        /// <summary></summary>
        Int32 TagId { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}