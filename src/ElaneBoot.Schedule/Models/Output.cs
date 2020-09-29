using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ElaneBoot.Schedule.Models
{
    /// <summary>
    /// 全局输出对象
    /// </summary>
    public class Output
    {
        public bool __out => true;

        /// <summary>
        /// 返回码
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int? ErrCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 异常栈
        /// </summary>
        public string ErrStack { get; set; }

        /// <summary>
        /// 业务数据
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 特殊列表输出对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListData<T>
    {
        private IList<T> items = new List<T>();

        /// <summary>
        /// 业务数据集合
        /// </summary>
        public IList<T> Items { get { return items; } set { if (value != null) { items = value; } } }

        /// <summary>
        /// 合计信息
        /// </summary>
        public object Total { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public object Tag { get; set; }
    }

    /// <summary>
    /// 分页时输出对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageData<T> : ListData<T>
    {
        private PageInfo pageInfo = new PageInfo();

        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInfo PageInfo { get { return pageInfo; } set { if (value != null) { pageInfo = value; } } }
    }

    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo
    {
        private int pageIndex = 1;
        private int pageSize = 20;

        public PageInfo()
        {

        }
        public PageInfo(int totalCount)
        {
            TotalCount = totalCount;
        }
        public PageInfo(int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        /// <summary>
        /// 页码（1开始）
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex; }
            set { if (value > 0) pageIndex = value; }
        }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { if (value > 0) pageSize = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (TotalCount == 0 || PageSize == 0) return 0;
                return (TotalCount % PageSize) > 0 ? TotalCount / PageSize + 1 : TotalCount / PageSize;
            }
        }

        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 排序
    /// </summary>
    public class SortInfo
    {
        /// <summary>
        /// 排序字段名称
        /// </summary>
        public string SortName { get; set; }

        /// <summary>
        /// 正序倒序
        /// </summary>
        public SortOrder SortOrder { get; set; }
    }

    /// <summary>
    /// 排序方式
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// 正序
        /// </summary>
        [Description("正序")]
        Asc,

        /// <summary>
        /// 倒序
        /// </summary>
        [Description("倒序")]
        Desc,
    }

}