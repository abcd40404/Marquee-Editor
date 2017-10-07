namespace Marquee_Editor.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MyDB : DbContext
    {
        // 您的內容已設定為使用應用程式組態檔 (App.config 或 Web.config)
        // 中的 'MyDB' 連接字串。根據預設，這個連接字串的目標是
        // 您的 LocalDb 執行個體上的 'Marquee_Editor.Models.MyDB' 資料庫。
        // 
        // 如果您的目標是其他資料庫和 (或) 提供者，請修改
        // 應用程式組態檔中的 'MyDB' 連接字串。
        public MyDB()
            : base("name=MyDB")
        {
        }

        // 針對您要包含在模型中的每種實體類型新增 DbSet。如需有關設定和使用
        // Code First 模型的詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=390109。

        //public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Mqtt> Mqtts { get; set; }
        public virtual DbSet<Marquee> Marquees { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
    }

}