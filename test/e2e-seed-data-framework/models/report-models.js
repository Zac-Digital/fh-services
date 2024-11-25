import { DataTypes } from "sequelize";
import { reportDb } from "../connections.js";

// This model is pre-seeded with static values in an earlier pipeline stage.
export const DateDim = reportDb.define(
  "DateDim",
  {
    DateKey: {
      type: DataTypes.INTEGER,
      autoIncrement: true,
      primaryKey: true,
      allowNull: false,
    },
    Date: {
      type: DataTypes.DATEONLY,
      allowNull: false,
    },
    DateString: {
      type: DataTypes.STRING(10),
    },
    DayNumberOfWeek: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    DayOfWeekName: {
      type: DataTypes.STRING(10),
      allowNull: false,
    },
    DayNumberOfMonth: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    DayNumberOfYear: {
      type: DataTypes.SMALLINT,
      allowNull: false,
    },
    WeekNumberOfYear: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    MonthName: {
      type: DataTypes.STRING(10),
      allowNull: false,
    },
    MonthNumberOfYear: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    CalendarQuarterNumberOfYear: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    CalendarYearNumber: {
      type: DataTypes.SMALLINT,
      allowNull: false,
    },
    IsWeekend: {
      type: DataTypes.BOOLEAN,
      allowNull: false,
    },
    IsLeapYear: {
      type: DataTypes.BOOLEAN,
      allowNull: false,
    },
  },
  {
    schema: "dim",
    tableName: "DateDim",
    timestamps: false,
  }
);

// This model is pre-seeded with static values in an earlier pipeline stage.
export const TimeDim = reportDb.define(
  "TimeDim",
  {
    TimeKey: {
      type: DataTypes.INTEGER,
      autoIncrement: true,
      primaryKey: true,
      allowNull: false,
    },
    Time: {
      type: DataTypes.TIME(0),
      allowNull: false,
    },
    TimeString: {
      type: DataTypes.STRING(8),
      allowNull: false,
    },
    HourNumberOfDay: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    MinuteNumberOfHour: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    SecondNumberOfMinute: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
  },
  {
    schema: "dim",
    tableName: "TimeDim",
    timestamps: false,
  }
);

export const OrganisationDim = reportDb.define(
  "OrganisationDim",
  {
    OrganisationKey: {
      type: DataTypes.BIGINT,
      autoIncrement: true,
      primaryKey: true,
      allowNull: false,
    },
    OrganisationTypeId: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    OrganisationTypeName: {
      type: DataTypes.STRING(50),
      allowNull: false,
    },
    OrganisationId: {
      type: DataTypes.BIGINT,
      allowNull: false,
    },
    OrganisationName: {
      type: DataTypes.STRING(512),
      allowNull: false,
    },
    Created: {
      type: DataTypes.DATE,
      allowNull: false,
    },
    CreatedBy: {
      type: DataTypes.STRING(320),
      allowNull: false,
    },
    Modified: {
      type: DataTypes.DATE,
      allowNull: false,
    },
    ModifiedBy: {
      type: DataTypes.STRING(320),
      allowNull: false,
    },
  },
  {
    schema: "idam",
    tableName: "OrganisationDim",
    timestamps: false,
  }
);

export const ServiceSearchesDim = reportDb.define(
  "ServiceSearchesDim",
  {
    ServiceSearchesKey: {
      type: DataTypes.INTEGER,
      autoIncrement: true,
      primaryKey: true,
      allowNull: false,
    },
    ServiceSearchId: {
      type: DataTypes.BIGINT,
      allowNull: false,
    },
    ServiceTypeId: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    ServiceTypeName: {
      type: DataTypes.STRING(255),
      allowNull: false,
    },
    EventId: {
      type: DataTypes.BIGINT,
      allowNull: false,
    },
    EventName: {
      type: DataTypes.STRING(255),
      allowNull: false,
    },
    UserId: {
      type: DataTypes.BIGINT,
    },
    UserName: {
      type: DataTypes.STRING(512),
    },
    UserEmail: {
      type: DataTypes.STRING(512),
    },
    RoleTypeId: {
      type: DataTypes.TINYINT,
    },
    RoleTypeName: {
      type: DataTypes.STRING(255),
    },
    OrganisationId: {
      type: DataTypes.BIGINT,
    },
    OrganisationName: {
      type: DataTypes.STRING(255),
    },
    OrganisationTypeId: {
      type: DataTypes.TINYINT,
    },
    OrganisationTypeName: {
      type: DataTypes.STRING(50),
    },
    PostCode: {
      type: DataTypes.STRING(50),
      allowNull: false,
    },
    SearchRadiusMiles: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    HttpRequestTimestamp: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
    HttpRequestCorrelationId: {
      type: DataTypes.STRING(50),
    },
    HttpResponseCode: {
      type: DataTypes.SMALLINT,
    },
    HttpResponseTimestamp: {
      type: DataTypes.DATE(7),
    },
    Created: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
    Modified: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
  },
  {
    schema: "dim",
    tableName: "ServiceSearchesDim",
    timestamps: false,
  }
);

export const ServiceSearchFacts = reportDb.define(
  "ServiceSearchFacts",
  {
    ServiceSearchesKey: {
      type: DataTypes.INTEGER,
      allowNull: false,
      references: {
        model: "ServiceSearchesDim",
        key: "ServiceSearchesKey",
      },
    },
    DateKey: {
      type: DataTypes.INTEGER,
      allowNull: false,
      references: {
        model: "DateDim",
        key: "DateKey",
      },
    },
    TimeKey: {
      type: DataTypes.INTEGER,
      allowNull: false,
      references: {
        model: "TimeDim",
        key: "TimeKey",
      },
    },
    Created: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
    Modified: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
    ServiceSearchId: {
      type: DataTypes.BIGINT,
      allowNull: false,
    },
    Id: {
      type: DataTypes.BIGINT,
      autoIncrement: false,
      primaryKey: true,
      allowNull: false,
    },
  },
  {
    schema: "dim",
    timestamps: false,
  }
);

export const ConnectionRequestsSentFacts = reportDb.define(
  "ConnectionRequestsSentFacts",
  {
    DateKey: {
      type: DataTypes.INTEGER,
      allowNull: false,
      references: {
        model: "DateDim",
        key: "DateKey",
      },
    },
    TimeKey: {
      type: DataTypes.INTEGER,
      allowNull: false,
      references: {
        model: "TimeDim",
        key: "TimeKey",
      },
    },
    OrganisationKey: {
      type: DataTypes.BIGINT,

      references: {
        model: "OrganisationDim",
        key: "OrganisationKey",
      },
    },
    ConnectionRequestsSentMetricsId: {
      type: DataTypes.BIGINT,
      allowNull: false,
    },
    RequestTimestamp: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
    RequestCorrelationId: {
      type: DataTypes.STRING(50),
      allowNull: false,
    },
    ResponseTimestamp: {
      type: DataTypes.DATE(7),
    },
    HttpResponseCode: {
      type: DataTypes.SMALLINT,
    },
    ConnectionRequestId: {
      type: DataTypes.BIGINT,
    },
    ConnectionRequestReferenceCode: {
      type: DataTypes.CHAR(6),
    },
    Created: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
    CreatedBy: {
      type: DataTypes.STRING(512),
      allowNull: false,
    },
    Modified: {
      type: DataTypes.DATE(7),
    },
    ModifiedBy: {
      type: DataTypes.STRING(512),
    },
    Id: {
      type: DataTypes.BIGINT,
      autoIncrement: false,
      primaryKey: true,
      allowNull: false,
    },
    VcsOrganisationKey: {
      type: DataTypes.BIGINT,
      references: {
        model: "OrganisationDim",
        key: "OrganisationKey",
      },
    },
  },
  {
    schema: "dim",
    timestamps: false,
  }
);

// -------------------------------- //

// I've moved these to the bottom as they appear to be unused in the web-app and so don't need seeding, but leaving them at least ensures the schema is identical to that of the web apps.

export const UserAccountDim = reportDb.define(
  "UserAccountDim",
  {
    UserAccountKey: {
      type: DataTypes.BIGINT,
      autoIncrement: true,
      primaryKey: true,
      allowNull: false,
    },
    UserAccountId: {
      type: DataTypes.BIGINT,
      allowNull: false,
    },
    UserAccountRoleTypeId: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    UserAccountRoleTypeName: {
      type: DataTypes.STRING(50),
      allowNull: false,
    },
    OrganisationTypeId: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    OrganisationId: {
      type: DataTypes.BIGINT,
      allowNull: false,
    },
    OrganisationName: {
      type: DataTypes.STRING(512),
      allowNull: false,
    },
    OrganisationTypeName: {
      type: DataTypes.STRING(50),
      allowNull: false,
    },
    Name: {
      type: DataTypes.STRING(255),
      allowNull: false,
    },
    Email: {
      type: DataTypes.STRING(512),
      allowNull: false,
    },
    Status: {
      type: DataTypes.TINYINT,
      allowNull: false,
    },
    Created: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
    LastModified: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
    SysStartTime: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
    SysEndTime: {
      type: DataTypes.DATE(7),
      allowNull: false,
    },
    CreatedBy: {
      type: DataTypes.STRING(320),
      allowNull: false,
      defaultValue: "",
    },
    LastModifiedBy: {
      type: DataTypes.STRING(320),
      allowNull: false,
      defaultValue: "",
    },
  },
  {
    schema: "idam",
    tableName: "UserAccountDim",
  }
);
