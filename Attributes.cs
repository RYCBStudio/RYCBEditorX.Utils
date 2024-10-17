using System;

namespace RYCBEditorX.Utils
{
    /// <summary>
    /// 定义一个SQLOperations类，该类是一个属性，用于标记接口上的操作方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class SQLOperations : Attribute
    {
        // 构造函数
        public SQLOperations() : base() { }
    }

    /// <summary>
    /// 定义一个SQLCritical类，该类是一个属性，用于标记方法是关键操作
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SQLCritical : Attribute
    {
        // 构造函数
        public SQLCritical() : base() { }
    }

    public class SQLPrivileges
    {
        /// <summary>
        /// 定义一个Select属性，用于标记方法为查询方法
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class Select : Attribute
        {
            // 构造函数
            public Select() : base() { }
        }

        /// <summary>
        /// 定义一个Insert属性，用于标记方法为插入方法
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class Insert : Attribute
        {
            // 构造函数
            public Insert() : base() { }
        }

        /// <summary>
        /// 定义一个Update属性，用于标记方法为更新方法
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class Update : Attribute
        {
            // 构造函数
            public Update() : base() { }
        }

        /// <summary>
        /// 定义一个Delete属性，用于标记方法为删除方法
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class Delete : Attribute
        {
            // 构造函数
            public Delete() : base() { }
        }

        /// <summary>
        /// 定义一个Execute属性，用于标记方法为执行方法
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class Execute : Attribute
        {
            // 构造函数
            public Execute() : base() { }
        }

        /// <summary>
        /// 定义一个Create属性，用于标记方法为创建方法
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class Create : Attribute
        {
            // 构造函数
            public Create() : base() { }
        }

        /// <summary>
        /// 定义一个All属性，用于标记方法为所有方法
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class All : Attribute
        {
            // 构造函数
            public All() : base() { }
        }
    }
}
