namespace Conscience.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        PictureUrl = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Device_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Device", t => t.Device_Id)
                .Index(t => t.Device_Id);
            
            CreateTable(
                "dbo.Device",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastConnection = c.DateTime(nullable: false),
                        DeviceId = c.String(),
                        BatteryLevel = c.Double(nullable: false),
                        BatteryStatus = c.Int(nullable: false),
                        CurrentLocation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Location", t => t.CurrentLocation_Id)
                .Index(t => t.CurrentLocation_Id);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Accuracy = c.Double(),
                        Altitude = c.Double(),
                        AltitudeAccuracy = c.Double(),
                        Heading = c.Double(),
                        HeadingAccuracy = c.Double(),
                        Speed = c.Double(),
                        TimeStamp = c.DateTime(nullable: false),
                        Device_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Device", t => t.Device_Id)
                .Index(t => t.Device_Id);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Host",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        CoreMemory1_Id = c.Int(),
                        CoreMemory2_Id = c.Int(),
                        CoreMemory3_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.Id)
                .ForeignKey("dbo.CoreMemories", t => t.CoreMemory1_Id)
                .ForeignKey("dbo.CoreMemories", t => t.CoreMemory2_Id)
                .ForeignKey("dbo.CoreMemories", t => t.CoreMemory3_Id)
                .Index(t => t.Id)
                .Index(t => t.CoreMemory1_Id)
                .Index(t => t.CoreMemory2_Id)
                .Index(t => t.CoreMemory3_Id);
            
            CreateTable(
                "dbo.CharacterInHost",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssignedOn = c.DateTime(nullable: false),
                        Character_Id = c.Int(nullable: false),
                        Host_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Character", t => t.Character_Id, cascadeDelete: true)
                .ForeignKey("dbo.Host", t => t.Host_Id, cascadeDelete: true)
                .Index(t => t.Character_Id)
                .Index(t => t.Host_Id);
            
            CreateTable(
                "dbo.Character",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Age = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                        Story = c.String(),
                        NarrativeFunction = c.String(),
                        PlotEvent_Id = c.Int(),
                        NotificationPlotChange_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlotEvent", t => t.PlotEvent_Id)
                .ForeignKey("dbo.NotificationsPlotChange", t => t.NotificationPlotChange_Id)
                .Index(t => t.PlotEvent_Id)
                .Index(t => t.NotificationPlotChange_Id);
            
            CreateTable(
                "dbo.Memory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Character", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterInPlot",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        PlotId = c.Int(nullable: false),
                        CharacterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plot", t => t.PlotId, cascadeDelete: true)
                .ForeignKey("dbo.Character", t => t.CharacterId, cascadeDelete: true)
                .Index(t => t.PlotId)
                .Index(t => t.CharacterId);
            
            CreateTable(
                "dbo.Plot",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlotEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hour = c.Int(nullable: false),
                        Minute = c.Int(nullable: false),
                        Location = c.String(),
                        Description = c.String(),
                        Plot_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plot", t => t.Plot_Id, cascadeDelete: true)
                .Index(t => t.Plot_Id);
            
            CreateTable(
                "dbo.CharacterRelation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Character", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.Trigger",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Character", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CoreMemories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Locked = c.Boolean(nullable: false),
                        Audio_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Audio", t => t.Audio_Id)
                .Index(t => t.Audio_Id);
            
            CreateTable(
                "dbo.Audio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Transcription = c.String(),
                        IsEmbeded = c.Boolean(nullable: false),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Processed = c.Boolean(nullable: false),
                        Host_Id = c.Int(),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Host", t => t.Host_Id)
                .ForeignKey("dbo.Employee", t => t.Employee_Id)
                .Index(t => t.Host_Id)
                .Index(t => t.Employee_Id);
            
            CreateTable(
                "dbo.Stats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Int(nullable: false),
                        Host_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Host", t => t.Host_Id, cascadeDelete: true)
                .Index(t => t.Host_Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CharacterCharacterRelation",
                c => new
                    {
                        Character_Id = c.Int(nullable: false),
                        CharacterRelation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Character_Id, t.CharacterRelation_Id })
                .ForeignKey("dbo.Character", t => t.Character_Id, cascadeDelete: true)
                .ForeignKey("dbo.CharacterRelation", t => t.CharacterRelation_Id, cascadeDelete: true)
                .Index(t => t.Character_Id)
                .Index(t => t.CharacterRelation_Id);
            
            CreateTable(
                "dbo.AccountRole",
                c => new
                    {
                        Account_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Account_Id, t.Role_Id })
                .ForeignKey("dbo.Account", t => t.Account_Id, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.Account_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.NotificationsStatChange",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Stat_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notification", t => t.Id)
                .ForeignKey("dbo.Stats", t => t.Stat_Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Stat_Id);
            
            CreateTable(
                "dbo.NotificationsPlotChange",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Plot_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notification", t => t.Id)
                .ForeignKey("dbo.Plot", t => t.Plot_Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Plot_Id);
            
            CreateTable(
                "dbo.NotificationsAudio",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Audio_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notification", t => t.Id)
                .ForeignKey("dbo.Audio", t => t.Audio_Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Audio_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotificationsAudio", "Audio_Id", "dbo.Audio");
            DropForeignKey("dbo.NotificationsAudio", "Id", "dbo.Notification");
            DropForeignKey("dbo.NotificationsPlotChange", "Plot_Id", "dbo.Plot");
            DropForeignKey("dbo.NotificationsPlotChange", "Id", "dbo.Notification");
            DropForeignKey("dbo.NotificationsStatChange", "Stat_Id", "dbo.Stats");
            DropForeignKey("dbo.NotificationsStatChange", "Id", "dbo.Notification");
            DropForeignKey("dbo.Notification", "Employee_Id", "dbo.Employee");
            DropForeignKey("dbo.Employee", "Id", "dbo.Account");
            DropForeignKey("dbo.AccountRole", "Role_Id", "dbo.Role");
            DropForeignKey("dbo.AccountRole", "Account_Id", "dbo.Account");
            DropForeignKey("dbo.Stats", "Host_Id", "dbo.Host");
            DropForeignKey("dbo.Notification", "Host_Id", "dbo.Host");
            DropForeignKey("dbo.Character", "NotificationPlotChange_Id", "dbo.NotificationsPlotChange");
            DropForeignKey("dbo.Host", "CoreMemory3_Id", "dbo.CoreMemories");
            DropForeignKey("dbo.Host", "CoreMemory2_Id", "dbo.CoreMemories");
            DropForeignKey("dbo.Host", "CoreMemory1_Id", "dbo.CoreMemories");
            DropForeignKey("dbo.CoreMemories", "Audio_Id", "dbo.Audio");
            DropForeignKey("dbo.CharacterInHost", "Host_Id", "dbo.Host");
            DropForeignKey("dbo.CharacterInHost", "Character_Id", "dbo.Character");
            DropForeignKey("dbo.Trigger", "Character_Id", "dbo.Character");
            DropForeignKey("dbo.CharacterCharacterRelation", "CharacterRelation_Id", "dbo.CharacterRelation");
            DropForeignKey("dbo.CharacterCharacterRelation", "Character_Id", "dbo.Character");
            DropForeignKey("dbo.CharacterRelation", "Character_Id", "dbo.Character");
            DropForeignKey("dbo.CharacterInPlot", "CharacterId", "dbo.Character");
            DropForeignKey("dbo.PlotEvent", "Plot_Id", "dbo.Plot");
            DropForeignKey("dbo.Character", "PlotEvent_Id", "dbo.PlotEvent");
            DropForeignKey("dbo.CharacterInPlot", "PlotId", "dbo.Plot");
            DropForeignKey("dbo.Memory", "Character_Id", "dbo.Character");
            DropForeignKey("dbo.Host", "Id", "dbo.Account");
            DropForeignKey("dbo.Account", "Device_Id", "dbo.Device");
            DropForeignKey("dbo.Location", "Device_Id", "dbo.Device");
            DropForeignKey("dbo.Device", "CurrentLocation_Id", "dbo.Location");
            DropIndex("dbo.NotificationsAudio", new[] { "Audio_Id" });
            DropIndex("dbo.NotificationsAudio", new[] { "Id" });
            DropIndex("dbo.NotificationsPlotChange", new[] { "Plot_Id" });
            DropIndex("dbo.NotificationsPlotChange", new[] { "Id" });
            DropIndex("dbo.NotificationsStatChange", new[] { "Stat_Id" });
            DropIndex("dbo.NotificationsStatChange", new[] { "Id" });
            DropIndex("dbo.AccountRole", new[] { "Role_Id" });
            DropIndex("dbo.AccountRole", new[] { "Account_Id" });
            DropIndex("dbo.CharacterCharacterRelation", new[] { "CharacterRelation_Id" });
            DropIndex("dbo.CharacterCharacterRelation", new[] { "Character_Id" });
            DropIndex("dbo.Stats", new[] { "Host_Id" });
            DropIndex("dbo.Notification", new[] { "Employee_Id" });
            DropIndex("dbo.Notification", new[] { "Host_Id" });
            DropIndex("dbo.CoreMemories", new[] { "Audio_Id" });
            DropIndex("dbo.Trigger", new[] { "Character_Id" });
            DropIndex("dbo.CharacterRelation", new[] { "Character_Id" });
            DropIndex("dbo.PlotEvent", new[] { "Plot_Id" });
            DropIndex("dbo.CharacterInPlot", new[] { "CharacterId" });
            DropIndex("dbo.CharacterInPlot", new[] { "PlotId" });
            DropIndex("dbo.Memory", new[] { "Character_Id" });
            DropIndex("dbo.Character", new[] { "NotificationPlotChange_Id" });
            DropIndex("dbo.Character", new[] { "PlotEvent_Id" });
            DropIndex("dbo.CharacterInHost", new[] { "Host_Id" });
            DropIndex("dbo.CharacterInHost", new[] { "Character_Id" });
            DropIndex("dbo.Host", new[] { "CoreMemory3_Id" });
            DropIndex("dbo.Host", new[] { "CoreMemory2_Id" });
            DropIndex("dbo.Host", new[] { "CoreMemory1_Id" });
            DropIndex("dbo.Host", new[] { "Id" });
            DropIndex("dbo.Employee", new[] { "Id" });
            DropIndex("dbo.Location", new[] { "Device_Id" });
            DropIndex("dbo.Device", new[] { "CurrentLocation_Id" });
            DropIndex("dbo.Account", new[] { "Device_Id" });
            DropTable("dbo.NotificationsAudio");
            DropTable("dbo.NotificationsPlotChange");
            DropTable("dbo.NotificationsStatChange");
            DropTable("dbo.AccountRole");
            DropTable("dbo.CharacterCharacterRelation");
            DropTable("dbo.Role");
            DropTable("dbo.Stats");
            DropTable("dbo.Notification");
            DropTable("dbo.Audio");
            DropTable("dbo.CoreMemories");
            DropTable("dbo.Trigger");
            DropTable("dbo.CharacterRelation");
            DropTable("dbo.PlotEvent");
            DropTable("dbo.Plot");
            DropTable("dbo.CharacterInPlot");
            DropTable("dbo.Memory");
            DropTable("dbo.Character");
            DropTable("dbo.CharacterInHost");
            DropTable("dbo.Host");
            DropTable("dbo.Employee");
            DropTable("dbo.Location");
            DropTable("dbo.Device");
            DropTable("dbo.Account");
        }
    }
}
