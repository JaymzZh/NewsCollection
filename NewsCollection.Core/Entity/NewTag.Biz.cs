﻿﻿using System;
﻿using System.ComponentModel;
﻿using System.Xml.Serialization;
﻿using NewLife.Data;
using XCode;
﻿using XCode.Membership;

namespace NewsCollention.Entity
{
    /// <summary>NewTag</summary>
    /// <remarks></remarks>
    public partial class NewTag : LogEntity<NewTag>
    {
        #region 对象操作
            ﻿

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
			// 如果没有脏数据，则不需要进行任何处理
			if (!HasDirty) return;

            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
            //if (isNew || Dirtys[__.Name]) CheckExist(__.Name);
            
        }

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected override void InitData()
        //{
        //    base.InitData();

        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    // Meta.Count是快速取得表记录数
        //    if (Meta.Count > 0) return;

        //    // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(NewTag).Name, Meta.Table.DataTable.DisplayName);

        //    var entity = new NewTag();
        //    entity.NewId = 0;
        //    entity.TagId = 0;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(NewTag).Name, Meta.Table.DataTable.DisplayName);
        //}


        ///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        ///// <returns></returns>
        //public override Int32 Insert()
        //{
        //    return base.Insert();
        //}

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnInsert()
        //{
        //    return base.OnInsert();
        //}

        #endregion

        #region 扩展属性

        [NonSerialized]
        private New _New;

        /// <summary>该新闻标签所对应的新闻</summary>
        [XmlIgnore]
        public New New
        {
            get
            {
                if (_New == null && NewId > 0 && !Dirtys.ContainsKey("New"))
                {
                    _New = New.FindById(NewId);
                    Dirtys["New"] = true;
                }
                return _New;
            }
            set { _New = value; }
        }

        /// <summary>该新闻标签所对应的新闻标题</summary>
        [XmlIgnore]
        public String NewTitle => New != null ? New.Title : String.Empty;

        [NonSerialized]
        private Tag _Tag;

        /// <summary>该新闻标签所对应的标签</summary>
        [XmlIgnore]
        public Tag Tag
        {
            get
            {
                if (_Tag == null && TagId > 0 && !Dirtys.ContainsKey("Tag"))
                {
                    _Tag = Tag.FindById(TagId);
                    Dirtys["Tag"] = true;
                }
                return _Tag;
            }
            set { _Tag = value; }
        }

        /// <summary>该新闻标签所对应的标签名称</summary>
        [XmlIgnore]
        public String TagName => Tag != null ? Tag.Name : String.Empty;

        #endregion

        #region 扩展查询

        /// <summary>根据NewId查找</summary>
        /// <param name="newid"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<NewTag> FindByNewId(Int32 newid)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.NewId, newid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.NewId == newid);
        }

        /// <summary>根据NewId、TagId查找</summary>
        /// <param name="newid"></param>
        /// <param name="tagid"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static NewTag FindByNewIdAndTagId(Int32 newid, Int32 tagid)
        {
            if (Meta.Count >= 1000)
                return Find(new String[] { __.NewId, __.TagId }, new Object[] { newid, tagid });
            else // 实体缓存
                return Meta.Cache.Entities.Find(e => e.NewId == newid && e.TagId == tagid);
        }

        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        /// <summary>查询满足条件的记录集，分页、排序</summary>
        /// <param name="userid">用户编号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="key">关键字</param>
        /// <param name="param">分页排序参数，同时返回满足条件的总记录数</param>
        /// <returns>实体集</returns>
        public static EntityList<NewTag> Search(Int32 userid, DateTime start, DateTime end, String key, PageParameter param)
        {
            // WhereExpression重载&和|运算符，作为And和Or的替代
            // SearchWhereByKeys系列方法用于构建针对字符串字段的模糊搜索，第二个参数可指定要搜索的字段
            var exp = SearchWhereByKeys(key, null, null);

            // 以下仅为演示，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符
            //if (userid > 0) exp &= _.OperatorID == userid;
            //if (isSign != null) exp &= _.IsSign == isSign.Value;
            //exp &= _.OccurTime.Between(start, end); // 大于等于start，小于end，当start/end大于MinValue时有效

            return FindAll(exp, param);
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        #endregion
    }
}